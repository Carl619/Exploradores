using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Personajes;




namespace Ruinas
{
	
	
	public class ProyectilView : ObjetoView
	{
		// variables
		public Proyectil proyectil { get { return (Proyectil)objeto; } }


		// constructor
		public ProyectilView(Trampa newTrampa, RuinaJugable ruina)
			: base(newTrampa, ruina)
		{
			requestedContentUpdate = true;

			onMouseOver = null;
			onMouseOut = null;
		}


		// funciones
		public override void updateContent()
		{
			actualizarTexturas();

			base.updateContent();
			requestedContentUpdate = true;

			if(parent != null)
			{
				parent.offsetX = proyectil.espacio.X;
				parent.offsetY = proyectil.espacio.Y;
			}
		}


		public override void actualizarTexturas()
		{
			texturas.Clear();

			addTextura(objeto.objetoFlyweight.iconosMovimiento[objeto.imagenActual]);
		}
	}
}




