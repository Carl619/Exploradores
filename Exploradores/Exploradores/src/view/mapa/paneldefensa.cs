using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class PanelDefensa : InterfazGrafica
	{
		// variables
		public uint anchoTexto { get; set; }


		// constructor
		public PanelDefensa(bool actualizarVista = true)
			: base()
		{
			anchoTexto = 512;

			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion !=
				Gestores.Pantallas.EstadoInteraccion.Defensa)
				return;
			
			
			Interaccion.Ataque ataque = Programa.Jugador.Instancia.protagonista.ataque;
			uint dineroDisponible = Programa.Jugador.Instancia.protagonista.dineroDisponible();
			uint comidaDisponible = Programa.Jugador.Instancia.protagonista.viaje.comidaDisponible;


			actualizarTexto();

			
			ILSXNA.Container botones = new ILSXNA.Container();
			botones.layout.equalCellWidth = true;
			botones.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			addComponent(botones);

			ILSXNA.Button boton;

			if(ataque.atacantes != Interaccion.Ataque.Atacantes.Animales)
			{
				if(dineroDisponible >= ataque.precioTotal)
				{
					boton = new ILSXNA.Button("Pagar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
					boton.onButtonPress = Controller.viajePagar;
					botones.addComponent(boton);
				}
			}
			else
			{
				if(comidaDisponible >= ataque.precioTotal)
				{
					boton = new ILSXNA.Button("Dar comida", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
					boton.onButtonPress = Controller.viajePagar;
					botones.addComponent(boton);
				}
			}

			boton = new ILSXNA.Button("Defender", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.onButtonPress = Controller.viajeDefender;
			botones.addComponent(boton);

			boton = new ILSXNA.Button("Huir", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.onButtonPress = Controller.viajeHuir;
			botones.addComponent(boton);
		}


		public void actualizarTexto()
		{
			Interaccion.Ataque ataque = Programa.Jugador.Instancia.protagonista.ataque;
			SpriteFont spriteFont = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			ILSXNA.Label label;
			ILSXNA.Paragraph par;
			uint dineroDisponible = Programa.Jugador.Instancia.protagonista.dineroDisponible();
			uint comidaDisponible = Programa.Jugador.Instancia.protagonista.viaje.comidaDisponible;

			String mensaje1;
			if(ataque.atacantes == Interaccion.Ataque.Atacantes.Ladrones)
				mensaje1 = "Mientras estabas viajando, un grupo de ladrones han decidido robarte.";
			else if(ataque.atacantes == Interaccion.Ataque.Atacantes.Asesinos)
				mensaje1 = "Alguien que te odia mucho ha pagado a unos asesinos para que te maten.";
			else
				mensaje1 = "Te ha encontrado con un grupo de lobos que quieren comer.";
			mensaje1 = mensaje1 + " El numero total es alrededor de " + ataque.numero + ".";
			
			label = new ILSXNA.Label();
			label.message = mensaje1;
			label.innerComponent = spriteFont;
			par = new ILSXNA.Paragraph(label, anchoTexto);
			addComponent(par);


			String mensaje2;
			if(ataque.atacantes != Interaccion.Ataque.Atacantes.Animales)
			{
				if(dineroDisponible >= ataque.precioTotal)
					mensaje2 = "Podrias ahorrarte este conflicto si pagas " +
								ataque.precioTotal + " monedas.";
				else
					mensaje2 = "Desafortunadamente no tienes suficiente dinero para convencerles de que te dejen viajar sin conflicto.";
			}
			else
			{
				if(comidaDisponible >= ataque.precioTotal)
					mensaje2 = "Como solo quieren comer porque tienen hambre, puedes escaparte si les das " +
								ataque.precioTotal + " comida.";
				else
					mensaje2 = "Desafortunadamente no tienes suficiente comida para los animales.";
			}
			
			label = new ILSXNA.Label();
			label.message = mensaje2;
			label.innerComponent = spriteFont;
			par = new ILSXNA.Paragraph(label, anchoTexto);
			addComponent(par);
			

			if(ataque.atacantes != Interaccion.Ataque.Atacantes.Animales &&
				dineroDisponible >= ataque.precioTotal)
			{
				label = new ILSXNA.Label();
				label.message = "Dinero disponible: " + dineroDisponible.ToString();
				label.innerComponent = spriteFont;
				addComponent(label);
			}
			else if(ataque.atacantes == Interaccion.Ataque.Atacantes.Animales &&
				comidaDisponible >= ataque.precioTotal)
			{
				label = new ILSXNA.Label();
				label.message = "Comida disponible: " + comidaDisponible.ToString();
				label.innerComponent = spriteFont;
				addComponent(label);
			}
		}
	}
}



