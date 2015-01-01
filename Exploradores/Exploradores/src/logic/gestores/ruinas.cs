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
		public uint tiempoAccionMinima { get; protected set; } // milisegundos
		public Ruinas.RuinaJugable ruinaActual { get; protected set; }
			// objetos
		public Dictionary<String, Ruinas.RuinaJugable> ruinasJugables { get; protected set; }
		public Dictionary<String, Ruinas.Objeto> objetos { get; protected set; }
		public Gestor<Ruinas.Habitacion> habitaciones { get; protected set; }
		public Gestor<Ruinas.Trampa> trampas { get; protected set; }
		public Gestor<Ruinas.Tesoro> tesoros { get; protected set; }
		public Gestor<Ruinas.Puerta> puertas { get; protected set; }
		public Gestor<Ruinas.PuertaSalida> puertaSalidas { get; protected set; }
		public Gestor<Ruinas.Activador> activadores { get; protected set; }
		public Gestor<Ruinas.Mecanismo> mecanismos { get; protected set; }
		public Gestor<Ruinas.PersonajeRuina> personajesRuinas { get; protected set; }


		// constructor
		public GRuinas()
		{
			tiempoAnteriorRuina = null;
			tiempoAccionMinima = 50;
			ruinaActual = null;
			
			ruinasJugables = new Dictionary<string,Ruinas.RuinaJugable>();
			objetos = new Dictionary<string,Ruinas.Objeto>();
			habitaciones = new Gestor<Ruinas.Habitacion>();
			trampas = new Gestor<Ruinas.Trampa>();
			tesoros = new Gestor<Ruinas.Tesoro>();
			puertas = new Gestor<Ruinas.Puerta>();
			activadores = new Gestor<Ruinas.Activador>();
			mecanismos = new Gestor<Ruinas.Mecanismo>();
			personajesRuinas = new Gestor<Ruinas.PersonajeRuina>();
			puertaSalidas = new Gestor<Ruinas.PuertaSalida>();
		}


		public void cargarTodo()
		{
			cargarContenidoRuinasJugables();
			cargarContenidoHabitaciones(@"data/ruinas/habitaciones.txt");
			cargarContenidoTrampas(@"data/ruinas/trampas.txt");
			cargarContenidoTesoros(@"data/ruinas/tesoros.txt");
			cargarContenidoPuertas(@"data/ruinas/puertas.txt");
			cargarContenidoPuertasSalidas(@"data/ruinas/puertassalidas.txt");
			cargarContenidoActivadores(@"data/ruinas/activadores.txt");
			cargarContenidoMecanismos(@"data/ruinas/mecanismos.txt");
			cargarContenidoPersonajesRuinas(@"data/ruinas/personajes.txt");
		}


		public void cargarRuinaActual(Mapa.Ruina ruina)
		{
			ruinaActual = ruinasJugables[ruina.id];
		}


		public void resetTiempoRuina(GameTime tiempo)
		{
			tiempoAnteriorRuina = tiempo;
		}


		public void avanzarTiempoRuina(int tiempoActual)
		{
			if(ruinaActual != null)
				ruinaActual.actualizarTiempo(tiempoActual);
		}


		public void actualizarRuinaActual(GameTime tiempo)
		{
			int t = (int)tiempo.TotalGameTime.TotalMilliseconds;
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

		
		protected void cargarContenidoTrampas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				trampas.loadAll(reader, Ruinas.Trampa.cargarObjeto);
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
			foreach(KeyValuePair<String, Ruinas.Puerta> objeto in puertas)
				objetos.Add(objeto.Key, objeto.Value);
		}

		protected void cargarContenidoPuertasSalidas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				puertaSalidas.loadAll(reader, Ruinas.PuertaSalida.cargarObjetoSalida);
			}
			foreach (KeyValuePair<String, Ruinas.PuertaSalida> objeto in puertaSalidas)
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


		protected void cargarContenidoPersonajesRuinas(String fileName)
		{
			using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
			{
				personajesRuinas.loadAll(reader, Ruinas.PersonajeRuina.cargarObjeto);
			}
		}
	}
}




