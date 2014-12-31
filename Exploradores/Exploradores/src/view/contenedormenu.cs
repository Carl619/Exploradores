using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;




namespace Programa
{
	
	
	public class ContenedorMenu : InterfazGrafica
	{
		// variables
		public MenuJuego menuPrincipal { get; protected set; }


		// constructor
		public ContenedorMenu(bool actualizarVista = true)
			: base()
		{
			menuPrincipal = null;

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents(true);
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoJuego != Gestores.Pantallas.EstadoJuego.MenuPrincipal)
				return;
			
			menuPrincipal = new MenuJuego();
			addComponent(menuPrincipal);
		}
	}
}




