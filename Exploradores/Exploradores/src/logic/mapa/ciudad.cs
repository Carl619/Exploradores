using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class Ciudad : LugarVisitable
	{
		// variables
		public Programa.ListaViewFlyweight flyweightLista { get; protected set; }
		public CiudadFlyweight flyweightCiudad { get; protected set; }
		public Gestores.Imagen imagenCiudad { get; set; }
		public List<Personajes.NPC> listaNPC { get; set; }


		// constructor
		public Ciudad(String newID, String newNombre,
						LugarVisitableFlyweight newFlyweightLugar,
						CiudadFlyweight newFlyweightCiudad,
						Programa.ListaViewFlyweight newFlyweightLista)
			: base(newID, newNombre, newFlyweightLugar)
		{
			if(newFlyweightCiudad == null || newFlyweightLista == null)
				throw new ArgumentNullException();
			flyweightCiudad = newFlyweightCiudad;
			flyweightLista = newFlyweightLista;
			imagenCiudad = new Gestores.Imagen();
			listaNPC = new List<Personajes.NPC>();
		}


		// funciones
		public static Ciudad cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Ciudad ciudad;
			CiudadFlyweight ciudadFlyweight = Gestores.Mundo.Instancia.ciudadFlyweights[campos["ciudad flyweight"]];
			LugarVisitableFlyweight lugarFlyweight = Gestores.Mundo.Instancia.lugarFlyweights[campos["lugar flyweight"]];
			Programa.ListaViewFlyweight listaViewFlyweight = Gestores.Mundo.Instancia.listaViewFlyweights[campos["lista flyweight"]];

			ciudad = new Ciudad(campos["id"], campos["nombre"], lugarFlyweight, ciudadFlyweight, listaViewFlyweight);
			ciudad.oculto = Convert.ToBoolean(campos["oculto"]);
			ciudad.coordenadas = new Tuple<int, int>(Convert.ToInt32(campos["coordenada x"]), Convert.ToInt32(campos["coordenada y"]));
			ciudad.imagenCiudad.path = campos["imagenCiudad"];
			ciudad.imagenCiudad.textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@ciudad.imagenCiudad.path);

			Mapa.Instancia.lugares.Add(ciudad);
			Gestores.Partidas.Instancia.lugares.Add(ciudad.id, ciudad);
			return ciudad;
		}


		public static String guardarObjeto(Ciudad ciudad)
		{
			String resultado;
			resultado = "	id						: " + ciudad.id + "\n" +
						"	nombre					: " + ciudad.nombre + "\n" +
						"	ciudad flyweight		: " + ciudad.flyweightCiudad.id + "\n" +
						"	lugar flyweight			: " + ciudad.flyweightLugar.id + "\n" +
						"	lista flyweight			: " + ciudad.flyweightLista.id + "\n" +
						"	coordenada x			: " + ciudad.coordenadas.Item1 + "\n" +
						"	coordenada y			: " + ciudad.coordenadas.Item2 + "\n" +
						"	imagenCiudad			: " + ciudad.imagenCiudad.path +  "\n";
			return resultado;
		}


		public List<Personajes.NPCFlyweight> generarListaEdificios()
		{
			Dictionary<String, Personajes.NPCFlyweight> diccionarioEdificios =
				new Dictionary<string, Personajes.NPCFlyweight>();
			
			foreach(Personajes.NPC npc in listaNPC)
			{
				Personajes.NPCFlyweight p;
				if(diccionarioEdificios.TryGetValue(npc.npcFlyweight.edificioEncuentro, out p) == false)
					diccionarioEdificios.Add(npc.npcFlyweight.edificioEncuentro, npc.npcFlyweight);
			}

			List<Personajes.NPCFlyweight> edificios = new List<Personajes.NPCFlyweight>();
			foreach(Personajes.NPCFlyweight value in diccionarioEdificios.Values)
				edificios.Add(value);
			
			return edificios;
		}


		public CiudadViewCentro crearVistaCentro()
		{
			return new CiudadViewCentro(this);
		}


		public CiudadViewLateral crearVistaLateral(Programa.PanelLateral panelLateral)
		{
			return new CiudadViewLateral(this, panelLateral);
		}


		public ListaEdificiosView crearVistaEdificios()
		{
			return new ListaEdificiosView(this, flyweightLista);
		}


		protected override List<String> getInformacionLugar()
		{
			List<String> listaParrafos = new List<String>();
			listaParrafos.Add("Las ciudades son centros donde puedes comprar y vender articulos" + 
				", contratar mercenarios, descansar, y utilizar otros servicios ofrecidos.");
			return listaParrafos;
		}
	}
}




