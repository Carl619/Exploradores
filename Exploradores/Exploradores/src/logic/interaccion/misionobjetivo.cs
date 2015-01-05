using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	public partial class Mision : Gestores.IObjetoIdentificable
	{
		
		
		public abstract class Objetivo : Gestores.IObjetoIdentificable
		{
			// variables
			public String id { get; protected set; }
			public Etapa etapa { get; protected set; }
			public String descripcion { get; set; }
			public Estado estado { get; protected set; }
			
			
			// constructor
			public Objetivo(String newID, Etapa newEtapa)
			{
				if(newID == null || newEtapa == null)
					throw new ArgumentNullException();
				id = newID;
				etapa = newEtapa;
				descripcion = null;
				estado = Estado.Inactivo;
			}


			// funciones
			public abstract void actualizacionPublisher(
									Interaccion.Mision.Evento evento,
									Interaccion.Mision.InfoEvento info);
			
			
			public virtual void empezar()
			{
				if(estado == Estado.Inactivo)
					estado = Estado.EnProgreso;
				Gestores.Partidas.Instancia.gestorMisiones.subscribe(this);
				actualizarCumplido();
			}


			public virtual void terminar()
			{
				if(estado == Estado.EnProgreso)
					estado = Estado.Inactivo;
				Gestores.Partidas.Instancia.gestorMisiones.unsubscribe(this);
			}
			
			
			public virtual void actualizarCumplido()
			{
				if(estado == Estado.Terminado || estado == Estado.Fracasado)
					etapa.actualizar();
			}


			public MisionObjetivoView crearVista(bool seleccionar, bool actualizarVista = true)
			{
				return new MisionObjetivoView(this, seleccionar, actualizarVista);
			}
		}
	}
}




