using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;




namespace Programa
{
	
	
	// This is the main type for your game
	public class Exploradores : Microsoft.Xna.Framework.Game
	{
		// variables
		private static Exploradores instance = null;
		public static Exploradores Instancia
		{
			get
			{
				if(instance == null)
					new Exploradores();
				return instance;
			}
		}
		public GraphicsDeviceManager graphics { get; set; }
		public SpriteBatch spriteBatch { get; set; }
		
		
		// constructor
		private Exploradores()
		{
			instance = this;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content/resources";
			VistaGeneral.Instancia.iniWindow();
		}
		

		// Allows the game to perform any initialization it needs to before starting to run.
		// This is where it can query for any required services and load any non-graphic
		// related content.  Calling base.Initialize will enumerate through any components
		// and initialize them as well.
		protected override void Initialize()
		{
			this.IsMouseVisible = true;
			base.Initialize();
		}

		
		// LoadContent will be called once per game and is the place to load
		// all of your content.
		protected override void LoadContent()
		{
			IniModel.iniAll();
			IniView.iniAll();
			
			Gestores.Mundo.Instancia.cargarTodo();
			
			VistaGeneral.Instancia.updateContent();
		}
		

		// UnloadContent will be called once per game and is the place to unload
		// all content.
		protected override void UnloadContent()
		{
		}
		
		
		// Allows the game to run logic such as updating the world,
		// checking for collisions, gathering input, and playing audio.
		protected override void Update(GameTime gameTime)
		{
			if(Gestores.Partidas.Instancia.gestorRuinas.tiempoAnteriorRuina == null)
				Gestores.Partidas.Instancia.gestorRuinas.resetTiempoRuina(gameTime);
			
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();
			
			if(VistaGeneral.Instancia.salirDelJuego == true)
				this.Exit();

			Gestores.Mundo.Instancia.actualizarTodo(gameTime);
			VistaGeneral.Instancia.parsearEventos(gameTime);
			VistaGeneral.Instancia.window.applyUpdateContent();

			base.Update(gameTime);
		}
		
		
		/// This is called when the game should draw itself.
		protected override void Draw(GameTime gameTime)
		{
			VistaGeneral.Instancia.draw();
			base.Draw(gameTime);
		}
	}
}




