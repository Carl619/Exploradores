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
		public override void draw(Object renderSurface)
		{
			if(visible == false)
				return;
			if (getCurrentAlternative() == null)
				return;
			
			foreach(Layer i in getCurrentAlternative().layers)
				draw(i, renderSurface);
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
		
		
		// private functions
		private void draw(Layer layer, Object renderSurface)
		{
			foreach(BaseComponent i in layer.group)
			{
				/*if (i.getFinalWidth() == 0 || i.getFinalHeight() == 0)
					continue;*/
				
				i.draw(renderSurface);
			}
		}
	}
	
	
	
} // end namespace ILS
/*--------------------------------------------------------------------------------*/




