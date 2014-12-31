using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class RuinaNodo : Dijkstra.INodo
	{
		// variables
		public int indiceGrafo { get; set; }
		public List<RuinaRama> ramas { get; protected set; }
		public Tuple<int, int> coordenadas { get; set; }
		public Tuple<int, int> coordenadasOpuestas { get; set; } // para puertas
		public Habitacion habitacion { get; set; } // para puertas


		// constructor
		public RuinaNodo(Tuple<int, int> newcoordenadas)
		{
			indiceGrafo = 0;
			ramas = new List<RuinaRama>();
			coordenadas = newcoordenadas;
			coordenadasOpuestas = null;
			habitacion = null;
		}


		// funciones
		public List<Dijkstra.IRama> ramasAdyacentes()
		{
			List<Dijkstra.IRama> listaRamas = new List<Dijkstra.IRama>();
			foreach (RuinaRama rama in ramas)
				listaRamas.Add(rama);
			
			return listaRamas;
		}


		public void addRama(RuinaRama rama)
		{
			if (rama == null)
				return;
			ramas.Add(rama);
		}


		public bool colisiona(int x, int y, Objeto objeto)
		{
			int xI = coordenadas.Item1 - PersonajeRuina.ancho / 2;
			int yI = coordenadas.Item2 - PersonajeRuina.alto / 2;
			int xF = x - PersonajeRuina.ancho / 2;
			int yF = y - PersonajeRuina.alto / 2;
			int minX = xI < xF ? xI : xF;
			int minY = yI  < yF ? yI : yF;
			int maxX = xI > xF ? xI : xF;
			int maxY = yI > yF ? yI : yF;
			maxX += PersonajeRuina.ancho;
			maxY += PersonajeRuina.alto;
			/*
			Rectangle posicionInicial = new Rectangle(xI, yI, PersonajeRuina.ancho, PersonajeRuina.alto);
			Rectangle posicionFinal = new Rectangle(xF, yF, PersonajeRuina.ancho, PersonajeRuina.alto);
			Rectangle movimientoPersonaje = new Rectangle(minX, minY, maxX - minX, maxY - minY);
			
			if(objeto.espacio.Intersects(posicionInicial))
				return true;
			if(objeto.espacio.Intersects(posicionFinal))
				return true;
			if(objeto.espacio.Intersects(movimientoPersonaje) == false)
				return false;*/
			
			bool conditionX = maxX > objeto.espacio.X && minX < objeto.espacio.X + objeto.espacio.Width;
			bool conditionY = maxY > objeto.espacio.Y && minY < objeto.espacio.Y + objeto.espacio.Height;

			if(conditionX)
			{
				int medY1 = minY + PersonajeRuina.alto;
				int medY2 = maxY - PersonajeRuina.alto;
				if(medY1 > maxY)
					medY1 = maxY;
				if(medY2 < minY)
					medY2 = minY;
				int minMedY = medY1 < medY2 ? medY1 : medY2;
				int maxMedY = medY1 > medY2 ? medY1 : medY2;
				if (maxMedY >= objeto.espacio.Y && conditionY)
					return true;
				if (minMedY <= objeto.espacio.Y + objeto.espacio.Height && conditionY)
					return true;
			}

			if(conditionY)
			{
				int medX1 = minX + PersonajeRuina.ancho;
				int medX2 = maxX - PersonajeRuina.ancho;
				if(medX1 > maxX)
					medX1 = maxX;
				if(medX2 < minX)
					medX2 = minX;
				int minMedX = medX1 < medX2 ? medX1 : medX2;
				int maxMedX = medX1 > medX2 ? medX1 : medX2;
				if (maxMedX >= objeto.espacio.X && conditionX)
					return true;
				if (minMedX <= objeto.espacio.X + objeto.espacio.Width && conditionX)
					return true;
			}

			return false;
		}
	}
}
