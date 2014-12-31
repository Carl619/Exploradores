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
											Layout
	------------------------------------------------------------------------------------
	Class for determining how sub-components are displayed inside a container.
	----------------------------------------------------------------------------------*/
	public class Layout
	{
		// public enums
		/* The directionality of component flow
		(Latin script paragraphs are usually HorizontalFirst). */
		public enum AxisPriority
		{
			HorizontalFirst,
			VerticalFirst
		}
		/* Where are the components located
		(Latin script paragraphs are usually LeftOrUpperAlignment). */
		public enum Alignment
		{
			LeftOrUpperAlignment,
			RightOrLowerAlignment,
			CenterAlignment
		}
		/* How spread out the content is
		(Latin script paragraphs are usually Minimize). */
		public enum SizePolicy
		{
			Minimize,
			Neutral,
			Maximize
		}
		/* In which direction are the components displayed
		(Latin script paragraphs are usually RightOrLowerFlow). */
		public enum Flow
		{
			LeftOrUpperFlow,
			RightOrLowerFlow
		}
		
		
		// public variables
		public AxisPriority axisPriority { get; set; }
		public Alignment horizontalAlignment { get; set; }
		public Alignment verticalAlignment { get; set; }
		public Flow horizontalFlow { get; set; }
		public Flow verticalFlow { get; set; }
		public SizePolicy horizontalSizePolicy { get; set; }
		public SizePolicy verticalSizePolicy { get; set; }
		
			// equal cell width or height and squares for sub-components
			// can cause conflict with sub-component fixed or max size
		public bool equalCellWidth { get; set; }
		public bool equalCellHeight { get; set; }
		public bool enableLineWrap { get; set; } // allow sub-component newline if they don't fit the container
		public bool noClip { get; set; } // true to display components outside their container if they don't fit
		
		
		// constructor, configured for Latin script single text line, with neutral size policy
		public Layout()
		{
			axisPriority = AxisPriority.HorizontalFirst;
			horizontalAlignment = Alignment.LeftOrUpperAlignment;
			verticalAlignment = Alignment.LeftOrUpperAlignment;
			horizontalFlow = Flow.RightOrLowerFlow;
			verticalFlow = Flow.RightOrLowerFlow;
			horizontalSizePolicy = SizePolicy.Neutral;
			verticalSizePolicy = SizePolicy.Neutral;

			equalCellWidth = false;
			equalCellHeight = false;
			enableLineWrap = false;
			noClip = false;
		}


		public Layout(Layout layout)
		{
			axisPriority = layout.axisPriority;
			horizontalAlignment = layout.horizontalAlignment;
			verticalAlignment = layout.verticalAlignment;
			horizontalFlow = layout.horizontalFlow;
			verticalFlow = layout.verticalFlow;
			horizontalSizePolicy = layout.horizontalSizePolicy;
			verticalSizePolicy = layout.verticalSizePolicy;

			equalCellWidth = layout.equalCellWidth;
			equalCellHeight = layout.equalCellHeight;
			enableLineWrap = layout.enableLineWrap;
			noClip = layout.noClip;
		}
		
		
		// public functions
		public Layout clone()
		{
			Layout newLayout = new Layout(this);
			return newLayout;
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




