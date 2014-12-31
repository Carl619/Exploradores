using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class CiudadView : InterfazGrafica
	{
		// variables
		public Ciudad ciudad { get; protected set; }
		public CiudadViewLateral panelNavegacion { get; protected set; }
		public Personajes.NPC npcSeleccionado { get; protected set; }


		// constructor
		public CiudadView(Ciudad newCiudad)
			: base()
		{
			if (newCiudad == null)
				throw new ArgumentNullException();
			
			ciudad = newCiudad;
			panelNavegacion = null;
			npcSeleccionado = null;

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
					npcSeleccionado = null;
					updateContent();
				}
			}
		}


		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			ILSXNA.Container ciudadIzquierda = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			addComponent(ciudadIzquierda);

			ILSXNA.Sprite sprite = new ILSXNA.Sprite();
			sprite.innerComponent = ciudad.imagenCiudad.textura;
			ciudadIzquierda.addComponent(sprite);
		}
	}
}