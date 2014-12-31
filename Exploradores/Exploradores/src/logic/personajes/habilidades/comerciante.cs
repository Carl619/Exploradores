using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Personajes
{
	
	
	public class Comerciante : Habilidad
	{
		// variables
		protected double _tasaBaseCompras { get; set; }
		protected double _tasaBaseVentas { get; set; }
		public double tasaBaseCompras { get { return _tasaBaseCompras; } set { _tasaBaseCompras = value; calcularTasas(); } }
		public double tasaBaseVentas { get { return _tasaBaseVentas; } set { _tasaBaseVentas = value; calcularTasas(); } }
		public double tasaCompras { get; protected set; }
		public double tasaVentas { get; protected set; }


		// constructor
		public Comerciante(String newID, String newNombre, uint newNivel = 1)
			: base(newID, newNombre, newNivel)
		{
			_tasaBaseCompras = 1.0f;
			_tasaBaseVentas = 1.0f;
			calcularTasas();
		}


		//funciones
		public override Habilidad clone(List<String> campos)
		{
			Comerciante comerciante = new Comerciante(id, nombre, Convert.ToUInt32(campos[0]));
			if(campos.Count > 2)
			{
				comerciante.tasaBaseCompras = Gestores.Mundo.parseFloat(campos[1]);
				comerciante.tasaBaseVentas = Gestores.Mundo.parseFloat(campos[2]);
			}
			else
			{
				comerciante.tasaBaseCompras = tasaBaseCompras;
				comerciante.tasaBaseVentas = tasaBaseVentas;
			}

			comerciante.descripcion = descripcion;
			comerciante.icono = icono;
			return comerciante;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Tasa de compras: " + tasaCompras.ToString("F4");
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Tasa de ventas: " + tasaVentas.ToString("F4");
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			return contenedor;
		}


		public override ILSXNA.Container getVistaEspecificaCompleta()
		{
			ILSXNA.Container contenedor = getVistaEspecifica();

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Tasa de compras base: " + tasaBaseCompras.ToString("F4");
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Tasa de ventas base: " + tasaBaseVentas.ToString("F4");
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			return contenedor;
		}


		public override void incrementarNivel(uint numero)
		{
			base.incrementarNivel(numero);
			calcularTasas();
		}


		protected void calcularTasas()
		{
			tasaCompras = 1.0f;
			for(int i=0; i<nivel; ++i)
				tasaCompras *= tasaBaseCompras;
			tasaVentas = 1.0f;
			for(int i=0; i<nivel; ++i)
				tasaVentas *= tasaBaseVentas;
		}
	}
}




