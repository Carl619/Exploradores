using System;
using System.Collections.Generic;
using Objetos;
using Ruinas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gestores;




namespace Personajes
{
	

	public class NPC : Personaje, Gestores.IObjetoIdentificable
	{
		// variables
		public NPCFlyweight flyweight { get; protected set; }
		public Interaccion.MenuDialogo menuDialogo { get; set; }
		public Mapa.Ciudad ciudadResidencia { get; set; }
		public Dictionary<String, String> cadenasRegexDialogo { get; set; } // cadenas para Regex replace
		public bool eliminar { get; set; }


		// constructor
		public NPC(String newID, String newNombre, NPCFlyweight newFlyweight, Mapa.Ciudad newResidencia)
			: base(newID, newNombre)
		{
			if(newFlyweight == null)
				throw new ArgumentNullException();
			
			flyweight = newFlyweight;
			menuDialogo = null;
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

			Acompanante acom = new Acompanante(id, nombre);
			foreach(KeyValuePair<String, Habilidad> habilidad in habilidades)
				acom.habilidades.Add(habilidad.Key, habilidad.Value);
			foreach(KeyValuePair<String, Atributo> atributo in atributos)
				acom.atributos.Add(atributo.Key, atributo.Value);

			PersonajeRuina personajeRuina;
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinasJugables["4"];

			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones["1"];

			personajeRuina = new PersonajeRuina(acom, habitacion);
			personajeRuina.posicion = new Rectangle(260,
										60,
										Ruinas.PersonajeRuina.ancho,
										Ruinas.PersonajeRuina.alto);

			personajeRuina.velocidadMovimiento = Gestores.Mundo.parseFloat("0.1");
			personajeRuina.velocidadAnimacion = 200;
			String nombreRuina = "images/sprites/ruin/personajes/" + "0" + "/parado";
			personajeRuina.imagenParado = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@nombreRuina);
			personajeRuina.imagenParado.Name = "parado";
			for (int i = 0; i < 4; ++i)
			{
				nombreRuina = "images/sprites/ruin/personajes/" + "0" + "/" + "movimiento" + i.ToString();
				personajeRuina.imagenesMovimiento.Add(Programa.Exploradores.Instancia.Content.Load<Texture2D>(@nombreRuina));
				personajeRuina.imagenesMovimiento[i].Name = "movimiento" + i.ToString();
			}
			personajeRuina.imagenActual = personajeRuina.imagenParado;

			habitacion.personajes.Add(personajeRuina);
			personajeRuina.ruina = ruina;
			personajeRuina.prioridad = 2;
			Partidas.Instancia.gestorRuinas.personajesRuinas.Add("Acom", personajeRuina);
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
			Mapa.Ciudad ciudad;
			
			ciudad = Gestores.Partidas.Instancia.ciudades[campos["ciudad"]];
			npcFlyweight = Gestores.Mundo.Instancia.npcFlyweights[campos["flyweight"]];

			npc = new NPC(campos["id"], campos["nombre"], npcFlyweight, ciudad);
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
			resultado = "	id						: " + npc.id + "\n" +
						"	nombre					: " + npc.nombre + "\n" +
						"	flyweight				: " + npc.flyweight.id + "\n" +
						"	ciudad					: " + npc.ciudadResidencia.id + "\n" +
						"	menuDialogo				: " + npc.menuDialogo.id + "\n";
			if(npc.inventario == null)
				return resultado;
			return resultado +
						"	inventario				: " + npc.inventario.id + "\n";
		}

		
		public NPCView crearVista(bool seleccionar, bool actualizarVista = true)
		{
			return new NPCView(this, seleccionar, actualizarVista);
		}
	}
}




