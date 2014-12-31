using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ruinas
{


	public class PuertaSalida : Puerta
	{

		// variables



		// constructor
		public PuertaSalida(String newid, ObjetoFlyweight newObjetoFlyweight)
			: base(newid, newObjetoFlyweight)
		{
			paredAsociada = Pared.Arriba;
			porcentajeLugar = 0.5f;
		}


		// funciones
		public static PuertaSalida cargarObjetoSalida(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			PuertaSalida puerta;

			ObjetoFlyweight flyweight = Gestores.Mundo.Instancia.objetoFlyweights[campos["flyweight"]];
			RuinaJugable ruina = Gestores.Partidas.Instancia.gestorRuinas.ruinasJugables[campos["ruina"]];
			Habitacion habitacionPrincipal =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacionPrincipal"]];
			Habitacion habitacionSecundaria =
				Gestores.Partidas.Instancia.gestorRuinas.habitaciones[campos["habitacionSecundaria"]];

			puerta = new PuertaSalida(campos["id"], flyweight);
			puerta.habitacionPrincipal = habitacionPrincipal;
			puerta.habitacionSecundaria = habitacionSecundaria;
			Puerta.Pared paredAsociada;
			paredAsociada = (Puerta.Pared)Enum.Parse(
						typeof(Puerta.Pared),
						campos["paredAsociada"]);
			puerta.paredAsociada = paredAsociada;
			puerta.porcentajeLugar = (float)Gestores.Mundo.parseFloat(campos["porcentajeLugar"]);
			puerta.actualizarEspacio();

			puerta.activado = Convert.ToBoolean(campos["activado"]);
			puerta.tiempoActivacion = Convert.ToInt32(campos["tiempoActivacion"]);

			habitacionPrincipal.puertas.Add(puerta);
			habitacionSecundaria.puertas.Add(puerta);
			ruina.puertas.Add(puerta);

			puerta.generarNodosYRamas();
			puerta.rama.cambioHabitacion = true;

			return puerta;
		}


		public override ObjetoView crearVista(RuinaJugable ruina)
		{
			vista = new PuertaSalidaView(this, ruina);
			return vista;
		}
	}
}