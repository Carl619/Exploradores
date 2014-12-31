using System;
using System.Collections.Generic;




namespace Dijkstra
{
	
	
	public interface IRama
	{
		double getPeso(int criterio);
		INodo verticeAdyacente(INodo nodoActual);
	}
	
	
	public interface INodo
	{
		int indiceGrafo { get; set; }
		List<IRama> ramasAdyacentes();
	}
	
	
	public class Nodo<TNodo> where TNodo : class, INodo
	{
		public TNodo referenciaNodo { get; set; }
		public int distanciaInfinita { get; set; }
		
		
		public Nodo(TNodo referencia, int distInfinita)
		{
			if(referencia == null)
				throw new ArgumentNullException();
			referenciaNodo = referencia;
			distanciaInfinita = distInfinita;
		}
		
		
		public List<IRama> buscarCaminoMinimo(TNodo verticeDestino,
										int criterio, int numeroNodosGrafo,
										ref double distanciaMinima)
		{
			PriorityQueue.PQ<TNodo, double> cola;
			List<double> distancias;
			List<IRama> previos;
			List<bool> vistos;

			distanciaMinima = distanciaInfinita;
			
			if(verticeDestino == null)
				return null;

			// inicializar
			cola = new PriorityQueue.PQ<TNodo, double>();
			distancias = new List<double>();
			previos = new List<IRama>();
			vistos = new List<bool>();
			
			for(int i=0; i<numeroNodosGrafo; ++i)
			{
				distancias.Add(distanciaInfinita);
				previos.Add(null);
				vistos.Add(false);
			}
			
			distancias[referenciaNodo.indiceGrafo] = 0;
			cola.push(referenciaNodo, 0);
			
			
			while(cola.Count != 0)
			{
				// extraemos el vertice de la cola de prioridad
				TNodo nodoActual = cola.pop();
				
				// si no esta ya visitado
				if(vistos[nodoActual.indiceGrafo] == false)
				{
					vistos[nodoActual.indiceGrafo] = true;
					List<IRama> ramasAdyacentes = nodoActual.ramasAdyacentes();
					// iterar sobre todas las ramas / vertices adyacentes
					foreach(IRama rama in ramasAdyacentes)
					{
						TNodo nodoAdyacente = (TNodo)rama.verticeAdyacente(nodoActual);
						
						// calculamos la nueva distancia
						double dist = distancias[nodoActual.indiceGrafo] + rama.getPeso(criterio);
						if(distancias[nodoAdyacente.indiceGrafo] > dist)
						{
							distancias[nodoAdyacente.indiceGrafo] = dist;
							previos[nodoAdyacente.indiceGrafo] = rama;
							// insertar vertice en la cola
							cola.push(nodoAdyacente, dist);
						}
					}
				}
			}
			
			List<IRama> camino = new List<IRama>();
			TNodo nodoCamino = verticeDestino;
			IRama ramaCamino = previos[nodoCamino.indiceGrafo];
			while(ramaCamino != null)
			{
				camino.Add(ramaCamino);
				nodoCamino = (TNodo)ramaCamino.verticeAdyacente(nodoCamino);
				ramaCamino = previos[nodoCamino.indiceGrafo];
			}
			
			distanciaMinima = distancias[verticeDestino.indiceGrafo];
			List<IRama> caminoFinal = new List<IRama>();
			for(int i=camino.Count - 1; i >= 0; --i)
			{
				caminoFinal.Add(camino[i]);
			}
			
			return caminoFinal;
		}
	}
}




