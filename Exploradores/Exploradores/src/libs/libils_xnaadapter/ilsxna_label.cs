using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
											Label
	------------------------------------------------------------------------------------
	Text label implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Label : ILS.Component<SpriteFont>
	{
		// public variables
		public String message { get; set; }
		public Color color { get; set; }
		public uint minLateralSpacing { get; set; }

		// constructors
		public Label(ILS.Layer newParent = null) : base(newParent)
		{
			message = null;
			color = Color.Black;
			minLateralSpacing = 0;
		}


		public Label(Label label, ILS.Layer newParent = null) : this(newParent)
		{
			if(label == null)
				return;
			message = (String)label.message.Clone();
			color = new Color(label.color.R, label.color.G, label.color.B, label.color.A);
			innerComponent = label.innerComponent;
		}
		

		// public functions
		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new Label(this, newParent);
		}


		public List<Label> splitWords()
		{
			List<Label> list = new List<Label>();
			char[] splitPoints = new char[]
			{
				' ',
				'\n',
			};

			Label label;
			String[] words = message.Split(splitPoints);
			int count = 0;
			foreach(String s in words)
			{
				label = (Label)clone();
				label.message = (String)s.Clone();
				if(count + 1 < words.Length)
					label.message = label.message + " ";
				list.Add(label);
				++count;
			}

			return list;
		}
		
		
		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			if(innerComponent == null)
				return;
			if(message == null)
				return;
			if(message.Equals(""))
				return;
			
			SpriteBatch spriteBatch = ((Window)renderSurface).innerWindow.spriteBatch;
			spriteBatch.DrawString(innerComponent, message,
									new Vector2(dimensions.positionX + minLateralSpacing, dimensions.positionY),
									color);
		}


		// protected functions
		protected override uint getMinInnerUnconstrainedWidth()
		{
			if(message == null)
				return getAdjustedMinWidth(0);
			if(message.Equals(""))
				return getAdjustedMinWidth(0);
			
			return getAdjustedMinWidth((uint)innerComponent.MeasureString(message).X + 2 * minLateralSpacing);
		}
		
		
		protected override uint getMinInnerUnconstrainedHeight()
		{
			if(message == null)
				return getAdjustedMinHeight(0);
			if(message.Equals(""))
				return getAdjustedMinHeight(0);
			
			return getAdjustedMinHeight((uint)innerComponent.MeasureString(message).Y);
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


