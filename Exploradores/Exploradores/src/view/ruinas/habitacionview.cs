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
		public HabitacionObjetosView interfazObjetos { get; protected set; }
		public HabitacionPersonajesView interfazPersonajes { get; protected set; }
		public Rectangle boundingBox { get { return habitacion.espacio;  } }
		public List<RuinaRama> ramas { get; set; }
		public bool mostrarRamas { get; set; }


		// constructor
		public HabitacionView(Habitacion newHabitacion)
			: base()
		{
			if (newHabitacion == null)
				throw new ArgumentNullException();
			
			habitacion = newHabitacion;
			interfazHabitacion = null;
			interfazObjetos = null;
			interfazPersonajes = null;
			ramas = null;
			mostrarRamas = false;
			onRightMousePress = Controller.moverPersonaje;

			updateContent();
		}


		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			interfazHabitacion = new ILSXNA.Container(Gestores.Mundo.Instancia.estilosHabitacion["room1"]);
			interfazHabitacion.border.background.scrollableContainer = Programa.VistaGeneral.Instancia.scrollableContainer;
			interfazHabitacion.sizeSettings.fixedInnerWidth = (uint)habitacion.espacio.Width;
			interfazHabitacion.sizeSettings.fixedInnerHeight = (uint)habitacion.espacio.Height;
			addComponent(interfazHabitacion);


			getCurrentAlternative().addLayer();
			setCurrentLayer(1);
			interfazObjetos = new HabitacionObjetosView(habitacion);
			addComponent(interfazObjetos);
			setCurrentLayer(0);
			
			
			getCurrentAlternative().addLayer();
			setCurrentLayer(2);
			interfazPersonajes = new HabitacionPersonajesView(habitacion);
			addComponent(interfazPersonajes);
			setCurrentLayer(0);

			
			getCurrentAlternative().addLayer();
			setCurrentLayer(3);
			if(ramas != null && mostrarRamas == true)
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


		public override void requestUpdateContent()
		{
			base.requestUpdateContent();
		}
	}
}




