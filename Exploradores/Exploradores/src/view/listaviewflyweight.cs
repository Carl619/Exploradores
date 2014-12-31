using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Programa
{
	
	
	public class ListaViewFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public ILSXNA.MultiSprite flechaDerechaActivada { get; set; }
		public ILSXNA.MultiSprite flechaIzquierdaActivada { get; set; }
		public ILSXNA.MultiSprite flechaDerechaDesactivada { get; set; }
		public ILSXNA.MultiSprite flechaIzquierdaDesactivada { get; set; }
		public SpriteFont spriteFont { get; set; }
		public Color colorNumeroPaginas { get; set; }


		// constructor
		public ListaViewFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			flechaDerechaActivada = new ILSXNA.MultiSprite();
			flechaIzquierdaActivada = new ILSXNA.MultiSprite();
			flechaDerechaDesactivada = new ILSXNA.MultiSprite();
			flechaIzquierdaDesactivada = new ILSXNA.MultiSprite();
			spriteFont = null;
			colorNumeroPaginas = Color.Black;
		}


		// funciones
		public static ListaViewFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			ListaViewFlyweight vista;
			Texture2D texture;
			
			vista = new ListaViewFlyweight(campos["id"]);

			vista.flechaDerechaActivada = new ILSXNA.MultiSprite();
			texture = Exploradores.Instancia.Content.Load<Texture2D>(campos["flechaDerechaActivada 1"]);
			vista.flechaDerechaActivada.innerComponent.Add(texture);
			texture = Exploradores.Instancia.Content.Load<Texture2D>(campos["flechaDerechaActivada 2"]);
			vista.flechaDerechaActivada.innerComponent.Add(texture);

			vista.flechaIzquierdaActivada = new ILSXNA.MultiSprite();
			texture = Exploradores.Instancia.Content.Load<Texture2D>(campos["flechaIzquierdaActivada 1"]);
			vista.flechaIzquierdaActivada.innerComponent.Add(texture);
			texture = Exploradores.Instancia.Content.Load<Texture2D>(campos["flechaIzquierdaActivada 2"]);
			vista.flechaIzquierdaActivada.innerComponent.Add(texture);

			vista.flechaDerechaDesactivada = new ILSXNA.MultiSprite();
			texture = Exploradores.Instancia.Content.Load<Texture2D>(campos["flechaDerechaDesactivada"]);
			vista.flechaDerechaDesactivada.innerComponent.Add(texture);

			vista.flechaIzquierdaDesactivada = new ILSXNA.MultiSprite();
			texture = Exploradores.Instancia.Content.Load<Texture2D>(campos["flechaIzquierdaDesactivada"]);
			vista.flechaIzquierdaDesactivada.innerComponent.Add(texture);

			vista.spriteFont = Gestores.Gestor<ListaViewFlyweight>.parseFont(campos["spriteFont"]);

			return vista;
		}
	}
}




