using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Programa
{
	

	public abstract partial class ListaView : InterfazGrafica
	{
		public abstract class Elemento : InterfazGrafica
		{
			// variables
			public bool seleccionado { get; private set; }


			// constructor
			public Elemento(bool seleccionar)
				: base()
			{
				seleccionado = seleccionar;
				onMouseOver = mouseActivate;
				onMouseOut = mouseDeactivate;
			}


			public override void updateContent()
			{
				requestedContentUpdate = false;
				border.updateContent();
			}


			public static void mouseActivate(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
			{
				Elemento elemento = ((Elemento)drawable);
				if(elemento.border == null)
					return;
				
				ushort newIndex = elemento.border.borderNumber;
				if(newIndex % 2 == 0)
					++newIndex;
				elemento.border.borderNumber = newIndex;
				elemento.border.updateContent();
			}


			public static void mouseDeactivate(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
			{
				Elemento elemento = ((Elemento)drawable);
				if(elemento.border == null)
					return;
				
				ushort newIndex = elemento.border.borderNumber;
				if(newIndex % 2 == 1)
					--newIndex;
				elemento.border.borderNumber = newIndex;
				elemento.border.updateContent();
			}


			public void select()
			{
				ushort newIndex = border.borderNumber;
				if((newIndex / 2) % 2 == 0)
					newIndex += 2;
				border.borderNumber = newIndex;
			}


			public void deselect()
			{
				ushort newIndex = border.borderNumber;
				if((newIndex / 2) % 2 == 1)
					newIndex -= 2;
				border.borderNumber = newIndex;
			}
		}
	}
}



