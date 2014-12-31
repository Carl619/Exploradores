using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class ActivadorView : ObjetoView
	{
		// variables
		public Activador interruptor { get { return (Activador)objeto; } }


		// constructor
		public ActivadorView(Activador newInterruptor, RuinaJugable ruina)
			: base(newInterruptor, ruina)
		{
			callbackConfigObj = new Tuple<RuinaJugable, Reloj.CallbackFinReloj>(
				ruina,
				Controller.activarInterruptor);
			onMousePress = Controller.buscarObjeto;
		}
	}
}




