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
		public Mapa.LugarVisitable lugarActual { get; set; }
		public List<Dijkstra.IRama> camino { get; set; }


		// constructor
		public Protagonista(String id, String nombre, Mapa.LugarVisitable lugarCreacion)
			: base(id, nombre)
		{
			lugarActual = lugarCreacion;
			camino = null;
		}


		// funciones
		public void viajarSiguiente()
		{
			if(camino == null)
				return;
			if(camino.Count == 0)
			{
				camino = null;
				return;
			}

			Mapa.Ruta ruta = (Mapa.Ruta)camino[0];
			Mapa.LugarVisitable lugar;
			lugar = (Mapa.LugarVisitable)(ruta.verticeAdyacente(Programa.Jugador.Instancia.protagonista.lugarActual));
			lugarActual = lugar;
			
			Gestores.Partidas.Instancia.avanzarTiempoMapa(0, (uint)ruta.distancia);

			camino.RemoveAt(0);
			if(camino.Count == 0)
				camino = null;
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
				Programa.Jugador.Instancia.grupoAcompanantes.Add(acom);
			}
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
	}
}




