using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ruinas
{


	public class PuertaSalida : Puerta
	{
		// constructor
		public PuertaSalida(String newID, ObjetoFlyweight newObjetoFlyweight)
			: base(newID, newObjetoFlyweight)
		{
		}


		// funciones
		public static PuertaSalida cargarObjetoSalida(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			PuertaSalida puerta;

			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinasJugables[campos["ruina"]];
			Habitacion habitacionPrincipal =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacionPrincipal"]];
			
			puerta = new PuertaSalida(campos["id"], flyweight);
			puerta.habitacionPrincipal = habitacionPrincipal;
			puerta.habitacionSecundaria = null;
			Puerta.Pared paredAsociada;
			paredAsociada = (Puerta.Pared) Enum.Parse(
						typeof(Puerta.Pared),
						campos["paredAsociada"]);
			puerta.paredAsociada = paredAsociada;
			puerta.porcentajeLugar = (float)Gestores.Mundo.parseFloat(campos["porcentajeLugar"]);
			puerta.actualizarEspacio();
			
			puerta.tiempoActivacion = Convert.ToInt32(campos["tiempoActivacion"]);
			
			habitacionPrincipal.puertas.Add(puerta);
			ruina.puertas.Add(puerta);
			
			puerta.generarNodosYRamas();

			return puerta;
		}


		public override void generarNodosYRamas()
		{
			if (paredAsociada == Pared.Arriba)
			{
				nodoPrincipal = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + espacio.Width / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + (espacio.Height + (int)habitacionPrincipal.grosorPared + PersonajeRuina.alto) / 2 - habitacionPrincipal.espacio.Y));
				nodoPrincipal.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + espacio.Width / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + (espacio.Height - (int)habitacionPrincipal.grosorPared - PersonajeRuina.alto) / 2 - habitacionPrincipal.espacio.Y);
			}
			else if (paredAsociada == Pared.Abajo)
			{
				nodoPrincipal = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + espacio.Width / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + (espacio.Height - (int)habitacionPrincipal.grosorPared - PersonajeRuina.alto) / 2 - habitacionPrincipal.espacio.Y));
				nodoPrincipal.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + espacio.Width / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + (espacio.Height + (int)habitacionPrincipal.grosorPared + PersonajeRuina.alto) / 2 - habitacionPrincipal.espacio.Y);
			}
			else if (paredAsociada == Pared.Izquierda)
			{
				nodoPrincipal = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + (espacio.Width + (int)habitacionPrincipal.grosorPared + PersonajeRuina.ancho) / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionPrincipal.espacio.Y));
				nodoPrincipal.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + (espacio.Width - (int)habitacionPrincipal.grosorPared - PersonajeRuina.ancho) / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionPrincipal.espacio.Y);
			}
			else if (paredAsociada == Pared.Derecha)
			{
				nodoPrincipal = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + (espacio.Width - (int)habitacionPrincipal.grosorPared - PersonajeRuina.ancho) / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionPrincipal.espacio.Y));
				nodoPrincipal.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + (espacio.Width + (int)habitacionPrincipal.grosorPared + PersonajeRuina.ancho) / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionPrincipal.espacio.Y);
			}

			nodoPrincipal.habitacion = habitacionPrincipal;
			rama = null;
		}


		public override List<RuinaNodo> nodos()
		{
			List<RuinaNodo> lista = new List<RuinaNodo>();
			return lista;
		}


		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new PuertaSalidaView(this, ruina);
			return vista;
		}
	}
}