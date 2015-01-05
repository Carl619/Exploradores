using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;




namespace Gestores
{
	
	
	public class Partidas
	{
		// variables
		private static Partidas instancia = null;
		public static Partidas Instancia
		{
			get
			{
				if (instancia == null)
					new Partidas();
				return instancia;
			}
		}
			// variables utiles
		public Random aleatorio { get; protected set; }
		public GMisiones gestorMisiones { get; protected set; }
		public GRuinas gestorRuinas { get; protected set; }
		public Pantallas gestorPantallas { get; protected set; }
		public uint numeroLugaresVisitables { get; protected set; }
		public uint dias { get; protected set; }
		public uint horas { get; protected set; }
		public Personajes.Atributo atributoSeleciconado { get; set; }
		public Personajes.Habilidad habilidadSeleciconada { get; set; }
		public Personajes.NPCFlyweight edificioSeleccionado { get; set; }
		public String npcSeleccionado { get; set; }
		public Song musicaActual { get; set; }
		public bool bloquearSonidoYMusica { get; set; }
		public List<String> mensajesFracasoJuego { get; set; }
		public List<String> mensajesExitoJuego { get; set; }
			// objetos
		public Gestor<Mapa.LugarVisitable> lugares { get; protected set; }
		public Gestor<Mapa.Ciudad> ciudades { get; protected set; }
		public Gestor<Mapa.Ruina> ruinas { get; protected set; }
		public Gestor<Mapa.Ruta> rutas { get; protected set; }
		public Gestor<Objetos.Articulo> articulos { get; protected set; }
		public Gestor<Objetos.Inventario> inventarios { get; protected set; }
		public Dictionary<String, Interaccion.Evento> eventos { get; protected set; }
		public Dictionary<String, Interaccion.ElementoDialogo> elementosDialogo { get; protected set; }
		public Gestor<Interaccion.MenuDialogo> dialogos { get; protected set; }
		public Gestor<Personajes.NPC> npcs { get; protected set; }
		//public Gestor<Personajes.Acompanante> acompanantes { get; protected set; }


		// constructor
		private Partidas()
		{
			instancia = this;
			clear();
		}


		// funciones
		public void clear()
		{
			aleatorio = new Random();
			gestorMisiones = new GMisiones();
			gestorRuinas = new GRuinas();
			gestorPantallas = new Pantallas();
			numeroLugaresVisitables = 0;
			dias = 1;
			horas = 8;
			atributoSeleciconado = null;
			habilidadSeleciconada = null;
			edificioSeleccionado = null;
			npcSeleccionado = null;
			musicaActual = null;
			bloquearSonidoYMusica = true;
			mensajesFracasoJuego = new List<String>();
			mensajesExitoJuego = new List<String>();
			mensajesExitoJuego.Add("Por fin, ya has conseguido cumplir con tus objetivos principales. Despues de vencer a tus enemigos, y sobrevivir las ruinas en busqueda de tesoros, tus aventuras ya se acaba aqui.");
			mensajesExitoJuego.Add("Ha llegado el momento de terminar el viaje que empezaste hace tanto tiempo, descansar y disfrutar de las riquiezas acumuladas.");

			lugares = new Gestor<Mapa.LugarVisitable>();
			ciudades = new Gestor<Mapa.Ciudad>();
			ruinas = new Gestor<Mapa.Ruina>();
			rutas = new Gestor<Mapa.Ruta>();
			articulos = new Gestor<Objetos.Articulo>();
			inventarios = new Gestor<Objetos.Inventario>();
			eventos = new Dictionary<String, Interaccion.Evento>();
			elementosDialogo = new Dictionary<String, Interaccion.ElementoDialogo>();
			dialogos = new Gestor<Interaccion.MenuDialogo>();
			npcs = new Gestor<Personajes.NPC>();
			//acompanantes = new Gestor<Personajes.Acompanante>();
		}


		public void cambiarMusica(String idMusica)
		{
			if(bloquearSonidoYMusica == true)
				return;
			Song musicaNueva = Gestores.Mundo.Instancia.musica[idMusica];
			if(musicaActual != musicaNueva)
			{
				musicaActual = musicaNueva;
				MediaPlayer.Stop();
				MediaPlayer.Play(musicaActual);
			}
		}


		public void actualizarTodo(GameTime tiempo)
		{
			gestorRuinas.actualizarRuinaActual(tiempo);
		}


		public void avanzarTiempoMapa(uint numeroDias, uint numeroHoras)
		{
			dias += numeroDias;
			horas += numeroHoras;
			while(horas > 23)
			{
				++dias;
				horas -= 24;
			}

			Programa.Jugador.Instancia.addHambre(numeroHoras + 24 * numeroDias);
			Programa.Jugador.Instancia.comerPorPrioridadGrupo();
			gestorMisiones.notify(Interaccion.Mision.Evento.Tiempo, null);
		}


		public void finalizarPartida(Pantallas.EstadoJuego estadoJuego)
		{
			if(estadoJuego != Pantallas.EstadoJuego.Exito &&
				estadoJuego != Pantallas.EstadoJuego.Fracaso)
				return;
			Programa.VistaGeneral.Instancia.cambiarVista(estadoJuego);
		}


		public void cargarNuevaPartida()
		{
			cargarPartida("data");
		}


		public void cargarPartidaAnterior(String carpeta)
		{
			cargarPartida("savegames/" + carpeta);
		}


		public void guardarPartidaActual(String carpeta)
		{
			guardarPartida("savegames/" + carpeta);
		}


		protected void cargarPartida(String carpeta)
		{
			clear();
			aleatorio = new Random();
			Partidas.Instancia.cambiarMusica("ciudad");

			// no cargar lugares por separado
			cargarContenidoCiudades(@carpeta + "/ciudades.txt");
			cargarContenidoRuinas(@carpeta + "/ruinas.txt");
			cargarContenidoRutas(@carpeta + "/rutas.txt");
			cargarContenidoArticulos(@carpeta + "/articulos.txt");
			cargarContenidoInventarios(@carpeta + "/inventarios.txt");
			cargarContenidoEventos();
			cargarContenidoDialogos(@carpeta + "/dialogos.txt");
			cargarContenidoNPCs(@carpeta + "/npcs.txt");
			//cargarContenidoAcompanantes(@carpeta + "/acompanantes.txt");


			Programa.Jugador.Instancia.crearProtagonista("Yo", ciudades["1"]);
			Personajes.Atributo atributo;
			List<String> lista;

			atributo = Gestores.Mundo.Instancia.atributos["idVida"];
			lista = new List<string>();
			lista.Add("300");
			Programa.Jugador.Instancia.protagonista.atributos.Add(atributo.id, atributo.clone(lista));
			atributo = Gestores.Mundo.Instancia.atributos["idFuerza"];
			lista = new List<string>();
			lista.Add("300");
			Programa.Jugador.Instancia.protagonista.atributos.Add(atributo.id, atributo.clone(lista));
			atributo = Gestores.Mundo.Instancia.atributos["idHambre"];
			lista = new List<string>();
			lista.Add("0");
			Programa.Jugador.Instancia.protagonista.atributos.Add(atributo.id, atributo.clone(lista));

			Programa.Jugador.Instancia.actualizarEspacioInventario();


			int i = 0;
			foreach(KeyValuePair<String, Mapa.Ciudad> ciudad in ciudades)
			{
				ciudad.Value.indiceGrafo = i;
				++i;
			}
			foreach(KeyValuePair<String, Mapa.Ruina> ruina in ruinas)
			{
				ruina.Value.indiceGrafo = i;
				++i;
			}
			numeroLugaresVisitables = (uint)i;

			gestorRuinas.cargarTodo();
			gestorMisiones.cargarTodo();
		}


		protected void guardarPartida(String carpeta)
		{
			guardarContenidoCiudades(@carpeta + "/ciudades.txt");
			guardarContenidoRuinas(@carpeta + "/ruinas.txt");
			guardarContenidoRutas(@carpeta + "/rutas.txt");
			guardarContenidoArticulos(@carpeta + "/articulos.txt");
			guardarContenidoInventarios(@carpeta + "/inventarios.txt");
			guardarContenidoDialogos(@carpeta + "/dialogos.txt");
			guardarContenidoNPCs(@carpeta + "/npcs.txt");
			//guardarContenidoAcompanantes(@carpeta + "/acompanantes.txt");
		}


		// cargar
		protected void cargarContenidoCiudades(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				ciudades.loadAll(reader, Mapa.Ciudad.cargarObjeto);
			}
		}


		protected void cargarContenidoRuinas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				ruinas.loadAll(reader, Mapa.Ruina.cargarObjeto);
			}
		}


		protected void cargarContenidoRutas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				rutas.loadAll(reader, Mapa.Ruta.cargarObjeto);
			}
		}


		protected void cargarContenidoArticulos(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				articulos.loadAll(reader, Objetos.Articulo.cargarObjeto);
			}
		}


		protected void cargarContenidoInventarios(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				inventarios.loadAll(reader, Objetos.Inventario.cargarObjeto, Objetos.Inventario.esLista);
			}
		}


		protected void cargarContenidoEventos()
		{
			Interaccion.EventoAtomico evento;
			Interaccion.Script script;

			evento = new Interaccion.EventoAtomico("abrirComercio");
			evento.accion = Mapa.Controller.abrirComercio;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("reclutar");
			evento.accion = Mapa.Controller.reclutar;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("empezarMision");
			evento.accion = Mapa.Controller.empezarMision;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("addEventoDialogo");
			evento.accion = Mapa.Controller.addEventoDialogo;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("removeEventoDialogo");
			evento.accion = Mapa.Controller.removeEventoDialogo;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("elementoDialogoCondicional");
			evento.accion = Mapa.Controller.elementoDialogoCondicional;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("habilitarElementoDialogo");
			evento.accion = Mapa.Controller.habilitarElementoDialogo;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("deshabilitarElementoDialogo");
			evento.accion = Mapa.Controller.deshabilitarElementoDialogo;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("recibirArticulos");
			evento.accion = Mapa.Controller.recibirArticulos;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("entregarArticulos");
			evento.accion = Mapa.Controller.entregarArticulos;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("comprobarExistenciaArticulos");
			evento.accion = Mapa.Controller.comprobarExistenciaArticulos;
			eventos.Add(evento.id, evento);


			List<String> listaLlamada;


			// ----------------------------------------------------------------
			script = new Interaccion.Script("dialogoEntregaArticulo");
			script.variablesLocales.Add("resultadoArticulos");
			script.parametrosEntrada.Add("idEntradaDialogo");
			script.parametrosEntrada.Add("idMenuDialogo");
			script.parametrosEntrada.Add("valorCondicion");
			script.parametrosEntrada.Add("idArticulo");
			script.parametrosEntrada.Add("cantidadArticulos");

			listaLlamada = new List<String>();
			listaLlamada.Add("idEntradaDialogo");
			listaLlamada.Add("idMenuDialogo");
			listaLlamada.Add("resultadoArticulos");
			listaLlamada.Add("idArticulo");
			listaLlamada.Add("cantidadArticulos");
			script.parametrosLlamadas.Add(listaLlamada);

			listaLlamada = new List<String>();
			listaLlamada.Add("idEntradaDialogo");
			listaLlamada.Add("idMenuDialogo");
			listaLlamada.Add("resultadoArticulos");
			listaLlamada.Add("valorCondicion");
			script.parametrosLlamadas.Add(listaLlamada);

			script.eventos.Add(eventos["comprobarExistenciaArticulos"]);
			script.eventos.Add(eventos["elementoDialogoCondicional"]);

			eventos.Add(script.id, script);
			// ----------------------------------------------------------------


			// ----------------------------------------------------------------
			script = new Interaccion.Script("dialogoDobleEntregaArticulo");
			script.parametrosEntrada.Add("idEntradaHabilitadaDialogo");
			script.parametrosEntrada.Add("idMenuHabilitadoDialogo");
			script.parametrosEntrada.Add("idEntradaDeshabilitadaDialogo");
			script.parametrosEntrada.Add("idMenuDeshabilitadoDialogo");
			script.parametrosEntrada.Add("idArticulo");
			script.parametrosEntrada.Add("cantidadArticulos");

			listaLlamada = new List<String>();
			listaLlamada.Add("idEntradaHabilitadaDialogo");
			listaLlamada.Add("idMenuHabilitadoDialogo");
			listaLlamada.Add("true");
			listaLlamada.Add("idArticulo");
			listaLlamada.Add("cantidadArticulos");
			script.parametrosLlamadas.Add(listaLlamada);

			listaLlamada = new List<String>();
			listaLlamada.Add("idEntradaDeshabilitadaDialogo");
			listaLlamada.Add("idMenuDeshabilitadoDialogo");
			listaLlamada.Add("false");
			listaLlamada.Add("idArticulo");
			listaLlamada.Add("cantidadArticulos");
			script.parametrosLlamadas.Add(listaLlamada);

			script.eventos.Add(eventos["dialogoEntregaArticulo"]);
			script.eventos.Add(eventos["dialogoEntregaArticulo"]);

			eventos.Add(script.id, script);
			// ----------------------------------------------------------------


			// ----------------------------------------------------------------
			script = new Interaccion.Script("relizarIntercambioUnico");
			script.parametrosEntrada.Add("idEntradaDialogo");
			script.parametrosEntrada.Add("idMenuDialogo");
			script.parametrosEntrada.Add("listaArticulosEntrega");
			script.parametrosEntrada.Add("listaArticulosRecibo");

			listaLlamada = new List<String>();
			listaLlamada.Add("idEntradaDialogo");
			listaLlamada.Add("idMenuDialogo");
			script.parametrosLlamadas.Add(listaLlamada);

			listaLlamada = new List<String>();
			listaLlamada.Add("idEntradaDialogo");
			listaLlamada.Add("idMenuDialogo");
			listaLlamada.Add("listaArticulosEntrega");
			script.parametrosLlamadas.Add(listaLlamada);

			listaLlamada = new List<String>();
			listaLlamada.Add("idEntradaDialogo");
			listaLlamada.Add("idMenuDialogo");
			listaLlamada.Add("listaArticulosRecibo");
			script.parametrosLlamadas.Add(listaLlamada);

			script.eventos.Add(eventos["deshabilitarElementoDialogo"]);
			script.eventos.Add(eventos["entregarArticulos"]);
			script.eventos.Add(eventos["recibirArticulos"]);

			eventos.Add(script.id, script);
			// ----------------------------------------------------------------
		}


		protected void cargarContenidoDialogos(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				dialogos.loadAll(reader, Interaccion.MenuDialogo.cargarObjeto, Interaccion.MenuDialogo.esLista);
			}
		}


		protected void cargarContenidoNPCs(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				npcs.loadAll(reader, Personajes.NPC.cargarObjeto, Personajes.NPC.esLista);
			}
		}


		// guardar
		protected void guardarContenidoCiudades(String fileName)
		{
			using (System.IO.StreamWriter reader = new System.IO.StreamWriter(fileName))
			{
				ciudades.saveAll(reader, Mapa.Ciudad.guardarObjeto);
			}
		}


		protected void guardarContenidoRuinas(String fileName)
		{
			using (System.IO.StreamWriter reader = new System.IO.StreamWriter(fileName))
			{
				ruinas.saveAll(reader, Mapa.Ruina.guardarObjeto);
			}
		}


		protected void guardarContenidoRutas(String fileName)
		{
			using (System.IO.StreamWriter reader = new System.IO.StreamWriter(fileName))
			{
				rutas.saveAll(reader, Mapa.Ruta.guardarObjeto);
			}
		}


		protected void guardarContenidoArticulos(String fileName)
		{
			using (System.IO.StreamWriter reader = new System.IO.StreamWriter(fileName))
			{
				articulos.saveAll(reader, Objetos.Articulo.guardarObjeto);
			}
		}


		protected void guardarContenidoInventarios(String fileName)
		{
			using (System.IO.StreamWriter reader = new System.IO.StreamWriter(fileName))
			{
				inventarios.saveAll(reader, Objetos.Inventario.guardarObjeto);
			}
		}


		protected void guardarContenidoDialogos(String fileName)
		{
			using (System.IO.StreamWriter reader = new System.IO.StreamWriter(fileName))
			{
				dialogos.saveAll(reader, Interaccion.MenuDialogo.guardarObjeto);
			}
		}


		protected void guardarContenidoNPCs(String fileName)
		{
			using (System.IO.StreamWriter reader = new System.IO.StreamWriter(fileName))
			{
				npcs.saveAll(reader, Personajes.NPC.guardarObjeto);
			}
		}
	}
}




