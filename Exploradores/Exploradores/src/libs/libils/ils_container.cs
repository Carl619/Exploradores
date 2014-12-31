using System;
using System.Collections.Generic;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
										Container
	------------------------------------------------------------------------------------
	Template class for window management and representation.
	----------------------------------------------------------------------------------*/
	public class Container<T> : BaseContainer where T : BaseComponent, IBorder
	{
		// public variables
		public T border { get; set; }
		
		
		// constructors
		public Container(T newBorder, Layer newParent = null)
			: base(newParent)
		{
			border = null;
			if(newBorder != null)
				border = (T)newBorder.clone();
		}
		
		
		public Container(Container<T> container, Layer newParent = null)
			: base(container, newParent)
		{
			border = null;
			if(container != null)
				if(container.border != null)
					border = (T)container.border.clone();
		}
		
		
		// public functions
		public override BaseComponent clone(Layer newParent = null)
		{
			return new Container<T>(this, newParent);
		}


		public override bool hasBorder()
		{
			return border != null;
		}


		public override void setMinOutterParentConstraints(uint elementWidth, uint elementHeight)
		{
			base.setMinOutterParentConstraints(elementWidth, elementHeight);
			if(hasBorder() == true)
				border.minimize(getMinInnerConstrainedWidth() + getTotalInnerSpacingWidth(),
								getMinInnerConstrainedHeight() + getTotalInnerSpacingHeight());
		}


		public override uint getSingleBorderWidth()
		{
			if (border == null)
				return 0;
			return border.getSingleBorderWidth();
		}


		public override uint getSingleBorderHeight()
		{
			if (border == null)
				return 0;
			return border.getSingleBorderHeight();
		}


		public override uint getTotalBorderWidth()
		{
			if (border == null)
				return 0;
			return border.getTotalBorderWidth();
		}


		public override uint getTotalBorderHeight()
		{
			if (border == null)
				return 0;
			return border.getTotalBorderHeight();
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




