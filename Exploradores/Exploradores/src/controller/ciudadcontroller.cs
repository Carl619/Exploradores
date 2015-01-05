using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public static partial class Controller
	{


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

				Personajes.NPC npc = Gestores.Partidas.Instancia.npcs[Gestores.Partidas.Instancia.npcSeleccionado];

				Interaccion.Mision.Evento evento = Interaccion.Mision.Evento.Dialogo;
				Interaccion.Mision.InfoEvento info = new Interaccion.Mision.InfoEvento();
				info.npc = npc;
				Gestores.Partidas.Instancia.gestorMisiones.notify(evento, info);

				evento = Interaccion.Mision.Evento.ElementoDialogo;
				info = new Interaccion.Mision.InfoEvento();
				info.elementoDialogo = npc.menuDialogo;
				Gestores.Partidas.Instancia.gestorMisiones.notify(evento, info);
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


		public static void abrirComercio(List<Interaccion.Evento.Argumento> valoresEntrada)
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


		public static void cerrarComercio(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
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
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioProtagonista;
				Objetos.InventarioView inv2 = 
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioVenta;
				Objetos.InventarioView inv3 = 
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioNPC;
				Objetos.InventarioView inv4 = 
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioCompra;
				inv1.inventario.transferenciaArticulos(inv2.inventario, true);
				inv3.inventario.transferenciaArticulos(inv4.inventario, true);
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaComercio();
			}
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


		public static void realizarIntercambio(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
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
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioProtagonista;
				Objetos.InventarioView inv2 = 
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioVenta;
				Objetos.InventarioView inv3 = 
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioNPC;
				Objetos.InventarioView inv4 = 
					Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.inventarioCompra;
				inv1.inventario.transferenciaArticulos(inv4.inventario, true);
				inv3.inventario.transferenciaArticulos(inv2.inventario, true);
				int diferencia = Programa.VistaGeneral.Instancia.contenedorJuego.panelComercio.diferenciaDinero;
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
				Programa.Jugador.Instancia.actualizarEspacioInventario();
				Gestores.Partidas.Instancia.gestorMisiones.notify(Interaccion.Mision.Evento.Articulo, null);
			}
		}


		public static void reclutar(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			String idNPC = Gestores.Partidas.Instancia.npcSeleccionado;
			Personajes.NPC npc = Gestores.Partidas.Instancia.npcs[idNPC];
			Programa.Jugador.Instancia.protagonista.reclutar(npc);
			Programa.Jugador.Instancia.actualizarEspacioInventario();

			if(npc.eliminar == true)
			{
				cerrarDialogoNPC(null, null, null);
				Programa.VistaGeneral.Instancia.contenedorJuego.panelLateral.vistaCiudad.requestUpdateContent();
			}
		}


		public static void empezarMision(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			Gestores.Partidas.Instancia.gestorMisiones.misiones[valoresEntrada[2].valor].empezarMision();
		}


		public static void addEventoDialogo(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			Interaccion.MenuDialogo menu =
				(Interaccion.MenuDialogo)Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[2].valor];
			menu.evento = Gestores.Partidas.Instancia.eventos[valoresEntrada[3].valor];
		}


		public static void removeEventoDialogo(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			Interaccion.MenuDialogo menu =
				(Interaccion.MenuDialogo)Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[2].valor];
			menu.evento = null;
		}


		public static void elementoDialogoCondicional(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			bool condicion =  Convert.ToBoolean(valoresEntrada[2].valor);
			bool valorCondicion =  Convert.ToBoolean(valoresEntrada[3].valor);
			Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[0].valor].visible = (condicion == valorCondicion);
			Interaccion.MenuDialogo menu =
				(Interaccion.MenuDialogo)Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[1].valor];
			menu.ordenarDialogoInvisible();
		}


		public static void habilitarElementoDialogo(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[0].valor].visible = true;
			Interaccion.MenuDialogo menu =
				(Interaccion.MenuDialogo)Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[1].valor];
			menu.ordenarDialogoInvisible();
		}


		public static void deshabilitarElementoDialogo(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[0].valor].visible = false;
			Interaccion.MenuDialogo menu =
				(Interaccion.MenuDialogo)Gestores.Partidas.Instancia.elementosDialogo[valoresEntrada[1].valor];
			menu.ordenarDialogoInvisible();
		}


		public static void recibirArticulos(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			List<String> lista = valoresEntrada[2].lista;
			int max = lista.Count;
			for(int i=0; i+1<max; i+=2)
			{
				Objetos.Articulo articulo = Gestores.Partidas.Instancia.articulos[lista[i]];
				uint cantidad = Convert.ToUInt32(lista[i + 1]);
				Programa.Jugador.Instancia.protagonista.inventario.addArticulo(articulo, cantidad);
				Programa.Jugador.Instancia.actualizarEspacioInventario();
				Gestores.Partidas.Instancia.gestorMisiones.notify(Interaccion.Mision.Evento.Articulo, null);
			}
		}


		public static void entregarArticulos(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			List<String> lista = valoresEntrada[2].lista;
			int max = lista.Count;
			for(int i=0; i+1<max; i+=2)
			{
				Objetos.Articulo articulo = Gestores.Partidas.Instancia.articulos[lista[i]];
				uint cantidad = Convert.ToUInt32(lista[i + 1]);
				Programa.Jugador.Instancia.protagonista.inventario.removeArticulo(articulo, cantidad);
			}
		}


		public static void comprobarExistenciaArticulos(List<Interaccion.Evento.Argumento> valoresEntrada)
		{
			String articuloID = valoresEntrada[3].valor;
			uint articuloCantidad = Convert.ToUInt32(valoresEntrada[4].valor);
			Objetos.ColeccionArticulos coleccion;
			if(Programa.Jugador.Instancia.protagonista.inventario.articulos.TryGetValue(
				articuloID, out coleccion) == true)
			{
				if(coleccion.cantidad >= articuloCantidad)
					valoresEntrada[2].valor = "true";
				else
					valoresEntrada[2].valor = "false";
				return;
			}
			if(articuloCantidad == 0)
				valoresEntrada[2].valor = "true";
			else
				valoresEntrada[2].valor = "false";
		}
	}
}




