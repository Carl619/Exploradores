using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class MisionObjetivoView : Programa.ListaView.Elemento
	{
		// variables
		public Mision.Objetivo objetivo { get; protected set; }
		public uint espacio { get; set; }


		// constructor
		public MisionObjetivoView(Mision.Objetivo newObjetivo, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newObjetivo == null)
				throw new ArgumentNullException();
			
			objetivo = newObjetivo;

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
			Texture2D texture;
			if(objetivo.estado == Mision.Estado.Terminado)
				texture = Gestores.Mundo.Instancia.imagenes["ok"].textura;
			else if(objetivo.estado == Mision.Estado.Fracasado)
				texture = Gestores.Mundo.Instancia.imagenes["cancel"].textura;
			else if(objetivo.estado == Mision.Estado.EnProgreso)
				texture = Gestores.Mundo.Instancia.imagenes["reloj"].textura;
			else
				texture = Gestores.Mundo.Instancia.imagenes["vacio"].textura;
			
			ILSXNA.Sprite sprite;
			ILSXNA.Label label;

			sprite = new ILSXNA.Sprite();
			sprite.innerComponent = texture;
			addComponent(sprite);

			label = new ILSXNA.Label();
			label.message = objetivo.descripcion;
			label.color = color;
			label.innerComponent = font;
			addComponent(label);
		}
	}
}




