using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Objetos;

namespace Ruinas
{
	public class Tesoro : Objeto
	{
		public int cantidad { get; set; }
		public Articulo articulo { get; set; }

		// cosntructor
        public Tesoro(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			cantidad = 0;
			articulo = null;
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

			tesoro.cantidad= Convert.ToInt32(campos["cantidad"]);
			tesoro.articulo = Gestores.Partidas.Instancia.articulos[campos["articulo"]];
			habitacion.objetos.Add(tesoro);

			return tesoro;
		}

		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new TesoroView(this, ruina);
			return vista;
		}
	}
}
