using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public partial class Mision : Gestores.IObjetoIdentificable
	{
		
		
		public class Etapa : Gestores.IObjetoIdentificable
		{
			// variables
			public String id { get; protected set; }
			public Mision mision { get; set; }
			public String titulo { get; set; }
			public List<String> descripcion { get; protected set; }
			public List<Objetivo> objetivos { get; protected set; }
			public ComprobacionEstadoEtapa funcionLogica { get; set; }
			public List<Interaccion.Evento.Argumento> argumentosPrincipio { get; set; }
			public List<Interaccion.Evento.Argumento> argumentosFin { get; set; }
			public Interaccion.Evento eventoPrincipio { get; set; }
			public Interaccion.Evento eventoFin { get; set; }
			public Estado estado { get; protected set; }
			
			
			// constructor
			public Etapa(String newID, Mision newMision)
			{
				if(newID == null || newMision == null)
					throw new ArgumentNullException();
				id = newID;
				mision = newMision;
				titulo = null;
				descripcion = new List<String>();
				objetivos = new List<Objetivo>();
				funcionLogica = comprobacionAND;
				argumentosPrincipio = new List<Interaccion.Evento.Argumento>();
				argumentosFin = new List<Interaccion.Evento.Argumento>();
				eventoPrincipio = null;
				eventoFin = null;
				estado = Estado.Inactivo;
			}


			// funciones
			public void empezar()
			{
				if(estado == Estado.Inactivo)
				{
					if(eventoPrincipio != null)
						eventoPrincipio.ejecutar(argumentosPrincipio);
					estado = Estado.EnProgreso;
					foreach(Objetivo objetivo in objetivos)
						objetivo.empezar();
					actualizar();
				}
			}


			public void terminar()
			{
				if(eventoFin != null)
					eventoFin.ejecutar(argumentosFin);
				foreach(Objetivo objetivo in objetivos)
					objetivo.terminar();
			}
			
			
			public void actualizar()
			{
				if(estado != Estado.EnProgreso || funcionLogica == null)
					return;
				List<Estado> estados = new List<Estado>();
				foreach(Objetivo objetivo in objetivos)
					estados.Add(objetivo.estado);
				estado = funcionLogica(estados);
				if(estado != Estado.EnProgreso)
				{
					terminar();
					mision.actualizar();
				}
			}


			public MisionEtapaView crearVista(bool seleccionar, bool actualizarVista = true)
			{
				return new MisionEtapaView(this, seleccionar, actualizarVista);
			}
		}
	}
}




