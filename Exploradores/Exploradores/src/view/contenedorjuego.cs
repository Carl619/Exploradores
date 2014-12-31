using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;




namespace Programa
{
	
	
	public class ContenedorJuego : InterfazGrafica
	{
		// variables
		public PanelMenu panelMenu { get; protected set; }
		public MenuJuego menuSecundario { get; protected set; }
		public InterfazGrafica panelContenido { get; protected set; }
		public InterfazRuinaView interfazRuina { get; protected set; }
		public PanelCentral panelCentral { get; protected set; }
		public PanelLateral panelLateral { get; protected set; }
		public Interaccion.PanelComercio panelComercio { get; protected set; }


		// constructor
		public ContenedorJuego(bool actualizarVista = true)
			: base()
		{
			panelMenu = null;
			menuSecundario = null;
			panelContenido = null;
			interfazRuina = null;
			panelCentral = null;
			panelLateral = null;
			panelComercio = null;

			alternativeNames.Add("mapa", 0);
			alternativeNames.Add("ciudad", 0);
			alternativeNames.Add("ruina", 1);

			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			layout.enableLineWrap = true;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents(true);
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego != Gestores.Pantallas.EstadoJuego.Jugando)
				return;
			
			getCurrentAlternative().addLayer();
			
			panelMenu = new PanelMenu();
			addComponent(panelMenu);

			panelContenido = new InterfazGrafica();
			addComponent(panelContenido);

			panelContenido.setNumberAlternatives(2);
			panelContenido.getCurrentAlternative().addLayer();

			panelCentral = new PanelCentral();
			panelContenido.addComponent(panelCentral);

			panelLateral = new PanelLateral();
			panelContenido.addComponent(panelLateral);

			panelContenido.setCurrentLayer(1);
			panelComercio = new Interaccion.PanelComercio();
			panelContenido.addComponent(panelComercio);
			actualizarVentanaComercio();
			panelContenido.setCurrentLayer(0);
			
			panelContenido.setCurrentAlternative(1);
			interfazRuina = new InterfazRuinaView();
			panelContenido.addComponent(interfazRuina);
			panelContenido.setCurrentAlternative(0);

			setCurrentLayer(1);
			menuSecundario = new MenuJuego();
			addComponent(menuSecundario);
			actualizarMenuSecundario();
			setCurrentLayer(0);
		}


		public void cambiarAlternativa(Gestores.Pantallas.EstadoPartida estadoPartida)
		{
			String alt = "";
			if(estadoPartida == Gestores.Pantallas.EstadoPartida.Mapa)
				alt = "mapa";
			else if(estadoPartida == Gestores.Pantallas.EstadoPartida.Ciudad)
				alt = "ciudad";
			else if(estadoPartida == Gestores.Pantallas.EstadoPartida.Ruina)
				alt = "ruina";
			panelContenido.setCurrentAlternative(alternativeNames[alt]);
		}


		public void actualizarContenidoRuina()
		{
			interfazRuina.updateContent();
		}


		public void actualizarVentanaComercio()
		{
			panelContenido.setCurrentLayer(1);
			bool interaccionVisible = (Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion ==
						Gestores.Pantallas.EstadoInteraccion.Comercio);
			panelComercio.visible = interaccionVisible;
			panelContenido.getCurrentAlternative().getCurrentLayer().blockSubsequentLayerEvents = interaccionVisible;
			panelComercio.requestUpdateContent();
			panelContenido.setCurrentLayer(0);
		}


		public void actualizarMenuSecundario()
		{
			setCurrentLayer(1);
			bool menuVisible = (Gestores.Partidas.Instancia.gestorPantallas.estadoMenu !=
						Gestores.Pantallas.EstadoMenu.Invisible);
			menuSecundario.visible = menuVisible;
			getCurrentAlternative().getCurrentLayer().blockSubsequentLayerEvents = menuVisible;
			setCurrentLayer(0);
		}
	}
}




