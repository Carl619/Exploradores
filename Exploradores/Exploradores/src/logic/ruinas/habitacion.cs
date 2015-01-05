using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public class Habitacion : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public uint grosorPared { get; set; }
		public Rectangle espacio { get; set; }
		public RuinaJugable ruina { get; set; }
		public List<Objeto> objetos { get; protected set; }
		public List<Proyectil> proyectiles { get; protected set; }
		public Dictionary<int, SpawnPersonaje> spawnPersonajes { get; protected set; }
		public List<PersonajeRuina> personajes { get; protected set; }
		public List<Puerta> puertas { get; protected set; }
		public HabitacionView vista { get; protected set; }


		// constructor
		public Habitacion(String newid)
		{
			if(newid == null)
				throw new ArgumentNullException();
			id = String.Copy(newid);
			grosorPared = 2;
			espacio = new Rectangle(0, 0, 1, 1);
			ruina = null;
			objetos = new List<Objeto>();
			proyectiles = new List<Proyectil>();
			spawnPersonajes = new Dictionary<int, SpawnPersonaje>();
			personajes = new List<PersonajeRuina>();
			puertas = new List<Puerta>();
			vista = null;
		}


		// funciones
		public static Habitacion cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Habitacion habitacion;
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinasJugables[campos["ruina"]];

			habitacion = new Habitacion(campos["id"]);
			habitacion.ruina = ruina;
			ruina.habitaciones.Add(habitacion);
			habitacion.grosorPared = Convert.ToUInt32(campos["grosorPared"]);
			habitacion.espacio = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
												Convert.ToInt32(campos["coordenada y"]),
												Convert.ToInt32(campos["ancho"]),
												Convert.ToInt32(campos["alto"]));
			
			return habitacion;
		}


		public void actualizarTiempo(int tiempo)
		{
			foreach(Objeto objeto in objetos)
				objeto.actualizarTiempo(tiempo);
			foreach(Proyectil proyectil in proyectiles)
				proyectil.actualizarTiempo(tiempo);
			List<Proyectil> newProyectiles = new List<Proyectil>();
			foreach(Proyectil proyectil in proyectiles)
			{
				if(proyectil.terminado == false)
					newProyectiles.Add(proyectil);
				else
					vista.interfazObjetos.requestUpdateContent();
			}
			proyectiles = newProyectiles;
		}


		public void removePersonajesMuertos()
		{
			List<PersonajeRuina> nuevosPersonajes = new List<PersonajeRuina>();
			foreach(PersonajeRuina personaje in personajes)
			{
				if(personaje.personaje.vivo == true)
					nuevosPersonajes.Add(personaje);
			}
			personajes = nuevosPersonajes;
		}


		public HabitacionView crearVista()
		{
			vista = new HabitacionView(this);
			return vista;
		}


		public List<RuinaNodo> getNodos(Objeto objetoDestino, bool nodosRincones)
		{
			List<RuinaNodo> nodosObjetos;
			List<RuinaNodo> nodos = new List<RuinaNodo>();
			
			int offset = (int)grosorPared;

			if(nodosRincones == true)
			{
				nodos.Add(new RuinaNodo(new Tuple<int, int>(offset + PersonajeRuina.ancho / 2, offset + PersonajeRuina.alto / 2)));
				nodos.Add(new RuinaNodo(new Tuple<int, int>(offset + PersonajeRuina.ancho / 2, offset - PersonajeRuina.alto / 2 + espacio.Height)));
				nodos.Add(new RuinaNodo(new Tuple<int, int>(offset - PersonajeRuina.ancho / 2 + espacio.Width, offset + PersonajeRuina.alto / 2)));
				nodos.Add(new RuinaNodo(new Tuple<int, int>(offset - PersonajeRuina.ancho / 2 + espacio.Width, offset - PersonajeRuina.alto / 2 + espacio.Height)));
			}

			foreach (Objeto objeto in objetos)
			{
				if(objeto == objetoDestino)
					continue;
				nodosObjetos = objeto.nodos();
				for (int i = 0; i < nodosObjetos.Count; i++)
					nodos.Add(nodosObjetos[i]);
			}

			foreach (PersonajeRuina personaje in Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.personajes)
			{
				if (personaje.habitacion == this &&
					personaje.estado == Ruinas.PersonajeRuina.Estado.Parado)
				{
					nodosObjetos = personaje.nodos();
					for (int i = 0; i < nodosObjetos.Count; i++)
						nodos.Add(nodosObjetos[i]);
				}
			}

			nodos.AddRange(getNodosPuertas());
			return nodos;
		}


		public List<RuinaRama> getRamas(List<RuinaNodo> nodosRuina, Objeto objetoDestino)
		{
			List<RuinaRama> ramas = new List<RuinaRama>();
			bool colision = false;
			
			foreach (RuinaNodo nodo1 in nodosRuina)
			{
				foreach (RuinaNodo nodo2 in nodosRuina)
				{
					if (nodosRuina.IndexOf(nodo1) >= nodosRuina.IndexOf(nodo2))
						continue;
					colision = false;
					foreach (Objeto objeto in objetos)
					{
						if(objeto.esBloqueante() == false)
							continue;
						if(objeto == objetoDestino)
							continue;
						if (nodo2.colisiona(nodo1.coordenadas.Item1, nodo1.coordenadas.Item2, objeto))
						{
							colision = true;
							break;
						}
					}

					RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual;
					foreach (PersonajeRuina personaje in ruina.personajes)
					{
						PersonajeRuina personajeSeleccionado;
						if(personaje.habitacion == this &&
							personaje.estado == Ruinas.PersonajeRuina.Estado.Parado &&
							!ruina.personajesSeleccionados.TryGetValue(personaje.id, out personajeSeleccionado))
						if (nodo2.colisiona(nodo1.coordenadas.Item1, nodo1.coordenadas.Item2, personaje))
						{
							colision = true;
							break;
						}
					}

					if(colision == true)
						continue;
					
					ramas.Add(new RuinaRama(nodo1, nodo2));
				}
			}

			return ramas;
		}


		protected List<RuinaNodo> getNodosPuertas()
		{
			List<RuinaNodo> nodos = new List<RuinaNodo>();
			
			foreach(Puerta puerta in puertas)
			{
				if(puerta.activado == true)
				{
					if(puerta.habitacionPrincipal == this)
						nodos.Add(puerta.nodoPrincipal);
					else
						nodos.Add(puerta.nodoSecundario);
				}
			}

			return nodos;
		}
	}
}




