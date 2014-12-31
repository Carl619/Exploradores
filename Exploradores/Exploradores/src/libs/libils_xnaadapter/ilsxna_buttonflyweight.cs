using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
										ButtonFlyweight
	------------------------------------------------------------------------------------
	BorderFlyweight that stores common data for borders
	----------------------------------------------------------------------------------*/
	public class ButtonFlyweight : Gestores.IObjetoIdentificable
	{
		// public variables
		public String id { get; protected set; }
		public Border border { get; set; }
		public SpriteFont textFont { get; set; }
		public Color textColorPassive { get; set; }
		public Color textColorActive { get; set; }
		public Color textColorSelected { get; set; }
		public Color textColorActiveSelected { get; set; }
		public Color textColorDisabled { get; set; }
		public uint contentSpacingX { get; set; }
		public uint contentSpacingY { get; set; }


		// constructor
		public ButtonFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			border = null;
			textFont = null;
			textColorPassive = Color.Black;
			textColorActive = Color.Black;
			textColorSelected = Color.Black;
			textColorActiveSelected = Color.Black;
			textColorDisabled = Color.Black;
			contentSpacingX = 0;
			contentSpacingY = 0;
		}


		public ButtonFlyweight(String newID, ButtonFlyweight flyweight)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			border = flyweight.border;
			textFont = flyweight.textFont;
			textColorPassive = flyweight.textColorPassive;
			textColorActive = flyweight.textColorActive;
			textColorSelected = flyweight.textColorSelected;
			textColorActiveSelected = flyweight.textColorActiveSelected;
			textColorDisabled = flyweight.textColorDisabled;
			contentSpacingX = flyweight.contentSpacingX;
			contentSpacingY = flyweight.contentSpacingY;
		}


		// public functions
		public static ButtonFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			ButtonFlyweight buttonFlyweight;
			
			buttonFlyweight = new ButtonFlyweight(campos["id"]);
			buttonFlyweight.border = Gestores.Mundo.Instancia.borders[campos["id border"]];
			buttonFlyweight.textFont = Gestores.Gestor<ButtonFlyweight>.parseFont(campos["textFont"]);
			buttonFlyweight.textColorPassive = Gestores.Gestor<ButtonFlyweight>.parseColor(campos["textColorPassive"]);
			buttonFlyweight.textColorActive = Gestores.Gestor<ButtonFlyweight>.parseColor(campos["textColorActive"]);
			buttonFlyweight.contentSpacingX = Convert.ToUInt32(campos["contentSpacingX"]);
			buttonFlyweight.contentSpacingY = Convert.ToUInt32(campos["contentSpacingY"]);

			return buttonFlyweight;
		}


		public ButtonFlyweight clone()
		{
			return new ButtonFlyweight(null, this);
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


