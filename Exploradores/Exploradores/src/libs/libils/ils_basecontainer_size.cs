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
		public override void minimize(uint outterConstraintWidth, uint outterConstraintHeight)
		{
			if(visible == false)
				return;
			if(getCurrentAlternative() == null)
				return;
			
			dimensions.clear();
			
			uint innerConstraintWidth = outterConstraintWidth;
			uint innerConstraintHeight = outterConstraintHeight;
			adjustConstraintsToInner(ref innerConstraintWidth, ref innerConstraintHeight);
			
			bool minimizeWidth = false;
			if(layout.axisPriority == Layout.AxisPriority.VerticalFirst)
				minimizeWidth = true;
			
			bool constraintEnabled = false;
			uint constraint = 0;
			
			if(minimizeWidth == true && innerConstraintHeight > 0)
			{
				constraintEnabled = true;
				constraint = innerConstraintHeight;
			}
			
			if(minimizeWidth == false && innerConstraintWidth > 0)
			{
				constraintEnabled = true;
				constraint = innerConstraintWidth;
			}
			
			minimizeAlternative(getCurrentAlternative().layers, minimizeWidth, constraintEnabled, constraint);
		}
		
		
		// private functions
		private void minimizeAlternative(LayerGroup layers, bool minimizeWidth, bool constraintEnabled, uint innerConstraint)
		{
			dimensions.minOutterUnconstrainedX = sizeSettings.minInnerWidth + getTotalOffsetWidth();
			dimensions.minOutterUnconstrainedY = sizeSettings.minInnerHeight + getTotalOffsetHeight();
			dimensions.minOutterLayerConstrainedX = sizeSettings.minInnerWidth + getTotalOffsetWidth();
			dimensions.minOutterLayerConstrainedY = sizeSettings.minInnerHeight + getTotalOffsetHeight();

			foreach(Layer layer in layers)
			{
				bool cEnabled;
				uint innerC;
				if(layout.enableLineWrap == true)
				{
					cEnabled = constraintEnabled;
					innerC = innerConstraint;
				}
				else
				{
					cEnabled = false;
					innerC = 0;
				}
				
				minimizeLayer(layer, minimizeWidth, cEnabled, innerC);

				int x, y;
				x = (int)layer.dimensions.afterMinimizeOutterX + layer.offsetX;
				y = (int)layer.dimensions.afterMinimizeOutterY + layer.offsetY;
				x = x < 0 ? 0 : x;
				y = y < 0 ? 0 : y;
				
				integrateLayerData((uint)x, (uint)y);
			}
			
			dimensions.minOutterConstrainedX = dimensions.minOutterLayerConstrainedX;
			dimensions.minOutterConstrainedY = dimensions.minOutterLayerConstrainedY;
		}
		
		
		private void minimizeLayer(Layer layer, bool minimizeWidth,
								bool constraintEnabled, uint innerConstraint)
		{
			uint maxWidth, maxHeight, totalWidth, totalHeight;
			maxWidth = maxHeight = totalWidth = totalHeight = 0;
			
			layer.dimensions.clear();
			minimizeLayerComponents(layer, ref maxWidth, ref maxHeight);
			
			Dimensions.TableLine line = new Dimensions.TableLine();
			getLayerLineBreaks(layer, line, maxWidth, maxHeight,
							minimizeWidth, constraintEnabled, innerConstraint);
			
			if(line.empty == false)
			{
				layer.dimensions.table.Add(line);
				line = null;
			}
			
			totalWidth = 0;
			totalHeight = 0;
			maxWidth = 0;
			maxHeight = 0;
			foreach(Dimensions.TableLine lineIterator in layer.dimensions.table)
			{
				if(maxWidth < lineIterator.sumX)
					maxWidth = lineIterator.sumX;
				if(maxHeight < lineIterator.sumY)
					maxHeight = lineIterator.sumY;
				
				if(minimizeWidth == true)
				{
					totalWidth += lineIterator.sumX;
					totalHeight = maxHeight;
				}
				else // minimizeWidth == false
				{
					totalWidth = maxWidth;
					totalHeight += lineIterator.sumY;
				}
			}
			
			
			layer.dimensions.minOutterUnconstrainedX = totalWidth;
			layer.dimensions.minOutterUnconstrainedY = totalHeight;
			layer.dimensions.minOutterLayerConstrainedX = totalWidth;
			layer.dimensions.minOutterLayerConstrainedY = totalHeight;
			layer.dimensions.minOutterConstrainedX = totalWidth;
			layer.dimensions.minOutterConstrainedY = totalHeight;
		}


		private void adjustConstraintsToInner(ref uint constraintWidth, ref uint constraintHeight)
		{
			uint aux = 0;

			if(constraintWidth >= getTotalOffsetWidth())
				constraintWidth -= getTotalOffsetWidth();
			else
				constraintWidth = 0;
			if(constraintHeight >= getTotalOffsetHeight())
				constraintHeight -= getTotalOffsetHeight();
			else
				constraintHeight = 0;
			
			aux = sizeSettings.maxInnerWidth;
			if (aux > 0)
			{
				if (constraintWidth > aux || constraintWidth == 0)
					constraintWidth = aux;
			}
			
			aux = sizeSettings.maxInnerHeight;
			if (aux > 0)
			{
				if (constraintHeight > aux || constraintHeight == 0)
					constraintHeight = aux;
			}
		}


		private void integrateLayerData(uint layerWidth, uint layerHeight)
		{
			if (dimensions.minOutterUnconstrainedX < layerWidth + getTotalOffsetWidth())
				dimensions.minOutterUnconstrainedX = layerWidth + getTotalOffsetWidth();
			if (dimensions.minOutterLayerConstrainedX < layerWidth + getTotalOffsetWidth())
				dimensions.minOutterLayerConstrainedX = layerWidth + getTotalOffsetWidth();
			
			if (dimensions.minOutterUnconstrainedY < layerHeight + getTotalOffsetHeight())
				dimensions.minOutterUnconstrainedY = layerHeight + getTotalOffsetHeight();
			if (dimensions.minOutterLayerConstrainedY < layerHeight + getTotalOffsetHeight())
				dimensions.minOutterLayerConstrainedY = layerHeight + getTotalOffsetHeight();
			
			if (dimensions.minOutterLayerConstrainedX > sizeSettings.maxInnerWidth + getTotalOffsetWidth() && sizeSettings.maxInnerHeight > 0)
				dimensions.minOutterLayerConstrainedX = sizeSettings.maxInnerWidth + getTotalOffsetWidth();
			
			if (dimensions.minOutterLayerConstrainedY > sizeSettings.maxInnerHeight + getTotalOffsetHeight() && sizeSettings.maxInnerHeight > 0)
				dimensions.minOutterLayerConstrainedY = sizeSettings.maxInnerHeight + getTotalOffsetHeight();
		}


		private void minimizeLayerComponents(Layer layer, ref uint maxWidth, ref uint maxHeight)
		{
			foreach(BaseComponent i in layer.group)
			{
				i.minimize(0, 0);
				uint elementWidth, elementHeight;
				elementWidth = i.getMinOutterUnconstrainedWidth();
				elementHeight = i.getMinOutterUnconstrainedHeight();

				if(maxWidth < elementWidth)
					maxWidth = elementWidth;
				if(maxHeight < elementHeight)
					maxHeight = elementHeight;
			}
		}


		private void getLayerLineBreaks(Layer layer, Dimensions.TableLine line,
							uint maxWidth, uint maxHeight, bool minimizeWidth,
							bool constraintEnabled, uint innerConstraint)
		{
			foreach(BaseComponent component in layer.group)
			{
				uint elementWidth, elementHeight;
				elementWidth = component.getMinOutterUnconstrainedWidth();
				elementHeight = component.getMinOutterUnconstrainedHeight();
				if(layout.equalCellWidth == true)
					elementWidth = maxWidth;
				if(layout.equalCellHeight == true)
					elementHeight = maxHeight;
				
				// important -> impose component restrictions when they don't fit
				component.setMinOutterParentConstraints(elementWidth, elementHeight);

				if(layout.enableLineWrap == true && constraintEnabled == true)
				{
					if(minimizeWidth == true)
					{
						if(line.empty == false && line.sumY + elementHeight > innerConstraint)
						{
							line.nextLineDrawable = component;
							layer.dimensions.table.Add(line);
							line = new Dimensions.TableLine();
						}
					}
					else // minimizeWidth == false
					{
						if(line.empty == false && line.sumX + elementWidth > innerConstraint)
						{
							line.nextLineDrawable = component;
							layer.dimensions.table.Add(line);
							line = new Dimensions.TableLine();
						}
					}
				}
				
				if(line.maxX < elementWidth)
					line.maxX = elementWidth;
				if(line.maxY < elementHeight)
					line.maxY = elementHeight;
				
				if(minimizeWidth == true)
				{
					line.sumX = line.maxX;
					line.sumY += elementHeight;
				}
				else // minimizeWidth == false
				{
					line.sumX += elementWidth;
					line.sumY = line.maxY;
				}
				
				line.empty = false;
				++line.size;
			}
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




