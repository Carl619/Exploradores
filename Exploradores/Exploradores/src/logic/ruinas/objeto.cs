using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public abstract class Objeto : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public ObjetoFlyweight objetoFlyweight { get; protected set; }
		public AreaActivacion areaActivacion { get; set; }
		public Habitacion habitacion { get; set; }
		public Rectangle espacio { get; set; }
        public bool activado { get; set; }
		public int tiempoActivacion { get; set; } // numero de acciones minimas
		public int imagenActual { get; set; }
		public ObjetoView vista { get; protected set; }


		// constructor
		public Objeto(String newID, ObjetoFlyweight newFlyweight)
		{
			if(newID == null || newFlyweight == null)
				throw new ArgumentNullException();
			id = (String)newID.Clone();
			objetoFlyweight = newFlyweight;
			areaActivacion = new AreaActivacion();
			areaActivacion.objeto = this;
			habitacion = null;
			espacio = new Rectangle(0, 0, 1, 1);
			activado = false;
			tiempoActivacion = 200;
			imagenActual = 0;
			vista = null;
		}

		
		// funciones
		public virtual void activar()
		{
			activado = true;
			vista.requestUpdateContent();
		}


		public virtual bool esBloqueante()
		{
			return true;
		}


		public virtual void actualizarEstado(bool estado)
		{
			activado = estado;
			vista.requestUpdateContent();
		}


		public virtual void actualizarTiempo(int tiempo)
		{
		}


		public virtual ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new ObjetoView(this, ruina);
			return vista;
		}


		public virtual List<RuinaNodo> nodos()
		{
			List<RuinaNodo> nodos = new List<RuinaNodo>();
			if(esBloqueante() == false)
				return nodos;
			
			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							espacio.X - PersonajeRuina.ancho / 2,
							espacio.Y - PersonajeRuina.alto / 2)));
			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							espacio.X - PersonajeRuina.ancho / 2,
							espacio.Y + PersonajeRuina.alto / 2 + espacio.Height)));
			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							espacio.X + PersonajeRuina.ancho / 2 + espacio.Width,
							espacio.Y - PersonajeRuina.alto / 2)));
			nodos.Add(new RuinaNodo(
						new Tuple<int, int>(
							espacio.X + PersonajeRuina.ancho / 2 + espacio.Width,
							espacio.Y + PersonajeRuina.alto / 2 + espacio.Height)));

			return nodos;
		}
	}
}



