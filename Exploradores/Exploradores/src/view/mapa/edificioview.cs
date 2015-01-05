using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class EdificioView : Programa.ListaView.Elemento
	{
		// variables
		public Personajes.NPCFlyweight edificio { get; set; }


		// constructor
		public EdificioView(Personajes.NPCFlyweight flyweight, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if (flyweight == null)
				throw new ArgumentNullException();
			
			edificio = flyweight;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			ILSXNA.Button boton = new ILSXNA.Button(edificio.edificioEncuentro, Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			if(edificio.iconoProfesion != null)
				boton.icons.Add(edificio.iconoProfesion);
			boton.updateContent();
			boton.sizeSettings.minInnerWidth = edificio.anchoEdificioView;
			boton.onLeftMousePress = null;
			addComponent(boton);
		}
	}
}




