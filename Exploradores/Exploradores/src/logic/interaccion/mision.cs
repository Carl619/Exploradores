using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public partial class Mision : Gestores.IObjetoIdentificable
	{
		// delegados
		public delegate Estado ComprobacionEstadoEtapa(List<Estado> listaEstados);


		// enumeraciones
		public enum Estado
		{
			Inactivo,
			EnProgreso,
			Terminado,
			Fracasado
		}


		public enum Evento
		{
			LlegadaLugar,
			Dialogo,
			ElementoDialogo,
			Articulo,
			Tiempo
		}	


		// variables
		public String id { get; protected set; }
		public String titulo { get; set; }
		public List<String> descripcion { get; protected set; }
		public List<Etapa> etapas { get; protected set; }
		public uint indiceEtapaActual { get; protected set; }
		public List<Interaccion.Evento.Argumento> argumentosPrincipio { get; set; }
		public List<Interaccion.Evento.Argumento> argumentosFin { get; set; }
		public Interaccion.Evento eventoPrincipio { get; set; }
		public Interaccion.Evento eventoFin { get; set; }
		public Estado estado { get; protected set; }
		
		
		// constructor
		public Mision(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			titulo = null;
			descripcion = new List<String>();
			etapas = new List<Etapa>();
			indiceEtapaActual = 0;
			argumentosPrincipio = new List<Interaccion.Evento.Argumento>();
			argumentosFin = new List<Interaccion.Evento.Argumento>();
			eventoPrincipio = null;
			eventoFin = null;
			estado = Estado.Inactivo;
		}


		// funciones
		public static Estado comprobacionAND(List<Estado> listaEstados)
		{
			if(listaEstados == null)
				return Estado.Fracasado;
			foreach(Estado b in listaEstados)
				if(b == Estado.Fracasado)
					return Estado.Fracasado;
			foreach(Estado b in listaEstados)
				if(b != Estado.Terminado)
					return Estado.EnProgreso;
			return Estado.Terminado;
		}


		public static Estado comprobacionANDTiempoPrincipio(List<Estado> listaEstados)
		{
			if(listaEstados == null)
				return Estado.Fracasado;
			foreach(Estado b in listaEstados)
				if(b == Estado.Fracasado)
					return Estado.Fracasado;
			int i = 0;
			foreach(Estado b in listaEstados)
			{
				if(b != Estado.Terminado && i != 0)
					return Estado.EnProgreso;
				++i;
			}
			return Estado.Terminado;
		}


		public static Estado comprobacionOR(List<Estado> listaEstados)
		{
			if(listaEstados == null)
				return Estado.Fracasado;
			foreach(Estado b in listaEstados)
				if(b == Estado.Fracasado)
					return Estado.Fracasado;
			foreach(Estado b in listaEstados)
				if(b == Estado.Terminado)
					return Estado.Terminado;
			return Estado.EnProgreso;
		}


		public void empezarMision()
		{
			if(estado == Estado.Inactivo)
			{
				if(eventoPrincipio != null)
					eventoPrincipio.ejecutar(argumentosPrincipio);
				estado = Estado.EnProgreso;
				if(etapas.Count > 0)
				{
					etapas[0].empezar();
					actualizar();
				}
			}
		}


		public void terminarMision()
		{
			if(estado == Estado.Terminado || estado == Estado.Fracasado)
			{
				etapas[(int)indiceEtapaActual].terminar();
				if(eventoFin != null)
					eventoFin.ejecutar(argumentosFin);
			}
		}
		
		
		public void actualizar()
		{
			if(estado != Estado.EnProgreso)
				return;
			if(indiceEtapaActual == etapas.Count)
				return;
			if(etapas[(int)indiceEtapaActual].estado == Estado.EnProgreso)
				return;
			if(etapas[(int)indiceEtapaActual].estado == Estado.Fracasado)
			{
				estado = Estado.Fracasado;
				terminarMision();
				return;
			}
			if(indiceEtapaActual + 1 == etapas.Count)
			{
				estado = Estado.Terminado;
				terminarMision();
				return;
			}
			else
			{
				++indiceEtapaActual;
				etapas[(int)indiceEtapaActual].empezar();
				actualizar();
			}
		}


		public MisionView crearVista(bool seleccionar, bool actualizarVista = true)
		{
			return new MisionView(this, seleccionar, actualizarVista);
		}
	}
}




