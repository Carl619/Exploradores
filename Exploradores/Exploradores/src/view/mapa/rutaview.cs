using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class RutaView : ILSXNA.Line
	{
		// variables
		public Ruta ruta { get; set; }
		public bool seleccionada { get; set; }


		// constructor
		public RutaView(Ruta newRuta, bool newSeleccionada)
			: base()
		{
			if(newRuta == null)
				throw new ArgumentNullException();
			
			ruta = newRuta;
			seleccionada = newSeleccionada;
			actualizarRutaView();
		}


		// funciones
		public void actualizarRutaView()
		{
			coordinates = new Tuple<Tuple<int, int>, Tuple<int, int>>
							(ruta.extremos.Item1.coordenadas,
							ruta.extremos.Item2.coordenadas);
			
			innerComponent = ruta.flyweight.sprite;

			if(seleccionada == false)
			{
				width = ruta.flyweight.anchuraPasivo;
				lineColor = ruta.flyweight.colorPasivo;
			}
			else
			{
				width = ruta.flyweight.anchuraActivo;
				lineColor = ruta.flyweight.colorActivo;
			}
		}
	}
}




