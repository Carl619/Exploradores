using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class PanelCentral : InterfazGrafica
	{
		// variables
		public PanelHUD panelHUD { get; protected set; }
		public PanelFondo panelFondo { get; protected set; }


		// constructor
		public PanelCentral(bool actualizarVista = true)
			: base()
		{
			panelHUD = null;
			panelFondo = null;
			
			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			ILSXNA.Container principal = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			//principal.layout.verticalAlignment = ILS.Layout.Alignment.RightOrBottomAlignment;
			addComponent(principal);

			principal.getCurrentAlternative().addLayer();
			principal.setCurrentLayer(0);
				panelFondo = new PanelFondo();
				principal.addComponent(panelFondo);
			principal.setCurrentLayer(1);
				panelHUD = new PanelHUD();
				principal.addComponent(panelHUD);
			principal.setCurrentLayer(0);

		}
	}
}



