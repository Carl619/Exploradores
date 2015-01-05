using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mapa;




namespace Ruinas
{
	
	
	public class HabitacionObjetosView : InterfazGrafica
	{
		// variables
		public Habitacion habitacion { get; protected set; }


		// constructor
		public HabitacionObjetosView(Habitacion newHabitacion)
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
			
			List<Objeto> objetos = new List<Objeto>();
			objetos.AddRange(habitacion.objetos);
			objetos.AddRange(habitacion.proyectiles);
			int i = 0, count = objetos.Count;
			foreach (Objeto objeto in objetos)
			{
				addComponent(objeto.crearVista(habitacion.ruina));

				getCurrentAlternative().getCurrentLayer().offsetX =
					(int)objeto.espacio.X;
				getCurrentAlternative().getCurrentLayer().offsetY =
					(int)objeto.espacio.Y;
				
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




