using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class ListaMisionView : Programa.ListaView
	{
		// variables
		public List<Mision> misiones { get; set; }
		public String cadenaTitulo { get; set; }


		// constructor
		public ListaMisionView(List<Mision> newMisiones, Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if(newMisiones == null)
				throw new ArgumentNullException();
			misiones = newMisiones;
			cadenaTitulo = "Misiones:";
			maxRowsPerPage = 1;

			contentSpacingX = 4;
			contentSpacingY = 4;
			sizeSettings.minInnerWidth = 580;
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			
			foreach(Mision mision in misiones)
			{
				bool seleccionado = false;
				listaElementos.Add(mision.crearVista(seleccionado, false));
			}
			
			actualizarTitulo();
			base.updateContent();
		}


		public override void requestUpdateContent()
		{
			base.requestUpdateContent();
		}


		public void actualizarTitulo()
		{
			titulo.clearComponents();

			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = cadenaTitulo;
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




