using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;




namespace Objetos
{


	public class Articulo : Gestores.IObjetoIdentificable
	{
		// variables
		public static String idDinero = "idDinero";
		public static String idComida = "idComida";
		public String id { get; protected set; }
		public String nombre { get; protected set; }
		public uint peso { get; set; }
		public uint valor { get; set; }
		public ArticuloFlyweight flyweight { get; set; }

		// constructor
		public Articulo(String newID, String newNombre, ArticuloFlyweight newFlyweight)
		{
			if (newFlyweight == null)
				throw new ArgumentNullException();
			id = (String)newID.Clone();
			nombre = (String)newNombre.Clone();
			peso = 1;
			valor = 1;
			flyweight = newFlyweight;
		}


		// funciones
		public static Articulo cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Articulo articulo;
			ArticuloFlyweight articuloFlyweight = Gestores.Mundo.Instancia.articuloFlyweights[campos["flyweight"]];

			articulo = new Objetos.Articulo(campos["id"], campos["nombre"], articuloFlyweight);
			articulo.peso = Convert.ToUInt32(campos["peso"]);
			articulo.valor = Convert.ToUInt32(campos["valor"]);

			return articulo;
		}


		public static String guardarObjeto(Articulo articulo)
		{
			String resultado;
			resultado = "	id						: " + articulo.id + "\n" +
						"	nombre					: " + articulo.nombre + "\n" +
						"	flyweight				: " + articulo.flyweight.id + "\n" +
						"	peso					: " + articulo.peso.ToString() + "\n" +
						"	valor					: " + articulo.valor.ToString() + "\n";
			return resultado;
		}
	}


	public class ColeccionArticulos
	{
		// variables
		public Articulo articulo { get; set; }
		public ArticuloView vista { get; protected set; }
		public uint cantidad { get; set; }
		public uint peso { get { return cantidad * articulo.peso; } }
		public uint valor { get { return cantidad * articulo.valor; } }


		// constructor
		public ColeccionArticulos(Articulo newArticulo, uint newCantidad = 1)
		{
			if (newArticulo == null)
				throw new ArgumentNullException();
			
			articulo = newArticulo;
			vista = null;

			cantidad = newCantidad;
		}


		// funciones
		public ArticuloView crearVista(bool seleccionado, bool actualizarVista)
		{
			if(vista == null)
				vista = new ArticuloView(this, seleccionado, actualizarVista);
			return vista;
		}
	}
}




