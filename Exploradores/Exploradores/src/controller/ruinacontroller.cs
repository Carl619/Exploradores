using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapa;
using Microsoft.Xna.Framework.Input;




namespace Ruinas
{
	
	
	public class Controller
	{
		public static void activarInterruptor(Reloj reloj)
		{
			Activador interruptor = (Activador)reloj.personaje.objetoInteraccionable;
			interruptor.alternar();
			interruptor.vista.requestUpdateContent();
		}


		public static void abrirTesoro(Reloj reloj)
		{
			Tesoro tesoro = (Tesoro)reloj.personaje.objetoInteraccionable;
			tesoro.personaje = reloj.personaje;
			tesoro.visto = true;
			tesoro.activado = true;
			tesoro.vista.requestUpdateContent();
			Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.tesoroAbierto = tesoro;
			Programa.VistaGeneral.Instancia.contenedorJuego.interfazRuina.abrirInventario(true);
		}


		public static void cerrarTesoro(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Tesoro tesoro = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.tesoroAbierto;
			tesoro.personaje = null;
			Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.tesoroAbierto = null;
			Programa.VistaGeneral.Instancia.contenedorJuego.interfazRuina.abrirInventario(false);
		}

		
		public static void activarPuerta(Reloj reloj)
		{
			Puerta puerta = (Puerta)reloj.personaje.objetoInteraccionable;
			if(puerta.activado == true)
			{
				puerta.actualizarEstado(false);
				return;
			}
			if(puerta.cerradoConLlave == false)
			{
				puerta.actualizarEstado(true);
				return;
			}

			PersonajeRuina personaje = reloj.personaje;
			if(personaje.inventario == null)
				return;
			
			Objetos.ColeccionArticulos coleccion;
			if(personaje.inventario.articulos.TryGetValue(puerta.llave, out coleccion) == false)
				return;
			puerta.cerradoConLlave = false;
			puerta.actualizarEstado(true);
		}


		public static void activarPuertaSalida(Reloj reloj)
		{
			PuertaSalida puerta = (PuertaSalida)reloj.personaje.objetoInteraccionable;
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual;
			PersonajeRuina personaje = reloj.personaje;

			foreach (PersonajeRuina personajeRuina in ruina.personajes)
			{
				if (personajeRuina.habitacion != personaje.habitacion)
				{
					return;
					/*if (Math.Abs(Math.Sqrt(Math.Pow(personaje.posicion.X, 2) + Math.Pow(personaje.posicion.Y, 2))
						- Math.Sqrt(Math.Pow(personaje.posicion.X, 2) + Math.Pow(personaje.posicion.Y, 2))) > 100)
					{
						return;
					}*/
				}
			}

			Gestores.Partidas.Instancia.cambiarMusica("mapa");
			Gestores.Partidas.Instancia.gestorRuinas.personajesRuinas.Clear();
			ruina.personajes.Clear();
			ruina.personajesSeleccionados.Clear();
			personaje.habitacion.personajes.Clear();
			Programa.VistaGeneral.Instancia.scrollableContainer =
				Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelFondo.vistaMapa.contenedorScroll;
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoPartida.Mapa);
		}


		public static void buscarObjeto(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			ObjetoView objetoDestino = (ObjetoView)drawable;
			Tuple<RuinaJugable, Reloj.CallbackFinReloj> ruina = (Tuple<RuinaJugable, Reloj.CallbackFinReloj>)configObj;

			if (objetoDestino.GetType() == typeof(PuertaSalidaView))
			{
				PuertaSalidaView puertaView = (PuertaSalidaView)objetoDestino;
				RuinaNodo nodoDestino1 = new RuinaNodo(new Tuple<int, int>(
							puertaView.puerta.nodoPrincipal.coordenadas.Item1,
							puertaView.puerta.nodoPrincipal.coordenadas.Item2));

				Dictionary<String, Tuple<HabitacionView, RuinaNodo>> destinos =
					new Dictionary<String, Tuple<HabitacionView, RuinaNodo>>();
				destinos.Add(
					puertaView.puerta.habitacionPrincipal.id,
					new Tuple<HabitacionView, RuinaNodo>(
						puertaView.puerta.habitacionPrincipal.vista,
						nodoDestino1));

				ruina.Item1.moverPersonaje(destinos, objetoDestino.objeto, ruina.Item2);
			}
			else if(objetoDestino.GetType() == typeof(PuertaView))
			{
				PuertaView puertaView = (PuertaView)objetoDestino;
				RuinaNodo nodoDestino1 = new RuinaNodo(new Tuple<int, int>(
							puertaView.puerta.nodoPrincipal.coordenadas.Item1,
							puertaView.puerta.nodoPrincipal.coordenadas.Item2));
				RuinaNodo nodoDestino2 = new RuinaNodo(new Tuple<int, int>(
							puertaView.puerta.nodoSecundario.coordenadas.Item1,
							puertaView.puerta.nodoSecundario.coordenadas.Item2));
				
				Dictionary<String, Tuple<HabitacionView, RuinaNodo>> destinos =
					new Dictionary<String, Tuple<HabitacionView,RuinaNodo>>();
				destinos.Add(
					puertaView.puerta.habitacionPrincipal.id,
					new Tuple<HabitacionView,RuinaNodo>(
						puertaView.puerta.habitacionPrincipal.vista,
						nodoDestino1));
				destinos.Add(
					puertaView.puerta.habitacionSecundaria.id,
					new Tuple<HabitacionView,RuinaNodo>(
						puertaView.puerta.habitacionSecundaria.vista,
						nodoDestino2));
				
				ruina.Item1.moverPersonaje(destinos, objetoDestino.objeto, ruina.Item2);
			}
			else
			{
				HabitacionView habitacion = objetoDestino.objeto.habitacion.vista;

				int x, y;
				x = objetoDestino.boundingBox.X + objetoDestino.boundingBox.Width / 2;
				y = objetoDestino.boundingBox.Y + objetoDestino.boundingBox.Height / 2;

				RuinaNodo nodoDestino = new RuinaNodo(new Tuple<int, int>(x, y));

				Dictionary<String, Tuple<HabitacionView, RuinaNodo>> destinos =
					new Dictionary<String, Tuple<HabitacionView,RuinaNodo>>();
				destinos.Add(
					habitacion.habitacion.id,
					new Tuple<HabitacionView,RuinaNodo>(
						habitacion,
						nodoDestino));
				
				ruina.Item1.moverPersonaje(destinos, objetoDestino.objeto, ruina.Item2);
			}
		}


		public static void selectPersonajeHUD(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			AvatarView personajeView = (AvatarView)drawable;
			RuinaJugable ruina = personajeView.personaje.ruina;
			if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl))
				ruina.personajesSeleccionados.Clear();
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
			{
				if (ruina.personajesSeleccionados.ContainsKey(personajeView.personaje.id))
					ruina.personajesSeleccionados.Remove(personajeView.personaje.id);
				else
					ruina.personajesSeleccionados.Add(personajeView.personaje.id, personajeView.personaje);
			}
			else
				ruina.personajesSeleccionados.Add(personajeView.personaje.id, personajeView.personaje);
			Programa.VistaGeneral.Instancia.contenedorJuego.interfazRuina.panelHudPersonajes.requestUpdateContent();
		}


		public static void selectPersonaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			PersonajeRuinaView personajeView = (PersonajeRuinaView)drawable;
			RuinaJugable ruina = personajeView.personaje.ruina;
			if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl))
				ruina.personajesSeleccionados.Clear();
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
			{
				if (ruina.personajesSeleccionados.ContainsKey(personajeView.personaje.id))
					ruina.personajesSeleccionados.Remove(personajeView.personaje.id);
				else
					ruina.personajesSeleccionados.Add(personajeView.personaje.id, personajeView.personaje);
			}
			else
				ruina.personajesSeleccionados.Add(personajeView.personaje.id, personajeView.personaje);
			Programa.VistaGeneral.Instancia.contenedorJuego.interfazRuina.panelHudPersonajes.requestUpdateContent();
		}


		public static void moverPersonaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			HabitacionView habitacionView = (HabitacionView)drawable;
			Habitacion habitacion = habitacionView.habitacion;
			RuinaJugable ruina = habitacion.ruina;
			ILSXNA.Container scrollableContainer = Programa.VistaGeneral.Instancia.scrollableContainer;

			int x, y;
			x = eventInfo.actualPositionX;
			y = eventInfo.actualPositionY;
			x -= habitacionView.getDrawPositionX();
			y -= habitacionView.getDrawPositionY();
			x += (int)habitacionView.getTotalOffsetWidth();
			y += (int)habitacionView.getTotalOffsetHeight();

			int a = - scrollableContainer.getCurrentAlternative().getCurrentLayer().offsetX;
			int b = - scrollableContainer.getCurrentAlternative().getCurrentLayer().offsetY;
			int offsetX = (int)ruina.vista.getParent().getTotalOffsetWidth();
			int offsetY = (int)ruina.vista.getParent().getTotalOffsetHeight();

			if(a > habitacionView.habitacion.espacio.X + offsetX / 2)
				x += a - habitacionView.habitacion.espacio.X - offsetX / 2;
			if(b > habitacionView.habitacion.espacio.Y + offsetY / 2)
				y += b - habitacionView.habitacion.espacio.Y - offsetY / 2;
			
			RuinaNodo nodoDestino = new RuinaNodo(new Tuple<int, int>(x, y));

			Dictionary<String, Tuple<HabitacionView, RuinaNodo>> destinos =
				new Dictionary<String, Tuple<HabitacionView,RuinaNodo>>();
			destinos.Add(
				habitacion.id,
				new Tuple<HabitacionView,RuinaNodo>(
					habitacion.vista,
					nodoDestino));
			
			ruina.moverPersonaje(destinos, null, null);
		}
	}
}




