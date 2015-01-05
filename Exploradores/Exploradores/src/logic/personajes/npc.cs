using System;
using System.Collections.Generic;
using Objetos;




namespace Personajes
{
	

	public class NPC : Personaje, Gestores.IObjetoIdentificable
	{
		// variables
		public NPCFlyweight npcFlyweight { get; protected set; }
		public Interaccion.MenuDialogo menuDialogo { get; set; }
		public Objetos.Inventario inventario { get; set; }
		public Mapa.Ciudad ciudadResidencia { get; set; }
		public Dictionary<String, String> cadenasRegexDialogo { get; set; } // cadenas para Regex replace
		public bool eliminar { get; set; }


		// constructor
		public NPC(String newID, String newNombre, Ruinas.PersonajeRuinaFlyweight newFlyweightRuina, NPCFlyweight newNPCFlyweight, Mapa.Ciudad newResidencia)
			: base(newID, newNombre, newFlyweightRuina)
		{
			if(newNPCFlyweight == null)
				throw new ArgumentNullException();
			
			npcFlyweight = newNPCFlyweight;
			menuDialogo = null;
			inventario = null;
			ciudadResidencia = newResidencia;
			cadenasRegexDialogo = new Dictionary<String, String>();
			eliminar = false;
		}


		public Acompanante reclutar()
		{
			if(eliminar == true)
				return null;
			eliminar = true;
			ciudadResidencia.listaNPC.Remove(this);
			Gestores.Partidas.Instancia.npcs.Remove(id);

			Acompanante acom = new Acompanante(id, nombre, flyweightPersonajeRuina);
			foreach(KeyValuePair<String, Habilidad> habilidad in habilidades)
				acom.habilidades.Add(habilidad.Key, habilidad.Value);
			foreach(KeyValuePair<String, Atributo> atributo in atributos)
				acom.atributos.Add(atributo.Key, atributo.Value);
			acom.avatarSeleccionado = avatarSeleccionado;
			acom.avatar = avatar;
			return acom;
		}


		// funciones
		public static bool esLista(String campo)
		{
			if(campo.Equals("atributos") == true)
				return true;
			if(campo.Equals("habilidades") == true)
				return true;
			return false;
		}


		public static NPC cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			NPC npc;
			NPCFlyweight npcFlyweight;
			Ruinas.PersonajeRuinaFlyweight ruinaFlyweight;
			Mapa.Ciudad ciudad;
			
			ciudad = Gestores.Partidas.Instancia.ciudades[campos["ciudad"]];
			npcFlyweight = Gestores.Mundo.Instancia.npcFlyweights[campos["flyweight npc"]];
			ruinaFlyweight = Gestores.Mundo.Instancia.personajeRuinaFlyweights[campos["flyweight personaje ruina"]];

			npc = new NPC(campos["id"], campos["nombre"], ruinaFlyweight, npcFlyweight, ciudad);
			npc.avatarSeleccionado = Gestores.Mundo.Instancia.imagenes[campos["avatar"]];
			npc.avatar = Gestores.Mundo.Instancia.imagenes[campos["avatar"] + "b"];
			npc.menuDialogo = Gestores.Partidas.Instancia.dialogos[campos["menuDialogo"]];

			List<String> atributos;
			if(listas.TryGetValue("atributos", out atributos) == true)
			{
				foreach(String atr in atributos)
				{
					String[] partes = atr.Split('|');
					List<String> lista = new List<string>();
					bool flag = false;
					foreach(String parte in partes)
					{
						if(flag == false)
						{
							flag = true;
							continue;
						}
						lista.Add(parte.Trim());
					}
					
					Atributo atributo = Gestores.Mundo.Instancia.atributos[partes[0].Trim()];
					npc.atributos.Add(atributo.id, atributo.clone(lista));
				}
			}

			List<String> habilidades;
			if(listas.TryGetValue("habilidades", out habilidades) == true)
			{
				foreach(String hab in habilidades)
				{
					String[] partes = hab.Split('|');
					List<String> lista = new List<string>();
					bool flag = false;
					foreach(String parte in partes)
					{
						if(flag == false)
						{
							flag = true;
							continue;
						}
						lista.Add(parte.Trim());
					}

					Habilidad habilidad = Gestores.Mundo.Instancia.habilidades[partes[0].Trim()];
					Habilidad habilidad2 = habilidad.clone(lista);
					npc.habilidades.Add(habilidad2.id, habilidad2);

					foreach(KeyValuePair<String, String> regex in habilidad2.cadenasRegexDialogo)
						npc.cadenasRegexDialogo.Add(regex.Key, regex.Value);
				}
			}
			
			String inventario;
			if(campos.TryGetValue("inventario", out inventario) == true)
				npc.inventario = Gestores.Partidas.Instancia.inventarios[inventario];
			ciudad.listaNPC.Add(npc);

			return npc;
		}


		public static String guardarObjeto(NPC npc)
		{
			String resultado;
			resultado = "	id							: " + npc.id + "\n" +
						"	nombre						: " + npc.nombre + "\n" +
						"	flyweight npc				: " + npc.npcFlyweight.id + "\n" +
						"	flyweight personaje ruina	: " + npc.flyweightPersonajeRuina.id + "\n" +
						"	ciudad						: " + npc.ciudadResidencia.id + "\n" +
						"	menuDialogo					: " + npc.menuDialogo.id + "\n";
			if(npc.inventario == null)
				return resultado;
			return resultado +
						"	inventario					: " + npc.inventario.id + "\n";
		}

		
		public NPCView crearVista(bool seleccionar, bool actualizarVista = true)
		{
			return new NPCView(this, seleccionar, actualizarVista);
		}
	}
}




