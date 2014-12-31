using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapa;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class ObjetoMovible : Objeto
	{
		// constructor
		public ObjetoMovible(String newID, ObjetoFlyweight newObjetoFlyweight)
			: base(newID, newObjetoFlyweight)
		{
		}
	}
}




