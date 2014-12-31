using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Mapa
{
	
	
	public class RuinaFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }


		// constructor
		public RuinaFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
		}


		// funciones
		public static RuinaFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			RuinaFlyweight ruinaFlyweight;

			ruinaFlyweight = new RuinaFlyweight(campos["id"]);
			
			return ruinaFlyweight;
		}
	}
}




