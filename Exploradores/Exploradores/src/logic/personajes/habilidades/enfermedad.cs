using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Personajes
{
	
	
	public class Enfermedad : Habilidad
	{
		// variables
		protected double _eficaciaBase { get; set; }
		public double eficaciaBase
		{
			get { return _eficaciaBase; }
			set { _eficaciaBase = value; calcularEficacia(); }
		}
		public double eficacia { get; protected set; }


		// constructor
		public Enfermedad(String newID, String newNombre, uint newNivel = 1)
			: base(newID, newNombre, newNivel)
		{
			_eficaciaBase = 0;
			calcularEficacia();
		}


		//funciones
		public override Habilidad clone(List<String> campos)
		{
			Enfermedad enfermedad = new Enfermedad(id, nombre, Convert.ToUInt32(campos[0]));

			if(campos.Count > 1)
			{
				enfermedad.eficaciaBase = Gestores.Mundo.parseFloat(campos[1]);
			}
			else
			{
				enfermedad.eficaciaBase = eficaciaBase;
			}
			
			enfermedad.descripcion = descripcion;
			enfermedad.icono = icono;
			return enfermedad;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Eficacia enfermedad: " + eficacia.ToString("F4");
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
			label.message = "Eficacia curacion base: " + eficaciaBase.ToString("F4");
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			return contenedor;
		}


		public override void incrementarNivel(uint numero)
		{
			base.incrementarNivel(numero);
			calcularEficacia();
		}


		protected void calcularEficacia()
		{
			eficacia = 1.0f;
			for(int i=0; i<nivel; ++i)
				eficacia *= eficaciaBase;
		}
	}
}




