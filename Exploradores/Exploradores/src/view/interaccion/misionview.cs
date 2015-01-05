using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class MisionView : Programa.ListaView.Elemento
	{
		// variables
		public Mision mision { get; protected set; }
		public uint anchoVista { get; set; }


		// constructor
		public MisionView(Mision newMision, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newMision == null)
				throw new ArgumentNullException();
			
			mision = newMision;
			anchoVista = 580;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

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
			Color colorCabecera = Gestores.Mundo.Instancia.colores["menuColor"];
			Programa.ListaViewFlyweight flyweight = Gestores.Mundo.Instancia.listaViewFlyweights["list1"];
			
			ILSXNA.Container contenedor = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border2"]);
			contenedor.contentSpacingX = 4;
			contenedor.contentSpacingY = 4;
			contenedor.sizeSettings.minInnerWidth = anchoVista;
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			addComponent(contenedor);


			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = mision.titulo;
			label.color = colorCabecera;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = " ";
			label.color = colorCabecera;
			label.innerComponent = font;
			contenedor.addComponent(label);

			int i = 0;
			foreach(String descripcion in mision.descripcion)
			{
				label = new ILSXNA.Label();
				label.message = descripcion;
				label.color = color;
				label.innerComponent = font;
				ILSXNA.Paragraph par = new ILSXNA.Paragraph(label, anchoVista);
				if(i + 1 == mision.descripcion.Count)
				{
					par.newLineAfterParagraph = false;
					par.setText(label, anchoVista);
				}
				contenedor.addComponent(par);
				++i;
			}

			if(mision.etapas.Count > 0)
			{
				ListaMisionEtapaView etapas;
				etapas = new ListaMisionEtapaView(mision.etapas, flyweight);
				etapas.updateContent();
				while(etapas.paginaActual < mision.indiceEtapaActual)
					etapas.navegarPaginaSiguiente();
				while(etapas.paginaActual > mision.indiceEtapaActual)
					etapas.navegarPaginaAnterior();
				//etapas.updateContent();
				addComponent(etapas);
			}
		}
	}
}




