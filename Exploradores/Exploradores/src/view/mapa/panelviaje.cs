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


		public void setViaje(Viaje viaje)
		{
			clearComponents();
			requestedContentUpdate = false;

			Gestores.Partidas.Instancia.gestorPantallas.estadoHUD = Gestores.Pantallas.EstadoHUD.Vacio;

			if(viaje.camino == null)
				return;
			
			Gestores.Partidas.Instancia.gestorPantallas.estadoHUD = Gestores.Pantallas.EstadoHUD.Viaje;

			ILSXNA.Container ventanaViaje = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			ventanaViaje.contentSpacingX = 4;
			ventanaViaje.contentSpacingY = 4;
			ventanaViaje.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			addComponent(ventanaViaje);

			viaje.actualizar();
			actualizarLabels(ventanaViaje, viaje);
			actualizarBotones(ventanaViaje);
		}


		protected void actualizarLabels(ILSXNA.Container ventanaViaje, Viaje viaje)
		{
			SpriteFont spriteFont = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Factor sobrepeso inventario: " + viaje.factorSobrepesoInventario.ToString("F4");
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Distancia total: " + viaje.distanciaTotal.ToString("F1");
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			if(viaje.destinoSiguiente.oculto == false)
			{
				label.message = "Distancia hasta " +
								viaje.destinoSiguiente.nombre +  ": " +
								viaje.distanciaSiguiente.ToString("F1");
			}
			else
			{
				label.message = "Distancia hasta el lugar desconocido: " +
								viaje.distanciaSiguiente.ToString("F1");
			}
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = " ";
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Defensa: " + viaje.defensa.ToString("F1");
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Peligro: " + viaje.peligro.ToString("F1");
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Probabilidad de ser atacado: " + viaje.probAtaque.ToString("F2") + "%";
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = " ";
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Comida disponible: " + viaje.comidaDisponible.ToString();
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Caza probable: " + viaje.caza.ToString("F1");
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Comida necesaria: " + viaje.comidaNecesaria.ToString("F1");
			label.innerComponent = spriteFont;
			ventanaViaje.addComponent(label);
		}


		protected void actualizarBotones(ILSXNA.Container ventanaViaje)
		{
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
			boton.onButtonPress = Controller.intentarViajar;
			botones.addComponent(boton);
		}
	}
}



