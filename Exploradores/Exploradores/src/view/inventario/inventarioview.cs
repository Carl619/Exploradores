using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Objetos
{
	

	public class InventarioView : Programa.ListaView
	{
		// variables
		public Inventario inventario { get; set; }
		public String textoTitulo { get; set; }
		public bool mostrarEspacioMax { get; set; }


		// constructor
		public InventarioView(Inventario newInventario)
			: base(null, null, null, false)
		{
			if(newInventario == null)
				throw new ArgumentNullException();
			
			inventario = newInventario;
			textoTitulo = "";
			flyweight = inventario.flyweight;
			mostrarFlechasVacio = true;
			mostrarEspacioMax = true;

			if(inventario.articulosSeleccionados != null)
			{
				guardarReferenciaSeleccion(null);
			}
		}


		// funciones
		public override void updateContent()
		{
			actualizarTitulo();

			listaElementos = new List<Elemento>();
			
			cabecera = new ArticuloView(Gestores.Mundo.Instancia.articuloFlyweights["item1"], false);

			foreach(KeyValuePair<String, ColeccionArticulos> coleccionArticulos in inventario.articulos)
			{
				bool s = (coleccionArticulos.Value == inventario.articulosSeleccionados);
				ArticuloView vista = coleccionArticulos.Value.crearVista(s, true);
				listaElementos.Add(vista);
				if(inventario.articulosSeleccionados != null)
				{
					if(inventario.articulosSeleccionados.articulo.id == coleccionArticulos.Key)
					{
						vista.select();
						ArticuloView.mouseActivate(vista, null, null);
					}
				}
			}

			base.updateContent();
		}


		public override void guardarReferenciaSeleccion(Elemento elemento)
		{
			if(inventario.articulosSeleccionados != null)
			{
				if(inventario.articulosSeleccionados.vista != null)
				{
					inventario.articulosSeleccionados.vista.deselect();
					ArticuloView.mouseDeactivate(inventario.articulosSeleccionados.vista, null, null);
					inventario.articulosSeleccionados.vista.updateContent();
				}
			}
			if(elemento != null)
				inventario.articulosSeleccionados = ((ArticuloView)elemento).coleccion;
			else
				inventario.articulosSeleccionados = null;
		}


		public void actualizarTitulo()
		{
			titulo.clearComponents();

			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = textoTitulo;
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = inventario.flyweight.spriteFont;
			label.minLateralSpacing = 16;
			titulo.addComponent(label);

			label = new ILSXNA.Label();
			label.message = " ";
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = inventario.flyweight.spriteFont;
			label.minLateralSpacing = 8;
			titulo.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Valor: ";
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = inventario.flyweight.spriteFont;
			label.minLateralSpacing = 8;
			titulo.addComponent(label);

			label = new ILSXNA.Label();
			label.message = inventario.valor.ToString("D");
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = inventario.flyweight.spriteFont;
			titulo.addComponent(label);

			label = new ILSXNA.Label();
			label.message = " ";
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = inventario.flyweight.spriteFont;
			label.minLateralSpacing = 12;
			titulo.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Peso: ";
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = inventario.flyweight.spriteFont;
			label.minLateralSpacing = 8;
			titulo.addComponent(label);

			label = new ILSXNA.Label();
			if(mostrarEspacioMax == true)
				label.message = inventario.espacio.ToString("D") + " / " + inventario.espacioMax.ToString("D");
			else
				label.message = inventario.espacio.ToString("D");
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = inventario.flyweight.spriteFont;
			titulo.addComponent(label);
		}
	}
}




