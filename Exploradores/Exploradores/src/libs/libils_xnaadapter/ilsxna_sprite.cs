using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
											Sprite
	------------------------------------------------------------------------------------
	Sprite implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Sprite : ILS.Component<Texture2D>
	{
		// enums
		public enum DisplayMode
		{
			Normal,
			Center,
			Fit,
			Stretch,
			Repeat
		}


		// variables
		public int displayOffsetX { get; set; }
		public int displayOffsetY { get; set; }
		public DisplayMode displayModeWidth { get; set; }
		public DisplayMode displayModeHeight { get; set; }
		public float opacity { get; set; }


		// constructors
		public Sprite(ILS.Layer newParent = null) : base(newParent)
		{
			displayOffsetX = 0;
			displayOffsetY = 0;
			displayModeWidth = DisplayMode.Normal;
			displayModeHeight = DisplayMode.Normal;
			opacity = 1.0f;
		}


		public Sprite(Sprite sprite, ILS.Layer newParent = null) : base(sprite, newParent)
		{
			if(sprite != null)
				innerComponent = sprite.innerComponent;
			displayOffsetX = sprite.displayOffsetX;
			displayOffsetY = sprite.displayOffsetY;
			displayModeWidth = sprite.displayModeWidth;
			displayModeHeight = sprite.displayModeHeight;
			opacity = sprite.opacity;
		}
		

		// public functions
		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new Sprite(this, newParent);
		}
		
		
		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			if(innerComponent == null)
				return;
			
			if(displayModeWidth != DisplayMode.Normal && displayModeWidth != DisplayMode.Stretch && displayModeWidth != DisplayMode.Repeat)
				throw new ArgumentException();
			if(displayModeHeight != DisplayMode.Normal && displayModeHeight != DisplayMode.Stretch && displayModeHeight != DisplayMode.Repeat)
				throw new ArgumentException();
			
			int opacityAmount = (int)(255.0f * opacity);
			if(opacityAmount < 0)
				opacityAmount = 0;
			if(opacityAmount > 255)
				opacityAmount = 255;
			Color opacityColor = new Color(255, 255, 255, opacityAmount);


			SpriteBatch spriteBatch = ((Window)renderSurface).innerWindow.spriteBatch;

			Vector2 position = new Vector2(dimensions.positionX + displayOffsetX,
											dimensions.positionY + displayOffsetY);
			float scaleX, scaleY;
			if(innerComponent.Width > 0 && displayModeWidth == DisplayMode.Stretch)
				scaleX = ((float)getFinalWidth()) / (float)innerComponent.Width;
			else
				scaleX = 1.0f;
			if(innerComponent.Height > 0 && displayModeHeight == DisplayMode.Stretch)
				scaleY = ((float)getFinalHeight()) / (float)innerComponent.Height;
			else
				scaleY = 1.0f;
			
			Vector2 scale = new Vector2(scaleX, scaleY);

			if(displayModeWidth != DisplayMode.Repeat && displayModeHeight != DisplayMode.Repeat)
			{
				spriteBatch.Draw(innerComponent, position, null,
								opacityColor, 0, Vector2.Zero,
								scale, SpriteEffects.None, 0.0f);
			}
			else
			{
				Rectangle remainingPart;
				int displayX, displayY;
				displayX = (int)(scaleX * (float)innerComponent.Width);
				displayY = (int)(scaleY * (float)innerComponent.Height);

				int x = 0, y = 0;
				for(; x + displayX <= getFinalWidth(); x += displayX)
				{
					for(y = 0; y + displayY <= getFinalHeight(); y += displayY)
					{
						position = new Vector2(dimensions.positionX + displayOffsetX + x,
											dimensions.positionY + displayOffsetY + y);
						spriteBatch.Draw(innerComponent, position, null,
										opacityColor, 0, Vector2.Zero,
										scale, SpriteEffects.None, 0.0f);
						if(displayModeHeight != DisplayMode.Repeat)
							break;
					}
					if(y < getFinalHeight() && displayModeHeight == DisplayMode.Repeat)
					{
						remainingPart = new Rectangle(0, 0, innerComponent.Width, (int)(getFinalHeight() - y));
						position = new Vector2(dimensions.positionX + displayOffsetX + x,
											dimensions.positionY + displayOffsetY + y);
						spriteBatch.Draw(innerComponent, position, remainingPart,
										opacityColor, 0, Vector2.Zero,
										scale, SpriteEffects.None, 0.0f);
					}
				}

				if(x < getFinalWidth() && displayModeWidth == DisplayMode.Repeat)
				{
					remainingPart = new Rectangle(0, 0, (int)(getFinalWidth() - x), innerComponent.Height);

					for(y = 0; y + displayY <= getFinalHeight(); y += displayY)
					{
						position = new Vector2(dimensions.positionX + displayOffsetX + x,
											dimensions.positionY + displayOffsetY + y);
						spriteBatch.Draw(innerComponent, position, remainingPart,
										opacityColor, 0, Vector2.Zero,
										scale, SpriteEffects.None, 0.0f);
						if(displayModeHeight != DisplayMode.Repeat)
							break;
					}
					if(y < getFinalHeight() && displayModeHeight == DisplayMode.Repeat)
					{
						remainingPart = new Rectangle(0, 0, (int)(getFinalWidth() - x), (int)(getFinalHeight() - y));
						position = new Vector2(dimensions.positionX + displayOffsetX + x,
											dimensions.positionY + displayOffsetY + y);
						spriteBatch.Draw(innerComponent, position, remainingPart,
										opacityColor, 0, Vector2.Zero,
										scale, SpriteEffects.None, 0.0f);
					}
				}
			}
		}


		// protected functions
		protected override uint getMinInnerUnconstrainedWidth()
		{
			return getAdjustedMinWidth((uint)innerComponent.Width);
		}
		
		
		protected override uint getMinInnerUnconstrainedHeight()
		{
			return getAdjustedMinHeight((uint)innerComponent.Height);
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


