using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public static partial class Controller
	{
		public static void mouseSelectLugar(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			((MapaViewCentro)configObj).seleccionarLugar((LugarVisitableView)drawable);
			LugarVisitableView.mouseSelect(drawable, eventInfo, configObj);
		}
		
		
		public static void mouseDeselectLugar(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			((MapaViewCentro)configObj).quitarSeleccionLugar();
		}
		
		
		public static void entrarCiudad(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Gestores.Partidas.Instancia.cambiarMusica("ciudad");
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoPartida.Ciudad);

			Interaccion.Mision.Evento evento = Interaccion.Mision.Evento.LlegadaLugar;
			Interaccion.Mision.InfoEvento info = new Interaccion.Mision.InfoEvento();
			info.lugar = Programa.Jugador.Instancia.protagonista.lugarActual;
			Gestores.Partidas.Instancia.gestorMisiones.notify(evento, info);
		}
		

		public static void salirCiudad(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Gestores.Partidas.Instancia.cambiarMusica("mapa");
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoPartida.Mapa);
		}
		
		
		public static void entrarRuina(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			LugarVisitable lugar = Programa.Jugador.Instancia.protagonista.lugarActual;
			if(lugar.GetType() != typeof(Ruina))
				return;
			Gestores.Partidas.Instancia.gestorRuinas.cargarRuinaActual((Ruina)lugar);
			Gestores.Partidas.Instancia.cambiarMusica("ruina");
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoPartida.Ruina);
		}
		

		public static void verInformacionLugar(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Tuple<Programa.PanelLateral, LugarVisitable> tupla = ((Tuple<Programa.PanelLateral, LugarVisitable>)configObj);
			tupla.Item1.verInformacion(tupla.Item2.getInformacion());
		}


		public static void quitarInformacion(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Programa.PanelLateral panel = (Programa.PanelLateral)configObj;
			panel.cerrarInformacion(true);
		}

		
		public static void accionLugarVisitable(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			MapaViewCentro mapaView = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelFondo.vistaMapa;
			LugarVisitable nodo = Programa.Jugador.Instancia.protagonista.lugarActual;

			if(nodo == mapaView.lugarSeleccionado)
			{
				if(nodo.GetType() == typeof(Ciudad))
					entrarCiudad(null, null, null);
				else if(nodo.GetType() == typeof(Ruina))
					entrarRuina(null, null, null);
			}
			else
			{
				abrirViaje(null, null, null);
			}
		}

		
		public static void abrirViaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD != Gestores.Pantallas.EstadoHUD.Vacio)
				return;
			
			MapaViewCentro mapaView = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelFondo.vistaMapa;

			int distInfinita = 1000000;
			double distancia = distInfinita;
			LugarVisitable nodo = Programa.Jugador.Instancia.protagonista.lugarActual;
			Dijkstra.Nodo<LugarVisitable> origen = new Dijkstra.Nodo<LugarVisitable>(nodo, distInfinita);
			List<Dijkstra.IRama> camino =
				origen.buscarCaminoMinimo(mapaView.lugarSeleccionado, 0,
							(int)Gestores.Partidas.Instancia.numeroLugaresVisitables,
							ref distancia);
			Programa.Jugador.Instancia.protagonista.viaje.asignar(camino, mapaView.lugarSeleccionado);
			
			mapaView.actualizarRutas();

			Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
			panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Viaje);
			panelHUD.panelVentanas.panelViaje.setViaje(Programa.Jugador.Instancia.protagonista.viaje);
			panelHUD.actualizarBloqueoVentana();
		}


		public static void cerrarViaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD != Gestores.Pantallas.EstadoHUD.Viaje)
				return;
			
			Gestores.Partidas.Instancia.gestorPantallas.estadoHUD = Gestores.Pantallas.EstadoHUD.Vacio;
			Programa.Jugador.Instancia.protagonista.viaje.clear(true);
			Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelFondo.vistaMapa.actualizarRutas();

			Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
			panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Vacio);
			panelHUD.actualizarBloqueoVentana();
		}

		
		public static void intentarViajar(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Programa.Jugador.Instancia.protagonista.viajarSiguiente(true) == false)
				actualizarViaje(drawable, eventInfo, configObj);
		}

		
		public static void actualizarViaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelFondo.vistaMapa.requestUpdateContent();
			Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.actualizarLugar();
			
			Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
			panelHUD.actualizarTiempo();
			panelHUD.panelVentanas.panelViaje.setViaje(Programa.Jugador.Instancia.protagonista.viaje);
			panelHUD.actualizarBloqueoVentana();
			
			Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaMapa.requestUpdateContent();
			Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaInformacion.requestUpdateContent();
		}


		public static void viajePagar(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Viaje &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion ==
				Gestores.Pantallas.EstadoInteraccion.Defensa)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion =
					Gestores.Pantallas.EstadoInteraccion.Vacio;
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaDefensa();
				
				Programa.Jugador.Instancia.protagonista.pagarAtacantes();
				Programa.Jugador.Instancia.protagonista.viajarSiguiente(false);
				actualizarViaje(null, null, null);
			}
		}


		public static void viajeDefender(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Viaje &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion ==
				Gestores.Pantallas.EstadoInteraccion.Defensa)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion =
					Gestores.Pantallas.EstadoInteraccion.Vacio;
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaDefensa();
				
				Programa.Jugador.Instancia.protagonista.defender();
				Programa.Jugador.Instancia.protagonista.viajarSiguiente(false);
				actualizarViaje(null, null, null);
			}
		}


		public static void viajeHuir(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Viaje &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion ==
				Gestores.Pantallas.EstadoInteraccion.Defensa)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion =
					Gestores.Pantallas.EstadoInteraccion.Vacio;
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaDefensa();
				
				Programa.Jugador.Instancia.protagonista.huir();
				Programa.Jugador.Instancia.protagonista.viajeVolver();
				actualizarViaje(null, null, null);
			}
		}
	}
}




