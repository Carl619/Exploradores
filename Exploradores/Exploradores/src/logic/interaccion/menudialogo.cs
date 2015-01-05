using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public class MenuDialogo : ElementoDialogo
	{
		// variables
		public List<ElementoDialogo> dialogo { get; set; }


		//constructor
		public MenuDialogo(String newID, String newTitulo, String newTexto)
			: base(newID, newTitulo, newTexto)
		{
			dialogo = new List<ElementoDialogo>();
		}


		// funciones
		public static bool esLista(String campo)
		{
			if(campo.Equals("entradas") == true)
				return true;
			if(campo.Equals("argumentosEvento") == true)
				return true;
			return false;
		}


		
		public static MenuDialogo cargarObjeto(Dictionary<String, String> campos,
											Dictionary<String, List<String>> listas)
		{
			MenuDialogo menu;

			menu = new MenuDialogo(campos["id"], campos["titulo"], campos["texto"]);
			foreach(String entradasDialogo in listas["entradas"])
			{
				String[] partes = entradasDialogo.Split('|');
				EntradaDialogo entrada = new EntradaDialogo(partes[0].Trim(), partes[2].Trim(), partes[3].Trim());
				entrada.visible = Convert.ToBoolean(partes[1].Trim());
				if(partes.Length > 4)
					entrada.evento = Gestores.Partidas.Instancia.eventos[partes[4].Trim()];
				if(partes.Length > 5)
					entrada.bloquearNavegacion = Convert.ToBoolean(partes[5].Trim());
				if(partes.Length > 6)
				{
					int i = 0;
					foreach(String argumento in partes)
					{
						if(i < 6)
						{
							++i;
							continue;
						}
						parseArgumento(entrada, partes[i].Trim());
						++i;
					}
				}
				menu.dialogo.Add(entrada);
				Gestores.Partidas.Instancia.elementosDialogo.Add(entrada.id, entrada);
			}

			String campo;
			if(campos.TryGetValue("evento", out campo) == true)
			{
				menu.evento = Gestores.Partidas.Instancia.eventos[campo];
			}
			if(campos.TryGetValue("bloquearNavegacion", out campo) == true)
			{
				menu.bloquearNavegacion = Convert.ToBoolean(campo);
			}
			List<String> lista;
			if(listas.TryGetValue("argumentosEvento", out lista) == true)
			{
				foreach(String argumento in lista)
					parseArgumento(menu, argumento);
			}


			menu.ordenarDialogoInvisible();
			Gestores.Partidas.Instancia.elementosDialogo.Add(menu.id, menu);

			return menu;
		}


		
		protected static void parseArgumento(ElementoDialogo elemento, String cadenaArgumento)
		{
			if(cadenaArgumento.Length < 2)
			{
				elemento.argumentosEvento.Add(new Evento.Argumento(cadenaArgumento));
				return;
			}
			if(cadenaArgumento[0] != '#' || cadenaArgumento[1] != '#')
			{
				elemento.argumentosEvento.Add(new Evento.Argumento(cadenaArgumento));
				return;
			}
			cadenaArgumento = cadenaArgumento.Substring(2, cadenaArgumento.Length - 2);
			String[] partes = cadenaArgumento.Split(',');

			Evento.Argumento argumento = new Evento.Argumento(true);
			foreach(String arg in partes)
				argumento.lista.Add(arg.Trim());
			elemento.argumentosEvento.Add(argumento);
		}


		public override ElementoDialogo clone()
		{
			MenuDialogo menu = new MenuDialogo(id, titulo, texto);
			menu.evento = evento;
			menu.bloquearNavegacion = bloquearNavegacion;
			menu.dialogo.AddRange(dialogo);
			return menu;
		}


		public static String guardarObjeto(MenuDialogo menu)
		{
			String resultado;
			resultado = "	id				: " + menu.id + "\n" +
						"	titulo			: " + menu.titulo + "\n" +
						"	texto			: " + menu.texto + "\n" +
						"	entradas		: " + menu.dialogo.Count.ToString() + "\n";
			String entradas;
			entradas = "";
			foreach(ElementoDialogo entrada in menu.dialogo)
			{
				entradas = entradas +
						"		" + entrada.id +
						" | " + entrada.titulo + " | " + entrada.texto + "\n";
			}
			return resultado + entradas;
		}


		// colocar elementos invisibles a final de la esValor
		public void ordenarDialogoInvisible()
		{
			List<ElementoDialogo> listaVisible = new List<ElementoDialogo>();
			List<ElementoDialogo> listaInvisible = new List<ElementoDialogo>();

			foreach(ElementoDialogo elemento in dialogo)
			{
				if(elemento.visible == true)
					listaVisible.Add(elemento);
				if(elemento.visible == false)
					listaInvisible.Add(elemento);
			}
			listaVisible.AddRange(listaInvisible);
			dialogo = listaVisible;
		}
	}
}




