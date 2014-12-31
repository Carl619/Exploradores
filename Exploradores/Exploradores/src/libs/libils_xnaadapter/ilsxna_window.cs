using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



/*----------------------------------------------------------------------------------
										ILSXNA
----------------------------------------------------------------------------------*/
namespace ILSXNA
{
	
	
	
	/*----------------------------------------------------------------------------------
											Window
	------------------------------------------------------------------------------------
	Sprite implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Window : ILS.Window<Programa.Exploradores, ILSXNA.Container>
	{
		
		
		// public variables
		public static uint defaultVideoModeMargin = 32;
		public static bool fullScreen = false;
		public Color backgroundColor { get; set; }
		// private variables
		private static Window instance = null;
		

		// constructors
		private Window(Programa.Exploradores gameObject)
			: base()
		{
			instance = this;
			innerWindow = gameObject;
			container = new ILSXNA.Container();
			backgroundColor = Color.Black;
			
			DisplayMode desktopVideoMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			DisplayMode bestVideMode = desktopVideoMode;
			if(Window.fullScreen == false)
			{
				bestVideMode = null;
				foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
				{
					if(mode.Width + 2 * defaultVideoModeMargin <= desktopVideoMode.Width &&
						mode.Height + 2 * defaultVideoModeMargin <= desktopVideoMode.Height)
					{
						if(bestVideMode == null)
							bestVideMode = mode;
						if(bestVideMode.Width < mode.Width || bestVideMode.Height < mode.Height)
							bestVideMode = mode;
					}
				}
				if(bestVideMode == null)
					bestVideMode = desktopVideoMode;
			
				innerWindow.graphics.IsFullScreen = false;
			}
			else
			{
				innerWindow.graphics.IsFullScreen = true;
			}

			resize((uint)bestVideMode.Width, (uint)bestVideMode.Height);
		}
		
		
		// public functions
		public static Window Instancia
		{
			get
			{
				if(instance == null)
					new Window(Programa.Exploradores.Instancia);
				return instance;
			}
		}


		public override bool hasBorder()
		{
			return false;
		}


		public override void draw()
		{
			if(visible == false)
				return;
			if(updated == false)
			{
				updateDimensions(currentWidth, currentHeight);
				ILS.Dimensions.ClipBox clipBox = new ILS.Dimensions.ClipBox();
				clipBox.width = currentWidth;
				clipBox.height = currentHeight;
				calculatePosition(0, 0, clipBox);

				innerWindow.spriteBatch.Begin();
					innerWindow.GraphicsDevice.Clear(backgroundColor);
					base.draw(this);
				innerWindow.spriteBatch.End();
			}
		}


		public override void resize(uint width, uint height)
		{
			DisplayMode desktopVideoMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			if(width > desktopVideoMode.Width)
				width = (uint)desktopVideoMode.Width;
			if(height > desktopVideoMode.Width)
				height = (uint)desktopVideoMode.Height;
			innerWindow.graphics.PreferredBackBufferWidth = (int)width;
			innerWindow.graphics.PreferredBackBufferHeight = (int)height;
			
			currentWidth = width;
			currentHeight = height;
			container.setMaxOutterWidth(currentWidth);
			container.setMaxOutterHeight(currentHeight);
		}


		// protected functions
		protected override uint getMinInnerUnconstrainedWidth()
		{
			return (uint)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		}
		
		
		protected override uint getMinInnerUnconstrainedHeight()
		{
			return (uint)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


