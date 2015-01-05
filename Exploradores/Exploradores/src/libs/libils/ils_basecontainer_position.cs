using System;
using System.Collections.Generic;



/*----------------------------------------------------------------------------------
										ILS
----------------------------------------------------------------------------------*/
namespace ILS
{
	
	
	
	/*----------------------------------------------------------------------------------
										BaseContainer
	------------------------------------------------------------------------------------
	A group of visual components arranged using a layout.
	----------------------------------------------------------------------------------*/
	public abstract partial class BaseContainer : BaseComponent
	{
		// public functions
		public override void calculatePosition(int xPos, int yPos, Dimensions.ClipBox outterClipBox)
		{
			int innerWidth, innerHeight;
			innerWidth = (int)dimensions.afterMinimizeOutterX - (int)getTotalOffsetWidth();
			innerHeight = (int)dimensions.afterMinimizeOutterY - (int)getTotalOffsetHeight();
			if(innerWidth < 0)
				innerWidth = 0;
			if(innerHeight < 0)
				innerHeight = 0;
			
			int marginLeft = (int)getTotalOffsetWidth() / 2;
			int marginTop = (int)getTotalOffsetHeight() / 2;

			int innerPosX = xPos;
			int innerPosY = yPos;
			if(outterClipBox.offsetLeft < marginLeft)
				innerPosX += marginLeft - outterClipBox.offsetLeft;
			if(outterClipBox.offsetTop < marginTop)
				innerPosY += marginTop - outterClipBox.offsetTop;
			
			/*int offsetRight = (int)dimensions.afterMinimizeOutterX - marginLeft;
			int offsetTop = (int)dimensions.afterMinimizeOutterY - marginTop;
			int clipboxRight = outterClipBox.offsetLeft + (int)outterClipBox.width;
			int clipboxTop = outterClipBox.offsetTop + (int)outterClipBox.height;

			int innerWidth = (int)dimensions.afterMinimizeOutterX;
			int innerHeight = (int)dimensions.afterMinimizeOutterY;
			if(offsetRight < clipboxRight)
				innerWidth -= clipboxRight - offsetRight;
			if(offsetTop < clipboxTop)
				innerHeight -= clipboxTop - offsetTop;*/
			
			//innerWidth -= innerPosX - xPos;

			Dimensions.ClipBox innerClipBox = getComponentClipBox(outterClipBox,
													marginLeft,
													marginTop,
													(uint)innerWidth, (uint)innerHeight);
			

			foreach (Layer layer in getCurrentAlternative().layers)
			{
				Dimensions.ClipBox layerClipBox = getComponentClipBox(innerClipBox,
													layer.offsetX, layer.offsetY,
													layer.dimensions.afterMinimizeOutterX,
													layer.dimensions.afterMinimizeOutterY);
				int x, y;
				x = innerPosX;
				if(layer.offsetX > 0)
					x -= layer.offsetX;
				if(outterClipBox.offsetLeft < layer.offsetX && layer.offsetX > 0)
					x += - outterClipBox.offsetLeft + layer.offsetX;
				y = innerPosY;
				if(layer.offsetY > 0)
					y -= layer.offsetY;
				if(outterClipBox.offsetTop < layer.offsetY && layer.offsetY > 0)
					y += - outterClipBox.offsetTop + layer.offsetY;
				calculateLayerPosition(layer, x, y, layerClipBox);
			}

			dimensions.positionX = xPos;
			dimensions.positionY = yPos;
			dimensions.calculateFinalDrawSpace(outterClipBox);
		}
		
		
		// private functions
		protected void calculateLayerPosition(Layer layer,
										int xPos, int yPos,
										Dimensions.ClipBox layerClipBox)
		{
			int currentX, currentY, constOffsetX, constOffsetY;
			uint x, y, maxLine;
			
			bool minimizeWidth = false;
			if(layout.axisPriority == Layout.AxisPriority.VerticalFirst)
				minimizeWidth = true;
			
			constOffsetX = 0;
			constOffsetY = 0;
			layer.dimensions.positionX = xPos;
			layer.dimensions.positionY = yPos;
			layer.dimensions.calculateFinalDrawSpace(layerClipBox);
			adjustPosition(layer, ref constOffsetX, ref constOffsetY, xPos, yPos);
			
			currentX = 0;
			currentY = 0;
			maxLine = 0;
			
			int tableLineIndex = 0;
			int componentIndex = 0;
			for(; componentIndex < layer.group.Count && tableLineIndex < layer.dimensions.table.Count; ++componentIndex)
			{
				Drawable component = layer.group[componentIndex];
				Dimensions.TableLine tableLine = layer.dimensions.table[tableLineIndex];
				if(component == tableLine.nextLineDrawable)
				{
					if(minimizeWidth == true)
					{
						if(layer.offsetY > 0)
							currentY = 0;
						else
							currentY = layer.offsetY;
						if(layout.horizontalFlow == Layout.Flow.RightOrBottomFlow)
							currentX += (int)maxLine;
						else
							currentX -= (int)maxLine;
					}
					else
					{
						if(layer.offsetX > 0)
							currentX = 0;
						else
							currentX = layer.offsetX;
						if(layout.verticalFlow == Layout.Flow.RightOrBottomFlow)
							currentY += (int)maxLine;
						else
							currentY -= (int)maxLine;
					}
					maxLine = 0;
					++tableLineIndex;
				}
				
				x = component.dimensions.afterMinimizeOutterX;
				y = component.dimensions.afterMinimizeOutterY;
				
				int componentOffsetX, componentOffsetY;
				if(layout.horizontalFlow == Layout.Flow.RightOrBottomFlow)
					componentOffsetX = currentX;
				else
					componentOffsetX = currentX - (int)x;
				if(layout.verticalFlow == Layout.Flow.RightOrBottomFlow)
					componentOffsetY = currentY;
				else
					componentOffsetY = currentY - (int)y;
				
				Dimensions.ClipBox componentOutterBox = getComponentClipBox(layerClipBox, currentX, currentY, x, y);

				if(minimizeWidth == true)
				{
					if(maxLine < x)
						maxLine = x;
				}
				else
				{
					if(maxLine < y)
						maxLine = y;
				}
				
				component.calculatePosition(constOffsetX + componentOffsetX - layerClipBox.offsetLeft + componentOutterBox.offsetLeft,
											constOffsetY + componentOffsetY - layerClipBox.offsetTop + componentOutterBox.offsetTop,
											componentOutterBox);
				
				if(minimizeWidth == true)
				{
					if(layout.verticalFlow == Layout.Flow.RightOrBottomFlow)
						currentY += (int)y;
					else
						currentY -= (int)y;
				}
				else
				{
					if(layout.horizontalFlow == Layout.Flow.RightOrBottomFlow)
						currentX += (int)x;
					else
						currentX -= (int)x;
				}
			}
		}


		protected Dimensions.ClipBox getComponentClipBox(Dimensions.ClipBox parentClipBox,
									int offsetX, int offsetY, uint width, uint height)
		{
			int left, top, right, bottom;
			left = parentClipBox.offsetLeft - offsetX;
			top = parentClipBox.offsetTop - offsetY;
			right = left + (int)parentClipBox.width;
			bottom = top + (int)parentClipBox.height;
			
			if(left < 0)
				left = 0;
			if(right < 0)
				right = 0;
			if(top < 0)
				top = 0;
			if(bottom < 0)
				bottom = 0;
			
			if(left > (int)width)
				left = (int)width;
			if(right > (int)width)
				right = (int)width;
			if(top > (int)height)
				top = (int)height;
			if(bottom > (int)height)
				bottom = (int)height;

			Dimensions.ClipBox componentClipBox = new Dimensions.ClipBox();
			componentClipBox.offsetLeft = left;
			componentClipBox.offsetTop = top;
			componentClipBox.width = (uint)(right - left);
			componentClipBox.height = (uint)(bottom - top);

			return componentClipBox;
		}
		
		
		private void adjustPosition(Layer layer,
									ref int constOffsetX, ref int constOffsetY,
									int parentXPos, int parentYPos)
		{
			// horizontal calculations
			layer.dimensions.positionX = parentXPos;
			layer.dimensions.positionY = parentYPos;
			if(layout.horizontalAlignment == Layout.Alignment.LeftOrTopAlignment)
			{
				if(layer.offsetX > 0)
					layer.dimensions.positionX += layer.offsetX;
				if(layout.horizontalFlow == Layout.Flow.LeftOrTopFlow)
					layer.dimensions.positionX += (int)layer.dimensions.minOutterUnconstrainedX;
			}
			else
			{
				layer.dimensions.positionX += (int)dimensions.minOutterLayerConstrainedX;
				if(layer.offsetX > 0)
					layer.dimensions.positionX -= layer.offsetX;
				if(layout.horizontalFlow == Layout.Flow.RightOrBottomFlow)
					layer.dimensions.positionX -= (int)layer.dimensions.minOutterUnconstrainedX;
			}
			

			// vertical calculations
			if(layout.verticalAlignment == Layout.Alignment.LeftOrTopAlignment)
			{
				if(layer.offsetY > 0)
					layer.dimensions.positionY += layer.offsetY;
				if(layout.verticalFlow == Layout.Flow.LeftOrTopFlow)
					layer.dimensions.positionY += (int)layer.dimensions.minOutterUnconstrainedY;
			}
			else
			{
				layer.dimensions.positionY += (int)dimensions.minOutterLayerConstrainedY;
				if(layer.offsetY > 0)
					layer.dimensions.positionY -= layer.offsetY;
				if(layout.verticalFlow == Layout.Flow.RightOrBottomFlow)
					layer.dimensions.positionY -= (int)layer.dimensions.minOutterUnconstrainedY;
			}
			
			
			if(layer.dimensions.positionX < 0)
				layer.dimensions.positionX = 0;
			if(layer.dimensions.positionY < 0)
				layer.dimensions.positionY = 0;
			
			constOffsetX = layer.dimensions.positionX;
			constOffsetY = layer.dimensions.positionY;
			/*
			if(layout.horizontalAlignment == Layout.Alignment.LeftOrTopAlignment)
				constOffsetX += (int)getTotalOffsetWidth() / 2;
			else
				constOffsetX -= (int)getTotalOffsetWidth() / 2;
			if(layout.verticalAlignment == Layout.Alignment.LeftOrTopAlignment)
				constOffsetY += (int)getTotalOffsetHeight() / 2;
			else
				constOffsetY -= (int)getTotalOffsetHeight() / 2;*/
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




