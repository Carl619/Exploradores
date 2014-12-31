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
			// objetos
		public Gestor<Mapa.LugarVisitable> lugares { get; protected set; }
		public Gestor<Mapa.Ciudad> ciudades { get; protected set; }
		public Gestor<Mapa.Ruina> ruinas { get; protected set; }
		public Gestor<Mapa.Ruta> rutas { get; protected set; }
		public Gestor<Objetos.Articulo> articulos { get; protected set; }
		public Gestor<Objetos.Inventario> inventarios { get; protected set; }
		public Dictionary<String, Interaccion.EventoAtomico> eventos { get; protected set; }
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
			bloquearSonidoYMusica = false;

			lugares = new Gestor<Mapa.LugarVisitable>();
			ciudades = new Gestor<Mapa.Ciudad>();
			ruinas = new Gestor<Mapa.Ruina>();
			rutas = new Gestor<Mapa.Ruta>();
			articulos = new Gestor<Objetos.Articulo>();
			inventarios = new Gestor<Objetos.Inventario>();
			eventos = new Dictionary<string,Interaccion.EventoAtomico>();
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

			evento = new Interaccion.EventoAtomico("comercio");
			evento.accion = Programa.Controller.funcionAbrirComercio;
			eventos.Add(evento.id, evento);

			evento = new Interaccion.EventoAtomico("reclutamiento");
			evento.accion = Programa.Controller.funcionReclutar;
			eventos.Add(evento.id, evento);
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




