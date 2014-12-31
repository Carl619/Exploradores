using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class PanelEdificio : InterfazGrafica
	{
		// constructor
		public PanelEdificio(bool actualizarVista = true)
			: base()
		{
			if(actualizarVista == true)
				updateContent();
			
			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD != Gestores.Pantallas.EstadoHUD.Edificio)
				return;
			
			List<Personajes.NPC> npcs = ((Ciudad)(Programa.Jugador.Instancia.protagonista.lugarActual)).listaNPC;
			String edificio = Gestores.Partidas.Instancia.edificioSeleccionado.edificioEncuentro;
			Programa.ListaViewFlyweight flyweight = Gestores.Mundo.Instancia.listaViewFlyweights["list1"];

			Personajes.ListaNPCView npcsView = new Personajes.ListaNPCView(npcs, edificio, flyweight);
			npcsView.updateContent();
			addComponent(npcsView);

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Controller.cerrarEdificio;
			addComponent(boton);
		}
	}
}



