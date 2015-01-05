using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class PuertaSalidaView : ObjetoView
	{
		// variables
		public PuertaSalida puerta { get { return (PuertaSalida)objeto; } }


		// constructor
		public PuertaSalidaView(PuertaSalida newPuerta, RuinaJugable ruina)
			: base(newPuerta, ruina)
		{
			callbackConfigObj = new Tuple<RuinaJugable, Reloj.CallbackFinReloj>(
									ruina,
									Controller.activarPuertaSalida);
			onRightMousePress = Controller.buscarObjeto;
		}
	}
}




