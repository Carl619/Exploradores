using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class PanelHUDVentanas : InterfazGrafica
	{
		// variables
		public Mapa.PanelViaje panelViaje { get; protected set; }
		public Personajes.PanelAtributo panelAtributo { get; protected set; }
		public Personajes.PanelHabilidad panelHabilidad { get; protected set; }
		public Personajes.PanelPersonajes panelPersonajes { get; protected set; }
		public Objetos.PanelInventario panelInventario { get; protected set; }
		public Mapa.PanelEdificio panelEdificio { get; protected set; }
		public Interaccion.VentanaDialogo panelDialogo { get; protected set; }


		// constructor
		public PanelHUDVentanas(bool actualizarVista = true)
			: base()
		{
			panelViaje = null;
			panelAtributo = null;
			panelHabilidad = null;
			panelPersonajes = null;
			panelInventario = null;
			panelEdificio = null;
			panelDialogo = null;
			
			alternativeNames.Add("vacio", 0);
			alternativeNames.Add("viaje", 1);
			alternativeNames.Add("personajes", 2);
			alternativeNames.Add("inventario", 3);
			alternativeNames.Add("edificio", 4);
			alternativeNames.Add("dialogo", 5);
			
			alternativeNames.Add("atributo", 1);
			alternativeNames.Add("habilidad", 2);

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents(true);
			requestedContentUpdate = false;

			setNumberAlternatives(6);

			setCurrentAlternative(alternativeNames["viaje"]);
			panelViaje = new Mapa.PanelViaje();
			addComponent(panelViaje);
			

			setCurrentAlternative(alternativeNames["personajes"]);
			getCurrentAlternative().addLayer();
			getCurrentAlternative().addLayer();
			setCurrentLayer((uint)alternativeNames["vacio"]);
			panelPersonajes = new Personajes.PanelPersonajes();
			addComponent(panelPersonajes);
			setCurrentLayer((uint)alternativeNames["atributo"]);
			panelAtributo = new Personajes.PanelAtributo();
			panelAtributo.visible = false;
			addComponent(panelAtributo);
			setCurrentLayer((uint)alternativeNames["habilidad"]);
			panelHabilidad = new Personajes.PanelHabilidad();
			panelHabilidad.visible = false;
			addComponent(panelHabilidad);
			setCurrentLayer((uint)alternativeNames["vacio"]);
			

			setCurrentAlternative(alternativeNames["inventario"]);
			panelInventario = new Objetos.PanelInventario();
			addComponent(panelInventario);
			
			setCurrentAlternative(alternativeNames["edificio"]);
			panelEdificio = new Mapa.PanelEdificio();
			addComponent(panelEdificio);
			
			setCurrentAlternative(alternativeNames["dialogo"]);
			panelDialogo = Interaccion.VentanaDialogo.Instancia;
			panelDialogo.cerrarDialogo();
			String npc = Gestores.Partidas.Instancia.npcSeleccionado;
			if(npc != null)
				panelDialogo.comenzarDialogo(Gestores.Partidas.Instancia.npcs[npc]);
			addComponent(panelDialogo);

			cambiarAlternativa(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD);
		}


		public void cambiarAlternativa(Gestores.Pantallas.EstadoHUD estadoHUD)
		{
			String alt = "";
			if(estadoHUD == Gestores.Pantallas.EstadoHUD.Vacio)
				alt = "vacio";
			else if(estadoHUD == Gestores.Pantallas.EstadoHUD.Viaje)
				alt = "viaje";
			else if(estadoHUD == Gestores.Pantallas.EstadoHUD.Personajes)
				alt = "personajes";
			else if(estadoHUD == Gestores.Pantallas.EstadoHUD.Inventario)
				alt = "inventario";
			else if(estadoHUD == Gestores.Pantallas.EstadoHUD.Edificio)
				alt = "edificio";
			else if(estadoHUD == Gestores.Pantallas.EstadoHUD.Dialogo)
				alt = "dialogo";
			setCurrentAlternative(alternativeNames[alt]);
		}


		public void activarLayer(Gestores.Pantallas.EstadoCaracteristica estado)
		{
			panelAtributo.visible = false;
			panelHabilidad.visible = false;
			String alt = "";
			if(estado == Gestores.Pantallas.EstadoCaracteristica.Vacio)
			{
				getCurrentAlternative().getCurrentLayer().blockSubsequentLayerEvents = false;
				alt = "vacio";
			}
			else if(estado == Gestores.Pantallas.EstadoCaracteristica.Atributo)
			{
				panelAtributo.visible = true;
				alt = "atributo";
			}
			else if(estado == Gestores.Pantallas.EstadoCaracteristica.Habilidad)
			{
				panelHabilidad.visible = true;
				alt = "habilidad";
			}
			setCurrentLayer((uint)alternativeNames[alt]);
			if(estado != Gestores.Pantallas.EstadoCaracteristica.Vacio)
				getCurrentAlternative().getCurrentLayer().blockSubsequentLayerEvents = true;
		}
	}
}



