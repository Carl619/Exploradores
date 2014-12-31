using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class HabilidadView : Programa.ListaView.Elemento
	{
		// variables
		public Habilidad habilidad { get; protected set; }
		public uint espacio { get; set; }
		public uint espacioContenedor { get; set; }


		// constructor
		public HabilidadView(Habilidad newHabilidad, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newHabilidad == null)
				throw new ArgumentNullException();
			habilidad = newHabilidad;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border3"].clone();
			if(habilidad == null)
				onMouseOver = onMouseOut = null;
			espacio = 512;
			espacioContenedor = 128;
			sizeSettings.minInnerWidth = espacio;
			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			
			ILSXNA.Label label;
			ILSXNA.Sprite sprite;

			sprite = new ILSXNA.Sprite();
			sprite.innerComponent = habilidad.icono.textura;
			addComponent(sprite);

			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			contenedor.sizeSettings.minInnerWidth = espacioContenedor;
			addComponent(contenedor);

			label = new ILSXNA.Label();
			label.message = habilidad.nombre;
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Nivel: " + habilidad.nivel.ToString();
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			addComponent(habilidad.getVistaEspecifica());
		}
	}
}




