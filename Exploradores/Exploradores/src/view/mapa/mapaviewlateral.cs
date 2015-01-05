using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class MapaViewLateral : InterfazGrafica
	{
		// variables
		public Programa.PanelLateral panelLateral { get; protected set; }
		public Mapa mapa { get; protected set; }
		protected LugarVisitable lugarSeleccionado { get; set; }


		// constructor
		public MapaViewLateral(Mapa newMapa, Programa.PanelLateral newPanelLateral)
			: base()
		{
			if (newMapa == null || newPanelLateral == null)
				throw new ArgumentNullException();
			
			mapa = newMapa;
			panelLateral = newPanelLateral;
			lugarSeleccionado = null;

			sizeSettings.minInnerWidth = panelLateral.anchoSeleccion;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			
			SpriteFont font = mapa.spriteFont;
			ILSXNA.Label posicion;
			posicion = new ILSXNA.Label();
			posicion.message = "Te encuentras en " +
								Programa.Jugador.Instancia.protagonista.lugarActual.nombre +
								".";
			posicion.color = mapa.colorTexto;
			posicion.innerComponent = font;

			addComponent(posicion);
			
			if(lugarSeleccionado == null)
			{
				ILSXNA.Label label;

				label = new ILSXNA.Label();
				label.message = "Selecciona un lugar para visualizarlo.";
				label.color = mapa.colorTexto;
				label.innerComponent = font;

				addComponent(label);
			}
			else
			{
				ILSXNA.Label label;
				ILSXNA.Button boton;

				label = new ILSXNA.Label();
				if(lugarSeleccionado.oculto == false)
					label.message = (String)lugarSeleccionado.nombre.Clone() +
									" ( " + 
									lugarSeleccionado.flyweightLugar.nombreTipoLugar +
									" ) ";
				else
					label.message = "Un lugar desconocido";
				label.color = mapa.colorTexto;
				label.innerComponent = font;
				addComponent(label);

				ILSXNA.Container opciones = new ILSXNA.Container();
				opciones.layout.equalCellWidth = true;
				opciones.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
				addComponent(opciones);

				if(lugarSeleccionado == Programa.Jugador.Instancia.protagonista.lugarActual)
				{
					String nombreLugar = "";
					if(lugarSeleccionado.GetType() == typeof(Ciudad))
						nombreLugar = "Ver Ciudad";
					else
						nombreLugar = "Explorar";
					
					boton = new ILSXNA.Button(nombreLugar, Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
					if(lugarSeleccionado.GetType() == typeof(Ciudad))
					{
						boton.icons.Add(Gestores.Mundo.Instancia.imagenes["ciudad"].textura);
						boton.updateContent();
						boton.onButtonPress = Controller.entrarCiudad;
					}
					else if(lugarSeleccionado.GetType() == typeof(Ruina))
					{
						boton.icons.Add(Gestores.Mundo.Instancia.imagenes["ruina"].textura);
						boton.updateContent();
						boton.onButtonPress = Controller.entrarRuina;
					}
					opciones.addComponent(boton);
				}
				else
				{
					boton = new ILSXNA.Button("Preparar Viaje", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
					boton.icons.Add(Gestores.Mundo.Instancia.imagenes["viajar"].textura);
					boton.updateContent();
					boton.onButtonPress = Controller.abrirViaje;
					opciones.addComponent(boton);
				}

				boton = new ILSXNA.Button("Informacion", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
				boton.icons.Add(Gestores.Mundo.Instancia.imagenes["info"].textura);
				boton.updateContent();
				boton.callbackConfigObj =
					new Tuple<Programa.PanelLateral, LugarVisitable>
					(panelLateral, lugarSeleccionado);
				boton.onButtonPress = Controller.verInformacionLugar;
				opciones.addComponent(boton);
			}
		}
		
		
		public void cambiarLugarSeleccionado(LugarVisitable lugarNuevo)
		{
			if(lugarSeleccionado == lugarNuevo)
				return;
			lugarSeleccionado = lugarNuevo;
			bool informacionVisible = 
				(Gestores.Partidas.Instancia.gestorPantallas.estadoInformacion ==
				Gestores.Pantallas.EstadoInformacion.Visible);
			
			if(lugarSeleccionado != null && informacionVisible)
				panelLateral.verInformacion(lugarSeleccionado.getInformacion());
			else
				panelLateral.cerrarInformacion(false);
			requestUpdateContent();
		}
	}
}