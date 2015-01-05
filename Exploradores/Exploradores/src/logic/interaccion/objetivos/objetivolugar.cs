using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	// objetivo de llegar a un lugar concreto
	public class ObjetivoLugar : Mision.Objetivo
	{
		// variables
		public Mapa.LugarVisitable lugar { get; set; }
		
		
		// constructor
		public ObjetivoLugar(String newID, Mision.Etapa newEtapa)
			: base(newID, newEtapa)
		{
			lugar = null;
		}


		// funciones
		public override void actualizacionPublisher(
									Interaccion.Mision.Evento evento,
									Interaccion.Mision.InfoEvento info)
		{
			if(evento != Mision.Evento.LlegadaLugar)
				return;
			if(info.lugar.id.Equals(lugar.id) == true)
			{
				estado = Mision.Estado.Terminado;
				actualizarCumplido();
			}
		}
	}
}




