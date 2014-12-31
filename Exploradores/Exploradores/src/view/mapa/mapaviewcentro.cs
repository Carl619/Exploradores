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
			onMousePress = Controller.mouseDeselectLugar;

			getCurrentAlternative().addLayer();
			getCurrentAlternative().addLayer();

			interfazLugares = new InterfazGrafica();
			interfazRutas = new InterfazGrafica();
			
			setCurrentLayer(2);
			addComponent(interfazLugares);
			setCurrentLayer(1);
			addComponent(interfazRutas);
			setCurrentLayer(0);
			
			ILSXNA.Sprite interior = new ILSXNA.Sprite();
			interior.innerComponent = mapa.imagenMapa;
			addComponent(interior);

			actualizarRutas();
			actualizarLugares();
		}


		public void actualizarRutas()
		{
			interfazRutas.clearComponents();

			foreach(Ruta ruta in mapa.rutas)
			{
				interfazRutas.getCurrentAlternative().addLayer();
				++interfazRutas.getCurrentAlternative().defaultLayer;
				bool parteCamino = false;
				List<Dijkstra.IRama> camino = Programa.Jugador.Instancia.protagonista.camino;
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
				interfazLugares.getCurrentAlternative().addLayer();
				++interfazLugares.getCurrentAlternative().defaultLayer;

				Texture2D icono;
				LugarVisitableView lugarView;
				lugarView = lugar.crearVista();
				lugarView.callbackConfigObj = this;
				lugarView.onMousePress = Controller.mouseSelectLugar;

				icono = lugar.flyweightLugar.iconoPasivo;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.innerComponent.Add(icono);

				icono = lugar.flyweightLugar.iconoActivo;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.innerComponent.Add(icono);

				icono = lugar.flyweightLugar.iconoSeleccionado;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.innerComponent.Add(icono);

				icono = lugar.flyweightLugar.iconoActivoSeleccionado;
				if(icono == null)
					icono = LugarVisitableFlyweight.iconoAusente;
				lugarView.innerComponent.Add(icono);
				
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




