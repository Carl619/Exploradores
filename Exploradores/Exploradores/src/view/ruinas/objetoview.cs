using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;




namespace Ruinas
{
	
	
	public class ObjetoView : ILSXNA.MultiSprite
	{
		// variables
		public Objeto objeto { get; set; }
		public Rectangle boundingBox { get { return objeto.espacio;  } }


		// constructor
		public ObjetoView(Objeto newobjeto, RuinaJugable ruina)
			: base()
		{
			if (newobjeto == null)
				throw new ArgumentNullException();
			
			objeto = newobjeto;
			innerComponent.Add(objeto.objetoFlyweight.iconoPasivo);
			innerComponent.Add(objeto.objetoFlyweight.iconoActivo);
			innerComponent.Add(objeto.objetoFlyweight.iconoSeleccionado);
			innerComponent.Add(objeto.objetoFlyweight.iconoActivoSeleccionado);
			onMousePress = null;
			onMouseRelease = null;
			if(objeto.activado == true)
				select();
		}


		// funciones
		public override void updateContent()
		{
			base.updateContent();
			deselect();
			if(objeto.activado == true)
				select();
		}
	}
}




