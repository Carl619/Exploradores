using System;




/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
											IBorder
	------------------------------------------------------------------------------------
	Interface for borders
	----------------------------------------------------------------------------------*/
	public interface IBorder
	{
		uint getSingleBorderWidth();
		uint getSingleBorderHeight();
		
		uint getTotalBorderWidth();
		uint getTotalBorderHeight();

		BaseComponent clone(ILS.Layer newParent);
		void minimize(uint outterConstraintWidth, uint outterConstraintHeight);
		void calculatePosition(int xPos, int yPos, ILS.Dimensions.ClipBox clipBox);
		void draw(Object renderSurface);
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




