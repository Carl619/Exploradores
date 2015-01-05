using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class TrampaLanzador : Trampa
	{
		// variables
		public uint dano { get; set; }
		public ObjetoFlyweight flyweightProyectil { get; set; }
		public Tuple<int, int> origen { get; set; }
		public Tuple<int, int> destino { get; set; }
		public float velocidad { get; set; }
		public int contadorProyectiles { get; set; }


		// constructor
        public TrampaLanzador(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			dano = 0;
			flyweightProyectil = null;
			origen = new Tuple<int, int>(0, 0);
			destino = new Tuple<int, int>(0, 0);
			velocidad = 2.0f;
			contadorProyectiles = 0;
		}


		// funciones
		public static Trampa cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			TrampaLanzador trampa;
			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			ObjetoFlyweight flyweightProyectil = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight proyectil"]];
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];
			
			trampa = new TrampaLanzador(campos["id"], flyweight);
			trampa.activado = Convert.ToBoolean(campos["activado"]);
			trampa.tiempoActivacion = Convert.ToInt32(campos["tiempoActivacion"]);
			trampa.espacio = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										flyweight.ancho,
										flyweight.alto);
			trampa.areaActivacion.espacio = new Rectangle(Convert.ToInt32(campos["area activacion x"]),
											Convert.ToInt32(campos["area activacion y"]),
											Convert.ToInt32(campos["area activacion ancho"]),
											Convert.ToInt32(campos["area activacion alto"]));
			trampa.origen = new Tuple<int, int>(Convert.ToInt32(campos["coordenada x origen"]),
										Convert.ToInt32(campos["coordenada y origen"]));
			trampa.destino = new Tuple<int, int>(Convert.ToInt32(campos["coordenada x destino"]),
										Convert.ToInt32(campos["coordenada y destino"]));
			trampa.flyweightProyectil = flyweightProyectil;
			trampa.dano = Convert.ToUInt32(campos["dano"]);
			trampa.velocidad = Gestores.Mundo.parseFloat(campos["velocidad"]);
			trampa.habitacion = habitacion;
			habitacion.objetos.Add(trampa);

			return trampa;
		}


		public override void activar()
		{
			if(activado == true)
				return;
			Proyectil proyectil;
			String contador = contadorProyectiles.ToString();
			proyectil = new Proyectil("proyectil_" + contador + "_" + id, flyweightProyectil);
			proyectil.dano = dano;
			proyectil.espacio = new Rectangle(origen.Item1,
											origen.Item2,
											objetoFlyweight.ancho,
											objetoFlyweight.alto);
			proyectil.areaActivacion.espacio = new Rectangle(origen.Item1,
															origen.Item2,
															objetoFlyweight.ancho,
															objetoFlyweight.alto);
			proyectil.origen = new Tuple<int, int>(origen.Item1, origen.Item2);
			proyectil.destino = new Tuple<int, int>(destino.Item1, destino.Item2);
			proyectil.velocidad = velocidad;
			proyectil.calcularDistanciaTotal();
			proyectil.habitacion = habitacion;
			habitacion.proyectiles.Add(proyectil);
			habitacion.vista.interfazObjetos.requestUpdateContent();
			base.activar();
		}


		public override void actualizarTiempo(int tiempo)
		{
			base.actualizarTiempo(tiempo);
		}


		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new TrampaLanzadorView(this, ruina);
			return vista;
		}
	}
}




