using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	// objetivo de hablar con un npc
	public class ObjetivoDialogo : Mision.Objetivo
	{
		// variables
		public Personajes.NPC npc { get; set; }
		
		
		// constructor
		public ObjetivoDialogo(String newID, Mision.Etapa newEtapa)
			: base(newID, newEtapa)
		{
			npc = null;
		}


		// funciones
		public override void actualizacionPublisher(
									Interaccion.Mision.Evento evento,
									Interaccion.Mision.InfoEvento info)
		{
			if(evento != Mision.Evento.Dialogo)
				return;
			if(info.npc.id.Equals(npc.id) == true)
			{
				estado = Mision.Estado.Terminado;
				actualizarCumplido();
			}
		}
	}
}




