using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Gestores
{
	
	
	public class GMisiones // gestor de misiones
	{
		// variables
		public Gestor<Interaccion.Mision.Objetivo> objetivosCompletos { get; protected set; }
		public Gestor<Interaccion.Mision.Etapa> etapas { get; protected set; }
		public Gestor<Interaccion.Mision> misiones { get; protected set; }
		public Gestor<Interaccion.Mision.Objetivo> objetivosActivos { get; protected set; }
		protected List<String> listaIteracionDinamica { get; set; }


		// constructor
		public GMisiones()
		{
			objetivosCompletos = new Gestor<Interaccion.Mision.Objetivo>();
			etapas = new Gestor<Interaccion.Mision.Etapa>();
			misiones = new Gestor<Interaccion.Mision>();
			objetivosActivos = new Gestor<Interaccion.Mision.Objetivo>();
			listaIteracionDinamica = null;
		}


		// funciones
		public void cargarTodo()
		{
			Interaccion.Mision mision;
			Interaccion.Mision.Etapa etapa;
			Interaccion.ObjetivoLugar objetivoLugar;
			Interaccion.ObjetivoElementoDialogo objetivoElementoDialogo;
			Interaccion.ObjetivoDialogo objetivoDialogo;
			Interaccion.ObjetivoArticulo objetivoArticulo;
			Interaccion.ObjetivoTiempo objetivoTiempo;

			mision = new Interaccion.Mision("1");
			mision.titulo = "La llamada de Saruman";
			mision.descripcion.Add("Ayer recibistes una llamada perdida de Saruman.");
			mision.descripcion.Add("Como no te quedan minutos gratis en tu movil (mejor dejar de hablar tanto por telefono y empezar a ahorrar), vas a tener que buscarle para preguntarle sobre el asunto que quiere discutir contigo.");
			misiones.Add(mision.id, mision);

			etapa = new Interaccion.Mision.Etapa("1", mision);
			etapa.titulo = "Buscarle a Saruman";
			etapa.descripcion.Add("A estas horas lo mas probable es que le vas a encontrar en Isengard.");
			etapa.descripcion.Add("Viaja hasta alli, a lo mejor le encuentras.");
			mision.etapas.Add(etapa);
			etapas.Add(etapa.id, etapa);
			objetivoLugar = new Interaccion.ObjetivoLugar("1", etapa);
			objetivoLugar.lugar = Gestores.Partidas.Instancia.ciudades["2"];
			objetivoLugar.descripcion = "Llegar a " + objetivoLugar.lugar.nombre;
			etapa.objetivos.Add(objetivoLugar);
			objetivosCompletos.Add(objetivoLugar.id, objetivoLugar);

			etapa = new Interaccion.Mision.Etapa("2", mision);
			etapa.titulo = "Hablar con Grima";
			etapa.descripcion.Add("Parece que Saruman no esta aqui.");
			mision.etapas.Add(etapa);
			etapas.Add(etapa.id, etapa);
			objetivoElementoDialogo = new Interaccion.ObjetivoElementoDialogo("2", etapa);
			objetivoElementoDialogo.elementoDialogo = Gestores.Partidas.Instancia.elementosDialogo["11"];
			objetivoElementoDialogo.descripcion = "Descubre donde esta Saruman";
			etapa.objetivos.Add(objetivoElementoDialogo);
			objetivosCompletos.Add(objetivoElementoDialogo.id, objetivoElementoDialogo);

			etapa = new Interaccion.Mision.Etapa("3", mision);
			etapa.funcionLogica = Interaccion.Mision.comprobacionANDTiempoPrincipio;
			etapa.titulo = "Obtener respuestas";
			etapa.descripcion.Add("Grima te ha dicho que Saruman se encuentra en Minas Tirith. Encuentrale hasta que no se vaya.");
			mision.etapas.Add(etapa);
			etapas.Add(etapa.id, etapa);
			objetivoTiempo = new Interaccion.ObjetivoTiempo("6", etapa);
			objetivoTiempo.numeroHorasLimite = 24;
			etapa.objetivos.Add(objetivoTiempo);
			objetivosCompletos.Add(objetivoTiempo.id, objetivoTiempo);
			objetivoDialogo = new Interaccion.ObjetivoDialogo("3", etapa);
			objetivoDialogo.npc = Gestores.Partidas.Instancia.npcs["9"];
			objetivoDialogo.descripcion = "Habla con " + objetivoDialogo.npc.nombre;
			etapa.objetivos.Add(objetivoDialogo);
			objetivosCompletos.Add(objetivoDialogo.id, objetivoDialogo);


			mision = new Interaccion.Mision("2");
			mision.titulo = "Lista de compras";
			mision.descripcion.Add("Saruman te ha indicado que le puedes hacer un favor si le haces las compras.");
			misiones.Add(mision.id, mision);

			etapa = new Interaccion.Mision.Etapa("4", mision);
			etapa.titulo = "Hacer las compras";
			etapa.descripcion.Add("Tienes que conseguir un diamante.");
			mision.etapas.Add(etapa);
			etapas.Add(etapa.id, etapa);
			objetivoArticulo = new Interaccion.ObjetivoArticulo("4", etapa);
			objetivoArticulo.articulo = Gestores.Partidas.Instancia.articulos["idDiamantes"];
			objetivoArticulo.cantidad = 1;
			objetivoArticulo.descripcion = "Obtener " + objetivoArticulo.cantidad.ToString() + " " + objetivoArticulo.articulo.nombre + ".";
			etapa.objetivos.Add(objetivoArticulo);
			etapa.eventoPrincipio = Gestores.Partidas.Instancia.eventos["addEventoDialogo"];
			etapa.argumentosPrincipio.Add(new Interaccion.Evento.Argumento(""));
			etapa.argumentosPrincipio.Add(new Interaccion.Evento.Argumento(""));
			etapa.argumentosPrincipio.Add(new Interaccion.Evento.Argumento("12"));
			etapa.argumentosPrincipio.Add(new Interaccion.Evento.Argumento("dialogoDobleEntregaArticulo"));
			objetivosCompletos.Add(objetivoArticulo.id, objetivoArticulo);

			etapa = new Interaccion.Mision.Etapa("5", mision);
			etapa.titulo = "Devolverle las compras";
			etapa.descripcion.Add("Hablar de nuevo con Saruman.");
			mision.etapas.Add(etapa);
			etapas.Add(etapa.id, etapa);
			objetivoElementoDialogo = new Interaccion.ObjetivoElementoDialogo("5", etapa);
			objetivoElementoDialogo.elementoDialogo = Gestores.Partidas.Instancia.elementosDialogo["15"];
			objetivoElementoDialogo.descripcion = "Dar el diamante a Saruman";
			etapa.objetivos.Add(objetivoElementoDialogo);
			etapa.eventoFin = Gestores.Partidas.Instancia.eventos["removeEventoDialogo"];
			etapa.argumentosFin.Add(new Interaccion.Evento.Argumento(""));
			etapa.argumentosFin.Add(new Interaccion.Evento.Argumento(""));
			etapa.argumentosFin.Add(new Interaccion.Evento.Argumento("12"));
			objetivosCompletos.Add(objetivoElementoDialogo.id, objetivoElementoDialogo);
			
			misiones["1"].empezarMision();
		}


		public void subscribe(Interaccion.Mision.Objetivo objetivo)
		{
			objetivosActivos.Add(objetivo.id, objetivo);
			if(listaIteracionDinamica != null)
				listaIteracionDinamica.Add(objetivo.id);
			else // comprobacion inmediata de objetivos
			{
				if(objetivo.GetType() == typeof(Interaccion.ObjetivoArticulo))
				{
					Programa.Jugador.Instancia.actualizarEspacioInventario();
					notify(Interaccion.Mision.Evento.Articulo, null);
				}
			}
		}


		public void unsubscribe(Interaccion.Mision.Objetivo objetivo)
		{
			objetivosActivos.Remove(objetivo.id);
		}


		public void notify(Interaccion.Mision.Evento evento,
							Interaccion.Mision.InfoEvento info)
		{
			listaIteracionDinamica = new List<String>();
			foreach(KeyValuePair<String, Interaccion.Mision.Objetivo> objetivo in objetivosActivos)
				listaIteracionDinamica.Add(objetivo.Key);
			
			for (int i = 0; i < listaIteracionDinamica.Count; i++)
			{
				String clave = listaIteracionDinamica[i];

				Interaccion.Mision.Objetivo objetivo;

				if(objetivosActivos.TryGetValue(clave, out objetivo) == false)
					continue;
				
				objetivo.actualizacionPublisher(evento, info);
			}

			listaIteracionDinamica = null;
		}
	}
}




