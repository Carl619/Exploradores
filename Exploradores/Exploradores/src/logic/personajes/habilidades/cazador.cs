using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Personajes
{
	
	
	public class Cazador : Habilidad
	{
		// variables
		protected uint _habilidadBase { get; set; }
		public uint habilidadBase
		{
			get { return _habilidadBase; }
			set { _habilidadBase = value; calcularHabilidad(); }
		}
		public uint habilidad { get; protected set; }


		// constructor
		public Cazador(String newID, String newNombre, uint newNivel = 1)
			: base(newID, newNombre, newNivel)
		{
			_habilidadBase = 0;
			calcularHabilidad();
		}


		//funciones
		public override Habilidad clone(List<String> campos)
		{
			Cazador cazador = new Cazador(id, nombre, Convert.ToUInt32(campos[0]));

			if(campos.Count > 1)
			{
				cazador.habilidadBase = Convert.ToUInt32(campos[1]);
			}
			else
			{
				cazador.habilidadBase = habilidadBase;
			}
			
			cazador.descripcion = descripcion;
			cazador.icono = icono;
			return cazador;
		}


		public override ILSXNA.Container getVistaEspecifica()
		{
			ILSXNA.Container contenedor = new ILSXNA.Container();
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			SpriteFont font = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			Color color = Gestores.Mundo.Instancia.colores["genericColor"];
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Habilidad de caza: " + habilidad.ToString();
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
			label.message = "Habilidad de caza base: " + habilidadBase.ToString();
			label.color = color;
			label.innerComponent = font;
			contenedor.addComponent(label);

			return contenedor;
		}


		public override void incrementarNivel(uint numero)
		{
			base.incrementarNivel(numero);
			calcularHabilidad();
		}


		protected void calcularHabilidad()
		{
			habilidad = 0;
			for(int i=0; i<nivel; ++i)
				habilidad += habilidadBase;
		}
	}
}




