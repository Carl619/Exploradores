using System;
using System.Collections.Generic;
using System.Linq;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
										Class list
	------------------------------------------------------------------------------------
	  - ComponentGroup
	  - Layer
	  - LayerGroup
	  - SingleAlternative
	  - Alternatives
	  - MouseEvent
	----------------------------------------------------------------------------------*/
	
	
	
	public class ComponentGroup : List<BaseComponent>
	{
		public ComponentGroup clone(Layer newParent = null)
		{
			ComponentGroup newComponentGroup = new ComponentGroup();
			foreach(BaseComponent i in this)
				newComponentGroup.Add(i.clone(newParent));
			return newComponentGroup;
		}
	}
	
	
	
	public class LayerGroup : List<Layer>
	{
		public LayerGroup clone(SingleAlternative newParent = null)
		{
			LayerGroup newLayerGroup = new LayerGroup();
			foreach(Layer i in this)
				newLayerGroup.Add(i.clone(newParent));
			return newLayerGroup;
		}
	}
	
	
	
	public class Alternatives : List<SingleAlternative>
	{
		public Alternatives clone(BaseContainer newParent = null)
		{
			Alternatives newAlternatives = new Alternatives();
			foreach(SingleAlternative i in this)
				newAlternatives.Add(i.clone(newParent));
			return newAlternatives;
		}
	}
	
	
	
	public class Layer
	{
		// public variables
		public ComponentGroup group { get; set; }
		public Dimensions dimensions { get; set; }
		public int offsetX { get; set; }
		public int offsetY { get; set; }
		public bool blockSubsequentLayerEvents { get; set; }
		public SingleAlternative parent { get; set; }
		
		
		// constructors
		public Layer(SingleAlternative newParent = null)
		{
			group = new ComponentGroup();
			dimensions = new Dimensions();
			offsetX = 0;
			offsetY = 0;
			blockSubsequentLayerEvents = false;
			parent = newParent;
		}
		
		
		public Layer(Layer layer, SingleAlternative newParent = null)
		{
			group = layer.group.clone(this);
			dimensions = layer.dimensions.clone();
			offsetX = layer.offsetX;
			offsetY = layer.offsetY;
			parent = newParent;
		}
		
		
		// public functions
		public Layer clone(SingleAlternative currentParent = null)
		{
			Layer newLayer = new Layer(this, currentParent);
			return newLayer;
		}


		public void applyUpdateContent()
		{
			foreach(BaseComponent component in group)
				component.applyUpdateContent();
		}


		public void clear(bool clearParent = false)
		{
			group.Clear();
			dimensions.clear();
			offsetX = 0;
			offsetY = 0;
			if(clearParent == true)
				parent = null;
		}
		
		
		public void updateComponentsParent()
		{
			foreach(BaseComponent i in group)
				i.parent = this;
		}


		public void scrollTop(uint amount)
		{
			if(offsetY >= 0)
				return;
			int min = (int)amount < - offsetY ? (int)amount : - offsetY;
			offsetY += min;
		}


		public void scrollBottom(uint amount)
		{
			if(parent == null)
				return;
			if(parent.parent == null)
				return;
			BaseComponent component;
			component = parent.parent;

			int parentHeight = (int)component.getMinOutterConstrainedHeight() - (int)component.getTotalOffsetHeight();
			int layerHeight = (int)dimensions.afterMinimizeOutterY;
			int maxOffset;
			if(parentHeight > layerHeight)
				maxOffset = 0;
			else
				maxOffset = layerHeight - parentHeight;
			
			maxOffset += offsetY;
			if(maxOffset < 1)
				return;
			int min = (int)amount < maxOffset ? (int)amount : maxOffset;
			offsetY -= min;
		}


		public void scrollLeft(uint amount)
		{
			if(offsetX >= 0)
				return;
			int min = (int)amount < - offsetX ? (int)amount : - offsetX;
			offsetX += min;
		}


		public void scrollRight(uint amount)
		{
			if(parent == null)
				return;
			if(parent.parent == null)
				return;
			BaseComponent component;
			component = parent.parent;

			int parentWidth = (int)component.getMinOutterConstrainedWidth() - (int)component.getTotalOffsetWidth();
			int layerWidth = (int)dimensions.afterMinimizeOutterX;
			int maxOffset;
			if(parentWidth > layerWidth)
				maxOffset = 0;
			else
				maxOffset = layerWidth - parentWidth;
			
			maxOffset += offsetX;
			if(maxOffset < 1)
				return;
			int min = (int)amount < maxOffset ? (int)amount : maxOffset;
			offsetX -= min;
		}
	}
	
	
	
	public class SingleAlternative
	{
		// public variables
		public LayerGroup layers { get; set; }
		public uint defaultLayer { get; set; }
		public BaseContainer parent { get; set; }
		
		
		// constructors
		public SingleAlternative(BaseContainer newParent = null)
		{
			layers = new LayerGroup();
			addLayer();
			defaultLayer = 0;
			parent = newParent;
		}
		
		
		public SingleAlternative(SingleAlternative alternative, BaseContainer newParent = null)
		{
			layers = alternative.layers.clone(this);
			defaultLayer = alternative.defaultLayer;
			parent = newParent;
		}
		
		
		// public functions
		public SingleAlternative clone(BaseContainer newParent = null)
		{
			return new SingleAlternative(this, newParent);
		}
		
		
		public bool isCurrentAlternative()
		{
			if(parent == null)
				return true;
			if(parent.getCurrentAlternative() != this)
				return false;
			return parent.isDisplayed();
		}


		public Layer getCurrentLayer()
		{
			if(defaultLayer >= layers.Count)
				return layers[0];
			return layers[(int)defaultLayer];
		}
		
		
		public void clear(bool clearParent = false)
		{
			layers.Clear();
			addLayer();
			defaultLayer = 0;
			if(clearParent == true)
				parent = null;
		}
		
		
		public void updateLayersParent()
		{
			foreach(Layer i in layers)
				i.parent = this;
		}
		
		
		// private functions
		public void addLayer()
		{
			Layer newLayer = new Layer(this);
			layers.Add(newLayer);
		}
	}

	
	
	/*----------------------------------------------------------------------------------
										MouseEvent
	------------------------------------------------------------------------------------
	Class for storing data related to mouse events and actions.
	----------------------------------------------------------------------------------*/
	public class MouseEvent
	{
		// delegates
		public delegate void CallbackFunction(Drawable drawable, MouseEvent eventInfo, Object configObj);

		// public variables
			// static
		public static uint DoubleClickSpeed = 300;
			// non-static
		public Drawable pressedObject { get; set; } // the drawable object that was pressed
		public Drawable dragableObject { get; set; } // the drawable object subject to dragging
		public Drawable droppedUponObject { get; set; } // the drawable upon which the draggable object was dropped
		public Drawable lastPressedObject { get; set; } // the drawable object that was pressed last time (for double click)
		public List<Drawable> mouseoverObjects { get; set; } // the drawable objects that have the mouse over them
			// absolute mouse coordinates of button press
		public int pressPositionX { get; set; } 
		public int pressPositionY { get; set; }
			// absolute mouse coordinates of current action
		public int actualPositionX { get; set; }
		public int actualPositionY { get; set; }
		public uint pressTime { get; set; } // the time the mouse button was pressed, with no specific unit
		public uint eventTime { get; set; } // the time this event occured, with no specific unit
		public uint lastPressTime { get; set; } // the last time the mouse button was pressed, with no specific unit
		public Type eventType { get; set; }
		public bool leftButton { get; set; }
		
		
		// enums
		/* The type indicates what occured during the event
		(only one mouse button is taken into account) */
		public enum Type
		{
			Move, // mouse move
			Drag, // mouse move while pressed
			Press, // mouse button pressed, or clicked
			Release, // release of pressed mouse button
			DoubleClick // mouse button double click
		}
		
		
		// constructor
		public MouseEvent()
		{
			clear();
		}


		public void clear()
		{
			pressedObject = null;
			dragableObject = null;
			droppedUponObject = null;
			lastPressedObject = null;
			mouseoverObjects = new List<Drawable>();

			pressPositionX = 0;
			pressPositionY = 0;
			actualPositionX = 0;
			actualPositionY = 0;

			pressTime = 0;
			eventTime = 0;
			lastPressTime = 0;
			eventType = Type.Press;
			leftButton = true;
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




