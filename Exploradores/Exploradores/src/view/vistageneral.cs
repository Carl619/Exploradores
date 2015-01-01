using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mapa;




namespace Programa
{
	
	
	public class VistaGeneral
	{
		// variables
		private static VistaGeneral instancia = null;
		public static VistaGeneral Instancia
		{
			get
			{
				if (instancia == null)
					new VistaGeneral();
				return instancia;
			}
		}
		public ILSXNA.Window window { get; protected set; }
		public MouseState lastMouseState { get; protected set; }
		public MouseState currentMouseState { get; protected set; }
		public KeyboardState lastKeyboardState { get; protected set; }
		public KeyboardState currentKeyboardState { get; protected set; }
		public bool salirDelJuego { get; set; }

		public ContenedorJuego contenedorJuego { get; protected set; }
		public ContenedorMenu contenedorMenu { get; protected set; }
		public Dictionary<String, int> alternativeNames
		{
			get
			{
				if(window == null)
					return null;
				if(window == null)
					return null;
				return window.container.alternativeNames;
			}
		}


		// constructor
		private VistaGeneral()
			: base()
		{
			instancia = this;
			
			window = null;
			lastMouseState = Mouse.GetState();
			currentMouseState = Mouse.GetState();
			lastKeyboardState = Keyboard.GetState();
			currentKeyboardState = Keyboard.GetState();
			salirDelJuego = false;
			
			contenedorJuego = null;
			contenedorMenu = null;

		}


		// funciones
		public void iniWindow()
		{
			window = ILSXNA.Window.Instancia;
			window.container.layout.enableLineWrap = true;
			window.container.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			alternativeNames.Clear();
			alternativeNames.Add("menu", 0);
			alternativeNames.Add("juego", 1);
		}


		public void updateContent()
		{
			if(window == null)
				throw new NullReferenceException();
			
			window.container.clearComponents(true);
			window.container.border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();
			window.container.contentSpacingX = 4;
			window.container.contentSpacingY = 4;

			window.container.setNumberAlternatives(2);

			contenedorMenu = new ContenedorMenu(true);
			window.container.addComponent(contenedorMenu);

			window.container.setCurrentAlternative(1);
			contenedorJuego = new ContenedorJuego(true);
			window.container.addComponent(contenedorJuego);
			
			cambiarVista(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego);
			cambiarVista(Gestores.Partidas.Instancia.gestorPantallas.estadoPartida);
		}


		public void cambiarVista(Gestores.Pantallas.EstadoJuego estadoJuego)
		{
			String alt = "";
			if(estadoJuego == Gestores.Pantallas.EstadoJuego.MenuPrincipal)
				alt = "menu";
			else if(estadoJuego == Gestores.Pantallas.EstadoJuego.Jugando)
			{
				alt = "juego";
				Gestores.Partidas.Instancia.gestorPantallas.estadoMenu = Gestores.Pantallas.EstadoMenu.Invisible;
			}
			window.container.setCurrentAlternative(alternativeNames[alt]);
			Gestores.Partidas.Instancia.gestorPantallas.estadoJuego = estadoJuego;
			contenedorJuego.updateContent();
			cambiarVista(Gestores.Partidas.Instancia.gestorPantallas.estadoPartida);
		}


		public void cambiarVista(Gestores.Pantallas.EstadoPartida estadoPartida)
		{
			Gestores.Pantallas.EstadoPartida estadoAnterior;
			estadoAnterior = Gestores.Partidas.Instancia.gestorPantallas.estadoPartida;
			
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego != Gestores.Pantallas.EstadoJuego.Jugando)
				return;
			
			if(estadoPartida == Gestores.Pantallas.EstadoPartida.Mapa ||
				estadoPartida == Gestores.Pantallas.EstadoPartida.Ciudad)
			{
				if(estadoAnterior == Gestores.Pantallas.EstadoPartida.Mapa ||
					estadoAnterior == Gestores.Pantallas.EstadoPartida.Ciudad)
				{
					contenedorJuego.panelCentral.panelFondo.cambiarAlternativa(estadoPartida);
					contenedorJuego.panelLateral.cambiarAlternativa(estadoPartida);
					Gestores.Partidas.Instancia.gestorPantallas.estadoPartida = estadoPartida;
				}
				else if(estadoAnterior == Gestores.Pantallas.EstadoPartida.Ruina)
				{
					contenedorJuego.panelCentral.panelFondo.cambiarAlternativa(estadoPartida);
					contenedorJuego.panelLateral.cambiarAlternativa(estadoPartida);
					Gestores.Partidas.Instancia.gestorPantallas.estadoPartida = estadoPartida;
				}
			}
			else if(estadoPartida == Gestores.Pantallas.EstadoPartida.Ruina)
			{
				if(estadoAnterior == Gestores.Pantallas.EstadoPartida.Mapa ||
					estadoAnterior == Gestores.Pantallas.EstadoPartida.Ciudad)
				{
					contenedorJuego.cambiarAlternativa(estadoPartida);
					Gestores.Partidas.Instancia.gestorPantallas.estadoPartida = estadoPartida;
				}
			}
		}


		public void draw()
		{
			if(window == null)
				throw new NullReferenceException();
			
			window.draw();
		}


		public void parsearEventos(GameTime gameTime)
		{
			if(window == null)
				throw new NullReferenceException();
			
			actualizarMouseState();
			actualizarKeyboardState();
			uint timer = (uint)gameTime.TotalGameTime.TotalMilliseconds;
			
			//Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
			window.mouseAction(currentMouseState.X, currentMouseState.Y,
								ILS.MouseEvent.Type.Move, timer);
			

			if(comprobarMouseLeftClick() == true)
			{
				window.mouseAction(currentMouseState.X, currentMouseState.Y,
									ILS.MouseEvent.Type.Press, timer);
			}
			
			if(comprobarMouseLeftRelease() == true)
			{
				window.mouseAction(currentMouseState.X, currentMouseState.Y,
									ILS.MouseEvent.Type.Release, timer);
			}

			if(comprobarKeyPress(Keys.Escape) == true)
			{
				Controller.funcionEscapePress(null, null, null);
			}
		}


		public void actualizarMouseState()
		{
			lastMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();
		}


		public void actualizarKeyboardState()
		{
			lastKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();
		}


		public bool comprobarMouseLeftClick()
		{
			if(lastMouseState.LeftButton == ButtonState.Released &&
				currentMouseState.LeftButton == ButtonState.Pressed)
				return true;
			return false;
		}


		public bool comprobarMouseLeftRelease()
		{
			if(lastMouseState.LeftButton == ButtonState.Pressed &&
				currentMouseState.LeftButton == ButtonState.Released)
				return true;
			return false;
		}


		public bool comprobarMouseRightClick()
		{
			if(lastMouseState.RightButton == ButtonState.Released &&
				currentMouseState.RightButton == ButtonState.Pressed)
				return true;
			return false;
		}


		public bool comprobarMouseRightRelease()
		{
			if(lastMouseState.RightButton == ButtonState.Pressed &&
				currentMouseState.RightButton == ButtonState.Released)
				return true;
			return false;
		}


		public bool comprobarKeyPress(Keys key)
		{
			if(lastKeyboardState.IsKeyDown(key) == false &&
				currentKeyboardState.IsKeyDown(key) == true)
				return true;
			return false;
		}


		public bool comprobarKeyRelease(Keys key)
		{
			if(lastKeyboardState.IsKeyDown(key) == true &&
				currentKeyboardState.IsKeyDown(key) == false)
				return true;
			return false;
		}
	}
}




