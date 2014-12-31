using System;


namespace Programa
{
#if WINDOWS || XBOX
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			//Pruebas.PruebasCompletas.probarTodo();

			using (Exploradores game = Exploradores.Instancia)
			{
				game.Run();
			}
		}
	}
#endif
}




