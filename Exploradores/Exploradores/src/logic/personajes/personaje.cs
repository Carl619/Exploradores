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
		public Ruinas.PersonajeRuinaFlyweight flyweightPersonajeRuina { get; set; }
		public Gestores.Imagen avatarSeleccionado { get; set; }
		public Gestores.Imagen avatar { get; set; }
		public bool vivo { get; set; }


		// constructor
		public Personaje(String newID, String newNombre, Ruinas.PersonajeRuinaFlyweight newFlyweightRuina)
		{
			if(newID == null || newNombre == null || newFlyweightRuina == null)
				throw new ArgumentNullException();
			id = String.Copy(newID);
			nombre = String.Copy(newNombre);
			habilidades = new Dictionary<String, Habilidad>();
			atributos = new Dictionary<String, Atributo>();
			flyweightPersonajeRuina = newFlyweightRuina;
			avatarSeleccionado = null;
			avatar = null;
			vivo = true;
		}


		// funciones
		public PersonajeView crearVistaPersonaje(bool seleccionar, bool actualizarVista = true)
		{
			return new PersonajeView(this, seleccionar, actualizarVista);
		}
	}
}




