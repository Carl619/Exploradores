using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Ruinas
{
	public class ObjetoFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public static Texture2D iconoAusente = null;
		public String id { get; protected set; }
		public Texture2D iconoPasivo { get; set; }
		public Texture2D iconoActivo { get; set; }
		public Texture2D iconoSeleccionado { get; set; }
		public Texture2D iconoActivoSeleccionado { get; set; }


		// constructor
		public ObjetoFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			iconoPasivo = null;
			iconoActivo = null;
			iconoSeleccionado = null;
			iconoActivoSeleccionado = null;
		}


		// funciones
		public static ObjetoFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			ObjetoFlyweight objetoFlyweight;

			objetoFlyweight = new ObjetoFlyweight(campos["id"]);
			objetoFlyweight.iconoPasivo = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + campos["iconoPasivo"]);
			objetoFlyweight.iconoActivo = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + campos["iconoActivo"]);
			objetoFlyweight.iconoSeleccionado = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + campos["iconoSeleccionado"]);
			objetoFlyweight.iconoActivoSeleccionado = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + campos["iconoActivoSeleccionado"]);
			
			return objetoFlyweight;
		}
	}
}
