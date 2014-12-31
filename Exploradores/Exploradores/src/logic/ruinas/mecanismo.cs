using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public partial class Mecanismo : Gestores.IObjetoIdentificable
	{
		// enums
		public enum Accion
		{
			Alternar,
			Activar,
			Desactivar
		}


		// variables
		public String id { get; protected set; }
		public List<Objeto> objetos { get; protected set; }
		public Estado estadoActual { get; set; }
		

		// constructor
		public Mecanismo(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = (String)newID.Clone();
			objetos = new List<Objeto>();
			estadoActual = crearEstadoActual();
		}


		// funciones
		public static bool esLista(String campo)
		{
			if(campo.Equals("activadores") == true)
				return true;
			if(campo.Equals("objetos") == true)
				return true;
			if(campo.Equals("estados") == true)
				return true;
			if(campo.Equals("reglas") == true)
				return true;
			if(campo.Equals("dependencias") == true)
				return true;
			return false;
		}
		
		
		public static Mecanismo cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Mecanismo mecanismo;

			mecanismo = new Mecanismo(campos["id"]);

			foreach(String activador in listas["activadores"])
			{
				Gestores.Partidas.Instancia.gestorRuinas.activadores[activador].mecanismo = mecanismo;
			}

			foreach(String objetos in listas["objetos"])
			{
				mecanismo.objetos.Add(Gestores.Partidas.Instancia.gestorRuinas.objetos[objetos]);
			}

			List<Mecanismo.Estado> estados = new List<Estado>();

			Estado estadoInicial = mecanismo.crearEstadoActual();
			foreach(String valoresEstado in listas["estados"])
			{
				Estado estadoActual = estadoInicial.clone();
				String[] partes = valoresEstado.Split('|');
				int i = 0;
				foreach(String valor in partes)
				{
					estadoActual.estadoObjetos[i] = Convert.ToBoolean(valor.Trim());
					++i;
				}
				estados.Add(estadoActual);
			}
			mecanismo.estadoActual = estados[0];
			
			List<Regla> reglas = new List<Regla>();
			foreach(String estadosRegla in listas["reglas"])
			{
				// estadoOrigen | estadoDestino
				String[] partes = estadosRegla.Split('|');
				Regla regla = new Regla(estados[Convert.ToInt32(partes[1].Trim())]);
				estados[Convert.ToInt32(partes[0].Trim())].reglas.Add(regla);
				reglas.Add(regla);
			}
			
			foreach(String valoresEstado in listas["dependencias"])
			{
				// regla | tipo | id objeto
				String[] partes = valoresEstado.Split('|');
				if(partes[1].Trim().Equals("alternar"))
					reglas[Convert.ToInt32(partes[0].Trim())].dependenciasAlternar.Add(partes[2].Trim());
				else if(partes[1].Trim().Equals("activar"))
					reglas[Convert.ToInt32(partes[0].Trim())].dependenciasActivar.Add(partes[2].Trim());
				else if(partes[1].Trim().Equals("desactivar"))
					reglas[Convert.ToInt32(partes[0].Trim())].dependenciasDesactivar.Add(partes[2].Trim());
			}

			return mecanismo;
		}


		public void ejecutarAccion(String idActivador, Accion accion)
		{
			// mirar si ya esta activado / desactivado
			bool searchBool = false;
			if(accion == Accion.Activar)
				searchBool = true;
			if(accion != Accion.Alternar)
			{
				foreach(Objeto objeto in objetos)
				{
					if(objeto.id.Equals(idActivador) == true)
					{
						if(objeto.activado == searchBool)
							return;
						break;
					}
				}
			}


			// cambiar el estado
			Estado estadoSiguiente = estadoActual.cambiarEstado(idActivador, accion);
			if(estadoSiguiente == null || estadoActual == estadoSiguiente ||
				estadoActual.equivalente(estadoSiguiente) == true)
				return;
			
			for(int i=0; i<objetos.Count; ++i)
			{
				if(estadoActual.estadoObjetos[i] != estadoSiguiente.estadoObjetos[i])
					objetos[i].actualizarEstado(estadoSiguiente.estadoObjetos[i]);
			}
			estadoActual = estadoSiguiente;
		}


		public Estado crearEstadoActual()
		{
			List<bool> lista = new List<bool>();
			foreach(Objeto objeto in objetos)
				lista.Add(objeto.activado);
			Estado estado = new Estado(lista);
			return estado;
		}
	}
}



