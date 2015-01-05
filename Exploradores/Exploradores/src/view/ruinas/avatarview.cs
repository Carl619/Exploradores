using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	

	public class AvatarView : InterfazGrafica
	{
		// variables
		public PersonajeRuina personaje { get; protected set; }


		// constructor
		public AvatarView(PersonajeRuina newPersonaje, bool actualizarVista = true)
			: base()
		{
			if(newPersonaje == null)
				throw new ArgumentNullException();
			
			personaje = newPersonaje;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();
			onLeftMousePress = Controller.selectPersonajeHUD;


			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			
			ILSXNA.Sprite avatar;

			Ruinas.PersonajeRuina seleciconado;
			if(Gestores.Partidas.Instancia.gestorRuinas.ruinaActual.personajesSeleccionados.TryGetValue(
				personaje.personaje.id, out seleciconado) == true)
			{
				avatar = new ILSXNA.Sprite();
				avatar.innerComponent = personaje.personaje.avatarSeleccionado.textura;
				addComponent(avatar);
			}
			else
			{
				avatar = new ILSXNA.Sprite();
				avatar.innerComponent = personaje.personaje.avatar.textura;
				addComponent(avatar);
			}

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["menuColor"];
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = personaje.personaje.atributos["idVida"].valor.ToString();
			label.color = color;
			label.innerComponent = font;
			addComponent(label);
		}
	}
}




