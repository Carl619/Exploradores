using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gestores;




namespace Ruinas
{
	
	
	public class RuinaJugable : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public List<Habitacion> habitaciones { get; protected set; }
		public List<Puerta> puertas { get; protected set; }
		public List<Reloj> relojes { get; protected set; }
		public List<PersonajeRuina> personajesSeleccionados { get; protected set; }
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
			personajesSeleccionados = new List<PersonajeRuina>();
			vista = null;
		}


		// functions
		public void actualizarTiempo(int tiempo)
		{
			foreach(Reloj reloj in relojes)
				reloj.actualizarTiempo(tiempo);
			foreach(PersonajeRuina personaje in Partidas.Instancia.gestorRuinas.personajesRuinas.Values)
				personaje.actualizarTiempo(tiempo);
		}


		public void crearReloj(RelojFlyweight newRelojFlyweight, PersonajeRuina newPersonaje,
								int tiempoMundo, int tiempoTotal,  Reloj.CallbackFinReloj accion)
		{
			Reloj reloj = new Reloj(newRelojFlyweight, newPersonaje, tiempoMundo, accion);
			reloj.setTiempoTotal(tiempoTotal);
			relojes.Add(reloj);
			vista.interfazRelojes.requestUpdateContent();
		}


		public void moverPersonaje(Dictionary<String, Tuple<HabitacionView, RuinaNodo>> destinos,
									Objeto objetoDestino, Reloj.CallbackFinReloj accionPosterior)
		{
			
			if(personajesSeleccionados.Count == 0)
				return;
			
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinaActual;
			List<RuinaNodo> nodosPersonajes = new List<RuinaNodo>();
			RuinaNodo nodoFinal=null;
			foreach(PersonajeRuina personaje in personajesSeleccionados)
				nodosPersonajes.Add(personaje.getNodo());
			
			int indiceGrafo = 0;
			bool hayColisionFinal = false;
			if(objetoDestino != null)
			{
				if(objetoDestino.GetType() == typeof(Puerta))
					hayColisionFinal = false;
				else
					hayColisionFinal = true;
			}

			foreach(Puerta puerta in ruina.puertas)
			{
				puerta.generarNodosYRamas();
				puerta.rama.cambioHabitacion = true;
			}
			
			List<RuinaNodo> nodosTotales = new List<RuinaNodo>();
			List<RuinaRama> ramasTotales = new List<RuinaRama>();
			
			foreach(Habitacion h in ruina.habitaciones)
			{
				List<RuinaNodo> nodosHabitacion = h.getNodos(objetoDestino, false);
				Tuple<HabitacionView, RuinaNodo> destino;
				if (destinos.TryGetValue(h.id, out destino) == true)
				{
					nodoFinal = destino.Item2;
					nodoFinal.coordenadas = new Tuple<int, int>(destino.Item2.coordenadas.Item1, destino.Item2.coordenadas.Item2);
					nodosHabitacion.Add(nodoFinal);
					nodoFinal = new RuinaNodo(destino.Item2.coordenadas);
					nodoFinal.habitacion = destino.Item2.habitacion;
					nodoFinal.coordenadas = new Tuple<int, int>(destino.Item2.coordenadas.Item1, destino.Item2.coordenadas.Item2-32);
					nodosHabitacion.Add(nodoFinal);
				}
				else
					destino = null;
				for(int i = 0; i<personajesSeleccionados.Count; ++i)
				{
					if(personajesSeleccionados[i].habitacion == h)
						nodosHabitacion.Add(nodosPersonajes[i]);
				}

				indiceGrafo = 0;
				foreach (RuinaNodo nodo in nodosHabitacion)
				{
					nodo.indiceGrafo = indiceGrafo;
					indiceGrafo++;
				}
				
				if(destino != null)
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
			foreach(Puerta puerta in ruina.puertas)
				ramasTotales.Add(puerta.rama);
			
			for(int i = 0; i<personajesSeleccionados.Count; ++i)
			{
				int distInfinita = 1000000;
				Dijkstra.Nodo<RuinaNodo> origen = new Dijkstra.Nodo<RuinaNodo>(nodosPersonajes[i], distInfinita);

				List<Dijkstra.IRama> caminoMinimo = null;
				double distMinima = distInfinita;
				double dist = distInfinita;
				int numeroDestinos = 0;
				foreach(KeyValuePair<String, Tuple<HabitacionView, RuinaNodo>> tupla in destinos)
				{
					numeroDestinos++;
					RuinaNodo nodofinal=tupla.Value.Item2;
					List<Dijkstra.IRama> camino;
					if(i==0 && numeroDestinos==1)
					camino =
					   origen.buscarCaminoMinimo(nodofinal, 0, nodosTotales.Count, ref dist);
					else
						camino =
					   origen.buscarCaminoMinimo(nodoFinal, 0, nodosTotales.Count, ref dist);
					personajesSeleccionados[i].destino = new RuinaNodo(nodofinal.coordenadas);
					personajesSeleccionados[i].destino.habitacion = tupla.Value.Item1.habitacion;
					personajesSeleccionados[i].destinoBackUp = null;
					if(camino != null)
					{
						if(dist < distMinima)
						{
							caminoMinimo = camino;
							distMinima = dist;
						}
					}
				}
				
				if(objetoDestino == null)
					personajesSeleccionados[i].mover(caminoMinimo, origen);
				else
					personajesSeleccionados[i].buscar(caminoMinimo, origen, objetoDestino, hayColisionFinal, accionPosterior);
			}
		}


		public RuinaJugableView crearVista()
		{
			vista = new RuinaJugableView(this);
			return vista;
		}
	}
}




