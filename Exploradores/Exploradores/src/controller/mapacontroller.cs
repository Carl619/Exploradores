using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public static class Controller
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
		}
		

		public static void salirCiudad(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Gestores.Partidas.Instancia.cambiarMusica("mapa");
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoPartida.Mapa);
		}
		
		
		public static void entrarRuina(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
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
			Programa.Jugador.Instancia.protagonista.camino = camino;
			
			mapaView.actualizarRutas();

			Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
			panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Viaje);
			panelHUD.panelVentanas.panelViaje.setViaje(camino);
			panelHUD.actualizarBloqueoVentana();
		}


		public static void cerrarViaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD != Gestores.Pantallas.EstadoHUD.Viaje)
				return;
			
			Gestores.Partidas.Instancia.gestorPantallas.estadoHUD = Gestores.Pantallas.EstadoHUD.Vacio;
			Programa.Jugador.Instancia.protagonista.camino = null;
			Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelFondo.vistaMapa.actualizarRutas();

			Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
			panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Vacio);
			panelHUD.actualizarBloqueoVentana();
		}

		
		public static void viajar(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Programa.Jugador.Instancia.protagonista.viajarSiguiente();
			Programa.VistaGeneral.Instancia.contenedorJuego.interfazRuina.requestUpdateContent();
			Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelFondo.vistaMapa.requestUpdateContent();
			Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.actualizarLugar();
			
			Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
			panelHUD.actualizarTiempo();
			panelHUD.panelVentanas.panelViaje.setViaje(Programa.Jugador.Instancia.protagonista.camino);
			panelHUD.actualizarBloqueoVentana();
			
			Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaMapa.requestUpdateContent();
			Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaInformacion.requestUpdateContent();
		}


		public static void verEdificio(Object infoObj, Object elemento)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Vacio)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Edificio;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Edificio);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void cerrarEdificio(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Edificio)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Vacio;
				Gestores.Partidas.Instancia.edificioSeleccionado = null;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Vacio);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void abrirDialogoNPC(Object infoObj, Object elemento)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Edificio ||
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Vacio))
			{
				if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Edificio)
				{
					cerrarEdificio(null, null, null);
				}
				
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Dialogo;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Dialogo);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void cerrarDialogoNPC(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Dialogo)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Vacio;
				Gestores.Partidas.Instancia.npcSeleccionado = null;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Vacio);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void elegirOpcionDialogo(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(configObj.GetType() != typeof(Tuple<Int32, Interaccion.VentanaDialogo>))
				return;
			Tuple<Int32, Interaccion.VentanaDialogo> t = (Tuple<Int32, Interaccion.VentanaDialogo>)configObj;
			t.Item2.activarOpcion(t.Item1);
		}


		public static void clickArticulo(Object infoObj, Object elemento)
		{
			transferenciaArticulo(infoObj, elemento, false);
		}


		public static void doubleClickArticulo(Object infoObj, Object elemento)
		{
			transferenciaArticulo(infoObj, elemento, true);
		}


		public static void transferenciaArticulo(Object infoObj, Object elemento, bool todo)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Dialogo &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion ==
				Gestores.Pantallas.EstadoInteraccion.Comercio)
			{
				Tuple<Objetos.InventarioView, Objetos.InventarioView> inventarios;
				inventarios = (Tuple<Objetos.InventarioView, Objetos.InventarioView>)infoObj;
				
				Objetos.ArticuloView articulo = (Objetos.ArticuloView)elemento;
				uint cantidad = inventarios.Item1.inventario.articulos[articulo.coleccion.articulo.id].cantidad;
				if(todo == false)
					cantidad = 1;
				
				bool transferencia = inventarios.Item2.inventario.addArticulo(articulo.coleccion.articulo, cantidad);
				if(transferencia == false)
					return;
				inventarios.Item1.inventario.removeArticulo(articulo.coleccion.articulo, cantidad);

				Objetos.ColeccionArticulos coleccion;
				if(inventarios.Item1.inventario.articulos.TryGetValue(
					articulo.coleccion.articulo.id, out coleccion) == false)
				{
					inventarios.Item1.inventario.articulosSeleccionados = null;
				}
				
				inventarios.Item1.requestUpdateContent();
				inventarios.Item2.requestUpdateContent();
				Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.actualizarCabecera();
			}
		}
	}
}




