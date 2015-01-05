using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class TrampaFisica : Trampa
	{
		// variables
		public uint dano { get; set; }


		// constructor
        public TrampaFisica(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			dano = 0;
		}


		// funciones
		public static Trampa cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			TrampaFisica trampa;
			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];
			
			trampa = new TrampaFisica(campos["id"], flyweight);
			trampa.activado = Convert.ToBoolean(campos["activado"]);
			trampa.tiempoActivacion = Convert.ToInt32(campos["tiempoActivacion"]);
			trampa.espacio = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										flyweight.ancho,
										flyweight.alto);
			trampa.areaActivacion.espacio = new Rectangle(Convert.ToInt32(campos["area activacion x"]),
											Convert.ToInt32(campos["area activacion y"]),
											Convert.ToInt32(campos["area activacion ancho"]),
											Convert.ToInt32(campos["area activacion alto"]));
			
			trampa.dano = Convert.ToUInt32(campos["dano"]);
			trampa.habitacion = habitacion;
			habitacion.objetos.Add(trampa);
			return trampa;
		}


		public override void activar()
		{
			if(activado == true)
				return;
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego != Gestores.Pantallas.EstadoJuego.Jugando)
				return;
			foreach(Ruinas.PersonajeRuina personaje in habitacion.personajes)
			{
				if(espacio.Intersects(personaje.posicion) == true)
					hacerDano(personaje);
			}
			base.activar();
		}
		
		
		public virtual void hacerDano(Ruinas.PersonajeRuina personaje)
		{
			personaje.hacerDano(dano);
		}
	}
}




