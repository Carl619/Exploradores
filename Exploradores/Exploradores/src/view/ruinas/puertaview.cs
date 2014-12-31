using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class PuertaView : ObjetoView
	{
		// variables
		public Puerta puerta { get { return (Puerta)objeto; } }


		// constructor
		public PuertaView(Puerta newPuerta, RuinaJugable ruina)
			: base(newPuerta, ruina)
		{
			callbackConfigObj = new Tuple<RuinaJugable, Reloj.CallbackFinReloj>(
									ruina,
									Controller.activarPuerta);
			onMousePress = Controller.buscarObjeto;
		}
	}
}




