using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Interaccion;
using Objetos;
using Personajes;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class Trampa : Objeto
	{
		// variables
		public int dano { get; set; }


		// cosntructor
        public Trampa(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			dano = 0;
		}


		// funciones
		public static Trampa cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Trampa trampa;
			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];
			
			trampa = new Trampa(campos["id"], flyweight);
			trampa.activado = Convert.ToBoolean(campos["activado"]);
			trampa.espacio = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										flyweight.iconoPasivo.Width,
										flyweight.iconoPasivo.Height);

			trampa.dano = Convert.ToInt32(campos["dano"]);
			habitacion.objetos.Add(trampa);

			return trampa;
		}


		
		/*
		public void hacerDano(Personaje personaje)
		{
			if (personaje.coordX >= coords.X && personaje.coordX <= coords.X + ancho &&
				personaje.coordY >= coords.Y && personaje.coordY <= coords.Y + alto)
			{
				foreach (Atributo a in personaje.atributos)
				{
					if (a.nombre.Equals("vida"))
					{
                        a.valor = a.valor - trampaflyweight.dano;
						break;
					}
				}
			}
		}*/
	}
}




