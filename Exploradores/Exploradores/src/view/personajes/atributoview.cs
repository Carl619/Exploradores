using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class AtributoView : Programa.ListaView.Elemento
	{
		// variables
		public Atributo atributo { get; protected set; }
		public uint espacio { get; set; }
		public uint espacioContenedor { get; set; }


		// constructor
		public AtributoView(Atributo newAtributo, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newAtributo == null)
				throw new ArgumentNullException();
			atributo = newAtributo;
			espacio = 512;
			espacioContenedor = 128;
			sizeSettings.minInnerWidth = espacio;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border3"].clone();

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
			sprite.innerComponent = atributo.icono.textura;
			addComponent(sprite);

			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			contenedor.sizeSettings.minInnerWidth = espacioContenedor;
			addComponent(contenedor);

			label = new ILSXNA.Label();
			label.message = atributo.nombre;
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Valor: " + atributo.valor.ToString() + " / " + atributo.valorMax.ToString();
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			addComponent(atributo.getVistaEspecifica());
		}
	}
}




