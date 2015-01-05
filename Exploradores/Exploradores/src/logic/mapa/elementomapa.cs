using System;
using System.Collections.Generic;




namespace Mapa
{


	public abstract class ElementoMapa : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; } // id tiene que ser el nombre del lugar
		public bool oculto { get; set; }


		// constructor
		public ElementoMapa(String newID, bool visible = true)
		{
			id = (String)newID.Clone();
			oculto = !visible;
		}
	}

}




