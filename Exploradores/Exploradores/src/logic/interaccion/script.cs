using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public class Script : Evento
	{
		// variables
		public List<List<String>> parametrosLlamadas { get; protected set; }
		public List<Evento> eventos { get; protected set; }
		public List<String> variablesLocales { get; protected set; }
		
		
		// constructor
		public Script(String newID)
			: base(newID, -1)
		{
			parametrosLlamadas = new List<List<String>>();
			eventos = new List<Evento>();
			variablesLocales = new List<String>();
		}
		
		
		// funciones
		public override void ejecutar(List<Argumento> valoresEntrada)
		{
			List<Argumento> valoresLocales = new List<Argumento>();
			foreach (String i in variablesLocales)
				valoresLocales.Add(new Argumento());
			
			int index = 0;
			foreach(Evento i in eventos)
			{
				List<Argumento> valoresLlamada = new List<Argumento>();
				if(parametrosLlamadas.Count > 0)
				{
					foreach (String j in parametrosLlamadas[index])
					{
						introducirValorEntrada(valoresLocales, valoresEntrada, valoresLlamada, j);
					}
				}

				i.ejecutar(valoresLlamada);

				if(parametrosLlamadas.Count > 0)
				{
					int k = 0;
					foreach (String j in parametrosLlamadas[index])
					{
						actualizarVariableLocal(valoresLocales, valoresEntrada, valoresLlamada[k], j);
						++k;
					}
				}
				
				++index;
			}
		}
		
		
		public void addEvento(Evento evento, List<String> parametros)
		{
			if (evento != null && parametros != null)
			{
				eventos.Add(evento);
				parametrosLlamadas.Add(parametros);
			}
		}
		
		
		protected void introducirValorEntrada(List<Argumento> valoresLocales, List<Argumento> valoresEntrada,
											List<Argumento> valoresLlamada, String parametro)
		{
			int index;
			
			index = 0;
			foreach (String i in variablesLocales)
			{
				if (i.Equals(parametro) == true)
				{
					valoresLlamada.Add(valoresLocales[index].clone());
					return;
				}
				++index;
			}
			
			index = 0;
			foreach (String i in parametrosEntrada)
			{
				if (i.Equals(parametro) == true)
				{
					valoresLlamada.Add(valoresEntrada[index].clone());
					return;
				}
				++index;
			}
			
			valoresLlamada.Add(new Argumento(parametro));
		}
		
		
		protected void actualizarVariableLocal(List<Argumento> valoresLocales, List<Argumento> valoresEntrada,
											Argumento valorLlamada, String parametro)
		{
			int index;
			
			index = 0;
			foreach (String i in variablesLocales)
			{
				if (i.Equals(parametro) == true)
				{
					valoresLocales[index] = valorLlamada.clone();
					return;
				}
				++index;
			}
			
			index = 0;
			foreach (String i in parametrosEntrada)
			{
				if (i.Equals(parametro) == true)
				{
					valoresEntrada[index] = valorLlamada.clone();
					return;
				}
				++index;
			}
		}
	}
}




