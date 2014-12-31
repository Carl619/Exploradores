using System;
using System.Collections.Generic;




namespace Pruebas
{
	

	public class PruebasScript : PruebaUnitaria
	{
		// variables
		private int var1 { get; set; }
		private Interaccion.Script script { get; set; }
		private Interaccion.Evento evento { get; set; }
		
		
		// constructor
		public PruebasScript() : base()
		{
			var1 = 0;
			script = null;
			evento = new Interaccion.EventoAtomico("1", 0);
			((Interaccion.EventoAtomico)evento).accion = accionEvento1;

			lista.Add(prueba1);
		}
		
		
		// funciones
		public override void beforeTest()
		{
			var1 = 0;
			script = new Interaccion.Script("1");
			script.eventos.Add(evento);
		}
		
		
		protected bool prueba1()
		{
			List<String> valoresEntrada = new List<String>();
			script.ejecutar(valoresEntrada);
			if(var1 == 5)
				return true;
			
			return false;
		}


		protected void accionEvento1(List<String> valoresEntrada)
		{
			var1 = 5;
		}
	}
}




