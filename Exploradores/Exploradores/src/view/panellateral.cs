using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public class PanelLateral : InterfazGrafica
	{
		// variables
		public Mapa.MapaViewLateral vistaMapa { get; protected set; }
		public Mapa.CiudadViewLateral vistaCiudad { get; protected set; }
		public ILSXNA.Container vistaPrincipal { get; protected set; }
		public ILSXNA.Container vistaInformacion { get; protected set; }
		public uint anchoSeleccion { get; set; }


		// constructor
		public PanelLateral(bool actualizarVista = true)
			: base()
		{
			vistaMapa = null;
			vistaCiudad = null;
			vistaPrincipal = null;
			vistaInformacion = null;
			anchoSeleccion = 320;

			alternativeNames.Add("mapa", 0);
			alternativeNames.Add("ciudad", 1);

			layout.equalCellWidth = true;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents(true);
			requestedContentUpdate = false;

			vistaPrincipal = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			vistaPrincipal.sizeSettings.minInnerWidth = anchoSeleccion;
			vistaPrincipal.contentSpacingX = 4;
			vistaPrincipal.contentSpacingY = 4;
			addComponent(vistaPrincipal);

			vistaPrincipal.setNumberAlternatives(2);
			
			vistaPrincipal.setCurrentAlternative(alternativeNames["mapa"]);
			vistaMapa = Mapa.Mapa.Instancia.crearVistaLateral(this);
			vistaPrincipal.addComponent(vistaMapa);
			
			
			Mapa.LugarVisitable lugarActual = Programa.Jugador.Instancia.protagonista.lugarActual;
			if(lugarActual.GetType() == typeof(Mapa.Ciudad))
			{
				vistaPrincipal.setCurrentAlternative(alternativeNames["ciudad"]);
				vistaCiudad = ((Mapa.Ciudad)lugarActual).crearVistaLateral(this);
				vistaPrincipal.addComponent(vistaCiudad);
			}

			cambiarAlternativa(Gestores.Partidas.Instancia.gestorPantallas.estadoPartida);
			bool informacionVisible = 
				(Gestores.Partidas.Instancia.gestorPantallas.estadoInformacion ==
				Gestores.Pantallas.EstadoInformacion.Visible);
			
			vistaInformacion = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			vistaInformacion.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			vistaInformacion.sizeSettings.minInnerWidth = anchoSeleccion;
			vistaInformacion.visible = informacionVisible;
			vistaInformacion.contentSpacingX = 4;
			vistaInformacion.contentSpacingY = 4;
			if(lugarActual != null && informacionVisible)
				verInformacion(lugarActual.getInformacion());
			else
				cerrarInformacion(false);
			addComponent(vistaInformacion);
		}


		public void actualizarLugar()
		{
			ILSXNA.Container aux = vistaMapa;

			clearComponents(true);
			requestedContentUpdate = false;

			vistaPrincipal = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			vistaPrincipal.sizeSettings.minInnerWidth = anchoSeleccion;
			vistaPrincipal.contentSpacingX = 4;
			vistaPrincipal.contentSpacingY = 4;
			addComponent(vistaPrincipal);

			vistaPrincipal.setNumberAlternatives(2);
			
			vistaPrincipal.setCurrentAlternative(alternativeNames["mapa"]);
			vistaPrincipal.addComponent(aux);
			
			
			Mapa.LugarVisitable lugarActual = Programa.Jugador.Instancia.protagonista.lugarActual;
			if(lugarActual.GetType() == typeof(Mapa.Ciudad))
			{
				vistaPrincipal.setCurrentAlternative(alternativeNames["ciudad"]);
				vistaCiudad = ((Mapa.Ciudad)lugarActual).crearVistaLateral(this);
				vistaPrincipal.addComponent(vistaCiudad);
			}

			cambiarAlternativa(Gestores.Partidas.Instancia.gestorPantallas.estadoPartida);
			bool informacionVisible = 
				(Gestores.Partidas.Instancia.gestorPantallas.estadoInformacion ==
				Gestores.Pantallas.EstadoInformacion.Visible);
			
			vistaInformacion = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			vistaInformacion.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			vistaInformacion.sizeSettings.minInnerWidth = anchoSeleccion;
			vistaInformacion.visible = informacionVisible;
			vistaInformacion.contentSpacingX = 4;
			vistaInformacion.contentSpacingY = 4;
			if(lugarActual != null && informacionVisible)
				verInformacion(lugarActual.getInformacion());
			else
				cerrarInformacion(false);
			addComponent(vistaInformacion);
		}


		public void cambiarAlternativa(Gestores.Pantallas.EstadoPartida estadoPartida)
		{
			String alt = "";
			if(estadoPartida == Gestores.Pantallas.EstadoPartida.Mapa)
				alt = "mapa";
			else if(estadoPartida == Gestores.Pantallas.EstadoPartida.Ciudad)
				alt = "ciudad";
			vistaPrincipal.setCurrentAlternative(alternativeNames[alt]);
		}


		public void verInformacion(List<String> info)
		{
			vistaInformacion.clearComponents();
			
			Gestores.Partidas.Instancia.gestorPantallas.estadoInformacion =
				Gestores.Pantallas.EstadoInformacion.Visible;
			vistaInformacion.visible = true;


			foreach(String frase in info)
			{
				SpriteFont fuente;
				fuente = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
				
				ILSXNA.Label texto = new ILSXNA.Label();
				texto.innerComponent = fuente;
				texto.color = Gestores.Mundo.Instancia.colores["genericColor"];
				texto.message = (String)frase.Clone();
				ILSXNA.Paragraph par = new ILSXNA.Paragraph(texto, anchoSeleccion);
				vistaInformacion.addComponent(par);
			}


			ILSXNA.Button boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.callbackConfigObj = this;
			boton.onButtonPress = Mapa.Controller.quitarInformacion;
			vistaInformacion.addComponent(boton);
		}


		public void cerrarInformacion(bool permanente)
		{
			if(permanente == true)
				Gestores.Partidas.Instancia.gestorPantallas.estadoInformacion =
					Gestores.Pantallas.EstadoInformacion.Invisible;
			
			vistaInformacion.clearComponents();
			vistaInformacion.visible = false;
		}
	}
}



