using System;
using System.Collections.Generic;


namespace Personajes
{
	
	
	public class Hambre : Atributo
	{
		// constructor
		public Hambre(String newID, String newNombre)
			: base(newID, newNombre)
		{
			valorMin = 0;
			valorMax = 0;
			valor = 0;
		}


		// functions
		public override Atributo clone(List<String> campos)
		{
			Hambre hambre = new Hambre(id, nombre);
			hambre.valorMin = valorMin;
			hambre.valorMax = valorMax;
			hambre.valor = Convert.ToInt32(campos[0]);
			hambre.descripcion = descripcion;
			hambre.icono = icono;
			return hambre;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			return contenedor;
		}
	}
}




