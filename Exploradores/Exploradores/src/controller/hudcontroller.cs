using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public static partial class Controller
	{


		public static void funcionAbrirMisiones(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Vacio)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Misiones;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Misiones);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionCerrarMisiones(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Misiones)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Vacio;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Vacio);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionVerCategoriaMisiones(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Misiones)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoMisiones =
					(Gestores.Pantallas.EstadoMisiones)configObj;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.panelMisiones.cambiarAlternativa(
					Gestores.Partidas.Instancia.gestorPantallas.estadoMisiones);
				//panelHUD.panelVentanas.panelMisiones.requestUpdateContent();
				//panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionAbrirPersonajes(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Vacio)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Personajes;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Personajes);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionCerrarPersonajes(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Personajes)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Vacio;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Vacio);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionAbrirInventario(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Vacio)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Inventario;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Inventario);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionCerrarInventario(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Inventario)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Vacio;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Vacio);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionVerAtributo(Object infoObj, Object elemento)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Personajes &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica ==
				Gestores.Pantallas.EstadoCaracteristica.Vacio)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica =
					Gestores.Pantallas.EstadoCaracteristica.Atributo;
				Gestores.Partidas.Instancia.atributoSeleciconado =
					((Personajes.AtributoView)elemento).atributo;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.activarLayer(Gestores.Pantallas.EstadoCaracteristica.Atributo);
				panelHUD.panelVentanas.panelAtributo.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}

		
		public static void funcionCerrarAtributo(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Personajes &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica ==
				Gestores.Pantallas.EstadoCaracteristica.Atributo)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica =
					Gestores.Pantallas.EstadoCaracteristica.Vacio;
				Gestores.Partidas.Instancia.atributoSeleciconado = null;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.activarLayer(Gestores.Pantallas.EstadoCaracteristica.Vacio);
				panelHUD.panelVentanas.panelAtributo.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}


		public static void funcionVerHabilidad(Object infoObj, Object elemento)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Personajes &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica ==
				Gestores.Pantallas.EstadoCaracteristica.Vacio)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica =
					Gestores.Pantallas.EstadoCaracteristica.Habilidad;
				Gestores.Partidas.Instancia.habilidadSeleciconada =
					((Personajes.HabilidadView)elemento).habilidad;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.activarLayer(Gestores.Pantallas.EstadoCaracteristica.Habilidad);
				panelHUD.panelVentanas.panelHabilidad.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}

		
		public static void funcionCerrarHabilidad(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Personajes &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica ==
				Gestores.Pantallas.EstadoCaracteristica.Habilidad)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoCaracteristica =
					Gestores.Pantallas.EstadoCaracteristica.Vacio;
				Gestores.Partidas.Instancia.habilidadSeleciconada = null;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.activarLayer(Gestores.Pantallas.EstadoCaracteristica.Vacio);
				panelHUD.panelVentanas.panelHabilidad.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();
			}
		}
	}
}




