using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public abstract partial class Evento
	{
		// variables
		public String id { get; set; }
		public int tipoObserver { get; set; }
		public List<String> parametrosEntrada { get; set; }
		
		
		// constructor
		public Evento(String newId, int tipoObs = 0)
		{
			id = (String)newId.Clone();
			tipoObserver = tipoObs;
			parametrosEntrada = new List<String>();
		}
		
		
		// funciones
		public abstract void ejecutar(List<Argumento> valoresEntrada);
	}
}




