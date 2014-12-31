using System;
using System.Collections.Generic;


namespace Personajes
{
	
	
	public abstract class Atributo : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public String nombre { get; set; }
		public String descripcion { get; set; }
		public int valorMin { get; set; }
		public int valorMax { get; set; }
		public int valor { get; set; }
		public Gestores.Imagen icono { get; set; }


		// constructor
		public Atributo(String newID, String newNombre)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = String.Copy(newID);
			nombre = String.Copy(newNombre);
			descripcion = "";
			valorMin = 0;
			valorMax = 0;
			valor = 0;
			icono = null;
		}


		// functions
		public abstract Atributo clone(List<String> campos);
		public abstract ILSXNA.Container getVistaEspecifica();

		
		public AtributoView crearVista(bool seleccionar, bool actualizarVista = true)
		{
			return new AtributoView(this, seleccionar, actualizarVista);
		}
	}
}




