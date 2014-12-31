using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class PanelHabilidad : InterfazGrafica
	{
		// variables
		public uint anchoVista { get; set; }


		// constructor
		public PanelHabilidad(bool actualizarVista = true)
			: base()
		{
			if(actualizarVista == true)
				updateContent();
			
			anchoVista = 480;
			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.equalCellHeight = false;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();

			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.habilidadSeleciconada == null)
				return;
			
			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			Color colorCabecera = Gestores.Mundo.Instancia.colores["menuColor"];
			Habilidad habilidad = Gestores.Partidas.Instancia.habilidadSeleciconada;

			ILSXNA.Label label;
			ILSXNA.Sprite sprite;

			sprite = new ILSXNA.Sprite();
			sprite.innerComponent = habilidad.icono.textura;
			addComponent(sprite);

			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			addComponent(contenedor);

			label = new ILSXNA.Label();
			label.message = " ";
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = habilidad.nombre;
			label.color = colorCabecera;
			label.innerComponent = font;
			contenedor.addComponent(label);
			
			label = new ILSXNA.Label();
			label.message = "Nivel: " + habilidad.nivel.ToString();
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			contenedor.addComponent(habilidad.getVistaEspecificaCompleta());

			label = new ILSXNA.Label();
			label.message = " ";
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Descripcion:";
			label.color = colorCabecera;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = habilidad.descripcion;
			label.color = color;
			label.innerComponent = font;
			ILSXNA.Paragraph par = new ILSXNA.Paragraph(label, anchoVista);
			contenedor.addComponent(par);

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Programa.Controller.funcionCerrarHabilidad;
			addComponent(boton);
		}
	}
}



