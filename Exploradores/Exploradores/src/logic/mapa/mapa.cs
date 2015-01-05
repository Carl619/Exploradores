using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class Mapa
	{
		// variables
		private static Mapa instancia = null;
		public static Mapa Instancia
		{
			get
			{
				if (instancia == null)
					new Mapa();
				return instancia;
			}
		}
		public List<LugarVisitable> lugares { get; set; }
		public List<Ruta> rutas { get; set; }
		public List<Texture2D> imagenesMapa { get; set; }
		public List<Vector2> offsetsImagenesMapa { get; set; }
		public Texture2D marcadorJugador { get; set; }
		public SpriteFont spriteFont { get; set; }
		public Color colorTexto { get; set; }


		// constructor
		private Mapa()
		{
			instancia = this;
			lugares = new List<LugarVisitable>();
			rutas = new List<Ruta>();
			imagenesMapa = new List<Texture2D>();
			offsetsImagenesMapa = new List<Vector2>();
			marcadorJugador = null;
			spriteFont = null;
			colorTexto = Color.Black;
		}


		// funciones
		public MapaViewCentro crearVistaCentral()
		{
			return new MapaViewCentro(this);
		}

		
		public MapaViewLateral crearVistaLateral(Programa.PanelLateral panelLateral)
		{
			return new MapaViewLateral(this, panelLateral);
		}
	}
}




