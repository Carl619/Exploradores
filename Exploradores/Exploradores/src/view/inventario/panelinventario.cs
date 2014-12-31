using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Objetos
{
	

	public class PanelInventario : InterfazGrafica
	{
		// constructor
		public PanelInventario(bool actualizarVista = true)
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

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD != Gestores.Pantallas.EstadoHUD.Inventario)
				return;
			
			Objetos.Inventario inventario = Programa.Jugador.Instancia.protagonista.inventario;
			
			Objetos.InventarioView inventarioView = inventario.crearVista();
			inventarioView.textoTitulo = "Inventario:";
			inventarioView.updateContent();
			addComponent(inventarioView);

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Programa.Controller.funcionCerrarInventario;
			addComponent(boton);
		}
	}
}



