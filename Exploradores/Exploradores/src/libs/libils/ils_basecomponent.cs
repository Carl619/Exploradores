using System;
using System.Collections.Generic;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
										BaseComponent
	------------------------------------------------------------------------------------
	Base class for atomic drawables and containers.
	----------------------------------------------------------------------------------*/
	public abstract class BaseComponent : Drawable
	{
		// protected variables
		public Layer parent { get; set; }
		
		
		// constructors
		public BaseComponent(Layer newParent = null)
			: base()
		{
			parent = newParent;
		}


		public BaseComponent(BaseComponent component, Layer newParent = null)
			: base(component)
		{
			parent = newParent;
		}
		
		
		// public functions
		public abstract BaseComponent clone(Layer newParent = null);


		public override bool isDisplayed()
		{
			if(visible == false)
				return false;
			if (parent != null)
			{
				if (parent.parent != null)
				{
					if (parent.parent.isCurrentAlternative() == false)
						return false;
				}
			}
			return true;
		}


		public override bool hasBorder()
		{
			return false;
		}
		
		
		public override BaseContainer getParent()
		{
			if (parent == null)
				return null;
			if (parent.parent == null)
				return null;
			return parent.parent.parent;
		}
		
		
		protected override void informUpdateContent()
		{
			if(getParent() != null)
				getParent().informUpdateContent();
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




