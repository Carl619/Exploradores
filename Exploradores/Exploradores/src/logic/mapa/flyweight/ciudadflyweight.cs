using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Mapa
{
	
	
	public class CiudadFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public static SpriteFont spriteFontAusente = null;
		public String id { get; protected set; }
		public SpriteFont spriteFont { get; set; }
		public Color colorTexto { get; set; }
		


		// constructor
		public CiudadFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			spriteFont = null;
			colorTexto = Color.Black;
		}


		// funciones
		public static CiudadFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			CiudadFlyweight ciudadFlyweight;

			ciudadFlyweight = new CiudadFlyweight(campos["id"]);
			ciudadFlyweight.spriteFont = Gestores.Gestor<CiudadFlyweight>.parseFont(campos["spriteFont"]);
			
			return ciudadFlyweight;
		}
	}
}




