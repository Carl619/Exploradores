using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public class RelojFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public List<Texture2D> iconos { get; protected set; }


		// constructor
		public RelojFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			iconos = new List<Texture2D>();
		}


		// funciones
		public static RelojFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Ruinas.RelojFlyweight relojFlyweight;

			relojFlyweight = new Ruinas.RelojFlyweight(campos["id"]);
			for (int i = 0; i < Convert.ToInt32(campos["numero imagenes"]); ++i)
			{
				String nombre = "images/icons/objects/" + campos["carpeta"] + "/" + i.ToString();
				relojFlyweight.iconos.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@nombre));
			}

			return relojFlyweight;
		}
	}
}




