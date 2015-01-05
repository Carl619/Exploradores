using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class Ruta : ElementoMapa, Dijkstra.IRama
	{
		// variables
		public Tuple<LugarVisitable, LugarVisitable> extremos { get; protected set; }
		public float distancia { get; set; }
		public float peligro { get; set; }
		public RutaFlyweight flyweight { get; set; }


		// constructor
		public Ruta(String newID, LugarVisitable lugar1, LugarVisitable lugar2, RutaFlyweight newFlyweight, bool visible = true)
			: base(newID, visible)
		{
			if(lugar1 == null || lugar2 == null || newFlyweight == null)
				throw new ArgumentNullException();
			
			extremos = new Tuple<LugarVisitable, LugarVisitable>(lugar1, lugar2);

			distancia = 1.0f;
			peligro = 0.0f;
			flyweight = newFlyweight;

			lugar1.addRuta(this);
			lugar2.addRuta(this);
		}


		// funciones
		public static Ruta cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Ruta ruta;

			RutaFlyweight rutaFlyweight = Gestores.Mundo.Instancia.rutaFlyweights[campos["flyweight"]];
			LugarVisitable lugarOrigen = Gestores.Partidas.Instancia.lugares[campos["lugar A"]];
			LugarVisitable lugarDestino = Gestores.Partidas.Instancia.lugares[campos["lugar B"]];

			ruta = new Ruta(campos["id"], lugarOrigen, lugarDestino, rutaFlyweight);
			ruta.oculto = Convert.ToBoolean(campos["oculto"]);
			ruta.distancia = (float)Gestores.Mundo.parseFloat(campos["distancia"]);
			ruta.peligro = (float)Gestores.Mundo.parseFloat(campos["peligro"]);

			Mapa.Instancia.rutas.Add(ruta);
			return ruta;
		}


		public static String guardarObjeto(Ruta ruta)
		{
			String resultado;
			resultado = "	id						: " + ruta.id + "\n" +
						"	flyweight				: " + ruta.flyweight.id + "\n" +
						"	lugar A					: " + ruta.extremos.Item1.id + "\n" +
						"	lugar B					: " + ruta.extremos.Item2.id + "\n" +
						"	distancia				: " + ruta.distancia.ToString() + "\n" +
						"	peligro					: " + ruta.peligro.ToString() +  "\n";
			return resultado;
		}


		public double getPeso(int criterio)
		{
			if(criterio == 0)
				return distancia;
			return peligro;
		}


		public Dijkstra.INodo verticeAdyacente(Dijkstra.INodo nodoActual)
		{
			if(nodoActual == extremos.Item1)
				return extremos.Item2;
			if(nodoActual == extremos.Item2)
				return extremos.Item1;
			return null;
		}


		public RutaView crearVista(bool parteCamino)
		{
			return new RutaView(this, parteCamino);
		}
	}
}




