using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public partial class MapaViewCentro : InterfazGrafica
	{
		// variables
		public Mapa mapa { get; protected set; }
		public LugarVisitable lugarSeleccionado { get; protected set; }
		public ILSXNA.Container contenedorScroll { get; protected set; }
		public InterfazGraficaMapa interfazMapas { get; protected set; }
		public InterfazGrafica interfazLugares { get; protected set; }
		public InterfazGrafica interfazRutas { get; protected set; }


		// constructor
		public MapaViewCentro(Mapa newMapa)
			: base()
		{
			if(newMapa == null)
				throw new ArgumentNullException();
			
			mapa = newMapa;
			lugarSeleccionado = null;
			contenedorScroll = null;
			interfazMapas = null;
			interfazLugares = null;
			interfazRutas = null;

			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			callbackConfigObj = this;
			onLeftMousePress = Controller.mouseDeselectLugar;

			contenedorScroll = new ILSXNA.Container();
			contenedorScroll.sizeSettings.fixedInnerWidth = Programa.VistaGeneral.Instancia.window.currentWidth - 372;
			contenedorScroll.sizeSettings.fixedInnerHeight = Programa.VistaGeneral.Instancia.window.currentHeight - 59;
			addComponent(contenedorScroll);

			ILSXNA.Container contenedorInterior = new ILSXNA.Container();
			contenedorScroll.addComponent(contenedorInterior);

			contenedorInterior.getCurrentAlternative().addLayer();
			contenedorInterior.getCurrentAlternative().addLayer();

			interfazMapas = new InterfazGraficaMapa();
			interfazMapas.border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["map1"].clone();
			interfazLugares = new InterfazGrafica();
			interfazRutas = new InterfazGrafica();
			
			contenedorInterior.setCurrentLayer(2);
			contenedorInterior.addComponent(interfazLugares);
			contenedorInterior.setCurrentLayer(1);
			contenedorInterior.addComponent(interfazRutas);
			contenedorInterior.setCurrentLayer(0);
			contenedorInterior.addComponent(interfazMapas);

			actualizarRutas();
			actualizarLugares();
			actualizarImagenesMapa();

			Programa.VistaGeneral.Instancia.scrollableContainer = contenedorScroll;
		}


		public void actualizarRutas()
		{
			interfazRutas.clearComponents();

			foreach(Ruta ruta in mapa.rutas)
			{
				if(ruta.oculto == true)
					continue;
				interfazRutas.getCurrentAlternative().addLayer();
				++interfazRutas.getCurrentAlternative().defaultLayer;
				bool parteCamino = false;
				List<Dijkstra.IRama> camino = Programa.Jugador.Instancia.protagonista.viaje.camino;
				if(camino != null)
					if(camino.Contains(ruta) == true)
						parteCamino = true;
				RutaView rutaView = ruta.crearVista(parteCamino);
				
				int minCoordenadaX, minCoordenadaY;
				minCoordenadaX = ruta.extremos.Item1.coordenadas.Item1;
				if(minCoordenadaX < ruta.extremos.Item2.coordenadas.Item1)
					minCoordenadaX = ruta.extremos.Item2.coordenadas.Item1;
				minCoordenadaY = ruta.extremos.Item1.coordenadas.Item2;
				if(minCoordenadaY < ruta.extremos.Item2.coordenadas.Item2)
					minCoordenadaY = ruta.extremos.Item2.coordenadas.Item2;
				
				interfazRutas.getCurrentAlternative().getCurrentLayer().offsetX = minCoordenadaX;
				interfazRutas.getCurrentAlternative().getCurrentLayer().offsetY = minCoordenadaY;
				interfazRutas.addComponent(rutaView);
			}
		}


		public void actualizarLugares()
		{
			interfazLugares.clearComponents();

			foreach(LugarVisitable lugar in mapa.lugares)
			{
				if(lugar.oculto == true)
				{
					if(lugar.esVisibleDesconocido() == false)
					continue;
				}
				interfazLugares.getCurrentAlternative().addLayer();
				++interfazLugares.getCurrentAlternative().defaultLayer;

				Texture2D icono;
				LugarVisitableView lugarView;
				LugarVisitableFlyweight flyweight = lugar.flyweightLugar;
				if(lugar.oculto == true)
					flyweight = Gestores.Mundo.Instancia.lugarFlyweights["desconocido"];
				lugarView = lugar.crearVista();
				lugarView.callbackConfigObj = this;
				lugarView.onLeftMousePress = Controller.mouseSelectLugar;

				icono = flyweight.iconoPasivo;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.addTextura(icono);

				icono = flyweight.iconoActivo;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.addTextura(icono);

				icono = flyweight.iconoSeleccionado;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.addTextura(icono);

				icono = flyweight.iconoActivoSeleccionado;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.addTextura(icono);
				
				interfazLugares.getCurrentAlternative().getCurrentLayer().offsetX =
					(int)lugar.coordenadas.Item1 - (int)(lugarView.getMinOutterUnconstrainedWidth() / 2);
				interfazLugares.getCurrentAlternative().getCurrentLayer().offsetY =
					(int)lugar.coordenadas.Item2 - (int)(lugarView.getMinOutterUnconstrainedHeight() / 2);
				interfazLugares.addComponent(lugarView);

				if (lugar == lugarSeleccionado)
					lugarView.select();
				
				if(Programa.Jugador.Instancia.protagonista.lugarActual == lugar)
				{
					ILSXNA.Sprite marcador = new ILSXNA.Sprite();
					marcador.innerComponent = mapa.marcadorJugador;
					if(marcador.innerComponent == null)
						marcador.innerComponent = LugarVisitableFlyweight.iconoAusente;
					marcador.displayOffsetX = - ((int)(lugarView.getMinOutterUnconstrainedWidth()) / 4);
					interfazLugares.addComponent(marcador);
				}
			}
		}


		public void actualizarImagenesMapa()
		{
			interfazMapas.clearComponents();

			ILSXNA.Sprite interior;
			int i = 0, count = mapa.imagenesMapa.Count;
			foreach(Texture2D textura in mapa.imagenesMapa)
			{
				ILSXNA.Container contenedor = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border2"]);
				interfazMapas.addComponent(contenedor);

				interior = new ILSXNA.Sprite();
				interior.innerComponent = textura;
				contenedor.addComponent(interior);
				
				interfazMapas.getCurrentAlternative().getCurrentLayer().offsetX =
					(int)mapa.offsetsImagenesMapa[i].X;
				interfazMapas.getCurrentAlternative().getCurrentLayer().offsetY =
					(int)mapa.offsetsImagenesMapa[i].Y;
				
				if(++i != count)
				{
					interfazMapas.getCurrentAlternative().addLayer();
					++interfazMapas.getCurrentAlternative().defaultLayer;
				}
			}
		}


		public void quitarSeleccionLugar()
		{
			if(lugarSeleccionado != null)
			{
				lugarSeleccionado.vista.deselect();
				lugarSeleccionado = null;
				Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaMapa.cambiarLugarSeleccionado(null);
			}
		}

		
		public void seleccionarLugar(LugarVisitableView lugar)
		{
			quitarSeleccionLugar();
			lugarSeleccionado = lugar.lugar;
			Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaMapa.cambiarLugarSeleccionado(lugarSeleccionado);
		}
	}
}




