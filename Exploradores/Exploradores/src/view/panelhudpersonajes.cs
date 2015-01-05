using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class PanelHUDPersonajes : InterfazGrafica
	{
		// variables


		// constructor
		public PanelHUDPersonajes(bool actualizarVista = true)
			: base()
		{
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorRuinas.ruinaActual == null)
				return;
			
			foreach(Ruinas.PersonajeRuina personaje in
				Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.personajes)
			{
				Ruinas.AvatarView avatarView = new Ruinas.AvatarView(personaje);
				addComponent(avatarView);
			}
		}
	}
}



