using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public abstract class Trampa : Objeto
	{
		// variables
		public int tiempoActual { get; set; }
		public int tiempoInicial { get; set; }
		public bool tiempoInicialAsignado { get; set; }


		// constructor
        public Trampa(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			tiempoActual = 0;
			tiempoInicial = 0;
			tiempoInicialAsignado = false;
		}


		// funciones
		public override bool esBloqueante()
		{
			return false;
		}


		public override void activar()
		{
			tiempoInicial = tiempoActual;
			base.activar();
		}


		public override void actualizarTiempo(int tiempo)
		{
			if(tiempoInicialAsignado == false)
			{
				tiempoInicial = tiempo;
				tiempoInicialAsignado = true;
			}

			tiempoActual = tiempo;
			if(tiempoActual - tiempoInicial >= tiempoActivacion)
			{
				activado = false;
			}
		}


		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new TrampaView(this, ruina);
			return vista;
		}
	}
}




