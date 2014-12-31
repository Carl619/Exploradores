using System;
using System.Collections.Generic;
using System.Linq;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
											Window
	------------------------------------------------------------------------------------
	Base class for window management and representation.
	----------------------------------------------------------------------------------*/
	public abstract class Window<W, C> : Drawable where C : BaseContainer // Window, Container templates
	{
		// public variables
		public W innerWindow { get; set; }
		public C container { get; set; }
		public MouseEvent mouseEvent;
		
		public uint currentWidth;
		public uint currentHeight;
		public bool updated { get; private set; }

		
		// constructor
		public Window()
			: base()
		{
			innerWindow = default(W);
			container = default(C);
			mouseEvent = new MouseEvent();

			currentWidth = 0;
			currentHeight = 0;
			updated = false;
		}
		
		
		// public functions
		public override bool isDisplayed()
		{
			return visible;
		}


		public override BaseContainer getParent()
		{
			return null;
		}


		public override void mouseAction(MouseEvent mouseEvent, List<Drawable> drawables)
		{
			container.mouseAction(mouseEvent, drawables);
		}


		public override void calculatePosition(int xPos, int yPos, Dimensions.ClipBox clipBox)
		{
			container.calculatePosition(xPos, yPos, clipBox);
		}


		public override void updateContent()
		{
			container.updateContent();
			requestedContentUpdate = false;
		}


		public override void applyUpdateContent()
		{
			container.applyUpdateContent();
			requestedContentUpdate = false;
		}


		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			
			container.draw(renderSurface);
		}

		
		public abstract void draw();
		public abstract void resize(uint width, uint height);
		
		
		public virtual void mouseAction(int x, int y, MouseEvent.Type eventType, uint timer)
		{
			mouseEvent.eventType = eventType;
			mouseEvent.eventTime = timer;
			
			if(eventType == MouseEvent.Type.Press)
			{
				parseMousePressEvent(x, y, eventType, timer);
			}
			else if(eventType == MouseEvent.Type.Release)
			{
				parseMouseReleaseEvent(x, y, eventType, timer);
			}
			else if(eventType == MouseEvent.Type.Move)
			{
				parseMouseMoveEvent(x, y, eventType, timer);
			}
		}

		
		// protected functions
		protected override void informUpdateContent()
		{
			requestedContentUpdate = true;
		}


		protected virtual void updateDimensions(uint windowWidth, uint windowHeight)
		{
			container.sizeSettings.fixedInnerWidth = windowWidth - container.getTotalOffsetWidth();
			container.sizeSettings.fixedInnerHeight = windowHeight - container.getTotalOffsetHeight();
			container.minimize(windowWidth, windowHeight);

			// important, for window border
			container.setMinOutterParentConstraints(windowWidth, windowHeight);
		}


		public virtual void parseMousePressEvent(int x, int y, MouseEvent.Type eventType, uint timer)
		{
			if(eventType != MouseEvent.Type.Press)
				return;
			
			mouseEvent.actualPositionX = x;
			mouseEvent.actualPositionY = y;
			mouseEvent.pressedObject = null;
			mouseEvent.dragableObject = null;
			mouseEvent.droppedUponObject = null;
			mouseEvent.pressPositionX = x;
			mouseEvent.pressPositionY = y;
			mouseEvent.lastPressTime = mouseEvent.pressTime;
			mouseEvent.pressTime = timer;
			
			// get pressable object
			
			List<Drawable> list = new List<Drawable>();
			mouseAction(mouseEvent, list);
			if(list.Any())
				mouseEvent.pressedObject = list.Last();
			
			// get dragable object
			list.Clear();
			mouseEvent.eventType = MouseEvent.Type.Drag;
			mouseAction(mouseEvent, list);
			if(list.Any())
				mouseEvent.dragableObject = list.Last();
			
			mouseEvent.eventType = MouseEvent.Type.Press;
			
			if(mouseEvent.dragableObject != null)
				if(mouseEvent.dragableObject.onMouseGrab != null)
					mouseEvent.dragableObject.onMouseGrab(mouseEvent.dragableObject,
															mouseEvent,
															mouseEvent.dragableObject.callbackConfigObj);
			
			if(mouseEvent.pressedObject != null)
			{
				if(mouseEvent.pressedObject.onMousePress != null)
				{
					mouseEvent.pressedObject.onMousePress(mouseEvent.pressedObject,
															mouseEvent,
															mouseEvent.pressedObject.callbackConfigObj);
				}
			}
		}


		public virtual void parseMouseReleaseEvent(int x, int y, MouseEvent.Type eventType, uint timer)
		{
			if(eventType != MouseEvent.Type.Release)
				return;
			
			mouseEvent.actualPositionX = x;
			mouseEvent.actualPositionY = y;
			
			// get dropped upon object
			List<Drawable> list = new List<Drawable>();
			mouseAction(mouseEvent, list);
			if(list.Any())
				mouseEvent.droppedUponObject = list.Last();
			
			if(mouseEvent.eventTime - mouseEvent.lastPressTime <= MouseEvent.DoubleClickSpeed)
				if(mouseEvent.lastPressedObject == mouseEvent.droppedUponObject)
					eventType = MouseEvent.Type.DoubleClick;
			
			
			if(mouseEvent.dragableObject != null)
				if(mouseEvent.dragableObject.onMouseDrop != null)
					mouseEvent.dragableObject.onMouseDrop(mouseEvent.dragableObject,
															mouseEvent,
															mouseEvent.dragableObject.callbackConfigObj);
			if(eventType == MouseEvent.Type.Release)
			{
				if(mouseEvent.pressedObject != null && mouseEvent.pressedObject != mouseEvent.dragableObject)
					if(mouseEvent.pressedObject.onMouseRelease != null)
						mouseEvent.pressedObject.onMouseRelease(mouseEvent.pressedObject,
																mouseEvent,
																mouseEvent.pressedObject.callbackConfigObj);
				
				if(mouseEvent.droppedUponObject != null && mouseEvent.droppedUponObject != mouseEvent.dragableObject &&
					mouseEvent.droppedUponObject != mouseEvent.pressedObject)
					if(mouseEvent.droppedUponObject.onMouseRelease != null)
						mouseEvent.droppedUponObject.onMouseRelease(mouseEvent.droppedUponObject,
																	mouseEvent,
																	mouseEvent.droppedUponObject.callbackConfigObj);
			}
			else if(eventType == MouseEvent.Type.DoubleClick)
			{
				mouseEvent.eventType = MouseEvent.Type.DoubleClick;
				if(mouseEvent.droppedUponObject != null)
					if(mouseEvent.droppedUponObject.onMouseDoubleClick != null)
						mouseEvent.droppedUponObject.onMouseDoubleClick(mouseEvent.droppedUponObject,
																	mouseEvent,
																	mouseEvent.droppedUponObject.callbackConfigObj);
			}

			mouseEvent.lastPressedObject = mouseEvent.pressedObject;
			mouseEvent.pressedObject = null;
			mouseEvent.dragableObject = null;
			mouseEvent.droppedUponObject = null;
		}


		public virtual void parseMouseMoveEvent(int x, int y, MouseEvent.Type eventType, uint timer)
		{
			if(eventType != MouseEvent.Type.Move)
				return;
			
			if(mouseEvent.actualPositionX == x &&
				mouseEvent.actualPositionY == y)
				return;
			
			mouseEvent.actualPositionX = x;
			mouseEvent.actualPositionY = y;
			
			if(mouseEvent.dragableObject != null) // drag action
			{
				mouseEvent.eventType = MouseEvent.Type.Drag;
				if(mouseEvent.dragableObject.onMouseDrag != null)
					mouseEvent.dragableObject.onMouseDrag(mouseEvent.dragableObject,
															mouseEvent,
															mouseEvent.dragableObject.callbackConfigObj);
			}
			else // mouseover and mouseout actions
			{
				List<Drawable> newList = new List<Drawable>();
				mouseAction(mouseEvent, newList);
				
				int counter = 0;
				List<Drawable> oldList = mouseEvent.mouseoverObjects;
				onMouseOutApply(oldList, newList, ref counter);
				onMouseOverApply(oldList, newList, ref counter);
				oldList.AddRange(newList);
			}
		}


		protected virtual void onMouseOutApply(List<Drawable> oldList, List<Drawable> newList, ref int counter)
		{
			bool flag = false;
			counter = 0;
			foreach(Drawable i in oldList)
			{
				if(newList.Count <= counter)
					flag = true;
				if(flag == false)
					if(newList[counter] != i)
						flag = true;
				if(flag == true)
				{
					if(i.onMouseOut != null)
						i.onMouseOut(i, mouseEvent, i.callbackConfigObj);
				}
				else
					++counter;
			}

			oldList.RemoveRange(counter, oldList.Count - counter);
			if(counter > 0)
				newList.RemoveRange(0, counter - 1);
		}


		protected virtual void onMouseOverApply(List<Drawable> oldList, List<Drawable> newList, ref int counter)
		{
			foreach(Drawable i in newList)
			{
				if(i.onMouseOver != null)
					i.onMouseOver(i, mouseEvent, i.callbackConfigObj);
			}
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




