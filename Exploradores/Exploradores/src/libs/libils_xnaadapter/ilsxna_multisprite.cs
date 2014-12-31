using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
											MultiSprite
	------------------------------------------------------------------------------------
	Sprite implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class MultiSprite : ILS.Component<List<Texture2D>>
	{
		// public variables
		private int _currentSpriteIndex;
		public int currentSpriteIndex
		{
			get
			{
				return _currentSpriteIndex;
			}

			set
			{
				if(innerComponent == null)
				{
					_currentSpriteIndex = 0;
					return;
				}
				if(value >= innerComponent.Count || value < 0)
					return;
				_currentSpriteIndex = value;
			}
		}


		// constructors
		public MultiSprite(ILS.Layer newParent = null) : base(newParent)
		{
			innerComponent = new List<Texture2D>();
			currentSpriteIndex = 0;
			onMouseOver = mouseActivate;
			onMouseOut = mouseDeactivate;
			onMousePress = mouseSelect;
		}


		public MultiSprite(MultiSprite multiSprite, ILS.Layer newParent = null) : base(multiSprite, newParent)
		{
			if(multiSprite != null)
			{
				innerComponent = new List<Texture2D>();
				foreach(Texture2D i in multiSprite.innerComponent)
					innerComponent.Add(i);
				currentSpriteIndex = multiSprite.currentSpriteIndex;
			}
			else
			{
				innerComponent = new List<Texture2D>();
				currentSpriteIndex = 0;
				onMouseOver = mouseActivate;
				onMouseOut = mouseDeactivate;
				onMousePress = mouseSelect;
			}
		}
		

		// public functions
		public MultiSprite clone()
		{
			return new MultiSprite(this);
		}


		public static void mouseActivate(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			int newIndex = ((MultiSprite)drawable).currentSpriteIndex;
			if(newIndex % 2 == 0)
				++newIndex;
			((MultiSprite)drawable).currentSpriteIndex = newIndex;
		}


		public static void mouseDeactivate(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			int newIndex = ((MultiSprite)drawable).currentSpriteIndex;
			if(newIndex % 2 == 1)
				--newIndex;
			((MultiSprite)drawable).currentSpriteIndex = newIndex;
		}

		
		public static void mouseSelect(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			((MultiSprite)drawable).select();
		}


		public void toggle()
		{
			if((currentSpriteIndex / 2) % 2 == 0)
				select();
			else
				deselect();
		}


		public void select()
		{
			int newIndex = currentSpriteIndex;
			if((newIndex / 2) % 2 == 0)
				newIndex += 2;
			currentSpriteIndex = newIndex;
		}


		public void deselect()
		{
			int newIndex = currentSpriteIndex;
			if((newIndex / 2) % 2 == 1)
				newIndex -= 2;
			currentSpriteIndex = newIndex;
		}


		public Texture2D getCurrentSprite()
		{
			if(innerComponent == null)
				return null;
			if(currentSpriteIndex >= innerComponent.Count)
				return null;
			return innerComponent[currentSpriteIndex];
		}
		
		
		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new MultiSprite(this, newParent);
		}
		
		
		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			if(getCurrentSprite() == null)
				return;
			
			SpriteBatch spriteBatch = ((Window)renderSurface).innerWindow.spriteBatch;
			spriteBatch.Draw(getCurrentSprite(), new Vector2(dimensions.positionX, dimensions.positionY), Color.White);
		}


		// protected functions
		protected override uint getMinInnerUnconstrainedWidth()
		{
			if(getCurrentSprite() == null)
				return getAdjustedMinWidth(0);
			
			return getAdjustedMinWidth((uint)getCurrentSprite().Width);
		}
		
		
		protected override uint getMinInnerUnconstrainedHeight()
		{
			if(getCurrentSprite() == null)
				return getAdjustedMinHeight(0);
			
			return getAdjustedMinHeight((uint)getCurrentSprite().Height);
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


