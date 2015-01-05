using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	// objetivo de no pasarse de una fecha limite
	public class ObjetivoTiempo : Mision.Objetivo
	{
		// variables
		public uint dia { get; protected set; }
		public uint hora { get; protected set; }
		public uint numeroHorasLimite { get; set; }
		
		
		// constructor
		public ObjetivoTiempo(String newID, Mision.Etapa newEtapa)
			: base(newID, newEtapa)
		{
			dia = 0;
			hora = 0;
			numeroHorasLimite = 0;
		}


		// funciones
		public override void actualizacionPublisher(
									Interaccion.Mision.Evento evento,
									Interaccion.Mision.InfoEvento info)
		{
			if(evento != Mision.Evento.Tiempo)
				return;
			if(Gestores.Partidas.Instancia.dias > dia ||
				(Gestores.Partidas.Instancia.dias == dia && Gestores.Partidas.Instancia.horas >= hora))
			{
				estado = Mision.Estado.Fracasado;
				actualizarCumplido();
			}
		}


		public override void empezar()
		{
			if(estado == Mision.Estado.Inactivo)
			{
				dia = Gestores.Partidas.Instancia.dias;
				hora = Gestores.Partidas.Instancia.horas + numeroHorasLimite;
				while(hora > 23)
				{
					++dia;
					hora -= 24;
				}
				descripcion = "Tienes como fecha limite: Dia " +
								dia.ToString() + ", " +
								hora.ToString() + ":00 h";
			}
			base.empezar();
		}


		public override void terminar()
		{
			if(estado == Mision.Estado.EnProgreso)
				estado = Mision.Estado.Terminado;
			base.terminar();
		}
	}
}




