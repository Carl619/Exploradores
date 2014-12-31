using System;
using System.Collections.Generic;
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
			Mapa.Mapa.Instancia.imagenMapa =
				Exploradores.Instancia.Content.Load<Texture2D>(@"images/sprites/map/mapa640x512");
			Mapa.Mapa.Instancia.marcadorJugador =
				Exploradores.Instancia.Content.Load<Texture2D>(@"images/sprites/map/flag");
			Mapa.Mapa.Instancia.spriteFont =
				Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
		}
	}
}




