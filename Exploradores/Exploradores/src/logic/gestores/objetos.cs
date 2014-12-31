using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Gestores
{
	
	
	public class Gestor<T> : Dictionary<String, T> where T : IObjetoIdentificable
	{
		// delegados
		public delegate bool objetoCargableEsLista(String nombre);
		public delegate T objetoCargableCrear(Dictionary<String, String> campos, Dictionary<String, List<String>> listas);
		public delegate String objetoCargableAlmacenar(T objeto);


		// funciones
		public static bool noEsLista(String name)
		{
			return false;
		}


		public static SpriteFont parseFont(String name)
		{
			return Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
		}


		public static Color parseColor(String name)
		{
			if(name[0] == '#')
			{
				String nombre = name.Substring(1).Trim();
				List<String> valores = new List<string>();
				foreach(String val in nombre.Split(','))
					valores.Add(val.Trim());
				if(valores.Count == 3)
					return new Color(
								Convert.ToInt32(valores[0]),
								Convert.ToInt32(valores[1]),
								Convert.ToInt32(valores[2])
								);
				if(valores.Count == 4)
					return new Color(
								Convert.ToInt32(valores[0]),
								Convert.ToInt32(valores[1]),
								Convert.ToInt32(valores[2]),
								Convert.ToInt32(valores[3])
								);
				return Gestores.Mundo.Instancia.colores["genericColor"];
			}
			else if(name[0] == '@')
			{
				String nombre = name.Substring(1).Trim();
				return Mundo.Instancia.colores[nombre];
			}

			return (Color)(typeof(Color).GetProperty(name).GetValue(null, null));	
		}


		public void loadAll(System.IO.StreamReader file,
							objetoCargableCrear funcCrear,
							objetoCargableEsLista funcEsLista = null)
		{
			if(funcCrear == null)
				throw new ArgumentNullException();
			if(funcEsLista == null)
				funcEsLista = noEsLista;
			
			while(true)
			{
				String line = file.ReadLine();
				if(line == null)
					return;
				line = line.Trim();
				if(line.Equals("{") == true)
					break;
			}
			
			bool finObjetos = false;
			while(finObjetos == false)
			{
				T myObj = readObject(file, funcCrear, funcEsLista, out finObjetos);
				if(myObj == null)
					continue;
				Add(myObj.id, myObj);
			}
		}


		public void saveAll(System.IO.StreamWriter file,
							objetoCargableAlmacenar funcAlmacenar)
		{
			file.WriteLine("{\n");
			foreach(KeyValuePair<String, T> objeto in this)
			{
				writeObject(file, objeto.Value, funcAlmacenar);
			}
			file.WriteLine("}\n\n\n");
		}
		
		
		protected T readObject(System.IO.StreamReader file,
							objetoCargableCrear funcCrear,
							objetoCargableEsLista funcEsLista,
							out bool finObjetos)
		{
			finObjetos = false;
			Dictionary<String, String> campos = new Dictionary<String, String>();
			Dictionary<String, List<String>> listas = new Dictionary<String, List<String>>();
			
			while(true)
			{
				String line = file.ReadLine();
				if(line == null)
					return default(T);
				
				line = line.Trim();
				if(line.Equals("}") == true)
				{
					finObjetos = true;
					break;
				}
				if(line.Length < 1)
					break;
				
				String[] words = line.Split(':');
				String left = words[0].Trim();
				String right = words[1].Trim();
				
				if(funcEsLista(left) == true)
				{
					int repeat = Convert.ToInt32(right);
					List<String> listaActual;
					if(listas.TryGetValue(left, out listaActual) == false)
					{
						listaActual = new List<string>();
						listas.Add(left, listaActual);
					}
					for (int i = 0; i < repeat; ++i)
						listaActual.Add(file.ReadLine().Trim());
				}
				else
				{
					campos.Add(left, right);
				}
			}
			
			if(campos.Count == 0 && listas.Count == 0)
				return default(T);
			return funcCrear(campos, listas);
		}
		
		
		protected void writeObject(System.IO.StreamWriter file, T objeto,
									objetoCargableAlmacenar funcAlmacenar)
		{
			file.WriteLine(funcAlmacenar(objeto) + "\n\n\n");
		}
	}
}




