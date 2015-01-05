using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Programa
{
	

	public static class IniView
	{
		// functions
		public static void iniAll()
		{
			Programa.Exploradores.Instancia.spriteBatch =
				new SpriteBatch(Programa.Exploradores.Instancia.GraphicsDevice);

			iniVentanaDialogoView();
			iniMapaView();
		}


		public static void iniVentanaDialogoView()
		{
			Interaccion.VentanaDialogo.Instancia.fuenteTitulo = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Interaccion.VentanaDialogo.Instancia.fuenteCuerpo = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Interaccion.VentanaDialogo.Instancia.fuenteOpcion = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
		}


		public static void iniMapaView()
		{
			Mapa.Mapa.Instancia.imagenesMapa.Add(
				Exploradores.Instancia.Content.Load<Texture2D>(@"images/sprites/map/mapa1"));
			Mapa.Mapa.Instancia.imagenesMapa.Add(
				Exploradores.Instancia.Content.Load<Texture2D>(@"images/sprites/map/mapa2"));
			Mapa.Mapa.Instancia.imagenesMapa.Add(
				Exploradores.Instancia.Content.Load<Texture2D>(@"images/sprites/map/mapa3"));
			Mapa.Mapa.Instancia.offsetsImagenesMapa.Add(
				new Vector2(300, 0));
			Mapa.Mapa.Instancia.offsetsImagenesMapa.Add(
				new Vector2(0, 400));
			Mapa.Mapa.Instancia.offsetsImagenesMapa.Add(
				new Vector2(1050, 450));
			
			Mapa.Mapa.Instancia.marcadorJugador =
				Exploradores.Instancia.Content.Load<Texture2D>(@"images/sprites/map/flag");
			Mapa.Mapa.Instancia.spriteFont =
				Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
		}
	}
}




