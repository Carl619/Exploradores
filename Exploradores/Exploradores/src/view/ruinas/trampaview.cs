using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Personajes;




namespace Ruinas
{
	
	
	public class TrampaView : ObjetoView
	{
		// constructor
		public TrampaView(Trampa newTrampa, RuinaJugable ruina)
			: base(newTrampa, ruina)
		{
		}


		// funciones
		/*
		public void ejecutar(Personaje persona)
		{
			if (trampa.activado)
			{
				trampa.hacerDano(persona);
			}
			else
			{

			}
		}*/
	}
}




