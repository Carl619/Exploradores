using System;
using System.Collections.Generic;




namespace Pruebas
{
	
	
	public abstract class PruebaUnitaria
	{
		// delegados
		public delegate bool pruebaUnitaria();


		// variables
		protected List<pruebaUnitaria> lista;


		// constructor
		public PruebaUnitaria()
		{
			lista = new List<pruebaUnitaria>();
		}


		// funciones
		public virtual void beforeTest()
		{
		}


		public virtual void afterTest()
		{
		}


		public int ejecutarPruebas()
		{
			if(lista == null)
				throw new ArgumentNullException();
			
			int fracaso = 0;
			foreach(pruebaUnitaria p in lista)
			{
				if(p != null)
				{
					beforeTest();
					if(p() == false)
						++fracaso;
					afterTest();
				}
			}

			return fracaso;
		}
	}


	public static class PruebasCompletas
    {
        public static void probarTodo()
        {
			int resultado;
			

			Console.WriteLine("Ejecutando pruebas de Script.");
			PruebasScript pruebasScript = new PruebasScript();
			resultado = pruebasScript.ejecutarPruebas();
			if(resultado > 0)
				Console.WriteLine("Error: hay " + resultado.ToString() + " pruebas de Script que han fracasado.");
			
			
			/*
			Console.WriteLine("Ejecutando pruebas de MenuDialogo.");
			MenuDialogo menuPrincipal = new MenuDialogo("prubas", "Esta es una prueba","ID menuPrincipal de prueba");
			Dialogo dialogo = new Dialogo(menuPrincipal);
			Dialogo_pruebas dialogopruebas = new Dialogo_pruebas(dialogo);
			dialogopruebas.elegirOpcion_pruebas();
			*/


			Console.WriteLine("\nSe ha terminado de ejecutar todas las pruebas.");
        }
    }
}




