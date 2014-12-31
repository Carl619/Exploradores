using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class Activador : Objeto
	{
		// variables
		public Mecanismo mecanismo { get; set; }


		// constructor
		public Activador(String newID, ObjetoFlyweight newObjetoFlyweight)
			: base(newID, newObjetoFlyweight)
		{
			mecanismo = null;
		}

		
		// funciones
		public static Activador cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Activador activador;
			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			Habitacion habitacion =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacion"]];
			
			activador = new Activador(campos["id"], flyweight);
			activador.activado = Convert.ToBoolean(campos["activado"]);
			activador.tiempoActivacion = Convert.ToInt32(campos["tiempoActivacion"]);
			activador.espacio = new Rectangle(Convert.ToInt32(campos["coordenada x"]),
										Convert.ToInt32(campos["coordenada y"]),
										flyweight.iconoPasivo.Width,
										flyweight.iconoPasivo.Height);
			
			habitacion.objetos.Add(activador);

			return activador;
		}


		public void alternar()
		{
			if(mecanismo == null)
				return;
			mecanismo.ejecutarAccion(id, Mecanismo.Accion.Alternar);
		}


		public void activar()
		{
			if(mecanismo == null)
				return;
			mecanismo.ejecutarAccion(id, Mecanismo.Accion.Activar);
		}


		public void desctivar()
		{
			if(mecanismo == null)
				return;
			mecanismo.ejecutarAccion(id, Mecanismo.Accion.Desactivar);
		}

		
		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new ActivadorView(this, ruina);
			return vista;
		}
	}
}




