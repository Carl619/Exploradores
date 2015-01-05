using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Gestores
{
	
	
	public class Pantallas
	{
		// enumeraciones
		public enum EstadoJuego
		{
			MenuPrincipal,
			Jugando,
			Exito,
			Fracaso
		}
		public enum EstadoPartida
		{
			Mapa,
			Ciudad,
			Ruina
		}
		public enum EstadoMenu
		{
			VistaPrincipal,
			VistaOpciones,
			Invisible
		}
		public enum EstadoInformacion
		{
			Visible,
			Invisible
		}
		public enum EstadoHUD
		{
			Vacio,
			Viaje,
			Misiones,
			Personajes,
			Inventario,
			Edificio,
			Dialogo
		}
		public enum EstadoInteraccion
		{
			Vacio,
			Comercio,
			Defensa
		}
		public enum EstadoCaracteristica
		{
			Vacio,
			Atributo,
			Habilidad
		}
		public enum EstadoMisiones
		{
			Activas,
			Terminadas,
			Fracasadas
		}


		// variables
		public EstadoJuego estadoJuego { get; set; }
		public EstadoPartida estadoPartida { get; set; }
		public EstadoMenu estadoMenu { get; set; }
		public EstadoInformacion estadoInformacion { get; set; }
		public EstadoHUD estadoHUD { get; set; }
		public EstadoInteraccion estadoInteraccion { get; set; }
		public EstadoCaracteristica estadoCaracteristica { get; set; }
		public EstadoMisiones estadoMisiones { get; set; }


		// constructor
		public Pantallas()
		{
			estadoJuego = EstadoJuego.MenuPrincipal;
			estadoPartida = EstadoPartida.Mapa;
			estadoMenu = EstadoMenu.VistaPrincipal;
			estadoInformacion = EstadoInformacion.Invisible;
			estadoHUD = EstadoHUD.Vacio;
			estadoInteraccion = EstadoInteraccion.Vacio;
			estadoCaracteristica = EstadoCaracteristica.Vacio;
			estadoMisiones = EstadoMisiones.Activas;
		}
	}
}




