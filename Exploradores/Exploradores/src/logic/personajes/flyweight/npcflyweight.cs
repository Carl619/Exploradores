using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Personajes
{
	
	
	public class NPCFlyweight : Gestores.IObjetoIdentificable
	{
		// variables
		public static Texture2D iconoAusente = null;
		public String id { get; protected set; }
		public String nombreProfesion { get; set; }
		public String edificioEncuentro { get; set; }
		public SpriteFont fuente { get; set; }
		public Texture2D iconoProfesion { get; set; }
		public Color color { get; set; }
		public uint anchoEdificioView { get; set; }


		// constructor
		public NPCFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			nombreProfesion = "Ciudadano";
			edificioEncuentro = "Calle";
			fuente = null;
			iconoProfesion = null;
			color = Color.Black;
			anchoEdificioView = 112;
		}


		// funciones
		public static NPCFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			NPCFlyweight npcFlyweight;

			npcFlyweight = new Personajes.NPCFlyweight(campos["id"]);
			npcFlyweight.iconoProfesion =
				Programa.Exploradores.Instancia.Content.Load<Texture2D>(@campos["iconoProfesion"]);
			npcFlyweight.nombreProfesion = campos["nombreProfesion"];
			npcFlyweight.edificioEncuentro = campos["edificioEncuentro"];
			npcFlyweight.fuente = Gestores.Gestor<NPCFlyweight>.parseFont(campos["nombreProfesion"]);
			
			return npcFlyweight;
		}


		public Mapa.EdificioView crearVistaProfesion(bool seleccionar, bool actualizarVista)
		{
			return new Mapa.EdificioView(this, seleccionar, actualizarVista);
		}
	}
}





