using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class PersonajeView : Programa.ListaView.Elemento
	{
		// variables
		public Personaje personaje { get; protected set; }


		// constructor
		public PersonajeView(Personaje newPersonaje, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newPersonaje == null)
				throw new ArgumentNullException();
			
			personaje = newPersonaje;
			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			sizeSettings.minInnerWidth = 512;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border4"].clone();

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			
			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["menuColor"];
			Programa.ListaViewFlyweight flyweight = Gestores.Mundo.Instancia.listaViewFlyweights["list1"];

			
			ILSXNA.Container contenedorCabecera = new ILSXNA.Container();
			addComponent(contenedorCabecera);

			
			ILSXNA.Container contenedorAvatar =
				new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border2"]);
			contenedorCabecera.addComponent(contenedorAvatar);
			ILSXNA.Sprite avatar;

			avatar = new ILSXNA.Sprite();
			avatar.innerComponent = personaje.avatarSeleccionado.textura;
			contenedorAvatar.addComponent(avatar);
			
			ILSXNA.Label label;
			String nombre = personaje.nombre;
			if(personaje == Programa.Jugador.Instancia.protagonista)
				nombre = nombre + " (Protagonista)";
			else
				nombre = nombre + " (Acompanante)";

			label = new ILSXNA.Label();
			label.message = nombre;
			label.color = color;
			label.innerComponent = font;
			label.minLateralSpacing = 4;
			contenedorCabecera.addComponent(label);

			if(personaje.atributos.Count > 0)
			{
				Dictionary<String, Personajes.Atributo> atributos = personaje.atributos;

				Personajes.ListaAtributoView atributosView =
					new Personajes.ListaAtributoView(atributos, flyweight);
				atributosView.updateContent();
				addComponent(atributosView);
			}

			if(personaje.habilidades.Count > 0)
			{
				Dictionary<String, Personajes.Habilidad> habilidades = personaje.habilidades;

				Personajes.ListaHabilidadView habilidadesView =
					new Personajes.ListaHabilidadView(habilidades, flyweight);
				habilidadesView.updateContent();
				addComponent(habilidadesView);
			}
		}
	}
}




