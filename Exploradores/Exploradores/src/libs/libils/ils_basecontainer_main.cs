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
		
		
		// protected functions
		protected override uint getMinInnerUnconstrainedWidth()
		{
			return getMinInnerLayerConstrainedWidth();
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
		
		
		// protected functions
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




