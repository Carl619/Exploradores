using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class MenuJuego : InterfazGrafica
	{
		// variables
		public bool pantallaInicial { get; set; }


		// constructor
		public MenuJuego(bool actualizarVista = true)
			: base()
		{
			pantallaInicial = true;

			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			layout.equalCellWidth = true;
			layout.equalCellHeight = true;

			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			pantallaInicial = (Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.MenuPrincipal);
			ILSXNA.Button boton;
			
			if(pantallaInicial == false)
			{
				boton = new ILSXNA.Button("Volver al juego", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
				boton.onButtonPress = Controller.funcionCerrarMenuSecundario;
				addComponent(boton);
			}
			else
			{
				boton = new ILSXNA.Button("Nueva Partida", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
				boton.onButtonPress = Controller.funcionNuevaPartida;
				addComponent(boton);
			}

			boton = new ILSXNA.Button("Cargar Partida", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.onButtonPress = Controller.funcionCargarPartida;
			addComponent(boton);

			if(pantallaInicial == false)
			{
				boton = new ILSXNA.Button("Guardar Partida", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
				boton.onButtonPress = Controller.funcionGuardarPartida;
				addComponent(boton);
			}

			boton = new ILSXNA.Button("Opciones", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			addComponent(boton);

			if(pantallaInicial == false)
			{
				boton = new ILSXNA.Button("Menu principal", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
				boton.onButtonPress = Controller.funcionMenuPrincipal;
				addComponent(boton);
			}

			boton = new ILSXNA.Button("Salir", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.onButtonPress = Controller.funcionSalirJuego;
			addComponent(boton);
		}
	}
}



