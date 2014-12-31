using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dijkstra;




namespace Ruinas
{
	
	
	public class RuinaRama : IRama
	{
		// variables
		public Tuple<RuinaNodo, RuinaNodo> extremos { get; protected set; }
		public float distancia { get; set; }
		public float peligro { get; set; }
		public bool cambioHabitacion { get; set; }


		// constructor
		public RuinaRama(RuinaNodo nodoInicio, RuinaNodo nodoFinal)
		{
			if(nodoInicio == null || nodoFinal == null)
				throw new ArgumentNullException();
			
			extremos = new Tuple<RuinaNodo, RuinaNodo>(nodoInicio,nodoFinal);
			double x = nodoInicio.coordenadas.Item1 - nodoFinal.coordenadas.Item1;
			double y = nodoInicio.coordenadas.Item2 - nodoFinal.coordenadas.Item2;
			distancia = (float)Math.Sqrt(x * x + y * y);
			peligro = 0.0f;
			nodoInicio.ramas.Add(this);
			nodoFinal.ramas.Add(this);
			cambioHabitacion = false;
		}


		// funciones
		public double getPeso(int criterio)
		{
			if (criterio == 0)
				return distancia;
			return peligro;
		}


		public INodo verticeAdyacente(INodo nodoActual)
		{
			if (nodoActual == extremos.Item1)
				return extremos.Item2;
			if (nodoActual == extremos.Item2)
				return extremos.Item1;
			return null;
		}
	}
}



