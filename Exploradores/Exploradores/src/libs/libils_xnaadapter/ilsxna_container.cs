using System;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
										Container
	------------------------------------------------------------------------------------
	Container implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Container : ILS.Container<Border>
	{
		// constructor
		public Container(Border border = null, ILS.Layer newParent = null)
			: base(border, newParent)
		{
		}


		public Container(Container container, ILS.Layer newParent = null)
			: base(container, newParent)
		{
		}
		

		// public functions
		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new Container(this, newParent);
		}


		// protected functions
		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			
			if(border != null)
				border.draw(renderSurface);
			base.draw(renderSurface);
		}


		public override void calculatePosition(int xPos, int yPos, ILS.Dimensions.ClipBox clipBox)
		{
			base.calculatePosition(xPos, yPos, clipBox);
			if(hasBorder() == true)
			{
				int innerWidth, innerHeight;
				innerWidth = (int)dimensions.afterMinimizeOutterX - (int)getTotalOutterSpacingWidth();
				innerHeight = (int)dimensions.afterMinimizeOutterY - (int)getTotalOutterSpacingHeight();
				if(innerWidth < 0)
					innerWidth = 0;
				if(innerHeight < 0)
					innerHeight = 0;
				int innerPosX = xPos + (int)getSingleOutterSpacingWidth();
				int innerPosY = yPos + (int)getSingleOutterSpacingHeight();
				
				ILS.Dimensions.ClipBox borderClipBox = getComponentClipBox(clipBox, 0, 0,
														(uint)innerWidth, (uint)innerHeight);
				border.calculatePosition(innerPosX, innerPosY, borderClipBox);
			}
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


