using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public abstract partial class Evento
	{
		public class Argumento
		{
			// variables
			public bool esValor { get; set; }
			public String valor { get; set; }
			public List<String> lista { get; set; }


			// constructor
			public Argumento(bool esLista = false)
			{
				esValor = !esLista;
				valor = "";
				lista = new List<String>();
			}


			public Argumento(String val)
			{
				esValor = true;
				valor = (String)val.Clone();
				lista = new List<String>();
			}


			public Argumento(List<String> listaVal)
			{
				esValor = false;
				valor = "";
				lista = new List<String>();
				foreach(String v in listaVal)
					lista.Add((String)v.Clone());
			}


			// funciones
			public Argumento clone()
			{
				Argumento argumento = new Argumento();

				argumento.esValor = esValor;
				argumento.valor = (String)valor.Clone();
				foreach(String v in lista)
					argumento.lista.Add((String)v.Clone());

				return argumento;
			}
		}
	}
}




