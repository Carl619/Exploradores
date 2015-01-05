using System;
using System.Collections.Generic;




namespace Interaccion
{
	

	// objetivo de obtener una cantidad minima de articulos
	public class ObjetivoArticulo : Mision.Objetivo
	{
		// variables
		public Objetos.Articulo articulo { get; set; }
		public uint cantidad { get; set; }
		
		
		// constructor
		public ObjetivoArticulo(String newID, Mision.Etapa newEtapa)
			: base(newID, newEtapa)
		{
			articulo = null;
			cantidad = 1;
		}


		// funciones
		public override void actualizacionPublisher(
									Interaccion.Mision.Evento evento,
									Interaccion.Mision.InfoEvento info)
		{
			if(evento != Mision.Evento.Articulo)
				return;
			Objetos.ColeccionArticulos coleccion;
			if(Programa.Jugador.Instancia.protagonista.inventario.articulos.TryGetValue(
				articulo.id, out coleccion) == true)
			{
				if(coleccion.cantidad >= cantidad)
				{
					estado = Mision.Estado.Terminado;
					actualizarCumplido();
				}
			}
		}
	}
}




