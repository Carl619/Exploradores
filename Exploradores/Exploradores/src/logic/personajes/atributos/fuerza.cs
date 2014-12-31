using System;
using System.Collections.Generic;


namespace Personajes
{
	
	
	public class Fuerza : Atributo
	{
		// constructor
		public Fuerza(String newID, String newNombre)
			: base(newID, newNombre)
		{
			valorMin = 0;
			valorMax = 0;
			valor = 0;
		}


		// functions
		public override Atributo clone(List<String> campos)
		{
			Fuerza fuerza = new Fuerza(id, nombre);
			fuerza.valorMin = valorMin;
			fuerza.valorMax = valorMax;
			fuerza.valor = Convert.ToInt32(campos[0]);
			fuerza.descripcion = descripcion;
			fuerza.icono = icono;
			return fuerza;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			return contenedor;
		}
	}
}




