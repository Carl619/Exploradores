using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mapa;




namespace Ruinas
{
	
	
	public class HabitacionView : InterfazGrafica
	{
		// variables
		public Habitacion habitacion { get; protected set; }
		public ILSXNA.Container interfazHabitacion { get; protected set; }
		public Rectangle boundingBox { get { return habitacion.espacio;  } }
		public List<RuinaRama> ramas { get; set; }


		// constructor
		public HabitacionView(Habitacion newHabitacion)
			: base()
		{
			if (newHabitacion == null)
				throw new ArgumentNullException();
			
			habitacion = newHabitacion;
			interfazHabitacion = null;
			ramas = null;
			onMousePress = Controller.moverPersonaje;

			updateContent();
		}


		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			interfazHabitacion = new ILSXNA.Container(Gestores.Mundo.Instancia.estilosHabitacion["room1"]);
			interfazHabitacion.sizeSettings.fixedInnerWidth = (uint)habitacion.espacio.Width;
			interfazHabitacion.sizeSettings.fixedInnerHeight = (uint)habitacion.espacio.Height;
			addComponent(interfazHabitacion);

			foreach (Objeto objeto in habitacion.objetos)
			{
				getCurrentAlternative().addLayer();
				++getCurrentAlternative().defaultLayer;

				addComponent(objeto.crearVista(habitacion.ruina));

				getCurrentAlternative().getCurrentLayer().offsetX =
					(int)objeto.espacio.X;
				getCurrentAlternative().getCurrentLayer().offsetY =
					(int)objeto.espacio.Y;
			}


			foreach (PersonajeRuina personaje in habitacion.personajes)
			{
				getCurrentAlternative().addLayer();
				++getCurrentAlternative().defaultLayer;

				addComponent(personaje.crearVista());

				getCurrentAlternative().getCurrentLayer().offsetX =
					(int)personaje.posicion.X;
				getCurrentAlternative().getCurrentLayer().offsetY =
					(int)personaje.posicion.Y;
			}
			
			if(ramas != null)
			{
				foreach(RuinaRama rama in ramas)
				{
					getCurrentAlternative().addLayer();
					++getCurrentAlternative().defaultLayer;

					RutaView rutaView = new RutaView(rama, false);
					
					int minCoordenadaX, minCoordenadaY;
					minCoordenadaX = rama.extremos.Item1.coordenadas.Item1;
					if(minCoordenadaX < rama.extremos.Item2.coordenadas.Item1)
						minCoordenadaX = rama.extremos.Item2.coordenadas.Item1;
					minCoordenadaY = rama.extremos.Item1.coordenadas.Item2;
					if(minCoordenadaY < rama.extremos.Item2.coordenadas.Item2)
						minCoordenadaY = rama.extremos.Item2.coordenadas.Item2;
				
					getCurrentAlternative().getCurrentLayer().offsetX = minCoordenadaX;
					getCurrentAlternative().getCurrentLayer().offsetY = minCoordenadaY;
					addComponent(rutaView);
				}
			}
			
			setCurrentLayer(0);
		}
	}
}




