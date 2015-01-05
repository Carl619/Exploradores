using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace Ruinas
{
	
	
	public class RuinaJugable : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public List<Habitacion> habitaciones { get; protected set; }
		public List<Puerta> puertas { get; protected set; }
		public List<Reloj> relojes { get; protected set; }
		public List<PersonajeRuina> personajes { get; protected set; }
		public Dictionary<String, PersonajeRuina> personajesSeleccionados { get; protected set; }
		public Tesoro tesoroAbierto { get; set; }
		public RuinaJugableView vista { get; protected set; }


		// constructor
		public RuinaJugable(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			habitaciones = new List<Habitacion>();
			puertas = new List<Puerta>();
			relojes = new List<Reloj>();
			personajes = new List<PersonajeRuina>();
			personajesSeleccionados = new Dictionary<String, PersonajeRuina>();
			tesoroAbierto = null;
			vista = null;
		}


		// functions
		public void actualizarTiempo(int tiempoAccionesMinimas)
		{
			foreach(Reloj reloj in relojes)
				reloj.actualizarTiempo(tiempoAccionesMinimas);
			foreach(PersonajeRuina personaje in Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.personajes)
				personaje.actualizarTiempo(tiempoAccionesMinimas);
			foreach(Habitacion habitacion in habitaciones)
				habitacion.actualizarTiempo(tiempoAccionesMinimas);
		}


		public void crearReloj(RelojFlyweight newRelojFlyweight, PersonajeRuina newPersonaje,
								int tiempoMundo, int tiempoTotal,  Reloj.CallbackFinReloj accion)
		{
			Reloj reloj = new Reloj(newRelojFlyweight, newPersonaje, tiempoMundo, accion);
			reloj.setTiempoTotal(tiempoTotal);
			relojes.Add(reloj);
			vista.interfazRelojes.requestUpdateContent();
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
			
			Dictionary<String, PersonajeRuina> nuevosPersonajesSeleccionados = new Dictionary<String, PersonajeRuina>();
			foreach(KeyValuePair<String, PersonajeRuina> personaje in personajesSeleccionados)
			{
				if(personaje.Value.personaje.vivo == true)
					nuevosPersonajesSeleccionados.Add(personaje.Key, personaje.Value);
			}
			personajesSeleccionados = nuevosPersonajesSeleccionados;
		}


		public void moverPersonaje(Dictionary<String, Tuple<HabitacionView, RuinaNodo>> destinos,
									Objeto objetoDestino, Reloj.CallbackFinReloj accionPosterior)
		{
			if (personajesSeleccionados.Count == 0)
				return;
			
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual;
			List<RuinaNodo> nodosPersonajes = new List<RuinaNodo>();
			RuinaNodo nodoFinal = null;
			foreach (PersonajeRuina personaje in personajesSeleccionados.Values)
				nodosPersonajes.Add(personaje.getNodo());
			
			int indiceGrafo = 0;
			bool hayColisionFinal = false;
			if (objetoDestino != null)
			{
				if (objetoDestino.GetType() == typeof(PuertaSalida))
					hayColisionFinal = false;
				else if (objetoDestino.GetType() == typeof(Puerta))
					hayColisionFinal = false;
				else
					hayColisionFinal = true;
			}

			foreach (Puerta puerta in ruina.puertas)
			{
				puerta.generarNodosYRamas();
			}

			List<RuinaNodo> nodosTotales = new List<RuinaNodo>();
			List<RuinaRama> ramasTotales = new List<RuinaRama>();
			List<RuinaNodo> nodosFinales = new List<RuinaNodo>();


			int distanciaAparte = (int)(1.5 * (double)PersonajeRuina.alto);
			foreach (Habitacion h in ruina.habitaciones)
			{
				List<RuinaNodo> nodosHabitacion = h.getNodos(objetoDestino, false);
				Tuple<HabitacionView, RuinaNodo> destino;
				if (destinos.TryGetValue(h.id, out destino) == true)
				{
					for (int j = 0; j < personajesSeleccionados.Count; j++)
					{
						nodoFinal = new RuinaNodo(destino.Item2.coordenadas);
						nodoFinal.coordenadas = new Tuple<int, int>(destino.Item2.coordenadas.Item1, destino.Item2.coordenadas.Item2 - j * distanciaAparte);
						nodosHabitacion.Add(nodoFinal);
						nodosFinales.Add(nodoFinal);
					}
				}
				else
					destino = null;

				int i=0;
				foreach (PersonajeRuina personaje in personajesSeleccionados.Values)
				{
					if (personaje.habitacion == h)
						nodosHabitacion.Add(nodosPersonajes[i]);
					i++;
				}

				i = 0;

				indiceGrafo = 0;
				foreach (RuinaNodo nodo in nodosHabitacion)
				{
					nodo.indiceGrafo = indiceGrafo;
					indiceGrafo++;
				}

				if (destino != null)
					h.vista.ramas = h.getRamas(nodosHabitacion, objetoDestino);
				else
					h.vista.ramas = h.getRamas(nodosHabitacion, null);

				nodosTotales.AddRange(nodosHabitacion);
				ramasTotales.AddRange(h.vista.ramas);
			}


			indiceGrafo = 0;
			foreach (RuinaNodo nodo in nodosTotales)
			{
				nodo.indiceGrafo = indiceGrafo;
				indiceGrafo++;
			}
			foreach (Puerta puerta in ruina.puertas)
				ramasTotales.Add(puerta.rama);

			int k=0;
			foreach (PersonajeRuina personaje in personajesSeleccionados.Values)
			{
				int distInfinita = 1000000;
				Dijkstra.Nodo<RuinaNodo> origen = new Dijkstra.Nodo<RuinaNodo>(nodosPersonajes[k], distInfinita);

				List<Dijkstra.IRama> caminoMinimo = null;
				double distMinima = distInfinita;
				double dist = distInfinita;
				int numeroDestinos = 0;
				foreach (KeyValuePair<String, Tuple<HabitacionView, RuinaNodo>> tupla in destinos)
				{
					numeroDestinos++;
					RuinaNodo nodofinal = tupla.Value.Item2;
					List<Dijkstra.IRama> camino;
					camino =
					   origen.buscarCaminoMinimo(nodosFinales[k], 0, nodosTotales.Count, ref dist);
					personaje.destino = new RuinaNodo(nodosFinales[k].coordenadas);
					personaje.destino.habitacion = tupla.Value.Item1.habitacion;
					personaje.destinoBackUp = null;
					if (camino != null)
					{
						if (dist < distMinima)
						{
							caminoMinimo = camino;
							distMinima = dist;
						}
					}
					k++;
				}

				if (objetoDestino == null)
					personaje.mover(caminoMinimo, origen);
				else
					personaje.buscar(caminoMinimo, origen, objetoDestino, hayColisionFinal, accionPosterior);
			}
		}


		public RuinaJugableView crearVista()
		{
			vista = new RuinaJugableView(this);
			return vista;
		}
	}
}




