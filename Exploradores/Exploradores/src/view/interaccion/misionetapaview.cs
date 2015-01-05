using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class MisionEtapaView : Programa.ListaView.Elemento
	{
		// variables
		public Mision.Etapa etapa { get; protected set; }
		public uint anchoVista { get; set; }


		// constructor
		public MisionEtapaView(Mision.Etapa newEtapa, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newEtapa == null)
				throw new ArgumentNullException();
			
			etapa = newEtapa;
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
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = etapa.titulo;
			label.color = colorCabecera;
			label.innerComponent = font;
			addComponent(label);

			label = new ILSXNA.Label();
			label.message = " ";
			label.color = colorCabecera;
			label.innerComponent = font;
			addComponent(label);

			foreach(String descripcion in etapa.descripcion)
			{
				label = new ILSXNA.Label();
				label.message = descripcion;
				label.color = color;
				label.innerComponent = font;
				ILSXNA.Paragraph par = new ILSXNA.Paragraph(label, anchoVista);
				addComponent(par);
			}

			ListaMisionObjetivoView objetivos;
			objetivos = new ListaMisionObjetivoView(etapa.objetivos, flyweight);
			objetivos.updateContent();
			addComponent(objetivos);
		}
	}
}




