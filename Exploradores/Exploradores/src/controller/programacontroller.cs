using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public static class Controller
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


		public static void funcionAbrirComercio(List<String> valoresEntrada)
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego ==
				Gestores.Pantallas.EstadoJuego.Jugando &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoPartida !=
				Gestores.Pantallas.EstadoPartida.Ruina &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD ==
				Gestores.Pantallas.EstadoHUD.Dialogo &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion ==
				Gestores.Pantallas.EstadoInteraccion.Vacio)
			{
				String idNPC = Gestores.Partidas.Instancia.npcSeleccionado;
				Personajes.NPC npc = Gestores.Partidas.Instancia.npcs[idNPC];
				if(npc.inventario == null)
					return;
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion =
					Gestores.Pantallas.EstadoInteraccion.Comercio;
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaComercio();
			}
		}


		public static void funcionCerrarComercio(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
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
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion =
					Gestores.Pantallas.EstadoInteraccion.Vacio;
				Objetos.InventarioView inv1 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioProtagonista;
				Objetos.InventarioView inv2 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioVenta;
				Objetos.InventarioView inv3 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioNPC;
				Objetos.InventarioView inv4 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioCompra;
				inv1.inventario.transferenciaArticulos(inv2.inventario, true);
				inv3.inventario.transferenciaArticulos(inv4.inventario, true);
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaComercio();
			}
		}


		public static void funcionRealizarIntercambio(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
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
				Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion =
					Gestores.Pantallas.EstadoInteraccion.Vacio;
				Objetos.InventarioView inv1 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioProtagonista;
				Objetos.InventarioView inv2 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioVenta;
				Objetos.InventarioView inv3 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioNPC;
				Objetos.InventarioView inv4 = 
					VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioCompra;
				inv1.inventario.transferenciaArticulos(inv4.inventario, true);
				inv3.inventario.transferenciaArticulos(inv2.inventario, true);
				int diferencia = VistaGeneral.Instancia.contenedorJuego.panelComercio.diferenciaDinero;
				if(diferencia > 0)
				{
					inv1.inventario.addArticulo(
						Gestores.Partidas.Instancia.articulos[Objetos.Articulo.idDinero],
						(uint)diferencia);
				}
				else
				{
					inv1.inventario.removeArticulo(
						Objetos.Articulo.idDinero,
						(uint)(-diferencia));
				}
				inv3.inventario.removeDinero();
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaComercio();
			}
		}


		public static void funcionReclutar(List<String> valoresEntrada)
		{
			String idNPC = Gestores.Partidas.Instancia.npcSeleccionado;
			Personajes.NPC npc = Gestores.Partidas.Instancia.npcs[idNPC];
			Programa.Jugador.Instancia.protagonista.reclutar(npc);

			if(npc.eliminar == true)
			{
				Mapa.Controller.cerrarDialogoNPC(null, null, null);
				Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaCiudad.requestUpdateContent();
			}
		}
	}
}




