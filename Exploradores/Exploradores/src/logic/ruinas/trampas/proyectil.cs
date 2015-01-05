using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public class Proyectil : TrampaFisica
	{
		// variables
		public float velocidad { get; set; }
		public Tuple<int, int> origen { get; set; }
		public Tuple<int, int> destino { get; set; }
		public double distanciaTotal { get; set; }
		public bool terminado { get; set; }


		// constructor
        public Proyectil(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			velocidad = 2.0f;
			origen = new Tuple<int,int>(0, 0);
			destino = new Tuple<int,int>(0, 0);
			distanciaTotal = 1.0f;
			terminado = false;

			tiempoActivacion = 100000000;
		}


		// funciones
		public override void activar()
		{
			int t = tiempoInicial;
			base.activar();
			tiempoInicial = t;
		}


		public override void actualizarTiempo(int tiempo)
		{
			if(tiempoInicialAsignado == false)
			{
				tiempoInicial = tiempo;
				tiempoInicialAsignado = true;
			}
			tiempoActual = tiempo;

			int difTiempo = tiempoActual - tiempoInicial;
			float desplazamiento = velocidad * (float)difTiempo;
			double p = (double)desplazamiento / distanciaTotal;
			if(p < 0.0f)
				p = 0.0f;
			if(p > 1.0f)
			{
				terminado = true;
				p = 1.0f;
			}
			espacio = new Rectangle(
						origen.Item1 + (int)(p * ((double)destino.Item1 - (double)origen.Item1)),
						origen.Item2 + (int)(p * ((double)destino.Item2 - (double)origen.Item2)),
						espacio.Width, espacio.Height);
			areaActivacion.espacio = new Rectangle(espacio.X, espacio.Y, espacio.Width, espacio.Height);
			getImagenActual();

			foreach(Ruinas.PersonajeRuina personaje in habitacion.personajes)
			{
				if(espacio.Intersects(personaje.posicion) == true)
					hacerDano(personaje);
			}
		}


		public override void hacerDano(Ruinas.PersonajeRuina personaje)
		{
			if(terminado == false)
			{
				terminado = true;
				personaje.hacerDano(dano);
			}
		}


		public void calcularDistanciaTotal()
		{
			double x = destino.Item1 - origen.Item1;
			double y = destino.Item2 - origen.Item2;
			distanciaTotal = Math.Sqrt(x*x + y*y);
		}


		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new ProyectilView(this, ruina);
			return vista;
		}


		protected void getImagenActual()
		{
			int nImagenes = objetoFlyweight.iconosMovimiento.Count;
			if(nImagenes == 0)
				return;
			imagenActual = ((int)((float)tiempoActual / (float)objetoFlyweight.velocidadAnimacion)) % nImagenes;
		}
	}
}




