﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;





namespace Ruinas
{
	
	
	public class TesoroView : ObjetoView
	{
		// variables
		public Tesoro tesoro { get { return (Tesoro)objeto; } }


		// constructor
		public TesoroView(Tesoro newInterruptor, RuinaJugable ruina)
			: base(newInterruptor, ruina)
		{
			callbackConfigObj = new Tuple<RuinaJugable, Reloj.CallbackFinReloj>(
				ruina,
				Controller.abrirTesoro);
			onRightMousePress = Controller.buscarObjeto;
		}
	}
}



