using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ruinas
{
	public class Tesoro : Objeto
	{
		public int valor { get; set; }


		// cosntructor
        public Tesoro(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			valor = 0;
		}

		public static Tesoro cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Tesoro tesoro;
			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];

			tesoro = new Tesoro(campos["id"], flyweight);
			tesoro.activado = Convert.ToBoolean(campos["activado"]);
			tesoro.espacio = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										flyweight.iconoPasivo.Width,
										flyweight.iconoPasivo.Height);

			tesoro.valor = Convert.ToInt32(campos["valor"]);
			habitacion.objetos.Add(tesoro);

			return tesoro;
		}
	}
}
