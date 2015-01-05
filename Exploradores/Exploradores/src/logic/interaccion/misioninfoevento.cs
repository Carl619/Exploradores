using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public partial class Mision : Gestores.IObjetoIdentificable
	{
		
		
		public class InfoEvento
		{
			// variables
			public Mapa.LugarVisitable lugar { get; set; }
			public Personajes.NPC npc { get; set; }
			public Interaccion.ElementoDialogo elementoDialogo { get; set; }
			
			
			// constructor
			public InfoEvento()
			{
				lugar = null;
				npc = null;
				elementoDialogo = null;
			}
		}
	}
}




