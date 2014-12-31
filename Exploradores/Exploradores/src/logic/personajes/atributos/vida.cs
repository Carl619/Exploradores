using System;
using System.Collections.Generic;


namespace Personajes
{
	
	
	public class Vida : Atributo
	{
		// constructor
		public Vida(String newID, String newNombre)
			: base(newID, newNombre)
		{
			valorMin = 0;
			valorMax = 100;
			valor = 1;
		}


		// functions
		public override Atributo clone(List<String> campos)
		{
			Vida vida = new Vida(id, nombre);
			vida.valorMin = valorMin;
			vida.valorMax = valorMax;
			vida.valor = Convert.ToInt32(campos[0]);
			vida.descripcion = descripcion;
			vida.icono = icono;
			return vida;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			return contenedor;
		}
	}
}




