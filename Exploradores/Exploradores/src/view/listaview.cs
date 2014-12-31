using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public abstract partial class ListaView : InterfazGrafica
	{
		// delegados
		public delegate void SeleccionElemento(Object infoObj, Object elemento);


		// variables
		public List<ListaView.Elemento> listaElementos { get; set; }
		public ILSXNA.Container titulo { get; protected set; }
		public ListaView.Elemento cabecera { get; set; }
		public ListaViewFlyweight flyweight { get; set; }
		public Object selectCallbackObject { get; set; }
		public SeleccionElemento onElementSelect { get; set; }
		public SeleccionElemento onElementDoubleClick { get; set; }
		public uint maxRowsPerPage { get; set; }
		public uint paginaActual { get; protected set; }
		public uint paginasTotales { get; protected set; }
		public bool mostrarFlechasVacio { get; set; }
		public ILSXNA.Container contenedorElementos { get; protected set; }
		protected ILSXNA.MultiSprite flechaDerecha { get; set; }
		protected ILSXNA.MultiSprite flechaIzquierda { get; set; }


		// constructor
		public ListaView(List<ListaView.Elemento> newComponentes, ListaView.Elemento newCabecera, ListaViewFlyweight newFlyweight, bool actualizarVista)
			: base()
		{
			listaElementos = newComponentes;
			titulo = new ILSXNA.Container();
			cabecera = newCabecera;
			flyweight = newFlyweight;
			selectCallbackObject = null;
			onElementSelect = null;
			onElementDoubleClick = null;

			maxRowsPerPage = 10;
			paginaActual = 0;
			if(listaElementos != null)
				paginasTotales = (uint)(listaElementos.Count / maxRowsPerPage);
			else
				paginasTotales = 0;
			mostrarFlechasVacio = false;
			flechaDerecha = null;
			flechaIzquierda = null;
			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;

			contenedorElementos = new ILSXNA.Container();
			contenedorElementos.layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			contenedorElementos.layout.equalCellWidth = true;
			contenedorElementos.layout.equalCellHeight = true;

			if(actualizarVista == false || listaElementos == null || flyweight == null)
				return;
			
			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			clearComponents();
			contenedorElementos.clearComponents();
			requestedContentUpdate = false;

			long nElementos;
			if(getNElementos(out nElementos) == true)
				return;
			
			mostrarPanelNavegacion(nElementos);
			if(cabecera != null)
				addComponent(cabecera);
			mostrarElementos(nElementos);
		}


		protected bool getNElementos(out long nElementos)
		{
			nElementos = 0;

			if(listaElementos == null || flyweight == null)
				return false;
			
			paginasTotales = (uint) (listaElementos.Count / maxRowsPerPage);
			if(listaElementos.Count % maxRowsPerPage > 0)
				++paginasTotales;
			
			if(nElementos > listaElementos.Count - paginaActual * maxRowsPerPage)
				nElementos = listaElementos.Count - paginaActual * maxRowsPerPage;
			
			nElementos = (long)maxRowsPerPage;
			if(nElementos <= 0)
			{
				if(paginaActual == 0)
					return false;
				navegarPaginaAnterior();
				return true;
			}

			return false;
		}


		protected void mostrarPanelNavegacion(long nElementos)
		{
			if(mostrarFlechasVacio == false && existePaginaAnterior() == false && existePaginaSiguiente() == false)
			{
				if(titulo != null)
					addComponent(titulo);
				return;
			}
			
			if(listaElementos == null || flyweight == null)
				return;
			
			ILSXNA.MultiSprite flecha;
			ILSXNA.Label label;
			SpriteFont font = flyweight.spriteFont;
			if(font == null)
				font = Gestores.Mundo.Instancia.fuentes["spriteFontAusente"];
			
			ILSXNA.Container panelNavegacion = new ILSXNA.Container();
			addComponent(panelNavegacion);
			
			if(existePaginaAnterior() == true)
			{
				flecha = flyweight.flechaIzquierdaActivada.clone();
				flecha.callbackConfigObj = this;
				flecha.onMousePress = activacionFlecha;
				flecha.onMouseRelease = null;
			}
			else
			{
				flecha = flyweight.flechaIzquierdaDesactivada.clone();
				flecha.onMousePress = null;
				flecha.onMouseRelease = null;
			}
			flechaIzquierda = null;
			if(mostrarFlechasVacio == true || existePaginaAnterior() == true || existePaginaSiguiente() == true)
			{
				panelNavegacion.addComponent(flecha);
				flechaIzquierda = flecha;
			}

			label = new ILSXNA.Label();
			uint pagActual = paginasTotales > 0 ? paginaActual + 1 : 0;
			label.message = pagActual.ToString("D") + " / " + paginasTotales.ToString("D");
			label.color = flyweight.colorNumeroPaginas;
			label.innerComponent = font;
			panelNavegacion.addComponent(label);
			

			if(existePaginaSiguiente() == true)
			{
				flecha = flyweight.flechaDerechaActivada.clone();
				flecha.callbackConfigObj = this;
				flecha.onMousePress = activacionFlecha;
				flecha.onMouseRelease = null;
			}
			else
			{
				flecha = flyweight.flechaDerechaDesactivada.clone();
				flecha.onMousePress = null;
				flecha.onMouseRelease = null;
			}
			flechaDerecha = flecha;
			panelNavegacion.addComponent(flecha);

			panelNavegacion.addComponent(titulo);
		}


		protected void mostrarElementos(long nElementos)
		{
			long nElementosAnteriores = paginaActual * maxRowsPerPage;
			int index = 0;

			addComponent(contenedorElementos);
			
			foreach(ListaView.Elemento comp in listaElementos)
			{
				if(index < nElementosAnteriores)
				{
					++index;
					comp.onMousePress = null;
					continue;
				}
				if(index >= nElementosAnteriores + nElementos)
					break;
				
				comp.updateContent();
				comp.onMousePress = seleccionElemento;
				comp.onMouseDoubleClick = seleccionDobleElemento;
				comp.callbackConfigObj = this;
				contenedorElementos.addComponent(comp);
				++index;
			}
		}


		public bool existePaginaSiguiente()
		{
			if(paginaActual + 1 < paginasTotales)
				return true;
			return false;
		}


		public bool existePaginaAnterior()
		{
			if(paginaActual > 0)
				return true;
			return false;
		}


		public void navegarPaginaSiguiente()
		{
			if(existePaginaSiguiente() == true)
			{
				++paginaActual;
				requestUpdateContent();
			}
		}


		public void navegarPaginaAnterior()
		{
			if(existePaginaAnterior() == true)
			{
				--paginaActual;
				requestUpdateContent();
			}
		}


		public void seleccionarElemento(Elemento elemento, bool doble)
		{
			guardarReferenciaSeleccion(elemento);
			requestUpdateContent();
			if(doble == false && onElementSelect != null)
				onElementSelect(selectCallbackObject, elemento);
			else if(doble == true && onElementDoubleClick != null)
				onElementDoubleClick(selectCallbackObject, elemento);
		}


		public void deseleccionarElemento(Elemento elemento)
		{
			guardarReferenciaSeleccion(elemento);
			requestUpdateContent();
		}


		public abstract void guardarReferenciaSeleccion(Elemento elemento);


		protected static void activacionFlecha(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			ListaView vistaLista = (ListaView)configObj;
			ILSXNA.MultiSprite flecha = (ILSXNA.MultiSprite)drawable;

			if(flecha == vistaLista.flechaDerecha)
				vistaLista.navegarPaginaSiguiente();
			if(flecha == vistaLista.flechaIzquierda)
				vistaLista.navegarPaginaAnterior();
		}


		protected static void seleccionElemento(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Elemento elementoSeleccionado = (Elemento)drawable;
			ListaView listaView = (ListaView)configObj;

			listaView.seleccionarElemento(elementoSeleccionado, false);
		}


		protected static void seleccionDobleElemento(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Elemento elementoSeleccionado = (Elemento)drawable;
			ListaView listaView = (ListaView)configObj;

			listaView.seleccionarElemento(elementoSeleccionado, true);
		}
	}
}




