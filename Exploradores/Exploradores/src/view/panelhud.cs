using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class PanelHUD : InterfazGrafica
	{
		// variables
		public InterfazGrafica panelMenu { get; protected set; }
		public PanelHUDVentanas panelVentanas { get; protected set; }
		public ILSXNA.Label labelTiempo { get; protected set; }


		// constructor
		public PanelHUD(bool actualizarVista = true)
			: base()
		{
			panelMenu = null;
			panelVentanas = null;
			labelTiempo = null;

			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			//layout.verticalFlow = ILS.Layout.Flow.LeftOrTopFlow;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			if(parent != null)
				parent.blockSubsequentLayerEvents = false;
			
			ILSXNA.Container menu = new ILSXNA.Container();
			addComponent(menu);

			panelMenu = new InterfazGrafica();
			panelMenu.layout.equalCellWidth = true;
			panelMenu.layout.equalCellHeight = true;
			menu.addComponent(panelMenu);

			labelTiempo = new ILSXNA.Label();
			labelTiempo.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			labelTiempo.color = Gestores.Mundo.Instancia.colores["menuColor"];
			menu.addComponent(labelTiempo);

			actualizarMenu();
			actualizarTiempo();

			panelVentanas = new PanelHUDVentanas();
			addComponent(panelVentanas);

			actualizarBloqueoVentana();
		}


		public void actualizarMenu()
		{
			panelMenu.clearComponents();

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Misiones", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["misiones"].textura);
			boton.updateContent();
			boton.onButtonPress = Controller.funcionAbrirMisiones;
			panelMenu.addComponent(boton);

			boton = new ILSXNA.Button("Personajes", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["personajes"].textura);
			boton.updateContent();
			boton.onButtonPress = Controller.funcionAbrirPersonajes;
			panelMenu.addComponent(boton);

			boton = new ILSXNA.Button("Inventario", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["inventario"].textura);
			boton.updateContent();
			boton.onButtonPress = Controller.funcionAbrirInventario;
			panelMenu.addComponent(boton);
		}


		public void actualizarTiempo()
		{
			labelTiempo.message = "Dia: " + Gestores.Partidas.Instancia.dias.ToString() +
							", " + Gestores.Partidas.Instancia.horas.ToString() + ":00 h";
			labelTiempo.requestUpdateContent();
		}


		public void actualizarBloqueoVentana()
		{
			if(parent != null)
			{
				parent.blockSubsequentLayerEvents =
					! (Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
					Gestores.Pantallas.EstadoHUD.Vacio);
			}
		}
	}
}



