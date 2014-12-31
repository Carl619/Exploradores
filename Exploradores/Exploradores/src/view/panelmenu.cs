using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class PanelMenu : InterfazGrafica
	{
		// constructor
		public PanelMenu(bool actualizarVista = true)
			: base()
		{
			layout.equalCellWidth = true;
			layout.equalCellHeight = true;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Menu", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.onButtonPress = Controller.funcionAbrirMenuSecundario;
			addComponent(boton);

			boton = new ILSXNA.Button("Pausar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			addComponent(boton);

			boton = new ILSXNA.Button("Ayuda", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			addComponent(boton);
		}
	}
}



