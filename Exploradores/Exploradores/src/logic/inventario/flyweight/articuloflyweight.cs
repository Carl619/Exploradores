using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Objetos
{
	
	
	public class ArticuloFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public Texture2D icono { get; set; }
		public Color colorNombre { get; set; }
		public Color colorCantidad { get; set; }
		public Color colorValorTotal { get; set; }
		public Color colorPesoTotal { get; set; }
		public Color colorValorUnitario { get; set; }
		public Color colorPesoUnitario { get; set; }


		// constructor
		public ArticuloFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			icono = null;
			colorNombre = Color.Black;
			colorCantidad = Color.Black;
			colorValorTotal = Color.Black;
			colorPesoTotal = Color.Black;
			colorValorUnitario = Color.Black;
			colorPesoUnitario = Color.Black;
		}


		// funciones
		public static ArticuloFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			ArticuloFlyweight articuloFlyweight;

			articuloFlyweight = new Objetos.ArticuloFlyweight(campos["id"]);
			articuloFlyweight.icono =
				Programa.Exploradores.Instancia.Content.Load<Texture2D>(@campos["icono"]);
			
			return articuloFlyweight;
		}
	}
}




