using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
										BorderFlyweight
	------------------------------------------------------------------------------------
	BorderFlyweight that stores common data for borders
	----------------------------------------------------------------------------------*/
	public class BorderFlyweight : Gestores.IObjetoIdentificable
	{
		// public variables
		public static String pathBorders = "images/borders";
		public static uint imagesPerBorder = 9;
		public String id { get; protected set; }
		public List<List<Texture2D>> borders { get; protected set; }
		public uint borderWidth { get; set; }
		public uint borderHeight { get; set; }
		public uint innerMargin { get; set; }
		public Sprite.DisplayMode backgroundMode { get; set; }
		public Sprite.DisplayMode borderMode { get; set; }


		// constructor
		public BorderFlyweight(String newID)
		{
			if(newID == null)
				throw new ArgumentNullException();
			id = newID;
			borders = new List<List<Texture2D>>();
			borderWidth = 0;
			borderHeight = 0;
			innerMargin = 0;
			backgroundMode = Sprite.DisplayMode.Stretch;
			borderMode = Sprite.DisplayMode.Stretch;
		}


		public BorderFlyweight(BorderFlyweight flyweight)
		{
			borders = new List<List<Texture2D>>();
			if (flyweight == null)
				return;
			
			foreach(List<Texture2D> textureList in flyweight.borders)
			{
				List<Texture2D> list = new List<Texture2D>();
				foreach(Texture2D texture in textureList)
					list.Add(texture);
				borders.Add(list);
			}
			
			borderWidth = flyweight.borderWidth;
			borderHeight = flyweight.borderHeight;
			innerMargin = flyweight.innerMargin;
			backgroundMode = flyweight.backgroundMode;
			borderMode = flyweight.borderMode;
		}

		

		// public functions
		public static BorderFlyweight cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			BorderFlyweight borderFlyweight;

			String path = BorderFlyweight.pathBorders + "/" + campos["carpeta"];
			borderFlyweight = BorderFlyweight.cargar(campos["id"], path, Convert.ToUInt32(campos["nSubCarpetas"]));
			borderFlyweight.innerMargin = Convert.ToUInt32(campos["innerMargin"]);

			Sprite.DisplayMode modo;
			modo = (Sprite.DisplayMode) Enum.Parse(
						typeof(Sprite.DisplayMode),
						campos["backgroundMode"]);
			borderFlyweight.backgroundMode = modo;
			modo = (Sprite.DisplayMode) Enum.Parse(
						typeof(Sprite.DisplayMode),
						campos["borderMode"]);
			borderFlyweight.borderMode = modo;
			return borderFlyweight;
		}


		public static BorderFlyweight cargar(String newID, String path, uint versions)
		{
			ILSXNA.BorderFlyweight borderFlyweight;
			List<Texture2D> listaTexturas;
			Texture2D textura;
			
			borderFlyweight = new ILSXNA.BorderFlyweight(newID);
			
			for(uint i=0; i<versions; ++i)
			{
				listaTexturas = new List<Texture2D>();
				String versionPath = path + "/" + i.ToString();
				for(uint j=0; j<imagesPerBorder; ++j)
				{
					String completePath = versionPath + "/" + j.ToString();
					textura = Programa.Exploradores.Instancia.Content.Load<Texture2D>(@completePath);
					listaTexturas.Add(textura);
				}
				borderFlyweight.borders.Add(listaTexturas);
				borderFlyweight.borderWidth = (uint)listaTexturas[3].Width;
				borderFlyweight.borderHeight = (uint)listaTexturas[1].Height;
			}

			return borderFlyweight;
		}


		public BorderFlyweight clone()
		{
			return new BorderFlyweight(this);
		}

		
		public uint getSingleBorderWidth()
		{
			return borderWidth;
		}


		public uint getSingleBorderHeight()
		{
			return borderHeight;
		}


		public uint getTotalBorderWidth()
		{
			return 2 * borderWidth;
		}


		public uint getTotalBorderHeight()
		{
			return 2 * borderHeight;
		}


		public uint getSingleInnerSpacing()
		{
			return innerMargin;
		}


		public uint getTotalInnerSpacing()
		{
			return 2 * innerMargin;
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


