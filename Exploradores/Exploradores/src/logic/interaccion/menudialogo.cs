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
			return false;
		}


		
		public static MenuDialogo cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			MenuDialogo menu;

			menu = new MenuDialogo(campos["id"], campos["titulo"], campos["texto"]);
			foreach(String entradasDialogo in listas["entradas"])
			{
				String[] partes = entradasDialogo.Split('|');
				EntradaDialogo entrada = new EntradaDialogo(partes[0].Trim(), partes[1].Trim(), partes[2].Trim());
				if(partes.Length > 2)
					entrada.evento = Gestores.Partidas.Instancia.eventos[partes[3].Trim()];
				if(partes.Length > 3)
					entrada.bloquearNavegacion = Convert.ToBoolean(partes[4].Trim());
				menu.dialogo.Add(entrada);
			}

			return menu;
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
	}
}




