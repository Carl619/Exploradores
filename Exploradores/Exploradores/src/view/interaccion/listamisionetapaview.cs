using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class ListaMisionEtapaView : Programa.ListaView
	{
		// variables
		public List<Mision.Etapa> etapas { get; set; }


		// constructor
		public ListaMisionEtapaView(List<Mision.Etapa> newEtapas, Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if(newEtapas == null)
				throw new ArgumentNullException();
			etapas = newEtapas;
			maxRowsPerPage = 1;
			mostrarFlechasVacio = true;

			contentSpacingX = 4;
			contentSpacingY = 4;
			sizeSettings.minInnerWidth = 580;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border2"].clone();
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			
			foreach(Mision.Etapa etapa in etapas)
			{
				if(etapa.estado != Mision.Estado.Inactivo)
					listaElementos.Add(etapa.crearVista(false, false));
			}
			
			actualizarTitulo();
			base.updateContent();
		}


		public void actualizarTitulo()
		{
			titulo.clearComponents();

			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Etapa " + (etapas[0].mision.indiceEtapaActual + 1).ToString();
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			label.minLateralSpacing = 16;
			titulo.addComponent(label);
		}


		public override void seleccionarElemento(Elemento elemento, bool doble)
		{
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
		}
	}
}




