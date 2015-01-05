using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public partial class PersonajeRuina : Gestores.IObjetoIdentificable
	{
		// enumeraciones
		public enum Estado
		{
			Parado,
			Moviendo,
			Buscando, // moviendo hacia un objeto para interactuar
			Interactuando,
			Colision,
			Reintentando,
			Volviendo,
			Esperando
		}


		// variables
		public static int ancho = 24;
		public static int alto = 24;
		public static uint espacioInventario = 30;
		public String id { get { return personaje.id; } }
		public Personajes.Personaje personaje { get; set; }
		
		public Tuple<int ,int> coordenadasRuina
		{
			get
			{
				return new Tuple<int,int>(
					posicion.X + habitacion.espacio.X,
					posicion.Y + habitacion.espacio.Y
				);
			}
		}
		public Rectangle posicion { get; set; }
		public Rectangle posicionAnterior { get; set; }
		public Estado estado { get; protected set; }
		public Texture2D imagenActual { get; set; }
		
		public int prioridad { get; set; }
		public int tiempoActual { get; protected set; }
		public int tiempoEspera { get; set; }
		public Movimiento movimiento { get; protected set; }
		public Movimiento movimientoBackUp { get; set; }
		public List<Dijkstra.IRama> camino { get; set; }
		public List<Dijkstra.IRama> caminoBackUp { get; set; }
		public RuinaNodo destino { get; set; }
		public RuinaNodo destinoBackUp { get; set; }

		public RuinaJugable ruina { get; set; }
		public Habitacion habitacion { get; set; }
		public Objetos.Inventario inventario { get; set; }
		public Reloj reloj { get; set; }
		public Objeto objetoInteraccionable { get; protected set; }
		public bool colisionFinal { get; protected set; }
		protected Reloj.CallbackFinReloj accionReloj { get; set; }
		protected PersonajeRuinaView vista { get; set; }


		// constructor
		public PersonajeRuina(Personajes.Personaje newPersonaje, Habitacion newHabitacion)
		{
			if(newPersonaje == null || newHabitacion == null)
				throw new ArgumentNullException();
			
			personaje = newPersonaje;

			posicion = new Rectangle(0, 0, ancho, alto);
			posicionAnterior = new Rectangle(0, 0, ancho, alto);
			estado = Estado.Parado;
			imagenActual = null;
			
			prioridad = 0;
			tiempoActual = 0;
			tiempoEspera = 0;
			movimiento = null;
			movimientoBackUp = null;
			camino = null;
			caminoBackUp = null;
			destino = null;
			destinoBackUp = null;

			ruina = null;
			habitacion = newHabitacion;
			inventario = null;
			reloj = null;
			objetoInteraccionable = null;
			colisionFinal = false;
			accionReloj = null;
			vista = null;
		}


		// funciones
		public static PersonajeRuina cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			PersonajeRuina personaje;
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinasJugables[campos["ruina"]];
			Personajes.Personaje protagonista =
				Programa.Jugador.Instancia.protagonista;
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];
			
			personaje = new PersonajeRuina(protagonista, habitacion);
			personaje.prioridad = Convert.ToInt32(campos["prioridad"]);
			personaje.posicion = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										Ruinas.PersonajeRuina.ancho,
										Ruinas.PersonajeRuina.alto);
			
			personaje.imagenActual = personaje.personaje.flyweightPersonajeRuina.imagenParado;

			personaje.inventario = Gestores.Partidas.Instancia.inventarios[campos["inventario"]];
			habitacion.personajes.Add(personaje);
			ruina.personajes.Add(personaje);
			personaje.ruina = ruina;

			return personaje;
		}


		public RuinaNodo colisionPersonaje()
		{
			foreach(PersonajeRuina personaje in Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.personajes)
			{
				if(personaje.estado == Estado.Moviendo)
				{
					if(this != personaje)
					{
						if (posicion.Intersects(personaje.posicion))
						{
							if (prioridad < personaje.prioridad)
							{
								estado = Estado.Colision;
								return personaje.destino;
							}
						}
					}
				}
			}
			return null;
		}


		public void caminoDeVuelta(RuinaNodo dest)
		{
			List<RuinaNodo> nodosPersonajes = new List<RuinaNodo>();
			nodosPersonajes.Add(getNodo());

			int indiceGrafo = 0;
			foreach (Puerta puerta in ruina.puertas)
			{
				puerta.generarNodosYRamas();
			}

			List<RuinaNodo> nodosTotales = new List<RuinaNodo>();
			List<RuinaRama> ramasTotales = new List<RuinaRama>();

			foreach (Habitacion h in ruina.habitaciones)
			{
				List<RuinaNodo> nodosHabitacion = h.getNodos(null, false);

					if (habitacion == h)
						nodosHabitacion.Add(nodosPersonajes[0]);
					if (h == dest.habitacion)
						nodosHabitacion.Add(dest);

				indiceGrafo = 0;
				foreach (RuinaNodo nodo in nodosHabitacion)
				{
					nodo.indiceGrafo = indiceGrafo;
					indiceGrafo++;
				}

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

			int distInfinita = 1000000;
			Dijkstra.Nodo<RuinaNodo> origen = new Dijkstra.Nodo<RuinaNodo>(nodosPersonajes[0], distInfinita);


			List<Dijkstra.IRama> caminoMinimo = null;
			double distMinima = distInfinita;
			double dist = distInfinita;

			RuinaNodo nodofinal = dest;
			caminoBackUp =
			   origen.buscarCaminoMinimo(nodofinal, 0, nodosTotales.Count, ref dist);
			destinoBackUp = dest;

			if (caminoBackUp != null)
			{
				if (dist < distMinima)
				{
					caminoMinimo = caminoBackUp;
					distMinima = dist;
				}
			}



			if (estado == Estado.Esperando)
			{
				if (objetoInteraccionable == null)
					mover(caminoMinimo, origen);
				else
					buscar(caminoMinimo, origen, objetoInteraccionable, movimiento.colisionFinal, accionReloj);
			}

			else
			moverBackUp(caminoMinimo, origen);
		}


		public void actualizarTiempo(int tiempoMundo)
		{
			RuinaNodo dest;
			tiempoActual = tiempoMundo;
			RuinaNodo nodoReintento = null;
			if(estado == Estado.Parado)
				return;

			if (estado != Estado.Interactuando)
			{
				if (estado == Estado.Esperando) {
					if (tiempoMundo - tiempoEspera > 20)
					{
						movimientoBackUp = null;
						caminoBackUp = null;
						posicionAnterior = new Rectangle();
						caminoDeVuelta(destino);
					}
				}
				else
				{
					if (estado == Estado.Moviendo || estado == Estado.Buscando)
					{
						dest = colisionPersonaje();
						if (estado == Estado.Colision)
						{
							posicionAnterior = new Rectangle();
							caminoDeVuelta(dest);
						}
					}
					if (estado == Estado.Volviendo)
					{
						bool haLlegado = desplazarBackUp(tiempoMundo);
						imagenActual = getImagenActual();
						nodoReintento = apartarse(tiempoMundo);
						if (nodoReintento != null)
						{
							nodoReintento.habitacion = destino.habitacion;
							caminoDeVuelta(nodoReintento);
							destinoBackUp = nodoReintento;
							estado = Estado.Reintentando;

						}

					}
					else
					{
						if (estado == Estado.Reintentando)
						{
							bool haLlegado = desplazarBackUp(tiempoMundo);
							imagenActual = getImagenActual();
							if (haLlegado)
							{
								tiempoEspera = tiempoMundo;
								estado = Estado.Esperando;
							}
						}
						else
						{
							bool haLlegado = desplazar(tiempoMundo);
							imagenActual = getImagenActual();

							if (haLlegado == true)
							{
								if (camino != null)
								{
									if (camino.Count > 0)
									{
										seguirCamino(camino, movimiento.posicionFinal);
										return;
									}
								}

								imagenActual = getImagenActual();

								if (estado == Estado.Buscando)
								{
									estado = Estado.Interactuando;
									crearReloj(objetoInteraccionable.tiempoActivacion, accionReloj);
								}
								else
									estado = Estado.Parado;
								camino = null;
								movimiento = null;
							}
						}
					}
				}
			}
		}


		public RuinaNodo getNodo()
		{
			return new RuinaNodo(new Tuple<int, int>(
							posicion.X + PersonajeRuina.ancho / 2,
							posicion.Y + PersonajeRuina.alto / 2));
		}


		public virtual List<RuinaNodo> nodos()
		{
			List<RuinaNodo> nodos = new List<RuinaNodo>();

			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							posicion.X - PersonajeRuina.ancho / 2,
							posicion.Y - PersonajeRuina.alto / 2)));
			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							posicion.X - PersonajeRuina.ancho / 2,
							posicion.Y + PersonajeRuina.alto / 2 + posicion.Height)));
			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							posicion.X + PersonajeRuina.ancho / 2 + posicion.Width,
							posicion.Y - PersonajeRuina.alto / 2)));
			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							posicion.X + PersonajeRuina.ancho / 2 + posicion.Width,
							posicion.Y + PersonajeRuina.alto / 2 + posicion.Height)));

			return nodos;
		}


		public void finReloj()
		{
			if(estado == Estado.Interactuando)
			{
				estado = Estado.Parado;
				objetoInteraccionable = null;
			}
		}
		
		protected bool colisionBackUp(int x, int y)
		{
			Rectangle rectangulo = new Rectangle(x, y,posicion.Width, posicion.Height);
			

			foreach(Objeto objeto in habitacion.objetos)
			{
				if(rectangulo.Intersects(objeto.espacio)) return true;
			}

			foreach(PersonajeRuina personaje in Gestores.Partidas.Instancia.gestorRuinas.personajesRuinas.Values){
				if (personaje != this)
					if (rectangulo.Intersects(personaje.posicion)) return true;
			}
			return false;
		}


		protected RuinaNodo apartarse(int tiempoMundo)
		{
			Vector2 vectorInicial = new Vector2(posicionAnterior.X - posicion.X, posicionAnterior.Y - posicion.Y);
			vectorInicial.Normalize();

			Vector2 vectorPerpendicular = new Vector2(-vectorInicial.Y, vectorInicial.X);
			vectorPerpendicular.Normalize();
			if (!colisionBackUp(posicion.X + Convert.ToInt32((vectorPerpendicular * 50).X), posicion.Y + Convert.ToInt32((vectorPerpendicular * 50).Y)))
			{
				return new RuinaNodo(new Tuple<int, int>(posicion.X + Convert.ToInt32((vectorPerpendicular * 50).X), posicion.Y + Convert.ToInt32((vectorPerpendicular * 50).Y)));
			}

			Vector2 vectorPerpendicularAlternativo = new Vector2(vectorInicial.Y, -vectorInicial.X);
			vectorPerpendicularAlternativo.Normalize();
			if (!colisionBackUp(posicion.X + Convert.ToInt32((vectorPerpendicularAlternativo * 50).X), posicion.Y + Convert.ToInt32((vectorPerpendicularAlternativo * 50).Y)))
			{
				return new RuinaNodo(new Tuple<int, int>(posicion.X + Convert.ToInt32((vectorPerpendicularAlternativo * 50).X), posicion.Y + Convert.ToInt32((vectorPerpendicularAlternativo * 50).Y)));
			}
			return null;
		}


		protected bool desplazarBackUp(int tiempoMundo)
		{
			bool llegada = false;
			posicionAnterior = new Rectangle(movimiento.posicionFinal.coordenadas.Item1, movimiento.posicionFinal.coordenadas.Item2, posicion.Width, posicion.Height);
			Tuple<int, int> posicionSiguiente =
				movimientoBackUp.getDesplazamiento(
						tiempoMundo,
						personaje.flyweightPersonajeRuina.velocidadMovimiento,
						objetoInteraccionable,
						out llegada);
			posicion = new Rectangle(posicionSiguiente.Item1 - ancho / 2,
									posicionSiguiente.Item2 - alto / 2,
									posicion.Width, posicion.Height);
			comprobarColisionesAreasActivacion();
			return llegada;
		}


		protected void seguirCaminoBackUp(List<Dijkstra.IRama> newCamino, RuinaNodo origen)
		{
			if (reloj != null)
				reloj.cancelarReloj();
			caminoBackUp = newCamino;
			Dijkstra.INodo nodo = caminoBackUp[0].verticeAdyacente(origen);
			RuinaNodo siguiente = (RuinaNodo)nodo;
			if (((RuinaRama)caminoBackUp[0]).cambioHabitacion == true)
			{
				origen.coordenadas = siguiente.coordenadasOpuestas;
				habitacion.personajes.Remove(this);
				habitacion = siguiente.habitacion;
				habitacion.personajes.Add(this);
				posicion = new Rectangle(origen.coordenadas.Item1, origen.coordenadas.Item2, ancho, alto);
				Programa.VistaGeneral.Instancia.contenedorJuego.interfazRuina.requestUpdateContent();
			}
			movimientoBackUp = new Movimiento(origen, siguiente, tiempoActual, colisionFinal);
			caminoBackUp.RemoveAt(0);
			vista.requestUpdateContent();
		}


		public void moverBackUp(List<Dijkstra.IRama> newCamino, Dijkstra.Nodo<RuinaNodo> origen)
		{
			if (newCamino != null)
			{
				if (newCamino.Count > 0)
				{
					estado = Estado.Volviendo;
					seguirCaminoBackUp(newCamino, origen.referenciaNodo);
					return;
				}
			}
			estado = Estado.Reintentando;
			caminoBackUp = null;
		}


		public void mover(List<Dijkstra.IRama> newCamino, Dijkstra.Nodo<RuinaNodo> origen)
		{
			colisionFinal = false;
			if(newCamino != null)
			{
				if(newCamino.Count > 0)
				{
					estado = Estado.Moviendo;
					objetoInteraccionable = null;
					seguirCamino(newCamino, origen.referenciaNodo);
					return;
				}
			}
			estado = Estado.Parado;
			camino = null;
			movimiento = null;
			colisionFinal = false;
		}


		public void buscar(List<Dijkstra.IRama> newCamino, Dijkstra.Nodo<RuinaNodo> origen,
							Objeto objetoDestino, bool hayColisionFinal, Reloj.CallbackFinReloj accionPosterior)
		{
			colisionFinal = hayColisionFinal;
			accionReloj = accionPosterior;
			if(newCamino != null)
			{
				if(newCamino.Count > 0)
				{
					estado = Estado.Buscando;
					objetoInteraccionable = objetoDestino;
					seguirCamino(newCamino, origen.referenciaNodo);
					return;
				}
			}
			estado = Estado.Parado;
			camino = null;
			movimiento = null;
			colisionFinal = false;
		}


		public bool colisiona(int x, int y, Objeto objeto)
		{
			int xF = x - ancho / 2;
			int yF = y - alto / 2;
			int minX = (posicion.X < xF ? posicion.X : xF) + ancho;
			int minY = (posicion.Y < yF ? posicion.Y : yF) + alto;
			int maxX = posicion.X > xF ? posicion.X : xF;
			int maxY = posicion.Y > yF ? posicion.Y : yF;

			Rectangle movimientoPersonaje = new Rectangle(minX, minY, maxX - minX, maxY - minY);
			Rectangle posicionFinal = new Rectangle(xF, yF, ancho, alto);

			if (objeto.espacio.Intersects(posicionFinal))
				return true;
			if (objeto.espacio.Intersects(movimientoPersonaje) == false)
				return false;
			
			if(maxX >= objeto.espacio.X && minX <= objeto.espacio.X + objeto.espacio.Width)
			{
				int aux1, aux2;
				aux1 = posicion.Y > yF ? posicion.Y + alto : yF + alto;
				aux2 = posicion.Y > yF ? yF : posicion.Y;
				if(aux1 > objeto.espacio.Y + objeto.espacio.Height)
					return true;
				if(aux2 > objeto.espacio.Y + objeto.espacio.Height)
					return true;
			}


			if(maxY >= objeto.espacio.Y && minY <= objeto.espacio.Y + objeto.espacio.Height)
			{
				int aux1, aux2;
				aux1 = posicion.X > xF ? posicion.X + ancho : xF + ancho;
				aux2 = posicion.X > xF ? xF : posicion.X;
				if(aux1 > objeto.espacio.X + objeto.espacio.Width)
					return true;
				if(aux2 > objeto.espacio.X + objeto.espacio.Width)
					return true;
			}

			return false;
		}
		
		
		public void hacerDano(uint dano)
		{
			Personajes.Atributo vida = null;
			personaje.atributos.TryGetValue("idVida", out vida);
			vida.valor -= (int)dano;
			Programa.VistaGeneral.Instancia.contenedorJuego.interfazRuina.panelHudPersonajes.requestUpdateContent();
			if(vida.valor < vida.valorMin)
			{
				morir();
				personaje.vivo = false;
				if(personaje.id.Equals(Personajes.Protagonista.idProtagonista))
				{
					List<String> causaFracaso = Gestores.Partidas.Instancia.mensajesFracasoJuego;
					causaFracaso.Clear();
					causaFracaso.Add("Cuando estabas explorando una ruina, alguna trampa te ha matado mientras estabas pensando en tesoros.");
					causaFracaso.Add("Deberias prestar mas atencion a tus entornos.");

					Gestores.Partidas.Instancia.finalizarPartida(Gestores.Pantallas.EstadoJuego.Fracaso);
					return;
				}
				else
				{
					Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.personajes.Remove(this);
					Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.personajesSeleccionados.Remove(id);
					Programa.Jugador.Instancia.acompanantes.Remove(personaje.id);
					Programa.Jugador.Instancia.actualizarEspacioInventario();
				}
			}
		}


		public void morir()
		{
			habitacion.vista.interfazPersonajes.requestUpdateContent();
		}


		protected bool desplazar(int tiempoMundo)
		{
			bool llegada = false;
			Tuple<int, int> posicionSiguiente =
				movimiento.getDesplazamiento(
						tiempoMundo,
						personaje.flyweightPersonajeRuina.velocidadMovimiento,
						objetoInteraccionable,
						out llegada);
			posicion = new Rectangle(posicionSiguiente.Item1 - ancho / 2,
									posicionSiguiente.Item2 - alto / 2,
									posicion.Width, posicion.Height);
			comprobarColisionesAreasActivacion();
			return llegada;
		}


		protected void seguirCamino(List<Dijkstra.IRama> newCamino, RuinaNodo origen)
		{
			if(reloj != null)
				reloj.cancelarReloj();
			camino = newCamino;
			Dijkstra.INodo nodo = camino[0].verticeAdyacente(origen);
			RuinaNodo siguiente = (RuinaNodo)nodo;
			if(((RuinaRama)camino[0]).cambioHabitacion == true)
			{
				habitacion.vista.interfazPersonajes.requestUpdateContent();
				siguiente.habitacion.vista.interfazPersonajes.requestUpdateContent();
				origen.coordenadas = siguiente.coordenadasOpuestas;
				habitacion.personajes.Remove(this);
				habitacion = siguiente.habitacion;
				habitacion.personajes.Add(this);
				posicion = new Rectangle(origen.coordenadas.Item1, origen.coordenadas.Item2, ancho, alto);
			}
			movimiento = new Movimiento(origen, siguiente, tiempoActual, colisionFinal);
			camino.RemoveAt(0);
			vista.requestUpdateContent();
		}


		public PersonajeRuinaView crearVista()
		{
			vista = new PersonajeRuinaView(this);
			return vista;
		}


		protected Texture2D getImagenActual()
		{
			PersonajeRuinaFlyweight flyweight = personaje.flyweightPersonajeRuina;
			if(estado == Estado.Parado || estado == Estado.Interactuando)
				return flyweight.imagenParado;
			int nImagenes = flyweight.imagenesMovimiento.Count;
			if(nImagenes == 0)
				return null;
			int indice = ((int)((float)tiempoActual / (float)flyweight.velocidadAnimacion)) % nImagenes;
			return flyweight.imagenesMovimiento[indice];
		}


		protected void comprobarColisionesAreasActivacion()
		{
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego != Gestores.Pantallas.EstadoJuego.Jugando)
				return;
			List<Objeto> objetos = new List<Objeto>();
			objetos.AddRange(habitacion.objetos);
			objetos.AddRange(habitacion.proyectiles);
			foreach(Objeto objeto in objetos)
			{
				if(objeto.areaActivacion.espacio.Intersects(posicion) == true)
					objeto.activar();
			}
		}

		
		protected void crearReloj(int numeroAccionesMinimas, Reloj.CallbackFinReloj accionPosterior)
		{
			ruina.crearReloj(Gestores.Mundo.Instancia.relojFlyweights["reloj2"],
				this,
				tiempoActual,
				numeroAccionesMinimas,
				accionPosterior);
		}
	}
}




