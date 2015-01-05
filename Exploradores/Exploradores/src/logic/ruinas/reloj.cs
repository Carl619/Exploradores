using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public class Reloj
	{
		// delegados
		public delegate void CallbackFinReloj(Reloj reloj);


		// variables
		public RelojFlyweight relojFlyweight { get; protected set; }
		public PersonajeRuina personaje { get; protected set; }
		public CallbackFinReloj accionFinalizacion { get; protected set; }
		public int tiempoInicial { get; protected set; }
		public float tiempoActual { get; protected set; }
		public float velocidad { get; protected set; }
		public Tuple<int, int> coordenadas { get; protected set; }
		public bool haTerminado { get; protected set; }
		protected RelojView vista { get; set; }


		// constructor
		public Reloj(RelojFlyweight newRelojFlyweight, PersonajeRuina newPersonaje,
					int tiempoMundo, CallbackFinReloj accion)
		{
			if(newRelojFlyweight == null || newPersonaje == null)
				throw new ArgumentNullException();
			relojFlyweight = newRelojFlyweight;
			personaje = newPersonaje;
			personaje.reloj = this;
			accionFinalizacion = accion;
			tiempoInicial = tiempoMundo;
			tiempoActual = 0.0f;
			velocidad = 0.01f;
			coordenadas = personaje.coordenadasRuina;
			haTerminado = false;
			vista = null;
		}

		
		// funciones
		public void resetReloj(int tiempoAccionesMinimas)
		{
			tiempoInicial = tiempoAccionesMinimas;
			tiempoActual = 0.0f;
			haTerminado = false;
		}


		public void cancelarReloj()
		{
			tiempoInicial = 0;
			tiempoActual = 0.99f;
			haTerminado = true;
		}


		public void setTiempoTotal(int tiempoAccionesMinimas)
		{
			if(tiempoAccionesMinimas > 0)
				velocidad = 1.0f / (float)tiempoAccionesMinimas;
			else
				velocidad = 1.0f;
		}


		public void actualizarTiempo(int tiempoAccionesMinimas)
		{
			if(haTerminado == true)
			{
				tiempoActual = 0.99f;
				return;
			}
			float newTiempo = velocidad * (float)(tiempoAccionesMinimas - tiempoInicial);
			while(newTiempo < 0.0f)
				newTiempo += 1.0f;
			if(newTiempo >= 1.0f)
			{
				ejecutarFinReloj();
			}
			tiempoActual = newTiempo;
		}


		public Texture2D getImagenActual()
		{
			if(relojFlyweight.iconos.Count == 0)
				return null;
			int indice = (int)(tiempoActual * (float)relojFlyweight.iconos.Count);
			if(indice < 0)
				indice = 0;
			if(indice >= relojFlyweight.iconos.Count)
				indice = relojFlyweight.iconos.Count - 1;
			return relojFlyweight.iconos[indice];
		}


		public RelojView crearVista()
		{
			vista = new RelojView(this);
			return vista;
		}


		protected void ejecutarFinReloj()
		{
			if(accionFinalizacion != null)
				accionFinalizacion(this);
			haTerminado = true;
			personaje.finReloj();
		}
	}
}



