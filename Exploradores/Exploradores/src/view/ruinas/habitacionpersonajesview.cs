using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mapa;




namespace Ruinas
{
	
	
	public class HabitacionPersonajesView : InterfazGrafica
	{
		// variables
		public Habitacion habitacion { get; protected set; }


		// constructor
		public HabitacionPersonajesView(Habitacion newHabitacion)
			: base()
		{
			if (newHabitacion == null)
				throw new ArgumentNullException();
			
			habitacion = newHabitacion;
			updateContent();
		}


		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			habitacion.removePersonajesMuertos();

			int i = 0, count = habitacion.personajes.Count;
			foreach (PersonajeRuina personaje in habitacion.personajes)
			{
				addComponent(personaje.crearVista());

				getCurrentAlternative().getCurrentLayer().offsetX =
					(int)personaje.posicion.X;
				getCurrentAlternative().getCurrentLayer().offsetY =
					(int)personaje.posicion.Y;
				
				if(++i != count)
				{
					getCurrentAlternative().addLayer();
					++getCurrentAlternative().defaultLayer;
				}
			}
		}


		public override void requestUpdateContent()
		{
			base.requestUpdateContent();
		}
	}
}




