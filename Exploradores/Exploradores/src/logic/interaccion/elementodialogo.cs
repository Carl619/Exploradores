using System;
using System.Collections.Generic;
using System.IO;




namespace Interaccion
{
	

	public abstract class ElementoDialogo : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; private set; }
		public String titulo { get; set; }
		public String texto { get; set; }
		public Evento evento { get; set; }
		public bool bloquearNavegacion { get; set; }


		// constructor
		public ElementoDialogo(String newID, String newTitulo, String newTexto)
		{
			id = String.Copy(newID);
			titulo = String.Copy(newTitulo);
			texto =  String.Copy(newTexto);
			evento = null;
			bloquearNavegacion = false;
		}


		// functions
		public abstract ElementoDialogo clone();
	}
}
