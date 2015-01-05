using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class ListaHabilidadView : Programa.ListaView
	{
		// variables
		public Dictionary<String, Habilidad> habilidades { get; set; }


		// constructor
		public ListaHabilidadView(Dictionary<String, Habilidad> newHabilidades, Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if(newHabilidades == null)
				throw new ArgumentNullException();
			habilidades = newHabilidades;
			maxRowsPerPage = 3;
			
			onElementSelect = Programa.Controller.funcionVerHabilidad;
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			
			foreach(KeyValuePair<String, Habilidad> habilidad in habilidades)
			{
				bool seleccionado = false;
				listaElementos.Add(habilidad.Value.crearVista(seleccionado, true));
			}
			
			actualizarTitulo();
			base.updateContent();
		}


		public void actualizarTitulo()
		{
			titulo.clearComponents();

			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Habilidades:";
			label.color = Gestores.Mundo.Instancia.colores["menuColor"];
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			label.minLateralSpacing = 49;
			titulo.addComponent(label);
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
		}
	}
}




