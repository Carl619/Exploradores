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

			ILSXNA.Sprite sprite = new ILSXNA.Sprite();
			sprite.innerComponent = ciudad.imagenCiudad.textura;

			addComponent(sprite);
		}
	}
}