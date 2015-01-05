using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class ListaMisionObjetivoView : Programa.ListaView
	{
		// variables
		public List<Mision.Objetivo> objetivos { get; set; }


		// constructor
		public ListaMisionObjetivoView(List<Mision.Objetivo> newObjetivos, Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if(newObjetivos == null)
				throw new ArgumentNullException();
			objetivos = newObjetivos;
			maxRowsPerPage = 4;
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			
			foreach(Mision.Objetivo objetivo in objetivos)
			{
				bool seleccionado = false;
				listaElementos.Add(objetivo.crearVista(seleccionado, true));
			}
			
			actualizarTitulo();
			base.updateContent();
		}


		public void actualizarTitulo()
		{
			titulo.clearComponents();

			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Objetivos:";
			label.color = Gestores.Mundo.Instancia.colores["menuColor"];
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			titulo.addComponent(label);
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
		}
	}
}




