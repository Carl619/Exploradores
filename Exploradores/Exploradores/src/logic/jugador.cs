using System;
using System.Collections.Generic;




namespace Programa
{
	
	
	public class Jugador
	{
		// variables
		private static Jugador instancia = null;
		public static Jugador Instancia
		{
			get
			{
				if(instancia == null)
					new Jugador();
				return instancia;
			}
		}
		public String nombre { get; protected set; }
		public Personajes.Protagonista protagonista { get; protected set; }
		public Dictionary<String, Personajes.Acompanante> acompanantes { get; protected set; }


		// constructor
		private Jugador()
		{
			instancia = this;
			nombre = "";
			protagonista = null;
			acompanantes = new Dictionary<String, Personajes.Acompanante>();
		}


		// funciones
		public void crearProtagonista(String nombreJugador, Mapa.LugarVisitable posicionProtagonista)
		{
			nombre = (String)nombreJugador.Clone();
			protagonista = new Personajes.Protagonista(
								Personajes.Protagonista.idProtagonista,
								nombreJugador,
								Gestores.Mundo.Instancia.personajeRuinaFlyweights["1"],
								posicionProtagonista);
			protagonista.inventario = Gestores.Partidas.Instancia.inventarios["1"];
			protagonista.avatarSeleccionado = Gestores.Mundo.Instancia.imagenes["protagonista"];
			protagonista.avatar = Gestores.Mundo.Instancia.imagenes["protagonistab"];
			actualizarEspacioInventario();
		}


		public List<Personajes.Personaje> getPersonajesGrupo()
		{
			List<Personajes.Personaje> personajes = new List<Personajes.Personaje>();
			personajes.Add(protagonista);
			foreach(KeyValuePair<String, Personajes.Acompanante> acompanante in acompanantes)
				personajes.Add(acompanante.Value);
			return personajes;
		}


		public void hacerDanoGrupo(uint puntos)
		{
			List<Personajes.Personaje> personajes = getPersonajesGrupo();
			
			for(int i=0; i<puntos;)
			{
				int indice = Gestores.Partidas.Instancia.aleatorio.Next(0, personajes.Count);
				if(personajes[indice].vivo == false)
					continue;
				
				Personajes.Atributo atributo;
				if(personajes[indice].atributos.TryGetValue("idVida", out atributo) == false)
					throw new NullReferenceException();
				if(atributo.GetType() != typeof(Personajes.Vida))
					throw new NullReferenceException();
				
				atributo.valor -= 1;
				if(atributo.valor < atributo.valorMin)
				{
					personajes[indice].vivo = false;
					if(personajes[indice].id.Equals(protagonista.id))
					{
						List<String> causaFracaso = Gestores.Partidas.Instancia.mensajesFracasoJuego;
						causaFracaso.Clear();
						causaFracaso.Add("En tus viajes te han atacado varios enemigos. Al final, tus defensas no han sido suficientes, y te han matado.");
						causaFracaso.Add("Quizas deberias haber reclutado mas mercenarios, o coger rutas mas seguras.");

						Gestores.Partidas.Instancia.finalizarPartida(Gestores.Pantallas.EstadoJuego.Fracaso);
						return;
					}
					else
					{
						acompanantes.Remove(personajes[indice].id);
						actualizarEspacioInventario();
					}
				}
				++i;
			}
		}


		public void addHambre(uint numeroHoras)
		{
			List<Personajes.Personaje> personajes = getPersonajesGrupo();

			foreach(Personajes.Personaje personaje in personajes)
			{
				Personajes.Atributo atributo;
				if(personaje.atributos.TryGetValue("idHambre", out atributo) == false)
					throw new NullReferenceException();
				if(atributo.GetType() != typeof(Personajes.Hambre))
					throw new NullReferenceException();
				
				atributo.valor += (int)numeroHoras;
			}
		}


		public void comerPorPrioridadGrupo()
		{
			int prioridadProtagonista = 1000000;
			List<Personajes.Personaje> personajes = getPersonajesGrupo();
			
			SortedDictionary<int, List<Personajes.Personaje>> diccionario;
			diccionario = new SortedDictionary<int, List<Personajes.Personaje>>();
			
			protagonista.atributos["idHambre"].valor += prioridadProtagonista;
			foreach(Personajes.Personaje personaje in personajes)
			{
				int valor = personaje.atributos["idHambre"].valor;
				List<Personajes.Personaje> lista;
				if(diccionario.TryGetValue(valor, out lista) == true)
				{
					lista.Add(personaje);
				}
				else
				{
					lista = new List<Personajes.Personaje>();
					lista.Add(personaje);
					diccionario.Add(valor, lista);
				}
			}
			protagonista.atributos["idHambre"].valor -= prioridadProtagonista;


			int hambreMax = protagonista.atributos["idHambre"].valorMax;
			foreach(KeyValuePair<int, List<Personajes.Personaje>> lista in diccionario)
			{
				foreach(Personajes.Personaje personaje in lista.Value)
				{
					int diferencia = personaje.atributos["idHambre"].valor - hambreMax;
					if(diferencia <= 0)
						continue;
					uint cantidad = (uint)Math.Ceiling(((double)diferencia) * Mapa.Viaje.consumoPorHoraBase);
					if(protagonista.comidaDisponible() < cantidad)
						continue;
					protagonista.extraerComida(cantidad);
					int consumo = (int)Math.Ceiling(((double)cantidad / Mapa.Viaje.consumoPorHoraBase));
					personaje.atributos["idHambre"].valor -= consumo;
				}
			}

			
			foreach(KeyValuePair<int, List<Personajes.Personaje>> lista in diccionario)
			{
				foreach(Personajes.Personaje personaje in lista.Value)
				{
					if(personaje.atributos["idHambre"].valor > hambreMax)
					{
						personaje.vivo = false;
						if(personaje.id.Equals(protagonista.id))
						{
							List<String> causaFracaso = Gestores.Partidas.Instancia.mensajesFracasoJuego;
							causaFracaso.Clear();
							causaFracaso.Add("Mientras estabas viajando, te han encontrado en una situacion en la que ya no tenias que comer. Desafortunadamente, has muerto por causa de hambre");
							causaFracaso.Add("Quizas deberias haber asegurado provisiones mas amplias para tu grupo, o reclutar a mas cazadores.");

							Gestores.Partidas.Instancia.finalizarPartida(Gestores.Pantallas.EstadoJuego.Fracaso);
							return;
						}
						else
						{
							acompanantes.Remove(personaje.id);
							actualizarEspacioInventario();
						}
					}
				}
			}
		}


		public void actualizarEspacioInventario()
		{
			double fuerzaTotal = getFuerza();
			if(fuerzaTotal < Objetos.Inventario.espacioMinimo)
				fuerzaTotal = Objetos.Inventario.espacioMinimo;
			protagonista.inventario.espacioMax = (uint)fuerzaTotal;
		}


		public double getFuerza()
		{
			List<Personajes.Personaje> personajes = getPersonajesGrupo();
			
			double fuerzaTotal = 0.0f;
			Personajes.Atributo atributo;
			Personajes.Habilidad habilidad;
			foreach(Personajes.Personaje personaje in personajes)
			{
				double habilidadPersonaje = 0.0f;
				if(personaje.atributos.TryGetValue("idFuerza", out atributo) == true)
				{
					if(atributo.GetType() != typeof(Personajes.Fuerza))
						continue;
					Personajes.Fuerza fuerza = (Personajes.Fuerza)atributo;
					habilidadPersonaje = (double)fuerza.valor;
				}
				if(personaje.habilidades.TryGetValue("idEnfermedad", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Enfermedad))
					{
						fuerzaTotal += habilidadPersonaje;
						continue;
					}
					Personajes.Enfermedad enfermedad = (Personajes.Enfermedad)habilidad;
					habilidadPersonaje /= (enfermedad.eficacia * enfermedad.eficacia);
					if(habilidadPersonaje < 0.0f)
						habilidadPersonaje = 0.0f;
				}
				fuerzaTotal += habilidadPersonaje;
			}

			return fuerzaTotal;
		}


		public double getHabilidadDefensa()
		{
			List<Personajes.Personaje> personajes = getPersonajesGrupo();
			
			double defensa = 0.0f;
			Personajes.Habilidad habilidad;
			foreach(Personajes.Personaje personaje in personajes)
			{
				double habilidadPersonaje = 0.0f;
				if(personaje.habilidades.TryGetValue("idMercenario", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Mercenario))
						continue;
					Personajes.Mercenario mercenario = (Personajes.Mercenario)habilidad;
					habilidadPersonaje = (double)mercenario.defensa;
				}
				if(personaje.habilidades.TryGetValue("idEnfermedad", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Enfermedad))
					{
						defensa += habilidadPersonaje;
						continue;
					}
					Personajes.Enfermedad enfermedad = (Personajes.Enfermedad)habilidad;
					habilidadPersonaje /= (enfermedad.eficacia * enfermedad.eficacia);
					if(habilidadPersonaje < 0.0f)
						habilidadPersonaje = 0.0f;
				}
				defensa += habilidadPersonaje;
			}

			return defensa;
		}


		public double getHabilidadCaza()
		{
			List<Personajes.Personaje> personajes = getPersonajesGrupo();
			
			double caza = 0.0f;
			Personajes.Habilidad habilidad;
			foreach(Personajes.Personaje personaje in personajes)
			{
				double habilidadPersonaje = 0.0f;
				if(personaje.habilidades.TryGetValue("idCazador", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Cazador))
						continue;
					Personajes.Cazador cazador = (Personajes.Cazador)habilidad;
					habilidadPersonaje = (double)cazador.habilidad;
				}
				if(personaje.habilidades.TryGetValue("idEnfermedad", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Enfermedad))
					{
						caza += habilidadPersonaje;
						continue;
					}
					Personajes.Enfermedad enfermedad = (Personajes.Enfermedad)habilidad;
					habilidadPersonaje /= enfermedad.eficacia;
					if(habilidadPersonaje < 0.0f)
						habilidadPersonaje = 0.0f;
				}
				caza += habilidadPersonaje;
			}

			return caza;
		}


		public double getTasaCompras()
		{
			List<Personajes.Personaje> personajes = getPersonajesGrupo();
			
			double tasa = 1.0f;
			Personajes.Habilidad habilidad;
			foreach(Personajes.Personaje personaje in personajes)
			{
				double habilidadPersonaje = 1.0f;
				if(personaje.habilidades.TryGetValue("idComerciante", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Comerciante))
						continue;
					Personajes.Comerciante comerciante = (Personajes.Comerciante)habilidad;
					habilidadPersonaje = (double)comerciante.tasaCompras;
				}
				if(personaje.habilidades.TryGetValue("idEnfermedad", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Enfermedad))
					{
						if(habilidadPersonaje < 1.0f)
							tasa *= habilidadPersonaje;
						continue;
					}
					Personajes.Enfermedad enfermedad = (Personajes.Enfermedad)habilidad;
					habilidadPersonaje *= enfermedad.eficacia;
				}
				if(habilidadPersonaje < 1.0f)
					tasa *= habilidadPersonaje;
			}

			return tasa;
		}


		public double getTasaVentas()
		{
			List<Personajes.Personaje> personajes = getPersonajesGrupo();
			
			double tasa = 1.0f;
			Personajes.Habilidad habilidad;
			foreach(Personajes.Personaje personaje in personajes)
			{
				double habilidadPersonaje = 1.0f;
				if(personaje.habilidades.TryGetValue("idComerciante", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Comerciante))
						continue;
					Personajes.Comerciante comerciante = (Personajes.Comerciante)habilidad;
					habilidadPersonaje = (double)comerciante.tasaVentas;
				}
				if(personaje.habilidades.TryGetValue("idEnfermedad", out habilidad) == true)
				{
					if(habilidad.GetType() != typeof(Personajes.Enfermedad))
					{
						if(habilidadPersonaje > 1.0f)
							tasa *= habilidadPersonaje;
						continue;
					}
					Personajes.Enfermedad enfermedad = (Personajes.Enfermedad)habilidad;
					habilidadPersonaje /= enfermedad.eficacia;
				}
				if(habilidadPersonaje > 1.0f)
					tasa *= habilidadPersonaje;
			}

			return tasa;
		}
	}
}




