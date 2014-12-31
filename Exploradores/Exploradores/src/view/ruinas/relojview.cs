using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Ruinas
{
	

	public class RelojView : ILSXNA.Sprite
	{
		// variables
		public Reloj reloj { get; set; }
		public bool actualizacionContinua { get; set; }


		// constructor
		public RelojView(Reloj newReloj)
			: base()
		{
			if (newReloj == null)
				throw new ArgumentNullException();

			reloj = newReloj;
			actualizacionContinua = true;

			updateContent();
		}


		// funciones
		public override void updateContent()
		{
			requestedContentUpdate = actualizacionContinua;
			innerComponent = reloj.getImagenActual();
			if(getParent() != null)
				getParent().requestUpdateContent();
		}
	}
}




