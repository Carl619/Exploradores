using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class AreaActivacion
	{
		// variables
		public String id { get { return objeto.id; } }
		public Objeto objeto { get; set; }
		public Rectangle espacio { get; set; }
        public bool activado { get; set; }


		// constructor
        public AreaActivacion()
		{
			objeto = null;
			espacio = new Rectangle(0, 0, 1, 1);
			activado = false;
		}
	}
}




