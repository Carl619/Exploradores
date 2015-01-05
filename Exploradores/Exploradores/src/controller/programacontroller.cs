using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public static partial class Controller
	{
		public static void funcionNuevaPartida(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Gestores.Partidas.Instancia.cargarNuevaPartida();
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoJuego.Jugando);
		}


		public static void funcionCargarPartida(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			/*Gestores.Partidas.Instancia.cargarPartidaAnterior("partida1");
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoJuego.Jugando);*/
		}


		public static void funcionGuardarPartida(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			/*Gestores.Partidas.Instancia.guardarPartidaActual("partida1");*/
		}


		public static void funcionMenuPrincipal(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoJuego.MenuPrincipal);
			Gestores.Partidas.Instancia.cambiarMusica("mapa");
		}


		public static void funcionSalirJuego(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Programa.VistaGeneral.Instancia.salirDelJuego = true;
		}


		public static void funcionEscapePress(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando)
				funcionToggleMenuSecundario(drawable, eventInfo, configObj);
			else
				VistaGeneral.Instancia.salirDelJuego = true;
		}


		public static void funcionToggleMenuSecundario(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoMenu == Gestores.Pantallas.EstadoMenu.Invisible)
				funcionAbrirMenuSecundario(drawable, eventInfo, configObj);
			else
				funcionCerrarMenuSecundario(drawable, eventInfo, configObj);
		}


		public static void funcionAbrirMenuSecundario(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoMenu =
					Gestores.Pantallas.EstadoMenu.VistaPrincipal;
				VistaGeneral.Instancia.contenedorJuego.actualizarMenuSecundario();
			}
		}


		public static void funcionCerrarMenuSecundario(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando)
			{
				Gestores.Partidas.Instancia.gestorPantallas.estadoMenu =
					Gestores.Pantallas.EstadoMenu.Invisible;
				VistaGeneral.Instancia.contenedorJuego.actualizarMenuSecundario();
			}
		}
	}
}




