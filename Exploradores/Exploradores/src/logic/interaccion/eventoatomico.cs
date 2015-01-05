using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public class EventoAtomico : Evento
	{
		// delegados
		public delegate void AccionEvento(List<Argumento> valoresEntrada);
		
		
		// variables
		public AccionEvento accion { get; set; }
		
		
		// constructor
		public EventoAtomico(String newID, int tipoObs = 0)
			: base(newID, tipoObs)
		{
			accion = null;
		}
		
		
		// funciones
		public override void ejecutar(List<Argumento> valoresEntrada)
		{
			if(accion != null)
				accion(valoresEntrada);
		}
	}
}




