using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class ObjetoMovibleView : ObjetoView
	{
		// constructor
		public ObjetoMovibleView(ObjetoMovible newObjeto, RuinaJugable ruina)
			: base(newObjeto, ruina)
		{
		}

		/*
		public void ejecutar(BoundingBox bpersona)
		{
			Vector3[] vector3objeto=new Vector3[8];
			Vector3[] vector3persona=new Vector3[8];

			vector3objeto = bObjeto.GetCorners();
			vector3persona = bpersona.GetCorners();

			if (bObjeto.Intersects(bpersona)) {
				
			}
		}*/
	}
}




