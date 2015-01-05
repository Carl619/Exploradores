using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public class PersonajeRuinaFlyweight : Gestores.IObjetoIdentificable
	{
		public String id { get; protected set; }
		public float velocidadMovimiento { get; protected set; } // pixeles / unidadTiempo
		public uint velocidadAnimacion { get; protected set; } // unidadTiempo / imagen
		public Texture2D imagenParado { get; set; }
		public List<Texture2D> imagenesMovimiento { get; set; }


		// constructor
		public PersonajeRuinaFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			velocidadMovimiento = 4.6f;
			velocidadAnimacion = 4;
			imagenParado = null;
			imagenesMovimiento = new List<Texture2D>();
			
		}


		// funciones
		public static PersonajeRuinaFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			PersonajeRuinaFlyweight flyweight;
			
			flyweight = new PersonajeRuinaFlyweight(campos["id"]);
			flyweight.velocidadMovimiento = Gestores.Mundo.parseFloat(campos["velocidadMovimiento"]);
			flyweight.velocidadAnimacion = Convert.ToUInt32(campos["velocidadAnimacion"]);
			String nombre = "images/sprites/ruin/personajes/" + campos["carpeta"] + "/parado";
			flyweight.imagenParado = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@nombre);
			for(int i=0; i < Convert.ToInt32(campos["numero imagenes animacion"]); ++i)
			{
				nombre = "images/sprites/ruin/personajes/" + campos["carpeta"] + "/" + "movimiento" + i.ToString();
				flyweight.imagenesMovimiento.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@nombre));
			}

			return flyweight;
		}
	}
}




