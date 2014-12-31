using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class ListaEdificiosView : Programa.ListaView
	{
		// variables
		public List<Personajes.NPCFlyweight> edificios { get; set; }


		// constructor
		public ListaEdificiosView(Ciudad ciudad,
									Programa.ListaViewFlyweight newFlyweight)
			: base(null, null, newFlyweight, false)
		{
			if (ciudad == null)
				throw new ArgumentNullException();
			
			edificios = ciudad.generarListaEdificios();
		}


		// funciones
		public override void updateContent()
		{
			listaElementos = new List<Elemento>();
			foreach (Personajes.NPCFlyweight edificio in edificios)
			{
				bool s = (edificio == Gestores.Partidas.Instancia.edificioSeleccionado);
				listaElementos.Add(edificio.crearVistaProfesion(s, true));
			}

			base.updateContent();
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
			Personajes.NPCFlyweight edificioSeleccionado =
				Gestores.Partidas.Instancia.edificioSeleccionado;
			if(edificioSeleccionado != null)
			{
				/*if(edificioSeleccionado.vista != null)
				{
					inventario.articulosSeleccionados.vista.deselect();
					ArticuloView.mouseDeactivate(inventario.articulosSeleccionados.vista, null, null);
					inventario.articulosSeleccionados.vista.updateContent();
				}*/
			}
			if(elemento != null)
				Gestores.Partidas.Instancia.edificioSeleccionado =
					((EdificioView)elemento).edificio;
			else
				Gestores.Partidas.Instancia.edificioSeleccionado = null;
		}
	}
}




