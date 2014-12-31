using System;
using System.Collections.Generic;




namespace Personajes
{
	
	
	public abstract class Habilidad : Gestores.IObjetoIdentificable
	{
		// variables
		public String id { get; protected set; }
		public String nombre { get; set; }
		public String descripcion { get; set; }
		public uint nivel { get; protected set; }
		public Atributo dependeciaAtributo { get; set; }
		public Gestores.Imagen icono { get; set; }
		public Dictionary<String, String> cadenasRegexDialogo { get; protected set; } // cadenas para Regex replace


		// constructor
		public Habilidad(String newID, String newNombre, uint newNivel = 1)
		{
			if(newID == null || newNombre == null || newNivel < 1)
				throw new ArgumentException();
			id = String.Copy(newID);
			nombre = String.Copy(newNombre);
			descripcion = "";
			nivel = newNivel;
			dependeciaAtributo = null;
			icono = null;
			cadenasRegexDialogo = new Dictionary<String, String>();
		}


		// funciones
		public abstract Habilidad clone(List<String> campos);
		public abstract ILSXNA.Container getVistaEspecifica();
		public abstract ILSXNA.Container getVistaEspecificaCompleta();


		public virtual void incrementarNivel(uint numero)
		{
			nivel += numero;
		}

		
		public HabilidadView crearVista(bool seleccionar, bool actualizarVista = true)
		{
			return new HabilidadView(this, seleccionar, actualizarVista);
		}
	}
}




