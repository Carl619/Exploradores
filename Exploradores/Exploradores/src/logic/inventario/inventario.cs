using System;
using System.Collections.Generic;


namespace Objetos
{
	
	
	public class Inventario : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public uint valor { get; protected set; }
		public uint espacio { get; protected set; }
		public uint espacioMax { get; protected set; }
		public Programa.ListaViewFlyweight flyweight { get; set; }
		public SortedDictionary<String, ColeccionArticulos> articulos { get; set; }
		public ColeccionArticulos articulosSeleccionados { get; set; }


		// constructor
		public Inventario(String newID, Programa.ListaViewFlyweight newFlyweight, uint newEspacioMax = 1)
		{
			if(newFlyweight == null)
				throw new ArgumentNullException();
			
			id = (String)newID.Clone();
			valor = 0;
			espacio = 0;
			espacioMax = newEspacioMax;
			flyweight = newFlyweight;
			articulos = new SortedDictionary<String, ColeccionArticulos>();
			articulosSeleccionados = null;
		}


		// funciones
		public static bool esLista(String campo)
		{
			if(campo.Equals("articulos") == true)
				return true;
			return false;
		}


		public static Inventario cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Inventario inventario;
			Programa.ListaViewFlyweight flyweight = Gestores.Mundo.Instancia.listaViewFlyweights[campos["flyweight"]];

			inventario = new Inventario(campos["id"], flyweight, Convert.ToUInt32(campos["espacio"]));
			foreach(String datosArticulo in listas["articulos"])
			{
				String[] partes = datosArticulo.Split('|');
				inventario.addArticulo(
						Gestores.Partidas.Instancia.articulos[partes[0].Trim()],
						Convert.ToUInt32(partes[1].Trim())
					);
			}

			String articuloSeleccionado = null;
			if(campos.TryGetValue("articulosSeleccionados", out articuloSeleccionado) == true)
			{
				inventario.articulosSeleccionados = inventario.articulos[articuloSeleccionado];
			}

			return inventario;
		}


		public static String guardarObjeto(Inventario inventario)
		{
			String resultado;
			resultado = "	id							: " + inventario.id + "\n" +
						"	flyweight					: " + inventario.flyweight.id + "\n" +
						"	espacio						: " + inventario.espacioMax.ToString() + "\n" +
						"	articulos					: " + inventario.articulos.Count.ToString() + "\n";
			String articulos;
			articulos = "";
			foreach(KeyValuePair<String, ColeccionArticulos> coleccion in inventario.articulos)
			{
				articulos = articulos +
						"		" + coleccion.Value.articulo.id.ToString() +
						" | " + coleccion.Value.cantidad.ToString() + "\n";
			}
			if(inventario.articulosSeleccionados != null)
			{
				return resultado + articulos + 
							"	articulosSeleccionados		: " +
							inventario.articulosSeleccionados.articulo.id.ToString() + "\n";
			}
			return resultado + articulos;
		}


		public void transferenciaArticulos(Inventario inventario, bool entrada)
		{
			if(entrada == false)
				inventario.transferenciaArticulos(this, true);
			
			
			foreach(KeyValuePair<String, ColeccionArticulos> coleccion in inventario.articulos)
			{
				uint cantidad = coleccion.Value.cantidad;
				addArticulo(coleccion.Value.articulo, cantidad);
			}
			inventario.articulos.Clear();
			inventario.valor = 0;
			inventario.espacio = 0;
		}


		public bool addArticulo(Articulo articulo, uint cantidad = 1)
		{
			ColeccionArticulos value = null;
			if(articulos.TryGetValue(articulo.id, out value))
				value.cantidad += cantidad;
			else
				articulos.Add(articulo.id, new ColeccionArticulos(articulo, cantidad));
			
			valor += articulo.valor * cantidad;
			espacio += articulo.peso * cantidad;
			return true;
		}


		public void removeDinero()
		{
			ColeccionArticulos value = null;
			if(articulos.TryGetValue(Articulo.idDinero, out value) == true)
			{
				removeArticulo(value.articulo, value.cantidad);
			}
		}


		public bool removeArticulo(String idArticulo, uint cantidad = 1)
		{
			if(idArticulo == null)
				return false;
			ColeccionArticulos coleccion;
			if(articulos.TryGetValue(idArticulo, out coleccion) == true)
				return removeArticulo(coleccion.articulo, cantidad);
			return false;
		}


		public bool removeArticulo(Articulo articulo, uint cantidad = 1)
		{
			if(articulo == null)
				return false;
			ColeccionArticulos value = null;
			if(articulos.TryGetValue(articulo.id, out value) == true)
			{
				if(value.cantidad < cantidad)
					return false;
				value.cantidad -= cantidad;
				valor -= articulo.valor * cantidad;
				espacio -= articulo.peso * cantidad;
				if(value.cantidad == 0)
					articulos.Remove(articulo.id);
				return true;
			}
			return false;
		}


		public void addEspacio(uint espacioExtra)
		{
			if(espacioExtra > 0)
				espacioMax += espacioExtra;
		}


		public InventarioView crearVista()
		{
			return new InventarioView(this);
		}
	}
}
