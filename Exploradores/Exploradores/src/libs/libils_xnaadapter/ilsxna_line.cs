using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
											Line
	------------------------------------------------------------------------------------
	Line implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Line : ILS.Component<Texture2D>
	{
		// variables
		public Tuple<Tuple<int, int>, Tuple<int, int>> coordinates { get; set; }
		public Color lineColor { get; set; }
		public uint width { get; set; }


		// constructors
		public Line(ILS.Layer newParent = null) : base(newParent)
		{
			coordinates = new Tuple<Tuple<int, int>, Tuple<int, int>>(
								new Tuple<int, int>(0, 0), new Tuple<int, int>(0, 0));
			lineColor = Color.White;
			width = 1;
		}


		public Line(Line line, ILS.Layer newParent = null) : base(newParent)
		{
			if(line != null)
				innerComponent = line.innerComponent;
		}
		

		// public functions
		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new Line(this, newParent);
		}


		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			if(innerComponent == null)
				return;
			

			int offsetX = (getDrawPositionX() - coordinates.Item1.Item1);
			if(offsetX > (getDrawPositionX() - coordinates.Item2.Item1))
				offsetX = (getDrawPositionX() - coordinates.Item2.Item1);
			int offsetY = (getDrawPositionY() - coordinates.Item1.Item2);
			if(offsetY > (getDrawPositionY() - coordinates.Item2.Item2))
				offsetY = (getDrawPositionY() - coordinates.Item2.Item2);
			int x1 = offsetX + coordinates.Item1.Item1;
			int x2 = offsetX + coordinates.Item2.Item1;
			int y1 = offsetY + coordinates.Item1.Item2;
			int y2 = offsetY + coordinates.Item2.Item2;

			Vector2 start = new Vector2(x1, y1);
			Vector2 end = new Vector2(x2, y2);

			Vector2 edge = end - start;
			// calculate angle to rotate line
			float angle =
				(float)Math.Atan2(edge.Y , edge.X);
			
			
			SpriteBatch spriteBatch = ((Window)renderSurface).innerWindow.spriteBatch;
			spriteBatch.Draw(innerComponent,
				new Rectangle(// rectangle defines shape of line and position of start of line
					(int)start.X,
					(int)start.Y,
					(int)edge.Length(), //sb will strech the texture to fill this rectangle
					(int)width), //width of line, change this to make thicker line
				null,
				lineColor, //colour of line
				angle,     //angle of line (calculated above)
				new Vector2(0, 0), // point in line about which to rotate
				SpriteEffects.None,
				0);
		}


		// protected functions
		protected override uint getMinInnerUnconstrainedWidth()
		{
			return 1;
		}
		
		
		protected override uint getMinInnerUnconstrainedHeight()
		{
			return 1;
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


