using System;
using System.Collections.Generic;


namespace Interaccion
{
	

	public class EntradaDialogo : ElementoDialogo
	{
		// constructor
		public EntradaDialogo(String newID, String newTitulo, String newTexto)
			: base(newID, newTitulo, newTexto)
		{
		}


		// functions
		public override ElementoDialogo clone()
		{
			EntradaDialogo entrada = new EntradaDialogo(id, titulo, texto);
			entrada.evento = evento;
			entrada.bloquearNavegacion = bloquearNavegacion;
			return entrada;
		}
	}
}




