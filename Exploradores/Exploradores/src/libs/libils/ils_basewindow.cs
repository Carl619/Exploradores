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
		public MouseEvent leftMouseEvent;
		public MouseEvent rightMouseEvent;
		
		public uint currentWidth;
		public uint currentHeight;

		
		// constructor
		public Window()
			: base()
		{
			innerWindow = default(W);
			container = default(C);
			leftMouseEvent = new MouseEvent();
			rightMouseEvent = new MouseEvent();
			rightMouseEvent.leftButton = false;

			currentWidth = 0;
			currentHeight = 0;
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
		
		
		public virtual void mouseAction(int x, int y, MouseEvent.Type eventType, bool leftButton, uint timer)
		{
			MouseEvent mouseEvent;
			if(leftButton == true)
				mouseEvent = leftMouseEvent;
			else
				mouseEvent = rightMouseEvent;
			mouseEvent.eventType = eventType;
			mouseEvent.eventTime = timer;
			
			if(eventType == MouseEvent.Type.Press)
			{
				parseMousePressEvent(x, y, eventType, leftButton, timer);
			}
			else if(eventType == MouseEvent.Type.Release)
			{
				parseMouseReleaseEvent(x, y, eventType, leftButton, timer);
			}
			else if(eventType == MouseEvent.Type.Move)
			{
				parseMouseMoveEvent(x, y, eventType, leftButton, timer);
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


		public virtual void parseMousePressEvent(int x, int y, MouseEvent.Type eventType, bool leftButton, uint timer)
		{
			if(eventType != MouseEvent.Type.Press)
				return;
			
			MouseEvent mouseEvent;
			if(leftButton == true)
				mouseEvent = leftMouseEvent;
			else
				mouseEvent = rightMouseEvent;
			

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
			{
				if(leftButton == true)
				{
					if(mouseEvent.dragableObject.onLeftMouseGrab != null)
						mouseEvent.dragableObject.onLeftMouseGrab(mouseEvent.dragableObject,
																mouseEvent,
																mouseEvent.dragableObject.callbackConfigObj);
				}
				else
				{
					if(mouseEvent.dragableObject.onRightMouseGrab != null)
						mouseEvent.dragableObject.onRightMouseGrab(mouseEvent.dragableObject,
																mouseEvent,
																mouseEvent.dragableObject.callbackConfigObj);
				}
			}
			

			if(mouseEvent.pressedObject != null)
			{
				if(leftButton == true)
				{
					if(mouseEvent.pressedObject.onLeftMousePress != null)
					{
						mouseEvent.pressedObject.onLeftMousePress(mouseEvent.pressedObject,
																mouseEvent,
																mouseEvent.pressedObject.callbackConfigObj);
					}
				}
				else
				{
					if(mouseEvent.pressedObject.onRightMousePress != null)
					{
						mouseEvent.pressedObject.onRightMousePress(mouseEvent.pressedObject,
																mouseEvent,
																mouseEvent.pressedObject.callbackConfigObj);
					}
				}
			}
		}


		public virtual void parseMouseReleaseEvent(int x, int y, MouseEvent.Type eventType, bool leftButton, uint timer)
		{
			if(eventType != MouseEvent.Type.Release)
				return;
			
			MouseEvent mouseEvent;
			if(leftButton == true)
				mouseEvent = leftMouseEvent;
			else
				mouseEvent = rightMouseEvent;
			
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
			
			
			if(leftButton == true)
			{
				if(mouseEvent.dragableObject != null)
					if(mouseEvent.dragableObject.onLeftMouseDrop != null)
						mouseEvent.dragableObject.onLeftMouseDrop(mouseEvent.dragableObject,
																mouseEvent,
																mouseEvent.dragableObject.callbackConfigObj);
			}
			else
			{
				if(mouseEvent.dragableObject != null)
					if(mouseEvent.dragableObject.onRightMouseDrop != null)
						mouseEvent.dragableObject.onRightMouseDrop(mouseEvent.dragableObject,
																mouseEvent,
																mouseEvent.dragableObject.callbackConfigObj);
			}


			if(eventType == MouseEvent.Type.Release)
			{
				if(mouseEvent.pressedObject != null && mouseEvent.pressedObject != mouseEvent.dragableObject)
				{
					if(leftButton == true)
					{
						if(mouseEvent.pressedObject.onLeftMouseRelease != null)
							mouseEvent.pressedObject.onLeftMouseRelease(mouseEvent.pressedObject,
																	mouseEvent,
																	mouseEvent.pressedObject.callbackConfigObj);
					}
					else
					{
						if(mouseEvent.pressedObject.onRightMouseRelease != null)
							mouseEvent.pressedObject.onRightMouseRelease(mouseEvent.pressedObject,
																	mouseEvent,
																	mouseEvent.pressedObject.callbackConfigObj);
					}
				}
				
				if(mouseEvent.droppedUponObject != null && mouseEvent.droppedUponObject != mouseEvent.dragableObject &&
					mouseEvent.droppedUponObject != mouseEvent.pressedObject)
				{
					if(leftButton == true)
					{
						if(mouseEvent.droppedUponObject.onLeftMouseRelease != null)
							mouseEvent.droppedUponObject.onLeftMouseRelease(mouseEvent.droppedUponObject,
																		mouseEvent,
																		mouseEvent.droppedUponObject.callbackConfigObj);
					}
					else
					{
						if(mouseEvent.droppedUponObject.onRightMouseRelease != null)
							mouseEvent.droppedUponObject.onRightMouseRelease(mouseEvent.droppedUponObject,
																		mouseEvent,
																		mouseEvent.droppedUponObject.callbackConfigObj);
					}
				}
			}
			else if(eventType == MouseEvent.Type.DoubleClick)
			{
				mouseEvent.eventType = MouseEvent.Type.DoubleClick;
				if(mouseEvent.droppedUponObject != null)
				{
					if(leftButton == true)
					{
						if(mouseEvent.droppedUponObject.onLeftMouseDoubleClick != null)
							mouseEvent.droppedUponObject.onLeftMouseDoubleClick(mouseEvent.droppedUponObject,
																		mouseEvent,
																		mouseEvent.droppedUponObject.callbackConfigObj);
					}
					else
					{
						if(mouseEvent.droppedUponObject.onRightMouseDoubleClick != null)
							mouseEvent.droppedUponObject.onRightMouseDoubleClick(mouseEvent.droppedUponObject,
																		mouseEvent,
																		mouseEvent.droppedUponObject.callbackConfigObj);
					}
				}
			}

			mouseEvent.lastPressedObject = mouseEvent.pressedObject;
			mouseEvent.pressedObject = null;
			mouseEvent.dragableObject = null;
			mouseEvent.droppedUponObject = null;
		}


		public virtual void parseMouseMoveEvent(int x, int y, MouseEvent.Type eventType, bool leftButton, uint timer)
		{
			if(eventType != MouseEvent.Type.Move)
				return;
			
			MouseEvent mouseEvent;
			if(leftButton == true)
				mouseEvent = leftMouseEvent;
			else
				mouseEvent = rightMouseEvent;
			
			if(mouseEvent.actualPositionX == x &&
				mouseEvent.actualPositionY == y)
				return;
			
			mouseEvent.actualPositionX = x;
			mouseEvent.actualPositionY = y;
			
			if(mouseEvent.dragableObject != null) // drag action
			{
				mouseEvent.eventType = MouseEvent.Type.Drag;
				if(leftButton == true)
				{
					if(mouseEvent.dragableObject.onLeftMouseDrag != null)
						mouseEvent.dragableObject.onLeftMouseDrag(mouseEvent.dragableObject,
																mouseEvent,
																mouseEvent.dragableObject.callbackConfigObj);
				}
				else
				{
					if(mouseEvent.dragableObject.onRightMouseDrag != null)
						mouseEvent.dragableObject.onRightMouseDrag(mouseEvent.dragableObject,
																mouseEvent,
																mouseEvent.dragableObject.callbackConfigObj);
				}
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
						i.onMouseOut(i, leftMouseEvent, i.callbackConfigObj);
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
					i.onMouseOver(i, leftMouseEvent, i.callbackConfigObj);
			}
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




