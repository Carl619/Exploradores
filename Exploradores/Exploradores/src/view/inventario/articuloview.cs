using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Objetos
{
	

	public class ArticuloView : Programa.ListaView.Elemento
	{
		// variables
		public ColeccionArticulos coleccion { get; protected set; }
		public ArticuloFlyweight flyweight { get; protected set; }
		public uint espacioIcono { get; set; }
		public uint espacioTitulo { get; set; }
		public uint espacioCampos { get; set; }


		// constructor
		public ArticuloView(ColeccionArticulos newColeccion, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(newColeccion == null)
				throw new ArgumentNullException();
			
			coleccion = newColeccion;
			flyweight = coleccion.articulo.flyweight;
			espacioIcono = 25;
			espacioTitulo = 200;
			espacioCampos = 75;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border3"].clone();

			if(actualizarVista == true)
				updateContent();
		}


		public ArticuloView(ArticuloFlyweight articuloFlyweight, bool seleccionar, bool actualizarVista = true)
			: base(seleccionar)
		{
			if(articuloFlyweight == null)
				throw new ArgumentNullException();
			
			coleccion = null;
			flyweight = articuloFlyweight;
			espacioIcono = 25;
			espacioTitulo = 200;
			espacioCampos = 75;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border3"].clone();
			onMouseOver = onMouseOut = null;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			
			ILSXNA.Sprite sprite;
			ILSXNA.Label label;

			if(coleccion != null)
			{
				sprite = new ILSXNA.Sprite();
				sprite.innerComponent = flyweight.icono;
				if(sprite.innerComponent == null)
					sprite.innerComponent = Gestores.Mundo.Instancia.imagenes["iconoAusenteArticulo"].textura;
				sprite.sizeSettings.fixedInnerWidth = espacioIcono;
				addComponent(sprite);
			}
			else
			{
				label = new ILSXNA.Label();
				label.message = " ";
				label.color = flyweight.colorNombre;
				label.innerComponent = font;
				label.sizeSettings.fixedInnerWidth = espacioIcono;
				addComponent(label);
			}

			label = new ILSXNA.Label();
			label.message = coleccion != null ? (String)coleccion.articulo.nombre.Clone() : "Nombre articulo";
			label.color = flyweight.colorNombre;
			label.innerComponent = font;
			label.sizeSettings.fixedInnerWidth = espacioTitulo;
			addComponent(label);

			label = new ILSXNA.Label();
			label.message = coleccion != null ? coleccion.cantidad.ToString("D") : "#";
			label.color = flyweight.colorCantidad;
			label.innerComponent = font;
			label.sizeSettings.fixedInnerWidth = espacioCampos;
			addComponent(label);

			label = new ILSXNA.Label();
			label.message = coleccion != null ? coleccion.valor.ToString("D") : "Valor T.";
			label.color = flyweight.colorValorTotal;
			label.innerComponent = font;
			label.sizeSettings.fixedInnerWidth = espacioCampos;
			addComponent(label);

			label = new ILSXNA.Label();
			label.message = coleccion != null ? coleccion.peso.ToString("D") : "Peso T.";
			label.color = flyweight.colorPesoTotal;
			label.innerComponent = font;
			label.sizeSettings.fixedInnerWidth = espacioCampos;
			addComponent(label);

			label = new ILSXNA.Label();
			label.message = coleccion != null ? coleccion.articulo.valor.ToString("D") : "Valor";
			label.color = flyweight.colorValorUnitario;
			label.innerComponent = font;
			label.sizeSettings.fixedInnerWidth = espacioCampos;
			addComponent(label);

			label = new ILSXNA.Label();
			label.message = coleccion != null ? coleccion.articulo.peso.ToString("D") : "Peso";
			label.color = flyweight.colorPesoUnitario;
			label.innerComponent = font;
			label.sizeSettings.fixedInnerWidth = espacioCampos;
			addComponent(label);
		}
	}
}




