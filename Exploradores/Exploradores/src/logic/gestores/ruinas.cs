using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Gestores
{
	
	
	public class GRuinas // gestor de ruinas
	{
		// variables
		public GameTime tiempoAnteriorRuina { get; protected set; }
		public uint tiempoActual { get; protected set; } // milisegundos
		public uint tiempoAccionMinima { get; protected set; } // milisegundos
		public Ruinas.RuinaJugable ruinaActual { get; protected set; }
			// objetos
		public Dictionary<String, Ruinas.RuinaJugable> ruinasJugables { get; protected set; }
		public Dictionary<String, Ruinas.Objeto> objetos { get; protected set; }
		public Gestor<Ruinas.Habitacion> habitaciones { get; protected set; }
		public Gestor<Ruinas.Trampa> trampas { get; protected set; }
		public Gestor<Ruinas.Tesoro> tesoros { get; protected set; }
		public Gestor<Ruinas.Puerta> puertas { get; protected set; }
		public Gestor<Ruinas.Activador> activadores { get; protected set; }
		public Gestor<Ruinas.Mecanismo> mecanismos { get; protected set; }
		public Gestor<Ruinas.SpawnPersonaje> spawnPersonajes { get; protected set; }
		public Gestor<Ruinas.PersonajeRuina> personajesRuinas { get; protected set; }


		// constructor
		public GRuinas()
		{
			tiempoAnteriorRuina = null;
			tiempoActual = 0;
			tiempoAccionMinima = 50;
			ruinaActual = null;
			
			ruinasJugables = new Dictionary<String, Ruinas.RuinaJugable>();
			objetos = new Dictionary<String, Ruinas.Objeto>();
			habitaciones = new Gestor<Ruinas.Habitacion>();
			trampas = new Gestor<Ruinas.Trampa>();
			tesoros = new Gestor<Ruinas.Tesoro>();
			puertas = new Gestor<Ruinas.Puerta>();
			activadores = new Gestor<Ruinas.Activador>();
			mecanismos = new Gestor<Ruinas.Mecanismo>();
			spawnPersonajes = new Gestor<Ruinas.SpawnPersonaje>();
			personajesRuinas = new Gestor<Ruinas.PersonajeRuina>();
		}


		// funciones
		public void cargarTodo()
		{
			cargarContenidoRuinasJugables();
			cargarContenidoHabitaciones(@"data/ruinas/habitaciones.txt");
			cargarContenidoTrampasFisicas(@"data/ruinas/trampasfisicas.txt");
			cargarContenidoTrampasLanzadores(@"data/ruinas/trampaslanzadores.txt");
			cargarContenidoTesoros(@"data/ruinas/tesoros.txt");
			cargarContenidoPuertas(@"data/ruinas/puertas.txt");
			cargarContenidoPuertasSalidas(@"data/ruinas/puertassalidas.txt");
			cargarContenidoActivadores(@"data/ruinas/activadores.txt");
			cargarContenidoMecanismos(@"data/ruinas/mecanismos.txt");
			cargarContenidoSpawnPersonajes(@"data/ruinas/spawnpersonajes.txt");
			cargarContenidoPersonajesRuinas(@"data/ruinas/personajes.txt");
		}


		public void cargarRuinaActual(Mapa.Ruina ruina)
		{
			ruinaActual = ruinasJugables[ruina.id];
			
			int i = 0;
			List<Personajes.Personaje> personajes = Programa.Jugador.Instancia.getPersonajesGrupo();
			foreach(Personajes.Personaje personaje in personajes)
			{
				Ruinas.PersonajeRuina personajeRuina = spawnPersonajes[i.ToString()].crearPersonajeRuina(personaje);
				personajesRuinas.Add(personajeRuina.id, personajeRuina);
				++i;
			}
		}


		public void resetTiempoRuina(GameTime tiempo)
		{
			tiempoAnteriorRuina = tiempo;
		}


		public void avanzarTiempoRuina(int tiempoActual)
		{
			if(ruinaActual != null)
				ruinaActual.actualizarTiempo((int)((float)tiempoActual / (float)tiempoAccionMinima));
		}


		public void actualizarRuinaActual(GameTime tiempo)
		{
			int t = (int)tiempo.TotalGameTime.TotalMilliseconds;
			tiempoActual = (uint)t;
			avanzarTiempoRuina(t);
			tiempoAnteriorRuina = tiempo;
		}

		
		protected void cargarContenidoRuinasJugables()
		{
			Ruinas.RuinaJugable ruinaJugable;
			
			foreach(KeyValuePair<String, Mapa.Ruina> ruina in Partidas.Instancia.ruinas)
			{
				ruinaJugable = new Ruinas.RuinaJugable(ruina.Key);
				ruinasJugables.Add(ruinaJugable.id, ruinaJugable);
			}
		}
		

		protected void cargarContenidoHabitaciones(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				habitaciones.loadAll(reader, Ruinas.Habitacion.cargarObjeto);
			}
		}

		
		protected void cargarContenidoTrampasFisicas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				trampas.loadAll(reader, Ruinas.TrampaFisica.cargarObjeto);
			}
			/*foreach(KeyValuePair<String, Ruinas.Trampa> objeto in trampas)
				objetos.Add(objeto.Key, objeto.Value);*/
		}

		
		protected void cargarContenidoTrampasLanzadores(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				trampas.loadAll(reader, Ruinas.TrampaLanzador.cargarObjeto);
			}
			foreach(KeyValuePair<String, Ruinas.Trampa> objeto in trampas)
				objetos.Add(objeto.Key, objeto.Value);
		}


		protected void cargarContenidoTesoros(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				tesoros.loadAll(reader, Ruinas.Tesoro.cargarObjeto);
			}
			foreach (KeyValuePair<String, Ruinas.Tesoro> objeto in tesoros)
				objetos.Add(objeto.Key, objeto.Value);
		}


		protected void cargarContenidoPuertas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				puertas.loadAll(reader, Ruinas.Puerta.cargarObjeto);
			}
			/*foreach(KeyValuePair<String, Ruinas.Puerta> objeto in puertas)
				objetos.Add(objeto.Key, objeto.Value);*/
		}

		
		protected void cargarContenidoPuertasSalidas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				puertas.loadAll(reader, Ruinas.PuertaSalida.cargarObjetoSalida);
			}
			foreach (KeyValuePair<String, Ruinas.Puerta> objeto in puertas)
				objetos.Add(objeto.Key, objeto.Value);
		}


		protected void cargarContenidoActivadores(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				activadores.loadAll(reader, Ruinas.Activador.cargarObjeto);
			}
			foreach(KeyValuePair<String, Ruinas.Activador> objeto in activadores)
				objetos.Add(objeto.Key, objeto.Value);
		}


		protected void cargarContenidoMecanismos(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				mecanismos.loadAll(reader, Ruinas.Mecanismo.cargarObjeto, Ruinas.Mecanismo.esLista);
			}
		}


		protected void cargarContenidoSpawnPersonajes(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				spawnPersonajes.loadAll(reader, Ruinas.SpawnPersonaje.cargarObjeto);
			}
		}


		protected void cargarContenidoPersonajesRuinas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				personajesRuinas.loadAll(reader, Ruinas.PersonajeRuina.cargarObjeto);
			}
		}
	}
}




