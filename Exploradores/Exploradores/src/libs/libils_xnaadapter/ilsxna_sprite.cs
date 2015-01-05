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
		public Container scrollableContainer { get; set; }
		public int displayOffsetX { get; set; }
		public int displayOffsetY { get; set; }
		public DisplayMode displayModeWidth { get; set; }
		public DisplayMode displayModeHeight { get; set; }
		public float opacity { get; set; }


		// constructors
		public Sprite(ILS.Layer newParent = null) : base(newParent)
		{
			scrollableContainer = null;
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
			//Color opacityColor = Color.White * ((float)opacityAmount / 255.0f);

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
			
			
			float texturePartX, texturePartY;
			if(getMinOutterUnconstrainedWidth() > 0 && displayModeWidth == DisplayMode.Stretch)
				texturePartX = ((float)getFinalWidth()) / (float)getMinOutterUnconstrainedWidth();
			else
				texturePartX = 1.0f;
			if(getMinOutterUnconstrainedHeight() > 0 && displayModeHeight == DisplayMode.Stretch)
				texturePartY = ((float)getFinalHeight()) / (float)getMinOutterUnconstrainedHeight();
			else
				texturePartY = 1.0f;
			

			if(displayModeWidth == DisplayMode.Stretch && displayModeHeight == DisplayMode.Stretch)
			{
				if(getFinalWidth() > innerComponent.Width && getFinalHeight() > innerComponent.Height)
				{
					Rectangle remainingPart;
					float w, h;
					w = texturePartX * (float)innerComponent.Width;
					h = texturePartY * (float)innerComponent.Height;
					float stretchScaleX, stretchScaleY;
					if(w > 0.0f)
						stretchScaleX = ((float)getFinalWidth()) / w;
					else
						stretchScaleX = 1.0f;
					if(h > 0.0f)
						stretchScaleY = ((float)getFinalHeight()) / h;
					else
						stretchScaleY = 1.0f;
					Vector2 stretchScale = new Vector2(stretchScaleX, stretchScaleY);
					remainingPart = new Rectangle((int)(texturePartX * (float)dimensions.drawSpace.offsetLeft),
												(int)(texturePartY * (float)dimensions.drawSpace.offsetTop),
												(int)w,
												(int)h);
					spriteBatch.Draw(innerComponent, position, remainingPart,
									opacityColor, 0, Vector2.Zero,
									stretchScale, SpriteEffects.None, 0.0f);
				}
				else
				{
					Rectangle remainingPart;
					Vector2 stretchScale = scale;
					remainingPart = new Rectangle(0, 0,
												(int)innerComponent.Width,
												(int)innerComponent.Height);
					spriteBatch.Draw(innerComponent, position, remainingPart,
									opacityColor, 0, Vector2.Zero,
									stretchScale, SpriteEffects.None, 0.0f);
				}
				return;
			}

			
			if(displayModeWidth != DisplayMode.Repeat && displayModeHeight != DisplayMode.Repeat)
			{
				Rectangle remainingPart;
				float w, h;
				w = (int)dimensions.drawSpace.width < innerComponent.Width ?
					(int)dimensions.drawSpace.width : innerComponent.Width;
				h = (int)dimensions.drawSpace.height < innerComponent.Height ?
					(int)dimensions.drawSpace.height : innerComponent.Height;
				remainingPart = new Rectangle(dimensions.drawSpace.offsetLeft,
											dimensions.drawSpace.offsetTop,
											(int)w,
											(int)h);
				spriteBatch.Draw(innerComponent, position, remainingPart,
								opacityColor, 0, Vector2.Zero,
								scale, SpriteEffects.None, 0.0f);
			}
			else
			{
				Rectangle remainingPart;
				int displayX, displayY;
				displayX = (int)(scaleX * (float)innerComponent.Width);
				displayY = (int)(scaleY * (float)innerComponent.Height);
				/*int scrollOffsetX = 0, scrollOffsetY = 0;
				if(scrollableContainer != null)
				{
					scrollOffsetX = - scrollableContainer.getCurrentAlternative().getCurrentLayer().offsetX;
					scrollOffsetX += dimensions.positionX + displayOffsetX;

					while(scrollOffsetX < 0)
						scrollOffsetX += innerComponent.Width;
					while(scrollOffsetX >= innerComponent.Width)
						scrollOffsetX -= innerComponent.Width;
					
					scrollOffsetY = - scrollableContainer.getCurrentAlternative().getCurrentLayer().offsetY;
					scrollOffsetY += dimensions.positionY + displayOffsetY;
					
					while(scrollOffsetY < 0)
						scrollOffsetY += innerComponent.Height;
					while(scrollOffsetY >= innerComponent.Height)
						scrollOffsetY -= innerComponent.Height;
				}*/

				int x = 0, y = 0;
				for(; x + displayX <= getFinalWidth(); x += displayX)
				{
					for(y = 0; y + displayY <= getFinalHeight(); y += displayY)
					{
						/*if(displayModeWidth == DisplayMode.Repeat && displayModeHeight == DisplayMode.Repeat)
						{
							int remainingY, partY;
							remainingY = ((int)getFinalHeight() - y);
							if(remainingY > innerComponent.Height)
								remainingY = innerComponent.Height;
							if(remainingY <= scrollOffsetY)
								continue;
							remainingPart = new Rectangle(0, scrollOffsetY, innerComponent.Width, remainingY);
							position = new Vector2(dimensions.positionX + displayOffsetX + x,
												dimensions.positionY + displayOffsetY + y);
							spriteBatch.Draw(innerComponent, position, remainingPart,
											opacityColor, 0, Vector2.Zero,
											scale, SpriteEffects.None, 0.0f);
							partY = innerComponent.Height - scrollOffsetY;
							remainingPart = new Rectangle(0, 0, innerComponent.Width, scrollOffsetY);
							position = new Vector2(dimensions.positionX + displayOffsetX + x,
												dimensions.positionY + displayOffsetY + y + partY);
							spriteBatch.Draw(innerComponent, position, remainingPart,
											opacityColor, 0, Vector2.Zero,
											scale, SpriteEffects.None, 0.0f);
						}
						else
						{*/
							position = new Vector2(dimensions.positionX + displayOffsetX + x,
												dimensions.positionY + displayOffsetY + y);
							spriteBatch.Draw(innerComponent, position, null,
											opacityColor, 0, Vector2.Zero,
											scale, SpriteEffects.None, 0.0f);
						//}
						// -------------------
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


