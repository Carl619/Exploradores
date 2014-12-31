using System;
using System.Collections.Generic;




namespace Programa
{
	
	
	public class Jugador
	{
		// variables
		private static Jugador instancia = null;
		public static Jugador Instancia
		{
			get
			{
				if(instancia == null)
					new Jugador();
				return instancia;
			}
		}
		public String nombre { get; protected set; }
		public List<Personajes.Acompanante> grupoAcompanantes { get; protected set; }
		public Personajes.Protagonista protagonista { get; protected set; }
		public Personajes.Personaje personajeSeleccionado { get; set; }


		// constructor
		private Jugador()
		{
			instancia = this;
			nombre = "";
			grupoAcompanantes = new List<Personajes.Acompanante>();
			protagonista = null;
			personajeSeleccionado = null;
		}


		// funciones
		public void crearProtagonista(String nombreJugador, Mapa.LugarVisitable posicionProtagonista)
		{
			nombre = (String)nombreJugador.Clone();
			protagonista = new Personajes.Protagonista(
								Personajes.Protagonista.idProtagonista,
								nombreJugador, posicionProtagonista);
			protagonista.inventario = Gestores.Partidas.Instancia.inventarios["1"];
			personajeSeleccionado = protagonista;
		}
	}
}




