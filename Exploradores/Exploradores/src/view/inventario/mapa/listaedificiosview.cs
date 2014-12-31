using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class ListaEdificiosView : Programa.ListaView
	{
		// variables
		public List<Personajes.NPCFlyweight> edificios { get; set; }
		public Personajes.NPCFlyweight edificioSeleccionado { get; protected set; }


		// constructor
		public ListaEdificiosView(List<Personajes.NPC> npcs,
									Programa.ListaViewFlyweight newFlyweight)
			: base(null, newFlyweight, false)
		{
			if (npcs == null)
				throw new ArgumentNullException();
			
			Dictionary<String, Personajes.NPCFlyweight> diccionarioProfesiones =
				new Dictionary<string, Personajes.NPCFlyweight>();
			foreach(Personajes.NPC npc in npcs)
			{
				Personajes.NPCFlyweight p;
				if(diccionarioProfesiones.TryGetValue(npc.flyweight.edificioEncuentro, out p) == false)
					diccionarioProfesiones.Add(npc.flyweight.edificioEncuentro, npc.flyweight);
			}

			edificios = new List<Personajes.NPCFlyweight>();
			foreach(Personajes.NPCFlyweight value in diccionarioProfesiones.Values)
				edificios.Add(value);
			edificioSeleccionado = null;
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			foreach (Personajes.NPCFlyweight edificio in edificios)
			{
				bool s = (edificio == edificioSeleccionado);
				listaElementos.Add(edificio.crearVistaProfesion(s, true));
			}

			base.updateContent();
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
			edificioSeleccionado = ((EdificioView)elemento).edificio;
		}
	}
}




