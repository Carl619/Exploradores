using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class ListaNPCView : Programa.ListaView
	{
		// variables
		public List<NPC> npcs { get; set; }


		// constructor
		public ListaNPCView(List<NPC> newNpcs, String edificioSeleccionado, Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if(newNpcs == null)
				throw new ArgumentNullException();
			
			if(edificioSeleccionado == null)
				npcs = newNpcs;
			else
			{
				npcs = new List<NPC>();
				foreach(NPC npc in newNpcs)
					if(npc.flyweight.edificioEncuentro.Equals(edificioSeleccionado))
						npcs.Add(npc);
			}

			onElementSelect = Mapa.Controller.abrirDialogoNPC;
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			foreach(NPC npc in npcs)
			{
				bool s = (npc.id.Equals(Gestores.Partidas.Instancia.npcSeleccionado));
				listaElementos.Add(npc.crearVista(s, true));
			}

			base.updateContent();
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
			Gestores.Partidas.Instancia.npcSeleccionado = ((NPCView)elemento).npc.id;
		}
	}
}




