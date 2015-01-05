using System;
using System.Collections.Generic;
using Objetos;
using Interaccion;




namespace Personajes
{
	
	
	public class Protagonista : Personaje
	{
		// variables
		public static String idProtagonista = "idProtagonista";
		public Objetos.Inventario inventario { get; set; }
		public Mapa.LugarVisitable lugarActual { get; set; }
		public Mapa.Viaje viaje { get; protected set; }
		public Ataque ataque { get; protected set; }


		// constructor
		public Protagonista(String id, String nombre, Ruinas.PersonajeRuinaFlyweight newFlyweightRuina, Mapa.LugarVisitable lugarCreacion)
			: base(id, nombre, newFlyweightRuina)
		{
			inventario = null;
			lugarActual = lugarCreacion;
			viaje = new Mapa.Viaje();
			ataque = null;
		}


		// funciones
		public bool viajarSiguiente(bool primeraVez)
		{
			if(viaje.camino == null)
				return false;
			if(viaje.camino.Count == 0)
			{
				viaje.camino = null;
				return false;
			}

			if(primeraVez == true)
			{
				double caza = viaje.caza;
				if(caza < 1.0f)
					cazar(1);
				else
					cazar((uint)Math.Round(caza));
				if(determinarAtaque() == true)
					return true;
			}
			
			Mapa.Ruta ruta = (Mapa.Ruta)viaje.camino[0];
			Mapa.LugarVisitable lugar;
			lugar = (Mapa.LugarVisitable)(ruta.verticeAdyacente(Programa.Jugador.Instancia.protagonista.lugarActual));
			lugarActual = lugar;
			lugarActual.oculto = false;
			
			Gestores.Partidas.Instancia.avanzarTiempoMapa(0, (uint)ruta.distancia);

			viaje.camino.RemoveAt(0);
			if(viaje.camino.Count == 0)
				viaje.clear(true);
			
			return false;
		}


		public void viajeVolver()
		{
			if(viaje.camino == null)
				return;
			if(viaje.camino.Count == 0)
			{
				viaje.camino = null;
				return;
			}
			
			Mapa.Ruta ruta = (Mapa.Ruta)viaje.camino[0];
			int distancia = (int)ruta.distancia;
			if(distancia < 1)
				distancia = 1;
			int tiempoTranscurrido = Gestores.Partidas.Instancia.aleatorio.Next(
						distancia, 2 * distancia);
			double caza = viaje.caza;
			caza *= (tiempoTranscurrido / ( 2 * distancia));
			caza /= 2;
			if(caza < 1.0f)
				cazar(1);
			else
				cazar((uint)Math.Round(caza));
			Gestores.Partidas.Instancia.avanzarTiempoMapa(0, (uint)tiempoTranscurrido);
		}


		public void reclutar(NPC npc)
		{
			uint precio = 0;

			Habilidad habilidad;
			if(npc.habilidades.TryGetValue("idMercenario", out habilidad) == true)
			{
				if(habilidad.GetType() != typeof(Mercenario))
					throw new ArgumentException();
				
				Mercenario mercenario = (Mercenario)habilidad;
				precio = mercenario.precioReclutamiento;
			}
			uint dinero = dineroDisponible();
			if(dinero < precio)
				return;
			
			Acompanante acom;
			acom = npc.reclutar();
			if(acom != null)
			{
				extraerDinero(precio);
				Programa.Jugador.Instancia.acompanantes.Add(acom.id, acom);
			}
		}


		public void pagarAtacantes()
		{
			if(ataque.atacantes != Interaccion.Ataque.Atacantes.Animales)
				extraerDinero(ataque.precioTotal);
			else
				extraerComida(ataque.precioTotal);
			ataque = null;
		}


		public void defender()
		{
			int factor = 1000000;
			int resultado = Gestores.Partidas.Instancia.aleatorio.Next(0, factor);
			if(resultado >= factor * ataque.determinacion)
				return;
			
			double puntos;
			puntos = ataque.habilidadTotal *
					Math.Pow(0.99, viaje.defensa - ataque.habilidadTotal);
			
			Programa.Jugador.Instancia.hacerDanoGrupo((uint)Math.Floor(puntos));
		}


		public void huir()
		{
			int factor = 1000000;
			int resultado = Gestores.Partidas.Instancia.aleatorio.Next(0, factor);
			if(resultado >= factor * ataque.determinacion)
				return;
			Programa.Jugador.Instancia.hacerDanoGrupo((uint)(1 + ataque.habilidadTotal));
		}


		public uint dineroDisponible()
		{
			if(inventario == null)
				return 0;
			ColeccionArticulos coleccion;
			if(inventario.articulos.TryGetValue(Articulo.idDinero, out coleccion) == true)
				return coleccion.cantidad;
			return 0;
		}


		public void extraerDinero(uint cantidad)
		{
			if(dineroDisponible() < cantidad)
				throw new ArgumentException();
			inventario.removeArticulo(Articulo.idDinero, cantidad);
		}


		public uint comidaDisponible()
		{
			if(inventario == null)
				return 0;
			ColeccionArticulos coleccion;
			if(inventario.articulos.TryGetValue(Articulo.idComida, out coleccion) == true)
				return coleccion.cantidad;
			return 0;
		}


		public void extraerComida(uint cantidad)
		{
			if(comidaDisponible() < cantidad)
				throw new ArgumentException();
			inventario.removeArticulo(Articulo.idComida, cantidad);
		}


		public void cazar(uint cantidad)
		{
			inventario.addArticulo(Articulo.idComida, cantidad);
		}

		
		protected bool determinarAtaque()
		{
			int probabilidad = (int)(100.0f * viaje.probAtaque);
			int resultado = Gestores.Partidas.Instancia.aleatorio.Next(0, 10000);
			if(resultado >= probabilidad)
				return false;
			
			uint atacantes = (uint)Math.Ceiling(viaje.peligroTotal / 2);
			if(atacantes < 5)
				atacantes = 5;
			atacantes += (uint)Gestores.Partidas.Instancia.aleatorio.Next(0, (int)atacantes / 5);
			ataque = new Ataque(atacantes, viaje.probAtaque);

			Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion =
					Gestores.Pantallas.EstadoInteraccion.Defensa;
				Programa.VistaGeneral.Instancia.contenedorJuego.actualizarVentanaDefensa();
			return true;
		}
	}
}




