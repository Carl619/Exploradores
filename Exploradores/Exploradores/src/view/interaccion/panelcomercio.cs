using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Interaccion
{
	

	public class PanelComercio : InterfazGrafica
	{
		// variables
		public Objetos.InventarioView inventarioProtagonista { get; protected set; }
		public Objetos.InventarioView inventarioNPC { get; protected set; }
		public Objetos.InventarioView inventarioVenta { get; protected set; }
		public Objetos.InventarioView inventarioCompra { get; protected set; }
		public ILSXNA.Container header { get; protected set; }
		public int diferenciaDinero { get; protected set; }
		public int dineroFinal { get; protected set; }
		public uint alturaInventarios { get; set; }
		public uint alturaInventariosCompraVenta { get; set; }
		protected double tasaVenta { get; set; }
		protected double tasaCompra { get; set; }


		// constructor
		public PanelComercio(bool actualizarVista = true)
			: base()
		{
			inventarioProtagonista = null;
			inventarioNPC = null;
			inventarioVenta = null;
			inventarioCompra = null;
			header = null;
			diferenciaDinero = 0;
			dineroFinal = 0;
			alturaInventarios = 150;
			alturaInventariosCompraVenta = 102;

			contentSpacingX = 4;
			contentSpacingY = 4;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			border = (ILSXNA.Border)Gestores.Mundo.Instancia.borders["border1"].clone();

			if(actualizarVista == true)
				updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			if(Gestores.Partidas.Instancia.gestorPantallas.estadoInteraccion !=
				Gestores.Pantallas.EstadoInteraccion.Comercio)
				return;
			
			String idNPC = Gestores.Partidas.Instancia.npcSeleccionado;
			Personajes.NPC npc = Gestores.Partidas.Instancia.npcs[idNPC];
			if(npc.inventario == null)
				return;
			
			actualizarTasas();
			
			header = new ILSXNA.Container();
			addComponent(header);

			inventarioProtagonista = mostrarInventario(
							Programa.Jugador.Instancia.protagonista.inventario,
							"Tu inventario:",
							4, alturaInventarios, true);
			inventarioNPC = mostrarInventario(
							npc.inventario,
							"Inventario de " + npc.nombre + ":",
							4, alturaInventarios, false);
			inventarioVenta = mostrarInventario(
							Gestores.Partidas.Instancia.inventarios["idVentas"],
							"Para vender (x" + tasaVenta.ToString("F4") + "):",
							2, alturaInventariosCompraVenta, false);
			inventarioCompra = mostrarInventario(
							Gestores.Partidas.Instancia.inventarios["idCompras"],
							"Para comprar (x" + tasaCompra.ToString("F4") + "):",
							2, alturaInventariosCompraVenta, false);
			
			inventarioProtagonista.selectCallbackObject =
				new Tuple<Objetos.InventarioView, Objetos.InventarioView>(
					inventarioProtagonista, inventarioVenta
				);
			inventarioNPC.selectCallbackObject =
				new Tuple<Objetos.InventarioView, Objetos.InventarioView>(
					inventarioNPC, inventarioCompra
				);
			inventarioVenta.selectCallbackObject =
				new Tuple<Objetos.InventarioView, Objetos.InventarioView>(
					inventarioVenta, inventarioProtagonista
				);
			inventarioCompra.selectCallbackObject =
				new Tuple<Objetos.InventarioView, Objetos.InventarioView>(
					inventarioCompra, inventarioNPC
				);
			
			actualizarCabecera();
		}


		public void actualizarTasas()
		{
			String idNPC = Gestores.Partidas.Instancia.npcSeleccionado;
			Personajes.NPC npc = Gestores.Partidas.Instancia.npcs[idNPC];
			if(npc.inventario == null)
				return;
			
			tasaVenta = Programa.Jugador.Instancia.getTasaVentas();
			tasaCompra = Programa.Jugador.Instancia.getTasaCompras();
			
			Personajes.Habilidad habilidad;
			if(npc.habilidades.TryGetValue("idComerciante", out habilidad) == true)
			{
				if(habilidad.GetType() != typeof(Personajes.Comerciante))
					throw new ArgumentException();
				Personajes.Comerciante comerciante = (Personajes.Comerciante)habilidad;
				// compra comerciante = venta protagonista
				tasaVenta *= comerciante.tasaCompras;
				tasaCompra *= comerciante.tasaVentas;
			}
			if(tasaVenta > tasaCompra)
			{
				double media = (tasaVenta + tasaCompra) / 2;
				tasaVenta = media;
				tasaCompra = media;
			}
		}

		
		public void actualizarCabecera()
		{
			header.clearComponents();

			String idNPC = Gestores.Partidas.Instancia.npcSeleccionado;
			Personajes.NPC npc = Gestores.Partidas.Instancia.npcs[idNPC];
			
			actualizarTasas();

			String etiquetaBalance = getEtiquetaBalance();
			ILSXNA.Button boton;

			boton = new ILSXNA.Button("Cerrar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			boton.icons.Add(Gestores.Mundo.Instancia.imagenes["cancel"].textura);
			boton.updateContent();
			boton.onButtonPress = Mapa.Controller.cerrarComercio;
			header.addComponent(boton);

			boton = new ILSXNA.Button("Intercambiar", Gestores.Mundo.Instancia.buttonFlyweights["button1"]);
			if(dineroFinal < 0)
			{
				boton.icons.Add(Gestores.Mundo.Instancia.imagenes["okDeshabilitado"].textura);
				boton.enabled = false;
			}
			else
			{
				boton.icons.Add(Gestores.Mundo.Instancia.imagenes["ok"].textura);
				boton.enabled = true;
			}
			boton.updateContent();
			boton.onButtonPress = Mapa.Controller.realizarIntercambio;
			header.addComponent(boton);
			
			ILSXNA.Label label;

			label = new ILSXNA.Label();
			label.message = "Comercio con " + npc.nombre;
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			label.minLateralSpacing = 16;
			header.addComponent(label);

			label = new ILSXNA.Label();
			label.message = etiquetaBalance;
			label.color = Gestores.Mundo.Instancia.colores["headerColor"];
			label.innerComponent = Gestores.Mundo.Instancia.fuentes["genericSpriteFont"];
			label.minLateralSpacing = 16;
			header.addComponent(label);
		}


		protected String getEtiquetaBalance()
		{
			int dineroActual;

			Objetos.ColeccionArticulos articulos;
			if(inventarioProtagonista.inventario.articulos.TryGetValue(
				Objetos.Articulo.idDinero, out articulos) == false)
				dineroActual = 0;
			else
				dineroActual = (int)articulos.cantidad;
			
			diferenciaDinero = (int)Math.Ceiling(((double)inventarioVenta.inventario.valor) * tasaVenta -
								((double)inventarioCompra.inventario.valor) * tasaCompra);
			dineroFinal = dineroActual + diferenciaDinero;

			String etiqueta1, etiqueta2;
			etiqueta1 = diferenciaDinero < 0 ?
								" - " + (-diferenciaDinero).ToString() :
								" + " + diferenciaDinero.ToString();
			etiqueta2 = dineroFinal < 0 ?
								"- " + (-dineroFinal).ToString() :
								dineroFinal.ToString();
			return "Dinero final: " + dineroActual.ToString() + etiqueta1 + " = " + etiqueta2;
		}

		
		public Objetos.InventarioView mostrarInventario(Objetos.Inventario inventario, String cabecera,
													uint maxRowsPerPage, uint altura, bool mostrarMax)
		{
			ILSXNA.Container contenedor = new ILSXNA.Container(Gestores.Mundo.Instancia.borders["border2"]);
			contenedor.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			contenedor.sizeSettings.minInnerHeight = altura;
			contenedor.contentSpacingX = 4;
			addComponent(contenedor);
			
			Objetos.InventarioView inventarioView = inventario.crearVista();
			inventarioView.textoTitulo = cabecera;
			inventarioView.mostrarEspacioMax = mostrarMax;
			inventarioView.onElementSelect = Mapa.Controller.clickArticulo;
			inventarioView.onElementDoubleClick = Mapa.Controller.doubleClickArticulo;
			inventarioView.maxRowsPerPage = maxRowsPerPage;
			inventarioView.updateContent();
			contenedor.addComponent(inventarioView);

			return inventarioView;
		}
	}
}



