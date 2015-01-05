using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	// objetivo de ver un elemento de dialogo concreto
	public class ObjetivoElementoDialogo : Mision.Objetivo
	{
		// variables
		public Interaccion.ElementoDialogo elementoDialogo { get; set; }
		
		
		// constructor
		public ObjetivoElementoDialogo(String newID, Mision.Etapa newEtapa)
			: base(newID, newEtapa)
		{
			elementoDialogo = null;
		}


		// funciones
		public override void actualizacionPublisher(
									Interaccion.Mision.Evento evento,
									Interaccion.Mision.InfoEvento info)
		{
			if(evento != Mision.Evento.ElementoDialogo)
				return;
			if(info.elementoDialogo.id.Equals(elementoDialogo.id) == true)
			{
				estado = Mision.Estado.Terminado;
				actualizarCumplido();
			}
		}
	}
}




