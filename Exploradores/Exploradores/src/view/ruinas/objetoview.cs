using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class ObjetoView : ILSXNA.MultiSprite
	{
		// variables
		public Objeto objeto { get; set; }
		public Rectangle boundingBox { get { return objeto.espacio;  } }


		// constructor
		public ObjetoView(Objeto newobjeto, RuinaJugable ruina)
			: base()
		{
			if (newobjeto == null)
				throw new ArgumentNullException();
			
			objeto = newobjeto;
			actualizarTexturas();
			onLeftMousePress = null;
			onLeftMouseRelease = null;
		}


		// funciones
		public override void updateContent()
		{
			base.updateContent();
			requestedContentUpdate = false;

			if(objeto.activado == true)
				select();
			else
				deselect();
		}


		public virtual void actualizarTexturas()
		{
			texturas.Clear();

			addTextura(objeto.objetoFlyweight.iconosParado[objeto.imagenActual]);
			addTextura(objeto.objetoFlyweight.iconosParadoSeleccionado[objeto.imagenActual]);
			addTextura(objeto.objetoFlyweight.iconosActivado[objeto.imagenActual]);
			addTextura(objeto.objetoFlyweight.iconosActivadoSeleccionado[objeto.imagenActual]);

			if(objeto.activado == true)
				select();
			else
				deselect();
		}
	}
}




