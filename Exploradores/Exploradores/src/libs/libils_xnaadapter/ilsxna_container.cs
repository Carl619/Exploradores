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
			
			if(border == null)
			{
				base.draw(renderSurface);
				return;
			}

			/*ILS.Dimensions.ClipBox clipBox = new ILS.Dimensions.ClipBox();
			clipBox.width = getFinalWidth() - (int)2*layout.outterSpacing;
			clipBox.height = getFinalHeight() - (int)2*layout.outterSpacing;

			uint width, height;
			if(getParent() != null)
			{
				width = getMinOutterConstrainedWidth() - (int)2*layout.outterSpacing;
				height = getMinOutterConstrainedHeight() - (int)2*layout.outterSpacing;
			}
			else
			{
				width = clipBox.width;
				height = clipBox.height;
			}*/

			border.draw(renderSurface);
			base.draw(renderSurface);
		}


		public override void minimize(uint outterConstraintWidth, uint outterConstraintHeight)
		{
			base.minimize(outterConstraintWidth, outterConstraintHeight);
		}


		public override void calculatePosition(int xPos, int yPos, ILS.Dimensions.ClipBox clipBox)
		{
			base.calculatePosition(xPos, yPos, clipBox);
			if (hasBorder() == true)
			{
				clipBox.width += getTotalBorderWidth();
				clipBox.height += getTotalBorderHeight();
				clipBox.width += getTotalInnerSpacingWidth();
				clipBox.height += getTotalInnerSpacingHeight();
				border.calculatePosition(xPos + (int)getSingleOutterSpacingWidth(),
										yPos + (int)getSingleOutterSpacingHeight(),
										clipBox);
			}
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


