using System;
using System.Collections.Generic;
using Objetos;
using Interaccion;




namespace Personajes
{
	
	
	public class Acompanante : Personaje, Gestores.IObjetoIdentificable
	{
		// constructor
		public Acompanante(String id, String nombre)
			: base(id, nombre)
		{
		}
	}
}




