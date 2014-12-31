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


		// constructor
		public InterfazRuinaView(bool actualizarVista = true)
			: base()
		{
			ruina = null;
			
			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			Mapa.LugarVisitable lugar = Programa.Jugador.Instancia.protagonista.lugarActual;
			
			if(lugar.GetType() != typeof(Mapa.Ruina))
				return;
			
			Gestores.Partidas.Instancia.gestorRuinas.cargarRuinaActual((Mapa.Ruina)lugar);
			Ruinas.RuinaJugable ruinaJugable = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual;
			ruina = ruinaJugable.crearVista();
			addComponent(ruina);
		}
	}
}



