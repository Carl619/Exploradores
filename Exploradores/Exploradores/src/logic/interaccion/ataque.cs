using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	
	
	public class Ataque
	{
		// enumeraciones
		public enum Atacantes
		{
			Ladrones,
			Asesinos,
			Animales
		}


		// variables
		public static double habilidadBase = 2.0f;
		public static double precioBase = 14.0f;

		public static double factorProbabilidadLadrones = 20.0f;
		public static double factorHabilidadLadrones = 0.8f;
		public static double factorPrecioLadrones = 1.0f;
		
		public static double factorProbabilidadAsesinos = 5.0f;
		public static double factorHabilidadAsesinos = 2.5f;
		public static double factorPrecioAsesinos = 5.4f;

		public static double factorProbabilidadAnimales = 12.0f;
		public static double factorHabilidadAnimales = 1.0f;
		public static double factorPrecioAnimales = 0.1f;

		public Atacantes atacantes { get ; set; }
		public uint numero { get ; set; }
		public double determinacion { get ; set; }
		public double habilidadTotal { get ; set; }
		public uint precioTotal { get ; set; }
		

		// constructor
		public Ataque(uint cardinalidad, double det)
		{
			if(det > 1.0f)
				det = 1.0f;
			getAtacantes();
			
			if(atacantes == Atacantes.Ladrones)
				setLadrones(cardinalidad, det);
			else if(atacantes == Atacantes.Asesinos)
				setAsesinos(cardinalidad, det);
			else if(atacantes == Atacantes.Animales)
				setAnimales(cardinalidad, det);
			else
				throw new ArgumentException();
		}


		// funciones
		protected void getAtacantes()
		{
			double factorTotal = factorProbabilidadLadrones +
								factorProbabilidadAsesinos +
								factorProbabilidadAnimales;
			int factor = 1000000;
			double probabilidadLadrones = factor * (factorProbabilidadLadrones / factorTotal);
			double probabilidadAsesinos = factor * (factorProbabilidadAsesinos / factorTotal);
			double probabilidadAnimales = factor * (factorProbabilidadAnimales / factorTotal);

			int resultado = Gestores.Partidas.Instancia.aleatorio.Next(0, factor);

			if(resultado < probabilidadLadrones)
			{
				atacantes = Atacantes.Ladrones;
				return;
			}

			if(resultado < probabilidadLadrones + probabilidadAsesinos)
			{
				atacantes = Atacantes.Asesinos;
				return;
			}

			atacantes = Atacantes.Animales;
		}


		protected void setLadrones(uint cardinalidad, double det)
		{
			atacantes = Atacantes.Ladrones;
			numero = cardinalidad;
			determinacion = det;
			if(determinacion < 0.3f)
				determinacion = 0.3f;
			habilidadTotal = factorHabilidadLadrones * habilidadBase * numero;
			precioTotal = (uint)(determinacion * factorPrecioLadrones * precioBase * numero);
		}


		protected void setAsesinos(uint cardinalidad, double det)
		{
			atacantes = Atacantes.Asesinos;
			numero = cardinalidad;
			determinacion = det;
			if(determinacion < 0.7f)
				determinacion = 0.7f;
			habilidadTotal = factorHabilidadAsesinos * habilidadBase * numero;
			precioTotal = (uint)(determinacion * factorPrecioAsesinos * precioBase * numero);
		}


		protected void setAnimales(uint cardinalidad, double det)
		{
			atacantes = Atacantes.Animales;
			numero = cardinalidad;
			determinacion = det;
			if(determinacion < 0.9f)
				determinacion = 0.9f;
			habilidadTotal = factorHabilidadAnimales * habilidadBase * numero;
			precioTotal = (uint)(determinacion * factorPrecioAnimales * precioBase * numero);
		}
	}
}




