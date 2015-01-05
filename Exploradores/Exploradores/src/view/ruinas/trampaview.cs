using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Personajes;




namespace Ruinas
{
	
	
	public class TrampaView : ObjetoView
	{
		// variables
		public Trampa trampa { get { return (Trampa)objeto; } }


		// constructor
		public TrampaView(Trampa newTrampa, RuinaJugable ruina)
			: base(newTrampa, ruina)
		{
			requestedContentUpdate = objeto.activado;

			onMouseOver = null;
			onMouseOut = null;
		}


		// funciones
		public override void updateContent()
		{
			base.updateContent();
			requestedContentUpdate = objeto.activado;
		}


		public override void actualizarTexturas()
		{
			texturas.Clear();

			addTextura(objeto.objetoFlyweight.iconosParado[trampa.imagenActual]);
			addTextura(objeto.objetoFlyweight.iconosParado[trampa.imagenActual]);
			addTextura(objeto.objetoFlyweight.iconosActivado[trampa.imagenActual]);
			addTextura(objeto.objetoFlyweight.iconosActivado[trampa.imagenActual]);

			if(objeto.activado == true)
				select();
			else
				deselect();
		}
	}
}




