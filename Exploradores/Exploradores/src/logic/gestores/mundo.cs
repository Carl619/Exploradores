using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;




namespace Gestores
{
	
	
	public class Mundo
	{
		// variables
		private static Mundo instancia = null;
		public static Mundo Instancia
		{
			get
			{
				if (instancia == null)
					new Mundo();
				return instancia;
			}
		}
			// variables utiles
		public Dictionary<String, SpriteFont> fuentes { get; protected set; }
		public Dictionary<String, Color> colores { get; protected set; }
		public Dictionary<String, Imagen> imagenes { get; protected set; }
			// elementos graficos
		public Gestor<ILSXNA.BorderFlyweight> borderFlyweights { get; protected set; }
		public Gestor<ILSXNA.Border> borders { get; protected set; }
		public Gestor<ILSXNA.ButtonFlyweight> buttonFlyweights { get; protected set; }
		public Gestor<Programa.ListaViewFlyweight> listaViewFlyweights { get; protected set; }
		public Gestor<ILSXNA.Border> estilosHabitacion { get; protected set; }
			// objetos comunes
		public Dictionary<String, Personajes.Atributo> atributos { get; protected set; }
		public Dictionary<String, Personajes.Habilidad> habilidades { get; protected set; }
		public Dictionary<String, Song> musica { get; protected set; }
			// flyweights modelos
		public Gestor<Mapa.LugarVisitableFlyweight> lugarFlyweights { get; protected set; }
		public Gestor<Mapa.CiudadFlyweight> ciudadFlyweights { get; protected set; }
		public Gestor<Mapa.RuinaFlyweight> ruinaFlyweights { get; protected set; }
		public Gestor<Mapa.RutaFlyweight> rutaFlyweights { get; protected set; }
		public Gestor<Objetos.ArticuloFlyweight> articuloFlyweights { get; protected set; }
		public Gestor<Ruinas.ObjetoFlyweight> objetoFlyweights { get; protected set; }
		public Gestor<Ruinas.RelojFlyweight> relojFlyweights { get; protected set; }
		public Gestor<Ruinas.PersonajeRuinaFlyweight> personajeRuinaFlyweights { get; protected set; }
		public Gestor<Personajes.NPCFlyweight> npcFlyweights { get; protected set; }


		// constructor
		private Mundo()
		{
			instancia = this;

			fuentes = new Dictionary<String, SpriteFont>();
			colores = new Dictionary<String, Color>();
			imagenes = new Dictionary<String, Imagen>();
			
			borderFlyweights = new Gestor<ILSXNA.BorderFlyweight>();
			borders = new Gestor<ILSXNA.Border>();
			buttonFlyweights = new Gestor<ILSXNA.ButtonFlyweight>();
			listaViewFlyweights = new Gestor<Programa.ListaViewFlyweight>();
			estilosHabitacion = new Gestor<ILSXNA.Border>();
			
			atributos = new Dictionary<String, Personajes.Atributo>();
			habilidades = new Dictionary<String, Personajes.Habilidad>();
			musica = new Dictionary<String, Song>();

			lugarFlyweights = new Gestor<Mapa.LugarVisitableFlyweight>();
			ciudadFlyweights = new Gestor<Mapa.CiudadFlyweight>();
			ruinaFlyweights = new Gestor<Mapa.RuinaFlyweight>();
			rutaFlyweights = new Gestor<Mapa.RutaFlyweight>();
			articuloFlyweights = new Gestor<Objetos.ArticuloFlyweight>();
			objetoFlyweights = new Gestor<Ruinas.ObjetoFlyweight>();
			relojFlyweights = new Gestor<Ruinas.RelojFlyweight>();
			personajeRuinaFlyweights = new Gestor<Ruinas.PersonajeRuinaFlyweight>();
			npcFlyweights = new Gestor<Personajes.NPCFlyweight>();
		}


		// funciones
		public static float parseFloat(String campo, char separador = '.')
		{
			String[] partesArray = campo.Split(separador);
			List<String> partes = new List<string>();
			partes.AddRange(partesArray);
			if(partes.Count == 1)
				return (float)(Convert.ToInt32(partes[0]));
			if(partes.Count > 2)
				throw new ArgumentException();
			
			int i = 0;
			for(;i < partes[1].Length && partes[1][i] == '0'; ++i)
				;

			double derecha = Convert.ToInt32(partes[1]);
			while(derecha >= 1.0f)
				derecha /= 10.0f;
			for(; i > 0; --i)
				derecha /= 10.0f;
			if(Convert.ToDouble(campo) < 0.0f)
				return - ((-(float)(Convert.ToInt32(partes[0]))) + (float)derecha);
			else
				return (float)(Convert.ToInt32(partes[0])) + (float)derecha;
		}


		public void actualizarTodo(GameTime tiempo)
		{
			Partidas.Instancia.actualizarTodo(tiempo);
		}


		public void cargarTodo()
		{
			cargarElementosGraficos();

			cargarAtributos();
			cargarHabilidades();
			cargarMusica();

			cargarFlyweights();

			if(Partidas.Instancia == null)
				throw new ArgumentNullException();
			
			Partidas.Instancia.cambiarMusica("mapa");
		}


		public void cargarDatosMundo()
		{
			cargarFuentes();
			cargarColores();
			cargarImagenes();
		}


		public void cargarElementosGraficos()
		{
			cargarFlyweightsBordes(@"data/borderflyweight.txt");
			cargarBordes(@"data/borders.txt");
			cargarFlyweightsBotones(@"data/buttonflyweight.txt");
			cargarFlyweightsListasView(@"data/listaviewflyweight.txt");
			cargarEstilosHabitaciones(@"data/estiloshabitacion.txt");
		}


		public void cargarFlyweights()
		{
			cargarFlyweightsLugares(@"data/lugarflyweight.txt");
			cargarFlyweightsCiudades(@"data/ciudadflyweight.txt");
			cargarFlyweightsRuinas(@"data/ruinaflyweight.txt");
			cargarFlyweightsRutas(@"data/rutaflyweight.txt");
			cargarFlyweightsArticulos(@"data/articuloflyweight.txt");
			cargarFlyweightsObjetos(@"data/objetoflyweight.txt");
			cargarFlyweightsRelojes(@"data/relojflyweight.txt");
			cargarFlyweightsPersonajeRuinas(@"data/personajeruinaflyweight.txt");
			cargarFlyweightsNPCs(@"data/npcflyweight.txt");
		}

		
		public void cargarFuentes()
		{
			SpriteFont fuente = Programa.Exploradores.Instancia.Content.Load<SpriteFont>(@"fonts/generic");
			fuentes.Add("genericSpriteFont", fuente);
		}


		public void cargarColores()
		{
			colores.Add("genericColor", Color.Black);
			colores.Add("headerColor", new Color(192, 32, 0));
			colores.Add("menuColor", new Color(254, 254, 220));
		}


		public void cargarImagenes()
		{
			Imagen imagen;

			List<Tuple<String, String>> lista;
			lista = new List<Tuple<String, String>>();

			String pathBase = "images/icons/personajes/avatares/";
			lista.Add(new Tuple<String, String>("cazador1", pathBase + "cazador1"));
			lista.Add(new Tuple<String, String>("cazador2", pathBase + "cazador2"));
			lista.Add(new Tuple<String, String>("cazador3", pathBase + "cazador3"));
			lista.Add(new Tuple<String, String>("comerciante1", pathBase + "comerciante1"));
			lista.Add(new Tuple<String, String>("comerciante2", pathBase + "comerciante2"));
			lista.Add(new Tuple<String, String>("comerciante3", pathBase + "comerciante3"));
			lista.Add(new Tuple<String, String>("medico1", pathBase + "medico1"));
			lista.Add(new Tuple<String, String>("medico2", pathBase + "medico2"));
			lista.Add(new Tuple<String, String>("mercenario1", pathBase + "mercenario1"));
			lista.Add(new Tuple<String, String>("mercenario2", pathBase + "mercenario2"));
			lista.Add(new Tuple<String, String>("mercenario3", pathBase + "mercenario3"));
			lista.Add(new Tuple<String, String>("protagonista", pathBase + "protagonista"));

			foreach(Tuple<String, String> tupla in lista)
			{
				imagen = new Imagen();
				imagen.path = tupla.Item2;
				imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
				imagenes.Add(tupla.Item1, imagen);
				
				imagen = new Imagen();
				imagen.path = tupla.Item2 + "b";
				imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
				imagenes.Add(tupla.Item1 + "b", imagen);
			}

			imagen = new Imagen();
			imagen.path = "images/sprites/other/fracaso";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("fracaso", imagen);

			imagen = new Imagen();
			imagen.path = "images/icons/items/genericitem16";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("iconoAusenteArticulo", imagen);

			imagen = new Imagen();
			imagen.path = "images/icons/other/ok";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("ok", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/other/okdeshabilitado";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("okDeshabilitado", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/other/cerrar";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("cancel", imagen);

			imagen = new Imagen();
			imagen.path = "images/icons/other/reloj";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("reloj", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/other/vacio";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("vacio", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/error/error16";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("errorA", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/error/error64";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("errorB", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/error/error256";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("errorC", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/misiones";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("misiones", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/personajes";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("personajes", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/inventario";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("inventario", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/viajar";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("viajar", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/info";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("info", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/mapa";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("mapa", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/ciudad";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("ciudad", imagen);
			
			imagen = new Imagen();
			imagen.path = "images/icons/buttons/ruina";
			imagen.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@imagen.path);
			imagenes.Add("ruina", imagen);
			
		}


		public void cargarFlyweightsBordes(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				borderFlyweights.loadAll(reader, ILSXNA.BorderFlyweight.cargarObjeto);
			}
		}


		public void cargarBordes(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				borders.loadAll(reader, ILSXNA.Border.cargarObjeto);
			}
		}


		public void cargarFlyweightsBotones(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				buttonFlyweights.loadAll(reader, ILSXNA.ButtonFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsListasView(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				listaViewFlyweights.loadAll(reader, Programa.ListaViewFlyweight.cargarObjeto);
			}
		}


		public void cargarEstilosHabitaciones(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				estilosHabitacion.loadAll(reader, ILSXNA.Border.cargarObjeto);
			}
		}


		protected void cargarAtributos()
		{
			Personajes.Vida vida;
			Personajes.Fuerza fuerza;
			Personajes.Hambre hambre;

			vida = new Personajes.Vida("idVida", "Vida");
			vida.valorMin = 1;
			vida.valorMax = 1000;
			vida.valor = 10;
			vida.descripcion = "La vida es un atributo importante que indica cuanto dano puedes recibir hasta morir.";
			vida.icono = new Imagen();
			vida.icono.path = "images/icons/personajes/vida";
			vida.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@vida.icono.path);
			atributos.Add(vida.id, vida);

			fuerza = new Personajes.Fuerza("idFuerza", "Fuerza");
			fuerza.valorMin = 0;
			fuerza.valorMax = 500;
			fuerza.valor = 100;
			fuerza.descripcion = "La fuerza te permite llevar mas peso en tu inventario a lo largo de tus viajes.";
			fuerza.icono = new Imagen();
			fuerza.icono.path = "images/icons/personajes/fuerza";
			fuerza.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(fuerza.icono.path);
			atributos.Add(fuerza.id, fuerza);

			hambre = new Personajes.Hambre("idHambre", "Hambre");
			hambre.valorMin = 0;
			hambre.valorMax = 120;
			hambre.valor = 0;
			hambre.descripcion = "Si pasa demasiado tiempo sin comer, puedes morir. El valor maximo indica el numero de horas que puedes estar sin comer.";
			hambre.icono = new Imagen();
			hambre.icono.path = "images/icons/personajes/hambre";
			hambre.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(hambre.icono.path);
			atributos.Add(hambre.id, hambre);
		}


		protected void cargarHabilidades()
		{
			Personajes.Comerciante comerciante;
			Personajes.Mercenario mercenario;
			Personajes.Cazador cazador;
			Personajes.Medico medico;
			Personajes.Enfermedad enfermedad;

			comerciante = new Personajes.Comerciante("idComerciante", "Comerciante");
			comerciante.tasaBaseCompras = 0.99f;
			comerciante.tasaBaseVentas = 1.01f;
			comerciante.descripcion = "Los comerciantes son capaces de obtener mejores precios cuando compran y venden articulos.";
			comerciante.icono = new Imagen();
			comerciante.icono.path = "images/icons/personajes/comerciante";
			comerciante.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(comerciante.icono.path);
			habilidades.Add(comerciante.id, comerciante);

			mercenario = new Personajes.Mercenario("idMercenario", "Mercenario");
			mercenario.precioBaseReclutamiento = 50;
			mercenario.defensaBase = 2;
			mercenario.descripcion = "Los mercenarios son capaces de protejerte en tus viajes.";
			mercenario.icono = new Imagen();
			mercenario.icono.path = "images/icons/personajes/mercenario";
			mercenario.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(mercenario.icono.path);
			habilidades.Add(mercenario.id, mercenario);

			cazador = new Personajes.Cazador("idCazador", "Cazador");
			cazador.habilidadBase = 0.02f;
			cazador.descripcion = "La caza es importante para obtener comida fuera de las ciudades.";
			cazador.icono = new Imagen();
			cazador.icono.path = "images/icons/personajes/cazador";
			cazador.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(cazador.icono.path);
			habilidades.Add(cazador.id, cazador);

			medico = new Personajes.Medico("idMedico", "Medico");
			medico.eficaciaBase = 3;
			medico.descripcion = "Los medicos pueden curar enfermedades y heridas de tu grupo.";
			medico.icono = new Imagen();
			medico.icono.path = "images/icons/personajes/medico";
			medico.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(medico.icono.path);
			habilidades.Add(medico.id, medico);

			enfermedad = new Personajes.Enfermedad("idEnfermedad", "Enfermedad");
			enfermedad.eficaciaBase = 1.23f;
			enfermedad.descripcion = "Una enfermedad afecta de forma negativa a varias caracteristicas de tu personaje.";
			enfermedad.icono = new Imagen();
			enfermedad.icono.path = "images/icons/personajes/enfermedad";
			enfermedad.icono.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(enfermedad.icono.path);
			habilidades.Add(enfermedad.id, enfermedad);
		}


		protected void cargarMusica()
		{
			MediaPlayer.IsRepeating = true;
			Song song;
			
			song = Programa.Exploradores.Instancia.Content.Load<Song>(@"music/ciudad");
			musica.Add("ciudad", song);
			
			song = Programa.Exploradores.Instancia.Content.Load<Song>(@"music/mapa");
			musica.Add("mapa", song);
			
			song = Programa.Exploradores.Instancia.Content.Load<Song>(@"music/ruina");
			musica.Add("ruina", song);
		}


		protected void cargarFlyweightsLugares(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				lugarFlyweights.loadAll(reader, Mapa.LugarVisitableFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsCiudades(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				ciudadFlyweights.loadAll(reader, Mapa.CiudadFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsRuinas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				ruinaFlyweights.loadAll(reader, Mapa.RuinaFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsRutas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				rutaFlyweights.loadAll(reader, Mapa.RutaFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsArticulos(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				articuloFlyweights.loadAll(reader, Objetos.ArticuloFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsObjetos(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				objetoFlyweights.loadAll(reader, Ruinas.ObjetoFlyweight.cargarObjeto, Ruinas.ObjetoFlyweight.esLista);
			}
		}


		protected void cargarFlyweightsRelojes(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				relojFlyweights.loadAll(reader, Ruinas.RelojFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsPersonajeRuinas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				personajeRuinaFlyweights.loadAll(reader, Ruinas.PersonajeRuinaFlyweight.cargarObjeto);
			}
		}


		protected void cargarFlyweightsNPCs(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				npcFlyweights.loadAll(reader, Personajes.NPCFlyweight.cargarObjeto);
			}
		}
	}
}




