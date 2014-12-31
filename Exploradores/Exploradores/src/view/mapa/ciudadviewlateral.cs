using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	
	
	public class CiudadViewLateral : InterfazGrafica
	{
		// variables
		public Programa.PanelLateral panelLateral { get; protected set; }
		public Ciudad ciudad { get; protected set; }
		public ListaEdificiosView edificiosView { get; protected set; }
		public String edificioSeleccionado { get; protected set; }


		// constructor
		public CiudadViewLateral(Ciudad newCiudad, Programa.PanelLateral newPanelLateral)
			: base()
		{
			if (newCiudad == null || newPanelLateral == null)
				throw new ArgumentNullException();
			
			ciudad = newCiudad;
			panelLateral = newPanelLateral;

			edificiosView = null;
			edificioSeleccionado = null;
			sizeSettings.minInnerWidth = panelLateral.anchoSeleccion;
			layout.equalCellWidth = true;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			ILSXNA.Label label;
			
			label = new ILSXNA.Label();
			label.message = (String)ciudad.nombre.Clone();
			label.color = ciudad.flyweightCiudad.colorTexto;
			label.innerComponent = ciudad.flyweightCiudad.spriteFont;
			addComponent(label);

			ILSXNA.Button boton = new ILSXNA.Button("Ver Mapa", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["mapa"].textura);
			boton.updateContent();
			boton.onButtonPress = Controller.salirCiudad;
			addComponent(boton);

			boton = new ILSXNA.Button("Informacion", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["info"].textura);
			boton.updateContent();
			boton.callbackConfigObj =
				new Tuple<Programa.PanelLateral, LugarVisitable>
				(panelLateral, ciudad);
			boton.onButtonPress = Controller.verInformacionLugar;
			addComponent(boton);

			if(ciudad.generarListaEdificios().Count > 0)
			{
				label = new ILSXNA.Label();
				label.message = " ";
				label.color = ciudad.flyweightCiudad.colorTexto;
				label.innerComponent = ciudad.flyweightCiudad.spriteFont;
				addComponent(label);

				label = new ILSXNA.Label();
				label.message = "Lugares";
				label.color = ciudad.flyweightCiudad.colorTexto;
				label.innerComponent = ciudad.flyweightCiudad.spriteFont;
				addComponent(label);
			
				edificiosView = ciudad.crearVistaEdificios();
				edificiosView.onElementSelect = Controller.verEdificio;
				edificiosView.updateContent();
				addComponent(edificiosView);
			}
		}
	}
}