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
		public String id { get; protected set; }
		public List<Texture2D> iconosParado { get; set; }
		public List<Texture2D> iconosActivado { get; set; }
		public List<Texture2D> iconosParadoSeleccionado { get; set; }
		public List<Texture2D> iconosActivadoSeleccionado { get; set; }
		public List<Texture2D> iconosMovimiento { get; set; }
		public float velocidadAnimacion { get; set; }
		public int ancho { get; set; }
		public int alto { get; set; }


		// constructor
		public ObjetoFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			iconosParado = new List<Texture2D>();
			iconosActivado = new List<Texture2D>();
			iconosParadoSeleccionado = new List<Texture2D>();
			iconosActivadoSeleccionado = new List<Texture2D>();
			iconosMovimiento = new List<Texture2D>();
			velocidadAnimacion = 1.0f;
			ancho = 1;
			alto = 1;
		}


		// funciones
		public static bool esLista(String campo)
		{
			if(campo.Equals("iconosParado") == true)
				return true;
			else if(campo.Equals("iconosActivado") == true)
				return true;
			else if(campo.Equals("iconosParadoSeleccionado") == true)
				return true;
			else if(campo.Equals("iconosActivadoSeleccionado") == true)
				return true;
			else if(campo.Equals("iconosMovimiento") == true)
				return true;
			return false;
		}


		public static ObjetoFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			ObjetoFlyweight objetoFlyweight;

			objetoFlyweight = new ObjetoFlyweight(campos["id"]);

			List<String> lista;
			if(listas.TryGetValue("iconosParado", out lista) == true)
			{
				foreach(String icono in lista)
					objetoFlyweight.iconosParado.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + icono));
			}
			if(listas.TryGetValue("iconosActivado", out lista) == true)
			{
				foreach(String icono in lista)
					objetoFlyweight.iconosActivado.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + icono));
			}
			if(listas.TryGetValue("iconosParadoSeleccionado", out lista) == true)
			{
				foreach(String icono in lista)
					objetoFlyweight.iconosParadoSeleccionado.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + icono));
			}
			if(listas.TryGetValue("iconosActivadoSeleccionado", out lista) == true)
			{
				foreach(String icono in lista)
					objetoFlyweight.iconosActivadoSeleccionado.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + icono));
			}
			if(listas.TryGetValue("iconosMovimiento", out lista) == true)
			{
				foreach(String icono in lista)
					objetoFlyweight.iconosMovimiento.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@"images/icons/objects/" + icono));
			}

			String campo;
			if(campos.TryGetValue("velocidadAnimacion", out campo) == true)
				objetoFlyweight.velocidadAnimacion = Gestores.Mundo.parseFloat(campo);
			if(campos.TryGetValue("ancho", out campo) == true)
				objetoFlyweight.ancho = Convert.ToInt32(campo);
			else
				objetoFlyweight.ancho = objetoFlyweight.iconosParado[0].Width;
			if(campos.TryGetValue("alto", out campo) == true)
				objetoFlyweight.alto = Convert.ToInt32(campo);
			else
				objetoFlyweight.alto = objetoFlyweight.iconosParado[0].Height;
			
			return objetoFlyweight;
		}
	}
}
