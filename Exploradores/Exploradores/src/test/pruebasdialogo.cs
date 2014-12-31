using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Interaccion
{
	class Dialogo_pruebas
	{
		public Dialogo prueba { get; set; }
		public Dialogo_pruebas(Dialogo p)
		{
			prueba = p;
		}

		public void elegirOpcion_pruebas()
		{
			MenuDialogo menu = (MenuDialogo)prueba.menu;
			menu.dialogo.Add(new EntradaDialogo("texto prueba 1", "bla bla", "texto prueba 1"));
			menu.dialogo.Add(new EntradaDialogo("texto prueba 2", "blo blo", "texto prueba 2"));
			prueba.elegirOpcion(1);
		}
	}
}
