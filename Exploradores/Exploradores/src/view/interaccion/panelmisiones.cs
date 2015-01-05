using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class PanelMisiones : InterfazGrafica
	{
		// variables
		public InterfazGrafica interfazMisiones { get; set; }


		// constructor
		public PanelMisiones(bool actualizarVista = true)
			: base()
		{
			interfazMisiones = null;

			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();
			
			alternativeNames.Add("activas", 0);
			alternativeNames.Add("terminadas", 1);
			alternativeNames.Add("fracasadas", 2);

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoHUD !=
				Gestores.Pantallas.EstadoHUD.Misiones)
				return;
			
			verBotonesOpciones();
			interfazMisiones = new InterfazGrafica();
			interfazMisiones.setNumberAlternatives(3);
			addComponent(interfazMisiones);

			Dictionary<String, Mision> misionesCompletas =
				Gestores.Partidas.Instancia.gestorMisiones.misiones;
			List<Mision> misionesActivas = new List<Mision>();
			List<Mision> misionesTerminadas = new List<Mision>();
			List<Mision> misionesFracasadas = new List<Mision>();
			foreach(KeyValuePair<String, Mision> mision in misionesCompletas)
			{
				if(mision.Value.estado == Mision.Estado.EnProgreso)
					misionesActivas.Add(mision.Value);
				else if(mision.Value.estado == Mision.Estado.Terminado)
					misionesTerminadas.Add(mision.Value);
				else if(mision.Value.estado == Mision.Estado.Fracasado)
					misionesFracasadas.Add(mision.Value);
			}
			cambiarAlternativa(Gestores.Pantallas.EstadoMisiones.Activas);
			verMisiones(misionesActivas, misionesActivas.Count > 0 ? "Misiones en progreso" : "No hay misiones en progreso.");
			cambiarAlternativa(Gestores.Pantallas.EstadoMisiones.Terminadas);
			verMisiones(misionesTerminadas, misionesTerminadas.Count > 0 ? "Misiones terminadas con exito" : "No hay misiones terminadas con exito.");
			cambiarAlternativa(Gestores.Pantallas.EstadoMisiones.Fracasadas);
			verMisiones(misionesFracasadas, misionesFracasadas.Count > 0 ? "Misiones fracasadas" : "No hay misiones fracasadas.");
			cambiarAlternativa(Gestores.Partidas.Instancia.gestorPantallas.estadoMisiones);


			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Programa.Controller.funcionCerrarMisiones;
			addComponent(boton);
		}


		public void cambiarAlternativa(Gestores.Pantallas.EstadoMisiones estadoMisiones)
		{
			String alt = "";
			if(estadoMisiones == Gestores.Pantallas.EstadoMisiones.Activas)
				alt = "activas";
			else if(estadoMisiones == Gestores.Pantallas.EstadoMisiones.Terminadas)
				alt = "terminadas";
			else if(estadoMisiones == Gestores.Pantallas.EstadoMisiones.Fracasadas)
				alt = "fracasadas";
			interfazMisiones.setCurrentAlternative(alternativeNames[alt]);
		}


		protected void verBotonesOpciones()
		{
			ILSXNA.Container botones = new ILSXNA.Container();
			addComponent(botones);

			ILSXNA.Button boton;
			Gestores.Pantallas.EstadoMisiones estado;

			boton = new ILSXNA.Button("En progreso", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["reloj"].textura);
			boton.updateContent();
			estado = Gestores.Pantallas.EstadoMisiones.Activas;
			boton.callbackConfigObj = estado;
			boton.onButtonPress = Programa.Controller.funcionVerCategoriaMisiones;
			botones.addComponent(boton);

			boton = new ILSXNA.Button("Terminadas", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["ok"].textura);
			boton.updateContent();
			estado = Gestores.Pantallas.EstadoMisiones.Terminadas;
			boton.callbackConfigObj = estado;
			boton.onButtonPress = Programa.Controller.funcionVerCategoriaMisiones;
			botones.addComponent(boton);

			boton = new ILSXNA.Button("Fracasadas", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			estado = Gestores.Pantallas.EstadoMisiones.Fracasadas;
			boton.callbackConfigObj = estado;
			boton.onButtonPress = Programa.Controller.funcionVerCategoriaMisiones;
			botones.addComponent(boton);
		}


		protected void verMisiones(List<Mision> misiones, String titulo)
		{
			Programa.ListaViewFlyweight flyweight = Gestores.Mundo.Instancia.listaViewFlyweights["list1"];
			
			ListaMisionView misionesView;
			misionesView = new ListaMisionView(misiones, flyweight);
			misionesView.cadenaTitulo = titulo;
			misionesView.mostrarFlechasVacio = misiones.Count > 0;
			misionesView.updateContent();
			interfazMisiones.addComponent(misionesView);
		}
	}
}



