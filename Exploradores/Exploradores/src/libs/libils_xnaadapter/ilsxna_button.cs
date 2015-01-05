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
										Button
	------------------------------------------------------------------------------------
	Border implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Button : Container
	{
		// private and protected variables
		private ushort _currentViewIndex { get; set; }
		private bool _enabled { get; set; }
		protected MultiSprite multiSprite { get; set; }
		protected Label text { get; set; }
		// public variables
		public ButtonFlyweight flyweight { get; protected set; }
		public List<Texture2D> icons { get; protected set; }
		public String message { get; protected set; }
		public ILS.MouseEvent.CallbackFunction onButtonPress { get; set; }
		public ushort currentBorderIndex
		{
			get
			{
				return _currentViewIndex;
			}
			set
			{
				if(border == null)
				{
					_currentViewIndex = 0;
					return;
				}
				if(value >= border.flyweight.borders.Count || value < 0)
					return;
				_currentViewIndex = value;
			}
		}
		public bool enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if(border.flyweight.borders.Count != 0)
				{
					if(value == false && _enabled == true)
					{
						currentBorderIndex = (ushort)(border.flyweight.borders.Count - 1);
						requestUpdateContent();
					}
					else if(value == true && _enabled == false)
					{
						currentBorderIndex = 0;
						requestUpdateContent();
					}
				}
				_enabled = value;
			}
		}


		// constructor
		public Button(String newMessage, ButtonFlyweight newFlyweight, ILS.Layer newParent = null)
			: base((Border)null, newParent)
		{
			if(newMessage == null || newFlyweight == null)
				throw new ArgumentNullException();
			
			_currentViewIndex = 0;
			_enabled = true;
			multiSprite = null;
			text = null;

			flyweight = newFlyweight;
			icons = new List<Texture2D>();
			message = (String)newMessage.Clone();
			border = (Border)flyweight.border.clone();
			border.borderNumber = 1;

			onMouseOver = mouseActivate;
			onMouseOut = mouseDeactivate;
			onLeftMousePress = mouseSelect;
			onButtonPress = null;

			updateContent();
		}


		public Button(Button button, ILS.Layer newParent = null)
			: base((Border)null, newParent)
		{
			if(button == null)
				throw new ArgumentNullException();
			
			_currentViewIndex = button._currentViewIndex;
			_enabled = button._enabled;
			multiSprite = null;
			text = null;

			flyweight = button.flyweight;
			icons = new List<Texture2D>();
			icons.AddRange(button.icons);
			message = (String)button.message.Clone();
			border = (Border)flyweight.border.clone();
			border.borderNumber = 1;

			onMouseOver = button.onMouseOver;
			onMouseOut = button.onMouseOut;
			onLeftMousePress = button.onLeftMousePress;
			onButtonPress = button.onButtonPress;

			updateContent();
		}
		

		// public functions
		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new Button(this, newParent);
		}


		public void setMessage(String newMessage)
		{
			if(newMessage == null)
				throw new ArgumentNullException();
			
			message = (String)newMessage.Clone();
			updateContent();
		}
		

		public override void updateContent()
		{
			clearComponents();
			requestedContentUpdate = false;

			border.borderNumber = _currentViewIndex;
			border.updateContent();
			contentSpacingX = flyweight.contentSpacingX;
			contentSpacingY = flyweight.contentSpacingY;

			if(icons.Count > 0)
			{
				multiSprite = new MultiSprite();
				foreach(Texture2D textura in icons)
					multiSprite.addTextura(textura);
				multiSprite.onMouseOver =
					multiSprite.onMouseOut =
					multiSprite.onLeftMousePress =
					multiSprite.onLeftMouseRelease =
					null;
				multiSprite.currentSpriteIndex = _currentViewIndex;
				addComponent(multiSprite);
			}

			text = new ILSXNA.Label();
			text.message = (String)message.Clone();
			text.color = getCurrentTextColor();
			text.innerComponent = flyweight.textFont;
			addComponent(text);
		}


		public Color getCurrentTextColor()
		{
			if(_currentViewIndex == 0)
				return flyweight.textColorPassive;
			if(_currentViewIndex == 1)
				return flyweight.textColorActive;
			if(_currentViewIndex == 2)
				return flyweight.textColorSelected;
			if(_currentViewIndex == 3)
				return flyweight.textColorActiveSelected;
			return flyweight.textColorDisabled;
		}


		public static void mouseActivate(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Button button = ((Button)drawable);
			if(button.enabled == false)
				return;
			if(button.border.flyweight.borders.Count > 1)
			{
				ushort newIndex = button.currentBorderIndex;
				if(newIndex % 2 == 0)
					++newIndex;
				button.currentBorderIndex = newIndex;
				button.requestUpdateContent();
			}
		}


		public static void mouseDeactivate(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Button button = ((Button)drawable);
			if(button.enabled == false)
				return;
			if(button.border.flyweight.borders.Count > 1)
			{
				ushort newIndex = button.currentBorderIndex;
				if(newIndex % 2 == 1)
					--newIndex;
				button.currentBorderIndex = newIndex;
				button.requestUpdateContent();
			}
		}

		
		public static void mouseSelect(ILS.Drawable drawable, ILS.MouseEvent eventInfo, Object configObj)
		{
			Button button = ((Button)drawable);
			if(button.enabled == false)
				return;
			if(button.border.flyweight.borders.Count > 3)
			{
				ushort newIndex = button.currentBorderIndex;
				if((newIndex / 2) % 2 == 0)
					newIndex += 2;
				button.currentBorderIndex = newIndex;
			}
			if(button.onButtonPress != null)
				button.onButtonPress(drawable, eventInfo, configObj);
		}


		public void deselect()
		{
			if(enabled == false)
				return;
			if(border.flyweight.borders.Count < 4)
				return;
			ushort newIndex = currentBorderIndex;
			if((newIndex / 2) % 2 == 1)
				newIndex -= 2;
			currentBorderIndex = newIndex;
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


