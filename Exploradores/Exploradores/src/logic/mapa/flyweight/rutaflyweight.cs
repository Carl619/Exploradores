using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class RutaFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public static Texture2D spriteAusente  = null;
		public String id { get; protected set; }
		public Texture2D sprite { get; set; }
		public Color colorPasivo { get; set; }
		public uint anchuraPasivo { get; set; }
		public Color colorActivo { get; set; }
		public uint anchuraActivo { get; set; }


		// constructor
		public RutaFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			sprite = null;
			colorPasivo = Color.Green;
			anchuraPasivo = 2;
			colorActivo = Color.Red;
			anchuraActivo = 3;
		}


		// funciones
		public static RutaFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			RutaFlyweight rutaFlyweight;

			rutaFlyweight = new RutaFlyweight(campos["id"]);
			rutaFlyweight.sprite = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@campos["sprite"]);
			
			return rutaFlyweight;
		}
	}
}




