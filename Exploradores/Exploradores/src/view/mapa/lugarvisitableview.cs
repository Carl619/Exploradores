using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace Mapa
{
	

	public class LugarVisitableView : ILSXNA.MultiSprite
	{
		// variables
		public LugarVisitable lugar { get; set; }


		// constructor
		public LugarVisitableView(LugarVisitable newLugar)
			: base()
		{
			if(newLugar == null)
				throw new ArgumentNullException();
			
			lugar = newLugar;
			onMouseDoubleClick = Controller.accionLugarVisitable;
		}
	}
}




