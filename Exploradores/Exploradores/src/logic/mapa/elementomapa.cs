using System;
using System.Collections.Generic;




namespace Mapa
{


	public abstract class ElementoMapa : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; } // id tiene que ser el nombre del lugar


		// constructor
		public ElementoMapa(String newID)
		{
			id = (String)newID.Clone();
		}
	}

}




