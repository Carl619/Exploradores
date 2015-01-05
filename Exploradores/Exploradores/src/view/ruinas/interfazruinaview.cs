using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class InterfazRuinaView : InterfazGrafica
	{
		// variables
		public Ruinas.RuinaJugableView ruina  { get; protected set; }
		public PanelHUDPersonajes panelHudPersonajes { get; protected set; }
		public InterfazGrafica contenedorPrincipal { get; protected set; }
		public Objetos.PanelInventarioRuina panelInventario { get; protected set; }


		// constructor
		public InterfazRuinaView(bool actualizarVista = true)
			: base()
		{
			ruina = null;
			panelHudPersonajes = null;
			contenedorPrincipal = null;
			panelInventario = null;
			
			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void requestUpdateContent()
		{
			base.requestUpdateContent();
		}


		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoPartida != Gestores.Pantallas.EstadoPartida.Ruina)
				return;
			
			panelHudPersonajes = new PanelHUDPersonajes();
			addComponent(panelHudPersonajes);

			contenedorPrincipal = new InterfazGrafica();
			contenedorPrincipal.getCurrentAlternative().addLayer();
			addComponent(contenedorPrincipal);

			ILSXNA.Container contenedor2 = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			contenedor2.sizeSettings.fixedInnerWidth = Programa.VistaGeneral.Instancia.window.currentWidth - 84;
			contenedor2.sizeSettings.fixedInnerHeight = Programa.VistaGeneral.Instancia.window.currentHeight - 59;
			contenedorPrincipal.addComponent(contenedor2);
			Programa.VistaGeneral.Instancia.scrollableContainer = contenedor2;

			ILSXNA.Container contenedor3 = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["ruin1"]);
			contenedor3.contentSpacingX = 64;
			contenedor3.contentSpacingY = 64;
			contenedor2.addComponent(contenedor3);
			
			Ruinas.RuinaJugable ruinaJugable = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual;

			ruina = ruinaJugable.crearVista();
			contenedor3.addComponent(ruina);

			contenedorPrincipal.setCurrentLayer(1);
			panelInventario = new Objetos.PanelInventarioRuina();
			bool tesoroVisible = (Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.tesoroAbierto != null);
			panelInventario.visible = tesoroVisible;
			contenedorPrincipal.getCurrentAlternative().getCurrentLayer().blockSubsequentLayerEvents = tesoroVisible;
			contenedorPrincipal.addComponent(panelInventario);
			contenedorPrincipal.setCurrentLayer(0);
		}


		public void abrirInventario(bool showInventario)
		{
			contenedorPrincipal.setCurrentLayer(1);
			panelInventario.visible = showInventario;
			contenedorPrincipal.getCurrentAlternative().getCurrentLayer().blockSubsequentLayerEvents = showInventario;
			panelInventario.requestUpdateContent();
			contenedorPrincipal.setCurrentLayer(0);
		}
	}
}



