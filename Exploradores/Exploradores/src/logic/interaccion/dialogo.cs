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

			if(menu.evento != null)
				menu.evento.ejecutar(menu.argumentosEvento);
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
			
			ElementoDialogo e = ((MenuDialogo)menu).dialogo[opcion];
			if(e.visible == false)
				return;
			List<Evento.Argumento> argumentos = new List<Evento.Argumento>();
			argumentos.Add(new Evento.Argumento(e.id));
			argumentos.Add(new Evento.Argumento(menu.id));
			argumentos.AddRange(e.argumentosEvento);

			if(e.evento != null)
				e.evento.ejecutar(argumentos);
			if(e.bloquearNavegacion == false)
				menu = e;
			
			Interaccion.Mision.Evento evento = Interaccion.Mision.Evento.ElementoDialogo;
			Interaccion.Mision.InfoEvento info = new Interaccion.Mision.InfoEvento();
			info.elementoDialogo = e;
			Gestores.Partidas.Instancia.gestorMisiones.notify(evento, info);
			if(menu.GetType() == typeof(MenuDialogo))
				((MenuDialogo)menu).ordenarDialogoInvisible();
		}
	}
}




