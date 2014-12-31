using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class ListaAtributoView : Programa.ListaView
	{
		// variables
		public Dictionary<String, Atributo> atributos { get; set; }


		// constructor
		public ListaAtributoView(Dictionary<String, Atributo> newAtributos, Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if(newAtributos == null)
				throw new ArgumentNullException();
			atributos = newAtributos;
			maxRowsPerPage = 2;

			onElementSelect = Programa.Controller.funcionVerAtributo;
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			
			foreach(KeyValuePair<String, Atributo> atributo in atributos)
			{
				bool seleccionado = false;
				listaElementos.Add(atributo.Value.crearVista(seleccionado, true));
			}
			
			actualizarTitulo();
			base.updateContent();
		}


		public void actualizarTitulo()
		{
			titulo.clearComponents();

			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Atributos:";
			label.color = Gestores.Mundo.Instancia.colores["menuColor"];
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			label.minLateralSpacing = 16;
			titulo.addComponent(label);
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
		}
	}
}




