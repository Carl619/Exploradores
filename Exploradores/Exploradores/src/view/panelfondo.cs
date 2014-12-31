using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class PanelFondo : InterfazGrafica
	{
		// variables
		public Mapa.MapaViewCentro vistaMapa { get; protected set; }
		public Mapa.CiudadViewCentro vistaCiudad { get; protected set; }

		// constructor
		public PanelFondo(bool actualizarVista = true)
			: base()
		{
			vistaMapa = null;
			vistaCiudad = null;

			alternativeNames.Add("mapa", 0);
			alternativeNames.Add("lugar", 1);

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents(true);
			requestedContentUpdate = false;

			setNumberAlternatives(2);
			
			setCurrentAlternative(alternativeNames["mapa"]);
			vistaMapa = Mapa.Mapa.Instancia.crearVistaCentral();
			addComponent(vistaMapa);

			setCurrentAlternative(alternativeNames["lugar"]);
			if(Programa.Jugador.Instancia.protagonista.lugarActual.GetType() == typeof(Mapa.Ciudad))
			{
				vistaCiudad =
					((Mapa.Ciudad)Programa.Jugador.Instancia.protagonista.lugarActual).crearVistaCentro();
				addComponent(vistaCiudad);
			}
			else
				vistaCiudad = null;
			

			cambiarAlternativa(Gestores.Partidas.Instancia.gestorPantallas.estadoPartida);
		}


		public void cambiarAlternativa(Gestores.Pantallas.EstadoPartida estadoPartida)
		{
			String alt = "";
			if(estadoPartida == Gestores.Pantallas.EstadoPartida.Mapa)
				alt = "mapa";
			else if(estadoPartida == Gestores.Pantallas.EstadoPartida.Ciudad)
				alt = "lugar";
			setCurrentAlternative(alternativeNames[alt]);
		}
	}
}



