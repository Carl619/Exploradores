using System;
using System.Collections.Generic;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
										Drawable
	------------------------------------------------------------------------------------
	Base class for objects that can be drawn on the screen.
	----------------------------------------------------------------------------------*/
	public abstract class Drawable
	{
		// public variables
		public bool visible { get; set; }
		public bool requestedContentUpdate { get; protected set; }
		public Dimensions dimensions { get; protected set; }
		public SizeSettings sizeSettings { get; protected set; }
			// event callback variables and functions
		public Object callbackConfigObj { get; set; } // object passed to callback functions as extra parameter
		public MouseEvent.CallbackFunction onMousePress { get; set; }
		public MouseEvent.CallbackFunction onMouseRelease { get; set; }
		public MouseEvent.CallbackFunction onMouseDoubleClick { get; set; }
		public MouseEvent.CallbackFunction onMouseGrab { get; set; }
		public MouseEvent.CallbackFunction onMouseDrag { get; set; }
		public MouseEvent.CallbackFunction onMouseDrop { get; set; }
		public MouseEvent.CallbackFunction onMouseOver { get; set; }
		public MouseEvent.CallbackFunction onMouseOut { get; set; }
		
		
		// constructors
		public Drawable()
		{
			visible = true;
			requestedContentUpdate = false;

			dimensions = new Dimensions();
			sizeSettings = new SizeSettings();
			callbackConfigObj = null;

			onMousePress = null;
			onMouseRelease = null;
			onMouseDoubleClick = null;
			onMouseGrab = null;
			onMouseDrag = null;
			onMouseDrop = null;
			onMouseOver = null;
			onMouseOut = null;
		}
		
		
		public Drawable(Drawable drawable)
		{
			visible = drawable.visible;
			requestedContentUpdate = drawable.requestedContentUpdate;

			dimensions = drawable.dimensions.clone();
			sizeSettings = drawable.sizeSettings.clone();

			callbackConfigObj = drawable.callbackConfigObj;

			onMousePress = drawable.onMousePress;
			onMouseRelease = drawable.onMouseRelease;
			onMouseDoubleClick = drawable.onMouseDoubleClick;
			onMouseGrab = drawable.onMouseGrab;
			onMouseDrag = drawable.onMouseDrag;
			onMouseDrop = drawable.onMouseDrop;
			onMouseOver = drawable.onMouseOver;
			onMouseOut = drawable.onMouseOut;
		}
		
		
		// public functions
			//static
		public static void blockCallbackFunction(Drawable drawable, MouseEvent eventInfo, Object configObj) {}
			// non-static
		public abstract bool isDisplayed(); // true if it is currently displayed
		public abstract bool hasBorder();
		public abstract BaseContainer getParent();

		public abstract void draw(Object renderSurface);
		public virtual void minimize(uint outterConstraintWidth, uint outterConstraintHeight) {}
		public virtual void updateContent() {}
		public virtual bool isFlexible() { return false; } // the contents can be manipulated (some containers)

		public virtual bool acceptKeyboardInput() { return false; }
		public virtual void clearInputText() {}
		public virtual void getInputText(Object leftToCursor, Object rightToCursor) {}
		public virtual void insertInputText(Object text) {}
		public virtual void removeInputText(int offset) {}
		public virtual void moveInputCursor(int offset) {}
		
		public virtual int getDrawPositionX() { return dimensions.positionX; }
		public virtual int getDrawPositionY() { return dimensions.positionY; }
		public virtual uint getFinalWidth() { return dimensions.drawSpace.width; }
		public virtual uint getFinalHeight() { return dimensions.drawSpace.height; }

		public virtual uint getSingleBorderWidth() { return 0; }
		public virtual uint getSingleBorderHeight() { return 0; }
		public virtual uint getTotalBorderWidth() { return 0; }
		public virtual uint getTotalBorderHeight() { return 0; }


		public virtual void applyUpdateContent()
		{
			if(requestedContentUpdate == true)
				updateContent();
		}


		public virtual void requestUpdateContent()
		{
			requestedContentUpdate = true;
			informUpdateContent();
		}


		public virtual uint getTotalOffsetWidth()
		{
			if(visible == false)
				return 0;
			uint total = getTotalInnerSpacingWidth() + getTotalOutterSpacingWidth();
			if(hasBorder() == true)
				total += getTotalBorderWidth();
			return total;
		}


		public virtual uint getTotalOffsetHeight()
		{
			if(visible == false)
				return 0;
			uint total = getTotalInnerSpacingHeight() + getTotalOutterSpacingHeight();
			if(hasBorder() == true)
				total += getTotalBorderHeight();
			return total;
		}
		
		
		public virtual uint getMinOutterUnconstrainedWidth()
		{
			if(visible == false)
				return 0;
			return getMinInnerUnconstrainedWidth() + getTotalOffsetWidth();
		}
		
		
		public virtual uint getMinOutterUnconstrainedHeight()
		{
			if(visible == false)
				return 0;
			return getMinInnerUnconstrainedHeight() + getTotalOffsetHeight();
		}
		
		
		public virtual uint getMinOutterConstrainedWidth()
		{
			if(visible == false)
				return 0;
			
			uint result = dimensions.minOutterConstrainedX;
			if(result == 0)
				result = getMinOutterUnconstrainedWidth();
		
			return result;
		}
		
		
		public virtual uint getMinOutterConstrainedHeight()
		{
			if(visible == false)
				return 0;
			
			uint result = dimensions.minOutterConstrainedY;
			if(result == 0)
				result = getMinOutterUnconstrainedHeight();
			return result;
		}


		public virtual void setMinOutterParentConstraints(uint elementWidth, uint elementHeight)
		{
			setMinOutterConstrainedWidth(elementWidth);
			setMinOutterConstrainedHeight(elementHeight);
		}
		
		
		public virtual void setMinOutterConstrainedWidth(uint width)
		{
			uint value = width;
			if (value == 0)
				value = getMinInnerUnconstrainedWidth();
			dimensions.minOutterConstrainedX = value;
		}
		
		
		public virtual void setMinOutterConstrainedHeight(uint height)
		{
			uint value = height;
			if (value == 0)
				value = getMinInnerUnconstrainedHeight();
			dimensions.minOutterConstrainedY = value;
		}
		
		
		public virtual void mouseAction(MouseEvent mouseEvent, List<Drawable> drawables)
		{
			if(mouseEvent.eventType == MouseEvent.Type.Press &&
					(onMousePress != null || onMouseGrab != null || onMouseDoubleClick != null))
				drawables.Add(this);
			else if(mouseEvent.eventType == MouseEvent.Type.Release &&
					(onMouseRelease != null || onMouseDrop != null || onMouseDoubleClick != null))
				drawables.Add(this);
			else if(mouseEvent.eventType == MouseEvent.Type.Drag && onMouseDrag != null)
				drawables.Add(this);
			else if(mouseEvent.eventType == MouseEvent.Type.Move && (onMouseOver != null || onMouseOut != null))
				drawables.Add(this);
		}
		
		
		public virtual void calculatePosition(int xPos, int yPos, Dimensions.ClipBox clipBox)
		{
			dimensions.positionX = xPos;
			dimensions.positionY = yPos;
			dimensions.calculateFinalDrawSpace(clipBox);
		}
		
		
		// protected functions
		protected abstract void informUpdateContent();
		protected abstract uint getMinInnerUnconstrainedWidth();
		protected abstract uint getMinInnerUnconstrainedHeight();
		

		protected virtual uint getMinInnerConstrainedWidth()
		{
			if(visible == false)
				return 0;
			uint result = getMinOutterConstrainedWidth();
			if(result < getTotalOffsetWidth())
				result = 0;
			else
				result -= getTotalOffsetWidth();
			return result;
		}
		

		protected virtual uint getMinInnerConstrainedHeight()
		{
			if(visible == false)
				return 0;
			uint result = getMinOutterConstrainedHeight();
			if(result < getTotalOffsetHeight())
				result = 0;
			else
				result -= getTotalOffsetHeight();
			return result;
		}
		

		protected virtual uint getSingleOutterSpacingWidth()
		{
			return sizeSettings.outterSpacingX;
		}
		

		protected virtual uint getSingleOutterSpacingHeight()
		{
			if(visible == false)
				return 0;
			return sizeSettings.outterSpacingY;
		}
		

		protected virtual uint getTotalOutterSpacingWidth()
		{
			if(visible == false)
				return 0;
			return 2 * sizeSettings.outterSpacingX;
		}
		

		protected virtual uint getTotalOutterSpacingHeight()
		{
			if(visible == false)
				return 0;
			return 2 * sizeSettings.outterSpacingY;
		}


		protected virtual uint getTotalInnerSpacingWidth()
		{
			if(visible == false)
				return 0;
			return 2 * getSingleInnerSpacingWidth();
		}


		protected virtual uint getTotalInnerSpacingHeight()
		{
			if(visible == false)
				return 0;
			return 2 * getSingleInnerSpacingHeight();
		}


		protected virtual uint getSingleInnerSpacingWidth() { return 0; }
		protected virtual uint getSingleInnerSpacingHeight() { return 0; }
		
		protected virtual void calculateMinOutterConstrains() {}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




