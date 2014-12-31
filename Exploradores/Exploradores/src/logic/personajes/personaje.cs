using System;
using System.Collections.Generic;




namespace Personajes
{
	
	
	public abstract class Personaje
	{
		// variables
		public String id { get; protected set; }
		public String nombre { get; set; }
		public Dictionary<String, Habilidad> habilidades { get; protected set; }
		public Dictionary<String, Atributo> atributos { get; protected set; }
		public Objetos.Inventario inventario { get; set; }


		// constructor
		public Personaje(String newID, String newNombre)
		{
			id = String.Copy(newID);
			nombre = String.Copy(newNombre);
			habilidades = new Dictionary<String, Habilidad>();
			atributos = new Dictionary<String, Atributo>();
			inventario = null;
		}


		// funciones
		public PersonajeView crearVistaPersonaje(bool seleccionar, bool actualizarVista = true)
		{
			return new PersonajeView(this, seleccionar, actualizarVista);
		}
	}
}




