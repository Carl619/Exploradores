using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public class Dialogo
	{
		// variables
		public ElementoDialogo menuOriginal { get; set; }
		public ElementoDialogo menu { get; set; }


		// constructor
		public Dialogo(MenuDialogo newMenu, Dictionary<String, String> cadenasRegex)
		{
			if(newMenu == null || cadenasRegex == null)
				throw new ArgumentNullException();
			menuOriginal = newMenu;
			menu = getMenuNuevo(menuOriginal, cadenasRegex);
		}


		// funciones
		public ElementoDialogo getMenuNuevo(ElementoDialogo elemento, Dictionary<String, String> cadenasRegex)
		{
			if(cadenasRegex.Count < 1)
				return elemento;
			
			ElementoDialogo elementoNuevo = elemento.clone();
			foreach(KeyValuePair<String, String> regex in cadenasRegex)
			{
				elementoNuevo.titulo = elementoNuevo.titulo.Replace("[$" + regex.Key + "]", regex.Value);
				elementoNuevo.texto = elementoNuevo.texto.Replace("[$" + regex.Key + "]", regex.Value);
			}

			if(elemento.GetType() == typeof(MenuDialogo))
			{
				MenuDialogo menu = (MenuDialogo)elementoNuevo;
				menu.dialogo.Clear();
				foreach(ElementoDialogo elem in ((MenuDialogo)elemento).dialogo)
					menu.dialogo.Add(getMenuNuevo(elem, cadenasRegex));
				return menu;
			}

			return elementoNuevo;
		}


		public void elegirOpcion(int opcion)
		{
			if(menu.GetType() != typeof(MenuDialogo))
				return;
			if(((MenuDialogo)menu).dialogo == null)
				return;
			if(opcion < 0 || opcion >= ((MenuDialogo)menu).dialogo.Count)
				return;
			
			List<String> ejecutarOpcion = new List<string>();
			ElementoDialogo e = ((MenuDialogo)menu).dialogo[opcion];
			ejecutarOpcion.Add(e.titulo);

			if(e.evento != null)
				e.evento.ejecutar(ejecutarOpcion);
			if(e.bloquearNavegacion == false)
				menu = e;
		}
	}
}




