using System;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
										Component
	------------------------------------------------------------------------------------
	Template class for component management and representation.
	----------------------------------------------------------------------------------*/
	public abstract class Component<T> : BaseComponent
	{
		// public variables
		public T innerComponent { get; set; }
		
		
		// constructors
		public Component(Layer newParent = null)
			: base(newParent)
		{
			innerComponent = default(T);
		}

		
		public Component(Component<T> component, Layer newParent = null)
			: base(component, newParent)
		{
			innerComponent = default(T);

			if(component != null)
				innerComponent = component.innerComponent;
		}


		public uint getAdjustedMinWidth(uint value)
		{
			if(value < sizeSettings.minInnerWidth)
				return sizeSettings.minInnerWidth;
			return value;
		}


		public uint getAdjustedMinHeight(uint value)
		{
			if(value < sizeSettings.minInnerHeight)
				return sizeSettings.minInnerHeight;
			return value;
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




