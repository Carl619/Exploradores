using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Personajes
{
	

	public class ListaPersonajeView : Programa.ListaView
	{
		// variables
		public List<Personaje> personajes { get; set; }


		// constructor
		public ListaPersonajeView(List<Personaje> newPersonajes, Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if(newPersonajes == null)
				throw new ArgumentNullException();
			
			personajes = newPersonajes;
			maxRowsPerPage = 1;
			mostrarFlechasVacio = true;
			contenedorElementos.layout.equalCellHeight = false;
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			foreach(Personaje personaje in personajes)
			{
				bool seleccionado = false;
				listaElementos.Add(personaje.crearVistaPersonaje(seleccionado, true));
			}

			actualizarTitulo();
			base.updateContent();
		}


		public void actualizarTitulo()
		{
			titulo.clearComponents();

			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Los personajes de tu grupo:";
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			label.minLateralSpacing = 16;
			titulo.addComponent(label);
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
		}
	}
}




