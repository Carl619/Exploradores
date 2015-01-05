using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class Viaje
	{
		// variables
		public static double consumoPorHoraBase = 0.12f;
		public static double probabilidadAtaqueBase = 0.05f;

		public List<Dijkstra.IRama> camino { get; set; }
		public LugarVisitable destinoSiguiente { get; set; }
		public LugarVisitable destinoFinal { get; set; }
		public double factorSobrepesoInventario { get; set; }
		public double distanciaTotal { get; set; }
		public double peligroTotal { get; set; }
		public double distanciaSiguiente { get; set; }
		public double peligro { get; set; }
		public double defensa { get; set; }
		public double probAtaque { get; set; }
		public double caza { get; set; }
		public uint comidaDisponible { get; set; }
		public double comidaNecesaria { get; set; }
		

		// constructor
		public Viaje()
		{
			clear(true);
		}


		// funciones
		public void clear(bool todo)
		{
			if(todo == true)
			{
				camino = null;
				destinoFinal = null;
			}
			destinoSiguiente = null;
			factorSobrepesoInventario = 1.0f;
			distanciaTotal = 0.0f;
			peligroTotal = 0.0f;
			distanciaSiguiente = 0.0f;
			defensa = 0.0f;
			peligro = 0.0f;
			probAtaque = 0.0f;
			caza = 0.0f;
			comidaDisponible = 0;
			comidaNecesaria = 0.0f;
		}


		public void asignar(List<Dijkstra.IRama> newCamino, LugarVisitable destino)
		{
			clear(true);

			if(newCamino.Count > 0)
			{
				camino = newCamino;
				destinoFinal = destino;
				actualizar();
			}
		}


		public void actualizar()
		{
			clear(false);

			Ruta ruta = (Ruta)camino[0];
			destinoSiguiente = (LugarVisitable)
				(ruta.verticeAdyacente(Programa.Jugador.Instancia.protagonista.lugarActual));
			
			foreach(Dijkstra.IRama rama in camino)
			{
				distanciaTotal += ((Ruta)rama).distancia;
				peligroTotal += ((Ruta)rama).peligro;
			}

			factorSobrepesoInventario = (float)Programa.Jugador.Instancia.protagonista.inventario.factorSobrePeso;
			distanciaSiguiente = ruta.distancia * factorSobrepesoInventario;
			distanciaTotal *= factorSobrepesoInventario;

			double probabilidad = 1.0f - probabilidadAtaqueBase;
			double consumoPorHora = 1 + Programa.Jugador.Instancia.acompanantes.Count;
			consumoPorHora *= consumoPorHoraBase;
			
			caza = Programa.Jugador.Instancia.getHabilidadCaza();
			caza *= distanciaSiguiente;
			
			defensa = Programa.Jugador.Instancia.getHabilidadDefensa();
			if(defensa < 1.0f)
				defensa = 1.0f;
			peligro = ruta.peligro;
			if(peligro < 1.0f)
				peligro = 1.0f;
			double factorAtaque = peligro / defensa;

			probAtaque = Math.Pow(probabilidad, factorAtaque * factorSobrepesoInventario);
			probAtaque = 100.0f * (1.0f - probAtaque);

			comidaNecesaria = ruta.distancia * consumoPorHora;
			comidaDisponible = Programa.Jugador.Instancia.protagonista.comidaDisponible();
		}
	}
}




