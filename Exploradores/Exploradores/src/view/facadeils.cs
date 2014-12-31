using System;
using System.Collections.Generic;




public class InterfazGrafica : ILSXNA.Container
{
	// constructor
	public InterfazGrafica(ILSXNA.Border border = null, ILS.Layer newParent = null) : base(border, newParent)
	{
	}


	public InterfazGrafica(InterfazGrafica interfaz, ILS.Layer newParent = null) : base(interfaz, newParent)
	{
	}


	// funciones
	public override ILS.BaseComponent clone(ILS.Layer newParent = null)
	{
		return new InterfazGrafica(this, newParent);
	}
}




