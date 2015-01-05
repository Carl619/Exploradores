using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class CiudadViewCentro : InterfazGrafica
	{
		// variables
		public Ciudad ciudad { get; protected set; }


		// constructor
		public CiudadViewCentro(Ciudad newCiudad)
			: base()
		{
			if (newCiudad == null)
				throw new ArgumentNullException();
			
			ciudad = newCiudad;

			updateContent();
		}


		// funciones
		public void cambiarCiudad(Ciudad newCiudad)
		{
			if (newCiudad != null)
			{
				if(ciudad != newCiudad)
				{
					ciudad = newCiudad;
					updateContent();
				}
			}
		}


		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			
			double ancho, alto;
			ancho = Programa.VistaGeneral.Instancia.window.currentWidth - 372;
			alto = Programa.VistaGeneral.Instancia.window.currentHeight - 59;

			sizeSettings.fixedInnerWidth = (uint)ancho;
			sizeSettings.fixedInnerHeight = (uint)alto;

			ILSXNA.Sprite sprite = new ILSXNA.Sprite();
			sprite.innerComponent = ciudad.imagenCiudad.textura;
			sprite.displayModeWidth = ILSXNA.Sprite.DisplayMode.Stretch;
			sprite.displayModeHeight = ILSXNA.Sprite.DisplayMode.Stretch;
			sprite.sizeSettings.fixedInnerWidth = (uint)ancho;
			sprite.sizeSettings.fixedInnerHeight =
				(uint)((double)sprite.innerComponent.Height *
				(ancho / (double)sprite.innerComponent.Width));
			if(sprite.sizeSettings.fixedInnerHeight > (uint)alto)
				sprite.sizeSettings.fixedInnerHeight = (uint)alto;
			if((uint)alto > sprite.sizeSettings.fixedInnerHeight)
				sizeSettings.fixedInnerHeight = sprite.sizeSettings.fixedInnerHeight;

			addComponent(sprite);
		}
	}
}