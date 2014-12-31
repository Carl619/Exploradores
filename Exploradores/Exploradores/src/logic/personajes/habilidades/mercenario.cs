using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Personajes
{
	
	
	public class Mercenario : Habilidad
	{
		// variables
		protected uint _precioBaseReclutamiento { get; set; }
		protected uint _defensaBase { get; set; }
		public uint precioBaseReclutamiento
		{
			get { return _precioBaseReclutamiento; }
			set { _precioBaseReclutamiento = value; calcularPrecio(); }
		}
		public uint defensaBase
		{
			get { return _defensaBase; }
			set { _defensaBase = value; calcularDefensa(); }
		}
		public uint precioReclutamiento { get; protected set; }
		public uint defensa { get; protected set; }


		// constructor
		public Mercenario(String newID, String newNombre, uint newNivel = 1)
			: base(newID, newNombre, newNivel)
		{
			_precioBaseReclutamiento = 0;
			_defensaBase = 0;
			calcularPrecio();
			calcularDefensa();
		}


		//funciones
		public override Habilidad clone(List<String> campos)
		{
			Mercenario mercenario = new Mercenario(id, nombre, Convert.ToUInt32(campos[0]));

			if(campos.Count > 2)
			{
				mercenario.precioBaseReclutamiento = Convert.ToUInt32(campos[1]);
				mercenario.defensaBase = Convert.ToUInt32(campos[2]);
			}
			else
			{
				mercenario.precioBaseReclutamiento = precioBaseReclutamiento;
				mercenario.defensaBase = defensaBase;
			}
			
			mercenario.descripcion = descripcion;
			mercenario.cadenasRegexDialogo.Add("precio", mercenario.precioReclutamiento.ToString());
			mercenario.icono = icono;
			return mercenario;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Precio de reclutamiento: " + precioReclutamiento.ToString();
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Defensa: " + defensa.ToString();
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
			label.message = "Precio de reclutamiento base: " + precioBaseReclutamiento.ToString();
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			label = new ILSXNA.Label();
			label.message = "Defensa base: " + defensaBase.ToString();
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			return contenedor;
		}


		public override void incrementarNivel(uint numero)
		{
			base.incrementarNivel(numero);
			calcularPrecio();
		}


		protected void calcularPrecio()
		{
			precioReclutamiento = 0;
			for(int i=0; i<nivel; ++i)
				precioReclutamiento += precioBaseReclutamiento;
		}


		protected void calcularDefensa()
		{
			defensa = 0;
			for(int i=0; i<nivel; ++i)
				defensa += defensaBase;
		}
	}
}




