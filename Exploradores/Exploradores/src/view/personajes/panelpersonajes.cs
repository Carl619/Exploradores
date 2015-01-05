using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class PanelPersonajes : InterfazGrafica
	{
		// constructor
		public PanelPersonajes(bool actualizarVista = true)
			: base()
		{
			if(actualizarVista == true)
				updateContent();
			
			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.equalCellHeight = false;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD != Gestores.Pantallas.EstadoHUD.Personajes)
				return;
			
			List<Personajes.Personaje> personajes = new List<Personaje>();
			personajes.Add(Programa.Jugador.Instancia.protagonista);
			foreach(KeyValuePair<String, Personajes.Acompanante> acompanante in
					Programa.Jugador.Instancia.acompanantes)
				personajes.Add(acompanante.Value);
			Programa.ListaViewFlyweight flyweight = Gestores.Mundo.Instancia.listaViewFlyweights["list1"];

			Personajes.ListaPersonajeView personajesView =
				new Personajes.ListaPersonajeView(personajes, flyweight);
			personajesView.updateContent();
			addComponent(personajesView);

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Programa.Controller.funcionCerrarPersonajes;
			addComponent(boton);
		}
	}
}



