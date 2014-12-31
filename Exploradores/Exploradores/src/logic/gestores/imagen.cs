using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;




namespace Gestores
{
	
	
	public class Imagen
	{
		// variables
		public String path { get; set; }
		public Texture2D textura { get; set; }


		// constructor
		public Imagen()
		{
			path = "";
			textura = null;
		}
	}

}




