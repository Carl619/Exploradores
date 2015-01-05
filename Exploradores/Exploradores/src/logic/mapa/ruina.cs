using System;
using System.Collections.Generic;




namespace Mapa
{
	
	
	public class Ruina : LugarVisitable
	{
		// variables
		public RuinaFlyweight flyweightRuina { get; protected set; }


		// constructor
		public Ruina(String newID, String newNombre,
						LugarVisitableFlyweight newFlyweightLugar,
						RuinaFlyweight newFlyweightRuina)
			: base(newID, newNombre, newFlyweightLugar)
		{
			if(newFlyweightRuina == null)
				throw new ArgumentNullException();
			flyweightRuina = newFlyweightRuina;
		}


		// funciones
		public static Ruina cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Ruina ruina;

			RuinaFlyweight ruinaFlyweight = Gestores.Mundo.Instancia.ruinaFlyweights[campos["ruina flyweight"]];
			LugarVisitableFlyweight lugarFlyweight = Gestores.Mundo.Instancia.lugarFlyweights[campos["lugar flyweight"]];

			ruina = new Ruina(campos["id"], campos["nombre"], lugarFlyweight, ruinaFlyweight);
			ruina.oculto = Convert.ToBoolean(campos["oculto"]);
			ruina.coordenadas = new Tuple<int, int>(Convert.ToInt32(campos["coordenada x"]), Convert.ToInt32(campos["coordenada y"]));

			Mapa.Instancia.lugares.Add(ruina);
			Gestores.Partidas.Instancia.lugares.Add(ruina.id, ruina);
			return ruina;
		}


		public static String guardarObjeto(Ruina ruina)
		{
			String resultado;
			resultado = "	id						: " + ruina.id + "\n" +
						"	nombre					: " + ruina.nombre + "\n" +
						"	ruina flyweight			: " + ruina.flyweightRuina.id + "\n" +
						"	lugar flyweight			: " + ruina.flyweightLugar.id + "\n" +
						"	coordenada x			: " + ruina.coordenadas.Item1 + "\n" +
						"	coordenada y			: " + ruina.coordenadas.Item2 +  "\n";
			return resultado;
		}


		protected override List<String> getInformacionLugar()
		{
			List<String> listaParrafos = new List<String>();
			listaParrafos.Add("Las ruinas son lugares abandonados, con cierto peligro, pero que puedan contener tesoros.");
			return listaParrafos;
		}
	}
}




