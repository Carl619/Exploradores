using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	

	public class InterfazRelojesView : InterfazGrafica
	{
		// variables
		public Ruinas.RuinaJugable ruina  { get; protected set; }


		// constructor
		public InterfazRelojesView(Ruinas.RuinaJugable newRuina, bool actualizarVista = true)
			: base()
		{
			if(newRuina == null)
				throw new ArgumentNullException();
			ruina = newRuina;
			
			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;


			for (int i = ruina.relojes.Count - 1; i >= 0; --i)
			{
				if(ruina.relojes[i].haTerminado == true)
					ruina.relojes.RemoveAt(i);
			}
			

			foreach(Reloj reloj in ruina.relojes)
			{
				getCurrentAlternative().addLayer();
				++getCurrentAlternative().defaultLayer;

				getCurrentAlternative().getCurrentLayer().offsetX =
					(int)reloj.coordenadas.Item1;
				getCurrentAlternative().getCurrentLayer().offsetY =
					(int)reloj.coordenadas.Item2;
				
				RelojView relojView;
				relojView = reloj.crearVista();
				addComponent(relojView);
			}
		}
	}
}



