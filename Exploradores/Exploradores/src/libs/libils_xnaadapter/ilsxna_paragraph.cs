using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
											Paragraph
	------------------------------------------------------------------------------------
	Text paragraph implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Paragraph : Container
	{
		// public variables
		public bool newLineAfterParagraph { get; set; }
		protected Container containerParagraph { get; set; }


		// constructors
		public Paragraph(Label text, uint maxInnerWidth = 400, ILS.Layer newParent = null) : base((Border)null, newParent)
		{
			newLineAfterParagraph = true;
			containerParagraph = null;

			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			setText(text, maxInnerWidth);
		}


		public Paragraph(Paragraph paragraph, ILS.Layer newParent = null) : base(paragraph, newParent)
		{
			newLineAfterParagraph = true;
			containerParagraph = null;

			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
		}
		

		// public functions
		public void setText(Label text, uint maxInnerWidth)
		{
			clearComponents();

			containerParagraph = new Container();
			containerParagraph.layout.enableLineWrap = true;
			addComponent(containerParagraph);
			setParagraphMaxWidth(maxInnerWidth);

			if(text == null)
				return;

			foreach(Label l in text.splitWords())
				containerParagraph.addComponent(l);
			
			if(newLineAfterParagraph == true)
			{
				Label label = (Label)text.clone();
				label.message = " ";
				addComponent(label);
			}
		}


		public void setParagraphMaxWidth(uint maxInnerWidth)
		{
			containerParagraph.sizeSettings.maxInnerWidth = maxInnerWidth;
		}


		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new Paragraph(this, newParent);
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


