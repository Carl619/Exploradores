using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public abstract class LugarVisitable : ElementoMapa, Dijkstra.INodo
	{
		// variables
		public LugarVisitableFlyweight flyweightLugar { get; protected set; }
		public List<Ruta> rutas { get; protected set; }
		public Tuple<int, int> coordenadas { get; set; }
		public String nombre { get; protected set; }
		public List<String> informacionEspecifica { get; protected set; }
		public int indiceGrafo { get; set; }
		public LugarVisitableView vista { get; protected set; }
		

		// constructor
		public LugarVisitable(String newID, String newNombre, LugarVisitableFlyweight newFlyweight)
			: base(newID)
		{
			if(newFlyweight == null)
				throw new ArgumentNullException();
			
			flyweightLugar = newFlyweight;
			rutas = new List<Ruta>();
			coordenadas = new Tuple<int, int>(0, 0);
			nombre = (String)newNombre.Clone();
			informacionEspecifica = new List<string>();
			indiceGrafo = 0;
		}


		// funciones
		public List<String> getInformacion()
		{
			List<String> lista = getInformacionLugar();
			lista.AddRange(informacionEspecifica);
			return lista;
		}


		public List<Dijkstra.IRama> ramasAdyacentes()
		{
			List<Dijkstra.IRama> listaRamas = new List<Dijkstra.IRama>();
			foreach(Ruta ruta in rutas)
				listaRamas.Add(ruta);
			
			return listaRamas;
		}


		public void addRuta(Ruta ruta)
		{
			if (ruta == null)
				return;
			rutas.Add(ruta);
		}


		public LugarVisitableView crearVista()
		{
			vista = new LugarVisitableView(this);
			return vista;
		}


		protected abstract List<String> getInformacionLugar();
	}
}




