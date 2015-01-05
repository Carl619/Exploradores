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
		public MouseEvent.CallbackFunction onMouseOver { get; set; }
		public MouseEvent.CallbackFunction onMouseOut { get; set; }

		public MouseEvent.CallbackFunction onLeftMousePress { get; set; }
		public MouseEvent.CallbackFunction onLeftMouseRelease { get; set; }
		public MouseEvent.CallbackFunction onLeftMouseDoubleClick { get; set; }
		public MouseEvent.CallbackFunction onLeftMouseGrab { get; set; }
		public MouseEvent.CallbackFunction onLeftMouseDrag { get; set; }
		public MouseEvent.CallbackFunction onLeftMouseDrop { get; set; }

		public MouseEvent.CallbackFunction onRightMousePress { get; set; }
		public MouseEvent.CallbackFunction onRightMouseRelease { get; set; }
		public MouseEvent.CallbackFunction onRightMouseDoubleClick { get; set; }
		public MouseEvent.CallbackFunction onRightMouseGrab { get; set; }
		public MouseEvent.CallbackFunction onRightMouseDrag { get; set; }
		public MouseEvent.CallbackFunction onRightMouseDrop { get; set; }
		
		
		// constructors
		public Drawable()
		{
			visible = true;
			requestedContentUpdate = false;

			dimensions = new Dimensions();
			sizeSettings = new SizeSettings();
			callbackConfigObj = null;
			onMouseOver = null;
			onMouseOut = null;

			onLeftMousePress = null;
			onLeftMouseRelease = null;
			onLeftMouseDoubleClick = null;
			onLeftMouseGrab = null;
			onLeftMouseDrag = null;
			onLeftMouseDrop = null;

			onRightMousePress = null;
			onRightMouseRelease = null;
			onRightMouseDoubleClick = null;
			onRightMouseGrab = null;
			onRightMouseDrag = null;
			onRightMouseDrop = null;
		}
		
		
		public Drawable(Drawable drawable)
		{
			visible = drawable.visible;
			requestedContentUpdate = drawable.requestedContentUpdate;

			dimensions = drawable.dimensions.clone();
			sizeSettings = drawable.sizeSettings.clone();

			callbackConfigObj = drawable.callbackConfigObj;
			onMouseOver = drawable.onMouseOver;
			onMouseOut = drawable.onMouseOut;

			onLeftMousePress = drawable.onLeftMousePress;
			onLeftMouseRelease = drawable.onLeftMouseRelease;
			onLeftMouseDoubleClick = drawable.onLeftMouseDoubleClick;
			onLeftMouseGrab = drawable.onLeftMouseGrab;
			onLeftMouseDrag = drawable.onLeftMouseDrag;
			onLeftMouseDrop = drawable.onLeftMouseDrop;

			onRightMousePress = drawable.onRightMousePress;
			onRightMouseRelease = drawable.onRightMouseRelease;
			onRightMouseDoubleClick = drawable.onRightMouseDoubleClick;
			onRightMouseGrab = drawable.onRightMouseGrab;
			onRightMouseDrag = drawable.onRightMouseDrag;
			onRightMouseDrop = drawable.onRightMouseDrop;
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
				value = getMinInnerUnconstrainedWidth() + getTotalOffsetWidth();
			dimensions.minOutterConstrainedX = value;
		}
		
		
		public virtual void setMinOutterConstrainedHeight(uint height)
		{
			uint value = height;
			if (value == 0)
				value = getMinInnerUnconstrainedHeight()+ getTotalOffsetHeight();
			dimensions.minOutterConstrainedY = value;
		}
		
		
		public virtual void mouseAction(MouseEvent mouseEvent, List<Drawable> drawables)
		{
			if(mouseEvent.leftButton == true)
			{
				if(mouseEvent.eventType == MouseEvent.Type.Press &&
						(onLeftMousePress != null || onLeftMouseGrab != null || onLeftMouseDoubleClick != null))
					drawables.Add(this);
				else if(mouseEvent.eventType == MouseEvent.Type.Release &&
						(onLeftMouseRelease != null || onLeftMouseDrop != null || onLeftMouseDoubleClick != null))
					drawables.Add(this);
				else if(mouseEvent.eventType == MouseEvent.Type.Drag && onLeftMouseDrag != null)
					drawables.Add(this);
				else if(mouseEvent.eventType == MouseEvent.Type.Move && (onMouseOver != null || onMouseOut != null))
					drawables.Add(this);
			}
			else
			{
				if(mouseEvent.eventType == MouseEvent.Type.Press &&
						(onRightMousePress != null || onRightMouseGrab != null || onRightMouseDoubleClick != null))
					drawables.Add(this);
				else if(mouseEvent.eventType == MouseEvent.Type.Release &&
						(onRightMouseRelease != null || onRightMouseDrop != null || onRightMouseDoubleClick != null))
					drawables.Add(this);
				else if(mouseEvent.eventType == MouseEvent.Type.Drag && onRightMouseDrag != null)
					drawables.Add(this);
				else if(mouseEvent.eventType == MouseEvent.Type.Move && (onMouseOver != null || onMouseOut != null))
					drawables.Add(this);
			}
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




