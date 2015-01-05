using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Objetos;



namespace Ruinas
{
	
	
	public class Tesoro : Objeto
	{
		// variables
		public Inventario inventario { get; set; }
		public PersonajeRuina personaje { get; set; } // el que abre el tesoro
		public bool visto { get; set; }


		// cosntructor
        public Tesoro(String newID, ObjetoFlyweight newObjetoFlyweight)
			: base(newID, newObjetoFlyweight)
		{
			inventario = null;
			personaje = null;
			visto = false;
		}


		// funciones
		public static Tesoro cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Tesoro tesoro;
			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];
			
			tesoro = new Tesoro(campos["id"], flyweight);
			tesoro.espacio = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										flyweight.ancho,
										flyweight.alto);
			
			tesoro.tiempoActivacion = Convert.ToInt32(campos["tiempoActivacion"]);
			tesoro.inventario = Gestores.Partidas.Instancia.inventarios[campos["inventario"]];
			tesoro.habitacion = habitacion;
			habitacion.objetos.Add(tesoro);

			return tesoro;
		}


		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new TesoroView(this, ruina);
			return vista;
		}
	}
}
