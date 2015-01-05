using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Objetos
{
	

	public class PanelInventarioRuina : InterfazGrafica
	{
		// constructor
		public PanelInventarioRuina(bool actualizarVista = true)
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
			
			Ruinas.Tesoro tesoro = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.tesoroAbierto;
			if(tesoro == null)
				return;
			Objetos.Inventario inventarioTesoro = tesoro.inventario;
			Objetos.Inventario inventarioPersonaje = tesoro.personaje.inventario;
			
			Objetos.InventarioView inventarioPersonajeView = inventarioPersonaje.crearVista();
			inventarioPersonajeView.textoTitulo = "Inventario:";
			inventarioPersonajeView.updateContent();
			addComponent(inventarioPersonajeView);
			
			Objetos.InventarioView inventarioTesoroView = inventarioTesoro.crearVista();
			inventarioTesoroView.textoTitulo = "Tesoro:";
			inventarioTesoroView.updateContent();
			addComponent(inventarioTesoroView);

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Ruinas.Controller.cerrarTesoro;
			addComponent(boton);
		}
	}
}



