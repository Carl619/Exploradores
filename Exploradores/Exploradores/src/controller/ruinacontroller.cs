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

		public static void activarTesoro(Reloj reloj)
		{
			Tesoro tesoro = (Tesoro)reloj.personaje.objetoInteraccionable;

				Gestores.Partidas.Instancia.gestorPantallas.estadoHUD =
					Gestores.Pantallas.EstadoHUD.Inventario;
				Programa.PanelHUD panelHUD = Programa.VistaGeneral.Instancia.contenedorJuego.panelCentral.panelHUD;
				panelHUD.panelVentanas.cambiarAlternativa(Gestores.Pantallas.EstadoHUD.Inventario);
				panelHUD.panelVentanas.requestUpdateContent();
				panelHUD.actualizarBloqueoVentana();

		}

		public static void activarPuertaSalida(Reloj reloj)
		{
			PuertaSalida puerta = (PuertaSalida)reloj.personaje.objetoInteraccionable;

			PersonajeRuina personaje = reloj.personaje;

			Gestores.Partidas.Instancia.cambiarMusica("mapa");
			Programa.VistaGeneral.Instancia.cambiarVista(Gestores.Pantallas.EstadoPartida.Mapa);
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
			if(personaje.personaje.inventario == null)
				return;
			
			Objetos.ColeccionArticulos coleccion;
			if(personaje.personaje.inventario.articulos.TryGetValue(puerta.llave, out coleccion) == false)
				return;
			puerta.cerradoConLlave = false;
			puerta.actualizarEstado(true);
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
				HabitacionView habitacion = (HabitacionView)(objetoDestino.getParent());

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


		public static void selectPersonaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			PersonajeRuinaView personajeView = (PersonajeRuinaView)drawable;
			RuinaJugable ruina = personajeView.personaje.ruina;
			if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl))
			ruina.personajesSeleccionados.Clear();
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
			{
				if (ruina.personajesSeleccionados.Contains(personajeView.personaje))
					ruina.personajesSeleccionados.Remove(personajeView.personaje);
				else
					ruina.personajesSeleccionados.Add(personajeView.personaje);
			}
			else
			ruina.personajesSeleccionados.Add(personajeView.personaje);
		}


		public static void moverPersonaje(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			HabitacionView habitacionView = (HabitacionView)drawable;
			Habitacion habitacion = habitacionView.habitacion;
			RuinaJugable ruina = habitacion.ruina;

			int x, y;
			x = eventInfo.actualPositionX;
			y = eventInfo.actualPositionY;
			x -= habitacionView.getDrawPositionX();
			y -= habitacionView.getDrawPositionY();
			x += (int)habitacionView.getTotalOffsetWidth();
			y += (int)habitacionView.getTotalOffsetHeight();

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




