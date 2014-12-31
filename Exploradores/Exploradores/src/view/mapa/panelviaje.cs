using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class PanelViaje : InterfazGrafica
	{
		// constructor
		public PanelViaje(bool actualizarVista = true)
			: base()
		{
			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
		}


		public void setViaje(List<Dijkstra.IRama> camino)
		{
			clearComponents();
			requestedContentUpdate = false;

			Gestores.Partidas.Instancia.gestorPantallas.estadoHUD = Gestores.Pantallas.EstadoHUD.Vacio;

			if(camino == null)
				return;
			
			Gestores.Partidas.Instancia.gestorPantallas.estadoHUD = Gestores.Pantallas.EstadoHUD.Viaje;

			ILSXNA.Container ventanaViaje = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			ventanaViaje.contentSpacingX = 4;
			ventanaViaje.contentSpacingY = 4;
			ventanaViaje.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			addComponent(ventanaViaje);

			ILSXNA.Label label;
			float distanciaTotal, peligroTotal;
			distanciaTotal = peligroTotal = 0.0f;

			foreach(Dijkstra.IRama rama in camino)
			{
				distanciaTotal += ((Ruta)rama).distancia;
				peligroTotal += ((Ruta)rama).peligro;
			}
			Ruta ruta = (Ruta)camino[0];
			LugarVisitable lugar;
			lugar = (LugarVisitable)(ruta.verticeAdyacente(Programa.Jugador.Instancia.protagonista.lugarActual));

			label = new ILSXNA.Label();
			label.message = "Distancia hasta " + lugar.nombre +  ": " + ruta.distancia.ToString();
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Peligro hasta " + lugar.nombre +  ": " + ruta.peligro.ToString();
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Distancia total: " + distanciaTotal.ToString();
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Peligro total: " + peligroTotal.ToString();
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			ventanaViaje.addComponent(label);

			ILSXNA.Container botones = new ILSXNA.Container();
			botones.layout.equalCellWidth = true;
			botones.layout.equalCellHeight = true;
			ventanaViaje.addComponent(botones);
			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Controller.cerrarViaje;
			botones.addComponent(boton);

			boton = new ILSXNA.Button("Viajar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.sizeSettings.outterSpacingX = 4;
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["ok"].textura);
			boton.updateContent();
			boton.onButtonPress = Controller.viajar;
			botones.addComponent(boton);
		}
	}
}



