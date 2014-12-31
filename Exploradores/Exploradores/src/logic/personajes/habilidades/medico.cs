using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Personajes
{
	
	
	public class Medico : Habilidad
	{
		// variables
		protected uint _eficaciaBase { get; set; }
		public uint eficaciaBase
		{
			get { return _eficaciaBase; }
			set { _eficaciaBase = value; calcularEficacia(); }
		}
		public uint eficacia { get; protected set; }


		// constructor
		public Medico(String newID, String newNombre, uint newNivel = 1)
			: base(newID, newNombre, newNivel)
		{
			_eficaciaBase = 0;
			calcularEficacia();
		}


		//funciones
		public override Habilidad clone(List<String> campos)
		{
			Medico medico = new Medico(id, nombre, Convert.ToUInt32(campos[0]));

			if(campos.Count > 1)
			{
				medico.eficaciaBase = Convert.ToUInt32(campos[1]);
			}
			else
			{
				medico.eficaciaBase = eficaciaBase;
			}
			
			medico.descripcion = descripcion;
			medico.icono = icono;
			return medico;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Eficacia curacion: " + eficacia.ToString();
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
			label.message = "Eficacia curacion base: " + eficaciaBase.ToString();
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
			eficacia = 0;
			for(int i=0; i<nivel; ++i)
				eficacia += eficaciaBase;
		}
	}
}




