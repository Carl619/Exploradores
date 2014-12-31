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
		public Programa.PanelLateralView panelLateral { get; protected set; }
		public Ciudad ciudad { get; protected set; }
		public ListaEdificiosView edificiosView { get; protected set; }
		public String edificioSeleccionado { get; protected set; }


		// constructor
		public CiudadViewLateral(Ciudad newCiudad, Programa.PanelLateralView newPanelLateral)
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
			boton.onButtonPress = Controller.salirCiudad;
			addComponent(boton);

			boton = new ILSXNA.Button("Informacion", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.callbackConfigObj =
				new Tuple<Programa.PanelLateralView, LugarVisitable>
				(panelLateral, ciudad);
			boton.onButtonPress = Controller.verInformacionLugar;
			addComponent(boton);

			edificiosView = ciudad.crearVistaEdificios();
			edificiosView.updateContent();
			addComponent(edificiosView);

			/*
			CiudadFlyweight ciudadFlyweight = vistaCiudad.ciudad.flyweightCiudad;
			SpriteFont font = ciudadFlyweight.spriteFont;
			if (font == null)
				font = Programa.ListaViewFlyweight.spriteFontAusente;

			if(edificioSeleccionado != null)
			{
				ILSXNA.Container panelNPCs = new ILSXNA.Container(Gestores.Mundo.Instancia.borders[0]);
				addComponent(panelNPCs);
				
				Personajes.ListaNPCView listaNPCs = new Personajes.ListaNPCView(
						vistaCiudad.ciudad.listaNPC,
						edificioSeleccionado,
						vistaCiudad.ciudad.flyweightLista);
				listaNPCs.updateContent();
				panelNPCs.addComponent(listaNPCs);
			}*/
		}
	}
}