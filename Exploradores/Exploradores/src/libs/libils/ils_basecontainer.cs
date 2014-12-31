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
	public abstract class BaseContainer : BaseComponent
	{
		// public variables
		public Layout layout { get; set; }
		public Dictionary<String, int> alternativeNames { get; protected set; }
		public uint contentSpacingX { get; set; }
		public uint contentSpacingY { get; set; }
		// protected variables
		protected Alternatives alternatives { get; set; }
		protected int currentAlternativeIndex { get; set; }
		
		
		// constructors
		public BaseContainer(Layer newParent = null)
			: base(newParent)
		{
			layout = new Layout();
			alternativeNames = new Dictionary<String, int>();

			contentSpacingX = 0;
			contentSpacingY = 0;

			alternatives = new Alternatives();
			alternatives.Add(new SingleAlternative(this));
			currentAlternativeIndex = 0;
		}
		
		
		public BaseContainer(BaseContainer container, Layer newParent = null)
			: base(container, newParent)
		{
			layout = container.layout.clone();

			alternativeNames = new Dictionary<String,int>();
			foreach(KeyValuePair<String, int> v in container.alternativeNames)
				alternativeNames.Add(v.Key, v.Value);
			
			contentSpacingX = container.contentSpacingX;
			contentSpacingY = container.contentSpacingY;

			alternatives = container.alternatives.clone(this);
			currentAlternativeIndex = container.currentAlternativeIndex;
		}
		
		
		// public functions
		public override void applyUpdateContent()
		{
			if(requestedContentUpdate == true)
				updateContent();
			foreach(SingleAlternative alt in alternatives)
			{
				foreach(Layer i in alt.layers)
					i.applyUpdateContent();
			}
		}


		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			if (getCurrentAlternative() == null)
				return;
			
			foreach(Layer i in getCurrentAlternative().layers)
				draw(i, renderSurface);
		}
		
		
		public override void minimize(uint outterConstraintWidth, uint outterConstraintHeight)
		{
			if(visible == false)
				return;
			if(getCurrentAlternative() == null)
				return;
			
			uint innerConstraintWidth = outterConstraintWidth;
			uint innerConstraintHeight = outterConstraintHeight;
			adjustConstraintsToInner(ref innerConstraintWidth, ref innerConstraintHeight);
			
			bool minimizeWidth = false;
			if(layout.axisPriority == Layout.AxisPriority.VerticalFirst)
				minimizeWidth = true;
			
			bool constraintEnabled = false;
			uint constraint;
			constraint = 0;
			
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
			
			dimensions.clear();
			minimize(getCurrentAlternative().layers, minimizeWidth, constraintEnabled, constraint);
		}
		
		
		public override bool isFlexible()
		{
			if (layout.horizontalSizePolicy == Layout.SizePolicy.Minimize)
				return false;
			if (layout.verticalSizePolicy == Layout.SizePolicy.Minimize)
				return false;
			if(layout.enableLineWrap == false)
				return false;
			return true;
		}
		
		
		public virtual SingleAlternative getCurrentAlternative()
		{
			if (currentAlternativeIndex < 0 || currentAlternativeIndex >= (int)alternatives.Count)
				return null;
			return alternatives[currentAlternativeIndex];
		}
		
		
		public virtual void addComponent(BaseComponent subComponent)
		{
			if(subComponent == null)
				return;
			if(getCurrentAlternative() == null)
				return;
			if (getCurrentAlternative().layers.Count == 0)
			{
				Layer newLayer = new Layer(getCurrentAlternative());
				getCurrentAlternative().layers.Add(newLayer);
			}
			getCurrentAlternative().getCurrentLayer().group.Add(subComponent);
			subComponent.parent = getCurrentAlternative().getCurrentLayer();
		}
		
		
		public virtual void setNumberAlternatives(uint numberOfAlternatives)
		{
			if(numberOfAlternatives == 0)
				numberOfAlternatives = 1;
			
			if(numberOfAlternatives > alternatives.Count)
			{
				for(int i=alternatives.Count; i<numberOfAlternatives; ++i)
				{
					alternatives.Add(new SingleAlternative(this));
				}
			}
			else if (numberOfAlternatives < alternatives.Count)
			{
				alternatives.RemoveRange((int)numberOfAlternatives,
									alternatives.Count - (int)numberOfAlternatives);
			}
			setCurrentAlternative(currentAlternativeIndex);
		}
		
		
		public virtual void setCurrentAlternative(int alternativeIndex)
		{
			if (alternativeIndex < 0 || alternativeIndex >= (int)alternatives.Count)
				return;
			else
				currentAlternativeIndex = alternativeIndex;
		}
		
		
		public virtual void setCurrentLayer(uint layerIndex)
		{
			if (getCurrentAlternative() == null)
				return;
			getCurrentAlternative().defaultLayer = layerIndex;
		}
		
		
		public virtual void clearComponents(bool allAlternatives = true)
		{
			if(allAlternatives == true || getCurrentAlternative() == null)
			{
				alternatives.Clear();
				SingleAlternative newAlternative = new SingleAlternative(this);
				alternatives.Add(newAlternative);
				currentAlternativeIndex = 0;
			}
			else
			{
				alternatives[currentAlternativeIndex].clear(false);
			}
		}
		
		
		public override void mouseAction(MouseEvent mouseEvent, List<Drawable> drawables)
		{
			base.mouseAction(mouseEvent, drawables);
			if(getCurrentAlternative() == null)
				return;
			
			for(int i = getCurrentAlternative().layers.Count - 1; i>=0; --i)
			{
				Layer layer = getCurrentAlternative().layers[i];
				foreach(BaseComponent j in layer.group)
				{
					int posX = j.getDrawPositionX();
					int posY = j.getDrawPositionY();
					uint spaceX = j.getFinalWidth();
					uint spaceY = j.getFinalHeight();
					if(posX > mouseEvent.actualPositionX)
						continue;
					if(posY > mouseEvent.actualPositionY)
						continue;
					if(posX + (int)spaceX < mouseEvent.actualPositionX)
						continue;
					if(posY + (int)spaceY < mouseEvent.actualPositionY)
						continue;
					
					int drawablesCount = drawables.Count;
					j.mouseAction(mouseEvent, drawables);
					if(drawablesCount < drawables.Count)
						return;
				}
				if(layer.blockSubsequentLayerEvents == true)
					return;
			}
		}
		
		
		public override void calculatePosition(int xPos, int yPos, Dimensions.ClipBox clipBox)
		{
			if (getCurrentAlternative() == null)
				return;
			
			foreach(Layer i in getCurrentAlternative().layers)
				calculatePosition(i, xPos, yPos, clipBox);
			
			dimensions.positionX = xPos;
			dimensions.positionY = yPos;
			dimensions.calculateFinalDrawSpace(clipBox);
		}
		
		
		// protected functions
		protected override uint getMinInnerUnconstrainedWidth()
		{return getMinInnerLayerConstrainedWidth();
		}
		
		
		protected override uint getMinInnerUnconstrainedHeight()
		{
			return getMinInnerLayerConstrainedHeight();
		}


		protected uint getMinInnerLayerConstrainedWidth()
		{
			if(visible == false)
				return 0;
			uint value = dimensions.minOutterLayerConstrainedX;
			if(value < getTotalOffsetWidth())
				value = 0;
			else
				value -= getTotalOffsetWidth();
			if(value < sizeSettings.minInnerWidth)
				value = sizeSettings.minInnerWidth;
			return value;
		}
		
		
		protected uint getMinInnerLayerConstrainedHeight()
		{
			if(visible == false)
				return 0;
			uint value = dimensions.minOutterLayerConstrainedY;
			if(value < getTotalOffsetHeight())
				value = 0;
			else
				value -= getTotalOffsetHeight();
			if(value < sizeSettings.minInnerHeight)
				value = sizeSettings.minInnerHeight;
			return value;
		}
		
		
		// private functions
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
		
		
		private void minimize(LayerGroup layers, bool minimizeWidth, bool constraintEnabled, uint innerConstraint)
		{
			foreach(Layer i in layers)
			{
				if (minimizeWidth == true)
				{
					if (i.offsetY > innerConstraint)
						innerConstraint = 0;
					else
						innerConstraint -= (uint)i.offsetY;
				}
				if (minimizeWidth == false)
				{
					if (i.offsetX > innerConstraint)
						innerConstraint = 0;
					else
						innerConstraint -= (uint)i.offsetX;
				}
				
				minimize(i, minimizeWidth, constraintEnabled, innerConstraint);
				
				if (dimensions.minOutterUnconstrainedX < i.dimensions.minOutterUnconstrainedX + (uint)i.offsetX)
					dimensions.minOutterUnconstrainedX = i.dimensions.minOutterUnconstrainedX + (uint)i.offsetX;
				if (dimensions.minOutterLayerConstrainedX < i.dimensions.minOutterLayerConstrainedX + (uint)i.offsetX)
					dimensions.minOutterLayerConstrainedX = i.dimensions.minOutterLayerConstrainedX + (uint)i.offsetX;
				
				if (dimensions.minOutterUnconstrainedY < i.dimensions.minOutterUnconstrainedY + (uint)i.offsetY)
					dimensions.minOutterUnconstrainedY = i.dimensions.minOutterUnconstrainedY + (uint)i.offsetY;
				if (dimensions.minOutterLayerConstrainedY < i.dimensions.minOutterLayerConstrainedY + (uint)i.offsetY)
					dimensions.minOutterLayerConstrainedY = i.dimensions.minOutterLayerConstrainedY + (uint)i.offsetY;
			}
			
			setMinOutterConstrainedWidth(getMinInnerLayerConstrainedWidth() + getTotalOffsetWidth());
			setMinOutterConstrainedHeight(getMinInnerLayerConstrainedHeight() + getTotalOffsetHeight());
		}
		
		
		private void minimize(Layer layer,
								bool minimizeWidth, bool constraintEnabled,
								uint innerConstraint)
		{
			uint maxWidth, maxHeight, totalWidth, totalHeight;
			maxWidth = 0;
			maxHeight = 0;
			totalWidth = 0;
			totalHeight = 0;
			
			layer.dimensions.clear();
			
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
			
			Dimensions.TableLine line = new Dimensions.TableLine();
			foreach(BaseComponent i in layer.group)
			{
				uint elementWidth, elementHeight;
				elementWidth = i.getMinOutterUnconstrainedWidth();
				elementHeight = i.getMinOutterUnconstrainedHeight();
				if(layout.equalCellWidth == true)
					elementWidth = maxWidth;
				if(layout.equalCellHeight == true)
					elementHeight = maxHeight;
				
				i.setMinOutterParentConstraints(elementWidth, elementHeight);
				
				if(layout.enableLineWrap == true && constraintEnabled == true)
				{
					if(minimizeWidth == true)
					{
						if(line.empty == false && line.sumY + elementHeight > innerConstraint)
						{
							line.nextLineDrawable = i;
							layer.dimensions.table.Add(line);
							line = new Dimensions.TableLine();
						}
					}
					else // minimizeWidth == false
					{
						if(line.empty == false && line.sumX + elementWidth > innerConstraint)
						{
							line.nextLineDrawable = i;
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
			
			
			if(line.empty == false)
			{
				layer.dimensions.table.Add(line);
				line = new Dimensions.TableLine();
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
			
			if(layer.dimensions.minOutterLayerConstrainedX < sizeSettings.minInnerWidth)
				layer.dimensions.minOutterLayerConstrainedX = sizeSettings.minInnerWidth;
			if(layer.dimensions.minOutterLayerConstrainedY < sizeSettings.minInnerHeight)
				layer.dimensions.minOutterLayerConstrainedY = sizeSettings.minInnerHeight;
			
			layer.dimensions.minOutterConstrainedX =
				layer.dimensions.minOutterLayerConstrainedX += getTotalOffsetWidth();
			layer.dimensions.minOutterConstrainedY =
				layer.dimensions.minOutterLayerConstrainedY += getTotalOffsetHeight();
		}
		
		
		private void calculatePosition(Layer layer,
										int xPos, int yPos,
										Dimensions.ClipBox clipBox)
		{
			Dimensions.ClipBox componentBox = new Dimensions.ClipBox();
			int currentX, currentY, constOffsetX, constOffsetY;
			uint x, y, maxLine;
			
			bool minimizeWidth = false;
			if(layout.axisPriority == Layout.AxisPriority.VerticalFirst)
				minimizeWidth = true;
			
			bool hasBorderValue = hasBorder();
			
			constOffsetX = 0;
			constOffsetY = 0;
			layer.dimensions.positionX = xPos;
			layer.dimensions.positionY = yPos;
			layer.dimensions.calculateFinalDrawSpace(clipBox);
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
						currentY = 0;
						if(layout.horizontalFlow	== Layout.Flow.RightOrLowerFlow)
							currentX += (int)maxLine;
						else
							currentX -= (int)maxLine;
					}
					else
					{
						currentX = 0;
						if(layout.verticalFlow	== Layout.Flow.RightOrLowerFlow)
							currentY += (int)maxLine;
						else
							currentY -= (int)maxLine;
					}
					maxLine = 0;
					++tableLineIndex;
				}
				
				x = component.getMinOutterConstrainedWidth();
				y = component.getMinOutterConstrainedHeight();
				
				int componentOffsetX, componentOffsetY;
				if(layout.horizontalFlow	== Layout.Flow.RightOrLowerFlow)
					componentOffsetX = currentX;
				else
					componentOffsetX = currentX - (int)x;
				if(layout.verticalFlow	== Layout.Flow.RightOrLowerFlow)
					componentOffsetY = currentY;
				else
					componentOffsetY = currentY - (int)y;
				
				componentBox.clear();
				componentBox.width = x;
				componentBox.height = y;
				componentBox.adjust(currentX, currentY,
									clipBox, layout);
				
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
				
				component.calculatePosition(constOffsetX + componentOffsetX, constOffsetY + componentOffsetY, componentBox);
				
				if(minimizeWidth == true)
				{
					if(layout.verticalFlow	== Layout.Flow.RightOrLowerFlow)
						currentY += (int)y;
					else
						currentY -= (int)y;
				}
				else
				{
					if(layout.horizontalFlow	== Layout.Flow.RightOrLowerFlow)
						currentX += (int)x;
					else
						currentX -= (int)x;
				}
			}
		}
		
		
		private void adjustPosition(Layer layer,
										ref int constOffsetX, ref int constOffsetY,
										int xPos, int yPos)
		{
			layer.dimensions.positionX = xPos;
			layer.dimensions.positionY = yPos;
			if(layout.horizontalAlignment == Layout.Alignment.LeftOrUpperAlignment)
			{
				if(layout.horizontalFlow == Layout.Flow.RightOrLowerFlow)
					layer.dimensions.positionX += (int)layer.offsetX;
				else
					layer.dimensions.positionX += (int)(dimensions.minOutterUnconstrainedX - layer.offsetX);
			}
			else
			{
				layer.dimensions.positionX += (int)layer.dimensions.drawSpace.width;
				if(layout.horizontalFlow == Layout.Flow.RightOrLowerFlow)
					layer.dimensions.positionX += (int)(layer.offsetX - dimensions.minOutterUnconstrainedX);
				else
					layer.dimensions.positionX -= (int)layer.offsetX;
			}
			
			if(layout.verticalAlignment == Layout.Alignment.LeftOrUpperAlignment)
			{
				if(layout.verticalFlow == Layout.Flow.RightOrLowerFlow)
					layer.dimensions.positionY += (int)layer.offsetY;
				else
					layer.dimensions.positionY += (int)(dimensions.minOutterUnconstrainedY - layer.offsetY);
			}
			else
			{
				layer.dimensions.positionY += (int)layer.dimensions.drawSpace.height;
				if(layout.verticalFlow == Layout.Flow.RightOrLowerFlow)
					layer.dimensions.positionY += (int)(layer.offsetY - dimensions.minOutterUnconstrainedY);
				else
					layer.dimensions.positionY -= (int)layer.offsetY;
			}
			
			
			if(layer.dimensions.positionX < 0)
				layer.dimensions.positionX = 0;
			if(layer.dimensions.positionY < 0)
				layer.dimensions.positionY = 0;
			
			constOffsetX = (int)layer.dimensions.positionX;
			constOffsetY = (int)layer.dimensions.positionY;
			if(hasBorder() == true)
			{
				if(layout.horizontalAlignment == Layout.Alignment.LeftOrUpperAlignment)
					constOffsetX += (int)getTotalOffsetWidth() / 2;
				else
					constOffsetX -= (int)getTotalOffsetWidth() / 2;
				if(layout.verticalAlignment == Layout.Alignment.LeftOrUpperAlignment)
					constOffsetY += (int)getTotalOffsetHeight() / 2;
				else
					constOffsetY -= (int)getTotalOffsetHeight() / 2;
			}
		}
		
		
		private void draw(Layer layer, Object renderSurface)
		{
			foreach(BaseComponent i in layer.group)
			{
				if (i.getFinalWidth() == 0 || i.getFinalHeight() == 0)
					continue;
				
				i.draw(renderSurface);
			}
		}


		protected override uint getSingleInnerSpacingWidth()
		{
			return contentSpacingX;
		}
		
		
		protected override uint getSingleInnerSpacingHeight()
		{
			return contentSpacingY;
		}
		
		
		public virtual void setMaxOutterWidth(uint value)
		{
			if (value < getTotalOffsetWidth())
				value = 0;
			else
				value -= getTotalOffsetWidth();
			sizeSettings.maxInnerWidth = value;
		}
		
		
		public virtual void setMaxOutterHeight(uint value)
		{
			if (value < getTotalOffsetHeight())
				value = 0;
			else
				value -= getTotalOffsetHeight();
			sizeSettings.maxInnerHeight = value;
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




