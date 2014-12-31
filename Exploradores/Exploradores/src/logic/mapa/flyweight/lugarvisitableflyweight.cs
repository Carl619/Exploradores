using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class LugarVisitableFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public static Texture2D iconoAusente  = null;
		public String id { get; protected set; }
		public Texture2D iconoPasivo { get; set; }
		public Texture2D iconoActivo { get; set; }
		public Texture2D iconoSeleccionado { get; set; }
		public Texture2D iconoActivoSeleccionado { get; set; }
		public String nombreTipoLugar { get; set; }


		// constructor
		public LugarVisitableFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			iconoPasivo = null;
			iconoActivo = null;
			iconoSeleccionado = null;
			iconoActivoSeleccionado = null;
			nombreTipoLugar = "";
		}


		// funciones
		public static LugarVisitableFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			LugarVisitableFlyweight lugarFlyweight;

			lugarFlyweight = new LugarVisitableFlyweight(campos["id"]);
			lugarFlyweight.iconoPasivo =
				Programa.Exploradores.Instancia.Content.Load<Texture2D>(@campos["iconoPasivo"]);
			lugarFlyweight.iconoActivo =
				Programa.Exploradores.Instancia.Content.Load<Texture2D>(@campos["iconoActivo"]);
			lugarFlyweight.iconoSeleccionado =
				Programa.Exploradores.Instancia.Content.Load<Texture2D>(@campos["iconoSeleccionado"]);
			lugarFlyweight.iconoActivoSeleccionado =
				Programa.Exploradores.Instancia.Content.Load<Texture2D>(@campos["iconoActivoSeleccionado"]);
			lugarFlyweight.nombreTipoLugar = campos["nombreTipoLugar"];
			
			return lugarFlyweight;
		}
	}
}




