using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
										Class list
	------------------------------------------------------------------------------------
	  - Dimensions
	  - Dimensions.TableLine
	  - Dimensions.Table
	  - Dimensions.ClipBox
	----------------------------------------------------------------------------------*/



	/*----------------------------------------------------------------------------------
										Dimensions
	------------------------------------------------------------------------------------
	Class for storing component dimensions, and position at execution time.
	----------------------------------------------------------------------------------*/
	public partial class Dimensions
	{
		// public variables
			// absolute coordinates of the top left corner of the component
		public int positionX { get; set; }
		public int positionY { get; set; }
			// amount of available space for drawing the component
		public ClipBox drawSpace { get; set; }
			// minimum sizes needed
		public uint minOutterUnconstrainedX { get; set; }
		public uint minOutterUnconstrainedY { get; set; }
		public uint minOutterLayerConstrainedX { get; set; }
		public uint minOutterLayerConstrainedY { get; set; }
			// minimum sizes, constrained by inheritance
		public uint minOutterInheritedX { get; set; }
		public uint minOutterInheritedY { get; set; }
		public uint minOutterConstrainedX { get; set; }
		public uint minOutterConstrainedY { get; set; }
		public Table table { get; set; }
		
		
		// constructor
		public Dimensions()
		{
			clear();
		}
		
		
		// public functions
		public Dimensions clone()
		{
			Dimensions newDimensions = new Dimensions();
			
			newDimensions.positionX = positionX;
			newDimensions.positionY = positionY;
			newDimensions.drawSpace = drawSpace.clone();
			
			newDimensions.minOutterUnconstrainedX = minOutterUnconstrainedX;
			newDimensions.minOutterUnconstrainedY = minOutterUnconstrainedY;
			newDimensions.minOutterLayerConstrainedX = minOutterLayerConstrainedX;
			newDimensions.minOutterLayerConstrainedY = minOutterLayerConstrainedY;
			
			newDimensions.minOutterInheritedX = minOutterInheritedX;
			newDimensions.minOutterInheritedY = minOutterInheritedY;
			newDimensions.minOutterConstrainedX = minOutterConstrainedX;
			newDimensions.minOutterConstrainedY = minOutterConstrainedY;

			newDimensions.table = table.clone();
			
			return newDimensions;
		}
		
		
		public void clear()
		{
			positionX = 0;
			positionY = 0;
			drawSpace = new ClipBox();
			
			minOutterUnconstrainedX = 0;
			minOutterUnconstrainedY = 0;
			minOutterLayerConstrainedX = 0;
			minOutterLayerConstrainedY = 0;
			
			minOutterInheritedX = 0;
			minOutterInheritedY = 0;
			minOutterConstrainedX = 0;
			minOutterConstrainedY = 0;
			
			table = new Table();
		}
		
		
		public void calculateFinalDrawSpace(ClipBox clipBox)
		{
			drawSpace.width = minOutterConstrainedX;
			drawSpace.height = minOutterConstrainedY;
			drawSpace.offsetTop = clipBox.offsetTop;
			drawSpace.offsetLeft = clipBox.offsetLeft;
			
			if (drawSpace.width > clipBox.width)
				drawSpace.width = clipBox.width;
			if (drawSpace.height > clipBox.height)
				drawSpace.height = clipBox.height;
		}
	}
	
	
	
	/*----------------------------------------------------------------------------------
									Dimensions.Table
	----------------------------------------------------------------------------------*/
	public partial class Dimensions
	{
		public class Table : List<TableLine>
		{
			public Table clone()
			{
				Table newTable = new Table();
				foreach(TableLine line in this) // Loop through table with foreach
					newTable.Add(line.clone());
				return newTable;
			}
		}
	}
	
	
	
	/*----------------------------------------------------------------------------------
									Dimensions.TableLine
	------------------------------------------------------------------------------------
	Class representing a row or a column, depending on the AxisPriority.
	----------------------------------------------------------------------------------*/
	public partial class Dimensions
	{
		public class TableLine
		{
			// public members
			public bool empty { get; set; }
			public uint size { get; set; }
				// maximum width and height of any subcomponent in the row/column
			public uint maxX { get; set; }
			public uint maxY { get; set; }
				// total sum of width and height of any subcomponent in the row/column
			public uint sumX { get; set; }
			public uint sumY { get; set; }
				// the next drawable item that needs a newline before it
			public Object nextLineDrawable { get; set; }
			
			
			// constructor
			public TableLine()
			{
				clear();
			}
			
			
			// public functions
			public TableLine clone()
			{
				TableLine newDimensionsTableLine = new TableLine();
				
				newDimensionsTableLine.empty = empty;
				newDimensionsTableLine.size = size;

				newDimensionsTableLine.maxX = maxX;
				newDimensionsTableLine.maxY = maxY;
				newDimensionsTableLine.sumX = sumX;
				newDimensionsTableLine.sumY = sumY;

				newDimensionsTableLine.nextLineDrawable = nextLineDrawable;
				
				return newDimensionsTableLine;
			}
			
			
			public void clear()
			{
				empty = true;
				size = 0;

				maxX = 0;
				maxY = 0;
				sumX = 0;
				sumY = 0;

				nextLineDrawable = null;
			}
		}
	}



	/*----------------------------------------------------------------------------------
									Dimensions.ClipBox
	------------------------------------------------------------------------------------
	Class representing a box that clips the display of a Drawable
	----------------------------------------------------------------------------------*/
	public partial class Dimensions
	{
		public class ClipBox
		{
			// public members
			public int offsetLeft { get; set; }
			public int offsetTop { get; set; }
			public uint width { get; set; }
			public uint height { get; set; }
			
			
			// constructor
			public ClipBox()
			{
				clear();
			}
			
			
			// public functions
			public void clear()
			{
				offsetLeft = 0;
				offsetTop = 0;
				width = 0;
				height = 0;
			}


			public ClipBox clone()
			{
				ClipBox newClipBox = new ClipBox();

				newClipBox.offsetLeft = offsetLeft;
				newClipBox.offsetTop = offsetTop;
				newClipBox.width = width;
				newClipBox.height = height;

				return newClipBox;
			}
			
			
			public void adjust(int xPos, int yPos,
								ClipBox containerClipBox,
								Layout layout)
			{
				if (layout.horizontalFlow == Layout.Flow.RightOrLowerFlow)
				{
					if (xPos >= containerClipBox.width)
						width = 0;
					else if (xPos + width >= containerClipBox.width)
						width = (uint)((int)containerClipBox.width - xPos);
				}
				else
				{
					if (xPos < 0)
						width = 0;
					else if (xPos - width < 0)
						width = (uint)xPos;
				}
				
				if (layout.verticalFlow == Layout.Flow.RightOrLowerFlow)
				{
					if (yPos >= containerClipBox.height)
						height = 0;
					else if (yPos + height >= containerClipBox.height)
						height = (uint)((int)containerClipBox.height - yPos);
				}
				else
				{
					if (yPos < 0)
						height = 0;
					else if (yPos - height < 0)
						height = (uint)yPos;
				}
			}
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




