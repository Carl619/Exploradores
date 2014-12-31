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
											SizeSettings
	------------------------------------------------------------------------------------
	Class for specifying the size of an object.
	----------------------------------------------------------------------------------*/
	public class SizeSettings
	{
		// public variables
			// inner refers to size without borders or spacing
			// 0 means no maximum or fixed size
		public uint outterSpacingX { get; set; } // space around the element
		public uint outterSpacingY { get; set; } // space around the element
		public uint fixedInnerWidth { get; set; }
		public uint fixedInnerHeight { get; set; }
		// protected variables
		protected uint _minInnerWidth { get; set; }
		protected uint _minInnerHeight { get; set; }
		protected uint _maxInnerWidth { get; set; }
		protected uint _maxInnerHeight { get; set; }
		public uint minInnerWidth
		{
			get
			{
				if (fixedInnerWidth > _minInnerWidth)
					return fixedInnerWidth;
				return _minInnerWidth;
			}
			set
			{
				_minInnerWidth = value;
			}
		}
		public uint minInnerHeight
		{
			get
			{
				if (fixedInnerHeight > _minInnerHeight)
					return fixedInnerHeight;
				return _minInnerHeight;
			}
			set
			{
				_minInnerHeight = value;
			}
		}
		public uint maxInnerWidth
		{
			get
			{
				if (fixedInnerWidth > _minInnerWidth)
					return fixedInnerWidth;
				if (_maxInnerWidth >= _minInnerWidth)
					return _maxInnerWidth;
				return 0;
			}
			set
			{
				_maxInnerWidth = value;
			}
		}
		
		
		public uint maxInnerHeight
		{
			get
			{
				if (fixedInnerHeight > _minInnerHeight)
					return fixedInnerHeight;
				if (_maxInnerHeight >= _minInnerHeight)
					return _maxInnerHeight;
				return 0;
			}
			set
			{
				_maxInnerHeight = value;
			}
		}
		
		
		// constructor, configured for Latin script single text line, with neutral size policy
		public SizeSettings()
		{
			outterSpacingX = 0;
			outterSpacingY = 0;

			fixedInnerWidth = 0;
			fixedInnerHeight = 0;

			_minInnerWidth = 0;
			_minInnerHeight = 0;
			_maxInnerWidth = 0;
			_maxInnerHeight = 0;
		}


		public SizeSettings(SizeSettings settings)
		{
			outterSpacingX = settings.outterSpacingX;
			outterSpacingY = settings.outterSpacingY;

			fixedInnerWidth = settings.fixedInnerWidth;
			fixedInnerHeight = settings.fixedInnerHeight;

			_minInnerWidth = settings._minInnerWidth;
			_minInnerHeight = settings._minInnerHeight;
			_maxInnerWidth = settings._maxInnerWidth;
			_maxInnerHeight = settings._maxInnerHeight;
		}
		
		
		// public functions
		public SizeSettings clone()
		{
			return new SizeSettings(this);
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




