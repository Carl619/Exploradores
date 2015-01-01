using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	
	
	public partial class RuinaJugableView : InterfazGrafica
	{
		// variables
		public RuinaJugable ruina { get; protected set; }
		public InterfazGrafica interfazHabitaciones { get; protected set; }
		public InterfazGrafica interfazPuertas { get; protected set; }
		public InterfazRelojesView interfazRelojes { get; protected set; }
		public Objetos.PanelInventarioRuina panelInventario { get; protected set; }
		public List<Dijkstra.IRama> camino { get; set; }


		// constructor
		public RuinaJugableView(RuinaJugable newRuina)
			: base()
		{
			if (newRuina == null)
				throw new ArgumentNullException();
			
			ruina = newRuina;
			interfazHabitaciones = null;
			interfazPuertas = null;
			interfazRelojes = null;
			camino = null;
			panelInventario = null;

			alternativeNames.Add("vacio", 0);
			alternativeNames.Add("inventario", 1);

			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			
			interfazHabitaciones = new InterfazGrafica();
			addComponent(interfazHabitaciones);

			getCurrentAlternative().addLayer();
			getCurrentAlternative().addLayer();

			setCurrentLayer(1);
			interfazPuertas = new InterfazGrafica();
			addComponent(interfazPuertas);
			setCurrentLayer(0);

			setCurrentLayer(2);
			interfazRelojes = new InterfazRelojesView(ruina);
			addComponent(interfazRelojes);
			

			setCurrentLayer(3);
			panelInventario = new Objetos.PanelInventarioRuina();
			addComponent(panelInventario);
			setCurrentLayer(0);
			
			actualizarHabitaciones();
			actualizarPuertas();
			interfazRelojes.updateContent();
		}


		public void actualizarHabitaciones()
		{
			interfazHabitaciones.clearComponents();
			
			foreach (Habitacion habitacion in ruina.habitaciones)
			{
				interfazHabitaciones.getCurrentAlternative().addLayer();
				++interfazHabitaciones.getCurrentAlternative().defaultLayer;

				interfazHabitaciones.getCurrentAlternative().getCurrentLayer().offsetX =
					(int)habitacion.espacio.X;
				interfazHabitaciones.getCurrentAlternative().getCurrentLayer().offsetY =
					(int)habitacion.espacio.Y;
				
				HabitacionView habitacionView;
				habitacionView = habitacion.crearVista();
				interfazHabitaciones.addComponent(habitacionView);
			}
		}


		public void actualizarPuertas()
		{
			interfazPuertas.clearComponents();
			
			foreach (Puerta puerta in ruina.puertas)
			{
				interfazPuertas.getCurrentAlternative().addLayer();
				++interfazPuertas.getCurrentAlternative().defaultLayer;

				interfazPuertas.getCurrentAlternative().getCurrentLayer().offsetX =
					(int)puerta.espacio.X;
				interfazPuertas.getCurrentAlternative().getCurrentLayer().offsetY =
					(int)puerta.espacio.Y;
				
				ObjetoView objetoView;
				objetoView = puerta.crearVista(ruina);
				interfazPuertas.addComponent(objetoView);
			}
		}

		public void abrirInventario(bool showInventario)
		{
			if (showInventario)
			{
				setCurrentAlternative(alternativeNames["inventario"]);
			}
			else
			{
				setCurrentAlternative(alternativeNames["vacio"]);
			}
		}
	}
}




