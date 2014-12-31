using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public class PersonajeRuinaView : ILSXNA.Sprite
	{
		// variables
		public PersonajeRuina personaje { get; set; }
		public bool actualizacionContinua { get { return personaje.estado != PersonajeRuina.Estado.Parado; } }


		// constructor
		public PersonajeRuinaView(PersonajeRuina newpersonaje)
			: base()
		{
			if (newpersonaje == null)
				throw new ArgumentNullException();
			
			personaje = newpersonaje;
			onMousePress = Ruinas.Controller.selectPersonaje;
			innerComponent = personaje.imagenActual;
			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			requestedContentUpdate = actualizacionContinua;
			if(parent != null)
			{
				parent.offsetX = personaje.posicion.X;
				parent.offsetY = personaje.posicion.Y;
				innerComponent = personaje.imagenActual;
			}
		}
	}
}



