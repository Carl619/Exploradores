using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	public partial class PersonajeRuina
	{
		public class Movimiento
		{
			// variables
			public RuinaNodo posicionInicial { get; set; }
			public RuinaNodo posicionFinal { get; set; }
			public int tiempoInicialMovimiento { get; set; }
			public bool colisionFinal { get; set; }
			public double distanciaTotal { get; set; }


			// constructor
			public Movimiento(RuinaNodo origen, RuinaNodo destino, int tiempoMundo, bool colisionFinalObjeto)
			{
				posicionInicial = origen;
				posicionFinal = destino;
				tiempoInicialMovimiento = tiempoMundo;
				colisionFinal = colisionFinalObjeto;
				distanciaTotal = 0.0f;

				calcularDistanciaTotal();
			}


			// funciones
			public void calcularDistanciaTotal()
			{
				double x = posicionFinal.coordenadas.Item1 - posicionInicial.coordenadas.Item1;
				double y = posicionFinal.coordenadas.Item2 - posicionInicial.coordenadas.Item2;
				distanciaTotal = Math.Sqrt(x * x + y * y);
			}


			public Tuple<int, int> getDesplazamiento(int tiempoActual, float velocidad,
														Objeto objetoInteraccionable,
														out bool haLlegado)
			{
				haLlegado = false;
				int difTiempo = tiempoActual - tiempoInicialMovimiento;
				float desplazamiento = velocidad * (float)(tiempoActual - tiempoInicialMovimiento);
				double p = (double)desplazamiento / distanciaTotal;
				if(p < 0.0f)
					p = 0.0f;
				if(p > 1.0f)
				{
					haLlegado = true;
					p = 1.0f;
				}
				
				int x = posicionFinal.coordenadas.Item1 - posicionInicial.coordenadas.Item1;
				int y = posicionFinal.coordenadas.Item2 - posicionInicial.coordenadas.Item2;

				if(objetoInteraccionable != null && colisionFinal == true)
				{
					double pMax;
					int distMaxX = x;
					if(distMaxX < 0)
						distMaxX = - distMaxX;
					int dist = objetoInteraccionable.espacio.Width / 2 + ancho / 2;
					if(distMaxX < dist)
					{
						int distMaxY = y;
						if(distMaxY < 0)
							distMaxY = - distMaxY;
						dist = objetoInteraccionable.espacio.Height / 2 + alto / 2;
						if(distMaxY < dist)
							pMax = 0.0f;
						else
							pMax = ((double)(distMaxY - dist) / (double)distMaxY);
					}
					else
						pMax = ((double)(distMaxX - dist) / (double)distMaxX);
					if(p > pMax)
					{
						haLlegado = true;
						p = pMax;
					}
				}

				return new Tuple<int,int>(posicionInicial.coordenadas.Item1 + (int)(p * x),
										posicionInicial.coordenadas.Item2 + (int)(p * y));
			}
		}
	}
}




