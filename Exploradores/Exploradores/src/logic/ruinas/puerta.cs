using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class Puerta : Objeto
	{
		// enumeraciones
		public enum Pared
		{
			Arriba,
			Abajo,
			Izquierda,
			Derecha
		}

		
		// variables
		public Habitacion habitacionPrincipal { get; set; }
		public Habitacion habitacionSecundaria { get; set; }
		public RuinaNodo nodoPrincipal { get; protected set; }
		public RuinaNodo nodoSecundario { get; protected set; }
		public RuinaRama rama { get; protected set; }
		public Pared paredAsociada { get; set; }
		public float porcentajeLugar { get; set; }
		public String llave { get; set; }
		public bool cerradoConLlave { get; set; }


		// constructor
		public Puerta(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			habitacionPrincipal = null;
			habitacionSecundaria = null;
			nodoPrincipal = null;
			nodoSecundario = null;
			rama = null;
			paredAsociada = Pared.Abajo;
			porcentajeLugar = 0.5f;
			llave = null;
			cerradoConLlave = false;
		}

		
		// funciones
		public static Puerta cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Puerta puerta;

			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinasJugables[campos["ruina"]];
			Habitacion habitacionPrincipal =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacionPrincipal"]];
			Habitacion habitacionSecundaria =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacionSecundaria"]];
			
			puerta = new Puerta(campos["id"], flyweight);
			puerta.habitacionPrincipal = habitacionPrincipal;
			puerta.habitacionSecundaria = habitacionSecundaria;
			Puerta.Pared paredAsociada;
			paredAsociada = (Puerta.Pared) Enum.Parse(
						typeof(Puerta.Pared),
						campos["paredAsociada"]);
			puerta.paredAsociada = paredAsociada;
			puerta.porcentajeLugar = (float)Gestores.Mundo.parseFloat(campos["porcentajeLugar"]);
			puerta.actualizarEspacio();
			
			puerta.activado = Convert.ToBoolean(campos["activado"]);
			puerta.tiempoActivacion = Convert.ToInt32(campos["tiempoActivacion"]);
			String llave;
			if(campos.TryGetValue("llave", out llave) == true)
			{
				puerta.llave = llave;
				puerta.cerradoConLlave = !puerta.activado;
			}
			
			habitacionPrincipal.puertas.Add(puerta);
			habitacionSecundaria.puertas.Add(puerta);
			ruina.puertas.Add(puerta);
			
			puerta.generarNodosYRamas();
			puerta.rama.cambioHabitacion = true;

			return puerta;
		}


		public void actualizarEspacio()
		{
			float posX = 0;
			float posY = 0;
			
			float sizeX = objetoFlyweight.iconoPasivo.Width;
			float sizeY = objetoFlyweight.iconoPasivo.Height;

			if(paredAsociada == Pared.Arriba || paredAsociada == Pared.Abajo)
			{
				posX = habitacionPrincipal.espacio.Width + 2 * habitacionPrincipal.grosorPared;
				posX *= porcentajeLugar;
				posX -= (sizeX / 2);

				if(paredAsociada == Pared.Arriba)
				{
					posY = habitacionPrincipal.grosorPared;
					posY /= 2;
					posY -= (sizeY / 2);
				}
				else
				{
					posY = habitacionPrincipal.espacio.Height + 1.5f * habitacionPrincipal.grosorPared;
					posY -= (sizeY / 2);
				}

				posX += habitacionPrincipal.espacio.X;
				posY += habitacionPrincipal.espacio.Y;

				espacio = new Rectangle((int)posX, (int)posY, (int)sizeX, (int)sizeY);
			}
		}

		
		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new PuertaView(this, ruina);
			return vista;
		}


		public void generarNodosYRamas()
		{
			if (paredAsociada == Pared.Arriba)
			{
				nodoPrincipal = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + espacio.Width / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + (espacio.Height + (int)habitacionPrincipal.grosorPared + PersonajeRuina.alto) / 2 - habitacionPrincipal.espacio.Y));
				nodoSecundario = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + espacio.Width / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + (espacio.Height - (int)habitacionSecundaria.grosorPared - PersonajeRuina.alto) / 2 - habitacionSecundaria.espacio.Y));
				nodoPrincipal.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + espacio.Width / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + (espacio.Height - (int)habitacionPrincipal.grosorPared - PersonajeRuina.alto) / 2 - habitacionPrincipal.espacio.Y);
				nodoSecundario.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + espacio.Width / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + (espacio.Height + (int)habitacionSecundaria.grosorPared + PersonajeRuina.alto) / 2 - habitacionSecundaria.espacio.Y);
			}
			else if (paredAsociada == Pared.Abajo)
			{
				nodoSecundario = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + espacio.Width / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + (espacio.Height + (int)habitacionSecundaria.grosorPared + PersonajeRuina.alto) / 2 - habitacionSecundaria.espacio.Y));
				nodoPrincipal = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + espacio.Width / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + (espacio.Height - (int)habitacionPrincipal.grosorPared - PersonajeRuina.alto) / 2 - habitacionPrincipal.espacio.Y));
				nodoSecundario.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + espacio.Width / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + (espacio.Height - (int)habitacionSecundaria.grosorPared - PersonajeRuina.alto) / 2 - habitacionSecundaria.espacio.Y);
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
				nodoSecundario = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + (espacio.Width - (int)habitacionSecundaria.grosorPared - PersonajeRuina.ancho) / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionSecundaria.espacio.Y));
				nodoPrincipal.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + (espacio.Width - (int)habitacionPrincipal.grosorPared - PersonajeRuina.ancho) / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionPrincipal.espacio.Y);
				nodoSecundario.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + (espacio.Width + (int)habitacionSecundaria.grosorPared + PersonajeRuina.ancho) / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionSecundaria.espacio.Y);
			}
			else if (paredAsociada == Pared.Derecha)
			{
				nodoSecundario = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + (espacio.Width + (int)habitacionSecundaria.grosorPared + PersonajeRuina.ancho) / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionSecundaria.espacio.Y));
				nodoPrincipal = 
					new RuinaNodo(new Tuple<int, int>(
						espacio.X + (espacio.Width - (int)habitacionPrincipal.grosorPared - PersonajeRuina.ancho) / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionPrincipal.espacio.Y));
				nodoSecundario.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + (espacio.Width - (int)habitacionSecundaria.grosorPared - PersonajeRuina.ancho) / 2 - habitacionSecundaria.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionSecundaria.espacio.Y);
				nodoPrincipal.coordenadasOpuestas = new Tuple<int,int>(
						espacio.X + (espacio.Width + (int)habitacionPrincipal.grosorPared + PersonajeRuina.ancho) / 2 - habitacionPrincipal.espacio.X,
						espacio.Y + espacio.Height / 2 - habitacionPrincipal.espacio.Y);
			}

			nodoPrincipal.habitacion = habitacionPrincipal;
			nodoSecundario.habitacion = habitacionSecundaria;

			rama = new RuinaRama(nodoPrincipal, nodoSecundario);
			if (paredAsociada == Pared.Arriba || paredAsociada == Pared.Abajo)
				rama.distancia = espacio.Height + PersonajeRuina.alto;
			else
				rama.distancia = espacio.Height + PersonajeRuina.ancho;
		}


		public override List<RuinaNodo> nodos()
		{
			List<RuinaNodo> lista = new List<RuinaNodo>();
			if(activado == true)
			{
				if(nodoPrincipal != null)
					lista.Add(nodoPrincipal);
				if(nodoSecundario != null)
					lista.Add(nodoSecundario);
			}
			return lista;
		}
	}
}




