using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public partial class Mecanismo
	{
		public class Estado
		{
			// variables
			public List<bool> estadoObjetos { get; protected set; }
			public List<Regla> reglas { get; protected set; }


			// constructor
			public Estado(List<bool> newEstadoObjetos)
			{
				if(newEstadoObjetos == null)
					throw new ArgumentNullException();
				estadoObjetos = newEstadoObjetos;
				reglas = new List<Regla>();
			}


			public Estado(Estado estado)
			{
				estadoObjetos = new List<bool>();
				reglas = new List<Regla>();

				foreach(bool b in estado.estadoObjetos)
					estadoObjetos.Add(b);
				foreach(Regla regla in estado.reglas)
					reglas.Add(regla.clone());
			}


			// funciones
			public Estado clone()
			{
				return new Estado(this);
			}


			public bool equivalente(Estado estado)
			{
				if(estadoObjetos.Count != estado.estadoObjetos.Count)
					return false;
				for(int i=0; i<estadoObjetos.Count; ++i)
					if(estadoObjetos[i] != estado.estadoObjetos[i])
						return false;
				return true;
			}


			public Estado cambiarEstado(String idActivador, Accion accion)
			{
				foreach(Regla regla in reglas)
				{
					if(regla.esAplicable(idActivador, accion) == true)
						return regla.estadoFinal;
				}

				return null;
			}
		}
	}
}



