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
		public override void ejecutar(List<String> valoresEntrada)
		{
			List<String> valoresLocales = new List<String>();
			foreach (String i in variablesLocales)
				valoresLocales.Add("");
			
			int index = 0;
			foreach(Evento i in eventos)
			{
				List<String> valoresLlamada = new List<String>();
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
					foreach (String j in parametrosLlamadas[index])
					{
						actualizarVariableLocal(valoresLocales, valoresEntrada, valoresLlamada, j);
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
		
		
		protected void introducirValorEntrada(List<String> valoresLocales, List<String> valoresEntrada,
											List<String> valoresLlamada, String parametro)
		{
			int index;
			
			index = 0;
			foreach (String i in variablesLocales)
			{
				if (i.Equals(parametro) == true)
				{
					valoresLlamada.Add((String)valoresLocales[index].Clone());
					return;
				}
				++index;
			}
			
			index = 0;
			foreach (String i in parametrosEntrada)
			{
				if (i.Equals(parametro) == true)
				{
					valoresLlamada.Add((String)valoresEntrada[index].Clone());
					return;
				}
				++index;
			}
			
			valoresLlamada.Add(parametro);
		}
		
		
		protected void actualizarVariableLocal(List<String> valoresLocales, List<String> valoresEntrada,
											List<String> valoresLlamada, String parametro)
		{
			int index;
			
			index = 0;
			foreach (String i in variablesLocales)
			{
				if (i.Equals(parametro) == true)
				{
					valoresLocales[index] = (String)valoresLlamada[index].Clone();
					return;
				}
				++index;
			}
			
			index = 0;
			foreach (String i in parametrosEntrada)
			{
				if (i.Equals(parametro) == true)
				{
					valoresEntrada[index] = (String)valoresEntrada[index].Clone();
					return;
				}
				++index;
			}
		}
	}
}




