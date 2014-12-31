using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class VentanaDialogo : InterfazGrafica
	{
		// variables
		private static VentanaDialogo instancia = null;
		public static VentanaDialogo Instancia
		{
			get
			{
				if(instancia == null)
					new VentanaDialogo();
				return instancia;
			}
		}
		protected Dialogo dialogo { get; set; }
		public SpriteFont fuenteTitulo { get; set; }
		public SpriteFont fuenteCuerpo { get; set; }
		public SpriteFont fuenteOpcion { get; set; }
		public uint minIndexWidth { get; set; }
		public uint minOptionHeight { get; set; }


		// constructor
		private VentanaDialogo()
			: base()
		{
			instancia = this;
			dialogo = null;
			fuenteTitulo = null;
			fuenteCuerpo = null;
			fuenteOpcion = null;

			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			sizeSettings.minInnerWidth = 320;
			contentSpacingX = 4;
			contentSpacingY = 4;

			minIndexWidth = 24;
			minOptionHeight = 24;
		}


		// funciones
		public void comenzarDialogo(Personajes.NPC npc)
		{
			dialogo = new Interaccion.Dialogo(npc.menuDialogo, npc.cadenasRegexDialogo);
			requestUpdateContent();
		}


		public void cerrarDialogo()
		{
			dialogo = null;
			requestUpdateContent();
		}


		public void activarOpcion(Int32 indiceElementoDialogo)
		{
			if(dialogo == null)
				return;
			dialogo.elegirOpcion(indiceElementoDialogo);
			requestUpdateContent();
		}


		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;
			
			if(border == null)
				border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();
			if(dialogo == null)
				return;
			if(dialogo.menu == null)
				return;
			
			SpriteFont fuente1, fuente2, fuente3;
			fuente1 = fuenteTitulo;
			if(fuente1 == null)
				fuente1 = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			fuente2 = fuenteCuerpo;
			if(fuente2 == null)
				fuente2 = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			fuente3 = fuenteOpcion;
			if(fuente3 == null)
				fuente3 = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			
			ILSXNA.Label labelCabecera = new ILSXNA.Label();
			String npc = Gestores.Partidas.Instancia.npcSeleccionado;
			labelCabecera.message = "Dialogo con " + Gestores.Partidas.Instancia.npcs[npc].nombre;
			labelCabecera.innerComponent = fuente1;
			labelCabecera.color = Gestores.Mundo.Instancia.colores["headerColor"];

			ILSXNA.Paragraph cabecera = new ILSXNA.Paragraph(labelCabecera);
			addComponent(cabecera);
			
			ILSXNA.Label texto = new ILSXNA.Label();
			texto.innerComponent = fuente2;
			texto.color = Gestores.Mundo.Instancia.colores["genericColor"];
			texto.message = dialogo.menu.texto;

			//ILSXNA.Paragraph cuerpoDialogo = new ILSXNA.Paragraph(texto);
			addComponent(texto);
			
			if (dialogo.menu.GetType() == typeof(MenuDialogo))
			{
				int indiceMenu = 1;
				ILSXNA.Container entradasMenu = new ILSXNA.Container();
				addComponent(entradasMenu);
				entradasMenu.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

				foreach(ElementoDialogo elem in ((MenuDialogo)dialogo.menu).dialogo)
				{
					ILSXNA.Label labelIndice = new ILSXNA.Label();
					labelIndice.innerComponent = fuente3;
					labelIndice.color = Gestores.Mundo.Instancia.colores["menuColor"];
					labelIndice.message = indiceMenu + ".";
					//ILSXNA.Paragraph opcionMenuIndice = new ILSXNA.Paragraph(labelIndice);

					ILSXNA.Label titulo = (ILSXNA.Label)labelIndice.clone();
					titulo.message = " " + elem.titulo;
					//ILSXNA.Paragraph opcionMenuTexto = new ILSXNA.Paragraph(titulo);
					
					ILSXNA.Container opcionMenu = new ILSXNA.Container();
					opcionMenu.sizeSettings.minInnerHeight = minOptionHeight;
					opcionMenu.layout.verticalAlignment = ILS.Layout.Alignment.RightOrLowerAlignment;
					Int32 i = new Int32();
					i = (Int32)indiceMenu - 1;
					opcionMenu.callbackConfigObj = new Tuple<Int32, VentanaDialogo>(i, this);
					opcionMenu.onMousePress = Mapa.Controller.elegirOpcionDialogo;
					
					ILSXNA.Container opcionMenuIC = new ILSXNA.Container();
					opcionMenuIC.sizeSettings.minInnerWidth = minIndexWidth;

					opcionMenuIC.addComponent(labelIndice);
					opcionMenu.addComponent(opcionMenuIC);
					opcionMenu.addComponent(titulo);
					entradasMenu.addComponent(opcionMenu);
					++indiceMenu;
				}
			}

			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Mapa.Controller.cerrarDialogoNPC;
			addComponent(boton);
		}
	}
}




