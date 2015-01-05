using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;




namespace Programa
{
	
	
	public class ContenedorFin : InterfazGrafica
	{
		// variables
		public uint anchoVista { get; set; }


		// constructor
		public ContenedorFin(bool actualizarVista = true)
			: base()
		{
			anchoVista = 640;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents(true);
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego != Gestores.Pantallas.EstadoJuego.Exito &&
				Gestores.Partidas.Instancia.gestorPantallas.estadoJuego != Gestores.Pantallas.EstadoJuego.Fracaso)
				return;
			
			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			Color colorCabecera = Gestores.Mundo.Instancia.colores["menuColor"];

			ILSXNA.Label label;
			ILSXNA.Paragraph par;

			label = new ILSXNA.Label();
			label.message = "Has perdido";
			label.color = colorCabecera;
			label.innerComponent = font;
			par = new ILSXNA.Paragraph(label, anchoVista);
			addComponent(par);

			List<String> mensajesFinales;
			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego == Gestores.Pantallas.EstadoJuego.Exito)
				mensajesFinales = Gestores.Partidas.Instancia.mensajesExitoJuego;
			else
				mensajesFinales = Gestores.Partidas.Instancia.mensajesFracasoJuego;
			foreach(String mensaje in mensajesFinales)
			{
				label = new ILSXNA.Label();
				label.message = mensaje;
				label.color = color;
				label.innerComponent = font;
				par = new ILSXNA.Paragraph(label, anchoVista);
				addComponent(par);
			}

			ILSXNA.Container contenedor = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border1"]);
			addComponent(contenedor);
			ILSXNA.Sprite sprite;

			sprite = new ILSXNA.Sprite();
			sprite.innerComponent = Gestores.Mundo.Instancia.imagenes["fracaso"].textura;
			contenedor.addComponent(sprite);
		}
	}
}




