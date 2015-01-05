using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class NPCView : Programa.ListaView.Elemento
	{
		// variables
		public NPC npc { get; protected set; }
		public uint espacioNombre { get; set; }


		// constructor
		public NPCView(NPC newNPC, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newNPC == null)
				throw new ArgumentNullException();
			
			npc = newNPC;
			espacioNombre = 192;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border3"].clone();

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			SpriteFont font = npc.npcFlyweight.fuente;
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = npc.nombre;
			label.color = npc.npcFlyweight.color;
			label.innerComponent = font;
			label.sizeSettings.fixedInnerWidth = espacioNombre;
			addComponent(label);

			label = new ILSXNA.Label();
			label.message = npc.npcFlyweight.nombreProfesion;
			label.color = npc.npcFlyweight.color;
			label.innerComponent = font;
			addComponent(label);
		}
	}
}




