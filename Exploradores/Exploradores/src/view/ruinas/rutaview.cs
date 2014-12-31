using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	

	public class RutaView : ILSXNA.Line
	{
		// variables
		public RuinaRama ruta { get; set; }
		public bool seleccionada { get; set; }


		// constructor
		public RutaView(RuinaRama newRuta, bool newSeleccionada)
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
			Mapa.RutaFlyweight flyweight = Gestores.Mundo.Instancia.rutaFlyweights["road1"];
			innerComponent = flyweight.sprite;

			coordinates = new Tuple<Tuple<int, int>, Tuple<int, int>>
							(ruta.extremos.Item1.coordenadas,
							ruta.extremos.Item2.coordenadas);

			if(seleccionada == false)
			{
				width = flyweight.anchuraPasivo;
				lineColor = flyweight.colorPasivo;
			}
			else
			{
				width = flyweight.anchuraActivo;
				lineColor = flyweight.colorActivo;
			}
		}
	}
}




