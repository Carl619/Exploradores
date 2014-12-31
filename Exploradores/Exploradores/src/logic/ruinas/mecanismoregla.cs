using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public partial class Mecanismo
	{
		public class Regla
		{
			// variables
			public Estado estadoFinal { get; protected set; }
			public List<String> dependenciasAlternar { get; set; }
			public List<String> dependenciasActivar { get; set; }
			public List<String> dependenciasDesactivar { get; set; }


			// constructor
			public Regla(Estado final)
			{
				estadoFinal = final;
				dependenciasAlternar = new List<String>();
				dependenciasActivar = new List<String>();
				dependenciasDesactivar = new List<String>();
			}


			public Regla(Regla regla)
			{
				estadoFinal = regla.estadoFinal;
				dependenciasAlternar = new List<String>();
				dependenciasActivar = new List<String>();
				dependenciasDesactivar = new List<String>();

				foreach(String s in regla.dependenciasAlternar)
					dependenciasAlternar.Add(s);
				foreach(String s in regla.dependenciasActivar)
					dependenciasActivar.Add(s);
				foreach(String s in regla.dependenciasDesactivar)
					dependenciasDesactivar.Add(s);
			}


			// functions
			public Regla clone()
			{
				return new Regla(this);
			}


			public bool esAplicable(String idActivador, Accion accion)
			{
				if(accion == Accion.Alternar)
				{
					if(dependenciasAlternar.Contains(idActivador) == true)
						return true;
				}
				else if(accion == Accion.Activar)
				{
					if(dependenciasActivar.Contains(idActivador) == true)
						return true;
				}
				else if(accion == Accion.Desactivar)
				{
					if(dependenciasDesactivar.Contains(idActivador) == true)
						return true;
				}
				return false;
			}
		}
	}
}



