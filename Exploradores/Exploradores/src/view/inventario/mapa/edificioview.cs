using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class EdificioView : Programa.ListaView.Elemento
	{
		// variables
		public Personajes.NPCFlyweight edificio { get; set; }


		// constructor
		public EdificioView(Personajes.NPCFlyweight flyweight, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if (flyweight == null)
				throw new ArgumentNullException();
			
			edificio = flyweight;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border2"].clone();

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			SpriteFont font = edificio.spriteFont;
			if(font == null)
				font = Personajes.NPCFlyweight.spriteFontAusente;
			
			ILSXNA.Sprite sprite;
			ILSXNA.Label label;

			sprite = new ILSXNA.Sprite();
			sprite.innerComponent = edificio.iconoProfesion;
			if(sprite.innerComponent == null)
				sprite.innerComponent = Personajes.NPCFlyweight.iconoAusente;
			addComponent(sprite);

			label = new ILSXNA.Label();
			label.message = (String)edificio.edificioEncuentro.ToString().Clone();
			label.color = edificio.colorEdificioEncuentro;
			label.innerComponent = font;
			addComponent(label);
		}
	}
}




