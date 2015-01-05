using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Personajes;




namespace Ruinas
{
	
	
	public class TrampaLanzadorView : TrampaView
	{
		// variables
		public TrampaLanzador trampaLanzador { get { return (TrampaLanzador)objeto; } }


		// constructor
		public TrampaLanzadorView(TrampaLanzador newTrampa, RuinaJugable ruina)
			: base(newTrampa, ruina)
		{
		}


		// funciones
		public override void actualizarTexturas()
		{
			texturas.Clear();

			addTextura(objeto.objetoFlyweight.iconosParado[trampa.imagenActual]);
		}
	}
}




