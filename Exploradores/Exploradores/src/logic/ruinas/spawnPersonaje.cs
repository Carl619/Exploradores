using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public class SpawnPersonaje : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; set; }
		public int prioridad { get; set; }
		public RuinaJugable ruina { get; set; }
		public Habitacion habitacion { get; set; }
		public Rectangle posicion { get; set; }


		// constructor
		public SpawnPersonaje(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			prioridad = 0;
			ruina = null;
			habitacion = null;
			posicion = new Rectangle(0, 0, PersonajeRuina.ancho, PersonajeRuina.alto);
		}


		// funciones
		public static SpawnPersonaje cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			SpawnPersonaje spawn;
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinasJugables[campos["ruina"]];
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];
			
			int prioridad = Convert.ToInt32(campos["prioridad"]);
			spawn = new SpawnPersonaje(prioridad.ToString());
			spawn.prioridad = prioridad;
			spawn.ruina = ruina;
			spawn.habitacion = habitacion;
			spawn.posicion = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										Ruinas.PersonajeRuina.ancho,
										Ruinas.PersonajeRuina.alto);
			
			return spawn;
		}


		public PersonajeRuina crearPersonajeRuina(Personajes.Personaje personaje)
		{
			PersonajeRuina personajeRuina;
			
			personajeRuina = new PersonajeRuina(personaje, habitacion);
			personajeRuina.prioridad = prioridad;
			personajeRuina.posicion = new Rectangle(posicion.X, posicion.Y, posicion.Width, posicion.Height);
			personajeRuina.imagenActual = personaje.flyweightPersonajeRuina.imagenParado;
			
			Objetos.Inventario inventario =
				new Objetos.Inventario("#" + personajeRuina.id,
										Programa.Jugador.Instancia.protagonista.inventario.flyweight,
										PersonajeRuina.espacioInventario);

			personajeRuina.inventario = inventario;
			habitacion.personajes.Add(personajeRuina);
			ruina.personajes.Add(personajeRuina);
			personajeRuina.ruina = ruina;

			return personajeRuina;
		}
	}
}




