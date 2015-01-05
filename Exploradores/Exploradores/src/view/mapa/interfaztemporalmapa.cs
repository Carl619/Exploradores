using System;
using System.Collections.Generic;




public class InterfazGraficaMapa : ILSXNA.Container
{
	// constructor
	public InterfazGraficaMapa(ILSXNA.Border border = null, ILS.Layer newParent = null) : base(border, newParent)
	{
	}


	public InterfazGraficaMapa(InterfazGraficaMapa interfaz, ILS.Layer newParent = null) : base(interfaz, newParent)
	{
	}


	// funciones
	public override ILS.BaseComponent clone(ILS.Layer newParent = null)
	{
		return new InterfazGraficaMapa(this, newParent);
	}


	public override void calculatePosition(int xPos, int yPos, ILS.Dimensions.ClipBox outterClipBox)
	{
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

			ILS.Dimensions.ClipBox innerClipBox = getComponentClipBox(outterClipBox,
													marginLeft,
													marginTop,
													(uint)innerWidth, (uint)innerHeight);
			

			foreach (ILS.Layer layer in getCurrentAlternative().layers)
			{
				ILS.Dimensions.ClipBox layerClipBox = getComponentClipBox(innerClipBox,
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
				
				ILSXNA.Container scrollableContainer = Programa.VistaGeneral.Instancia.scrollableContainer;
				int hackEspecialX, hackEspecialY;
				hackEspecialX =
					scrollableContainer.getCurrentAlternative().getCurrentLayer().offsetX;
				hackEspecialY =
					scrollableContainer.getCurrentAlternative().getCurrentLayer().offsetY;
				if(layer.offsetX != 0)
				{
					if(hackEspecialX < 0 && hackEspecialX > - 64)
						x -= hackEspecialX;
					if(hackEspecialX <= -64 && hackEspecialX > - layer.offsetX)
						x += 64;
					if(hackEspecialX <= -layer.offsetX && hackEspecialX > - layer.offsetX - 64)
						x += layer.offsetX + 64 + hackEspecialX;
				}
				if(layer.offsetY != 0)
				{
					if(hackEspecialY < 0 && hackEspecialY > - 64)
						y -= hackEspecialY;
					if(hackEspecialY <= -64 && hackEspecialY > - layer.offsetY)
						y += 64;
					if(hackEspecialY <= -layer.offsetY && hackEspecialY > - layer.offsetY - 64)
						y += layer.offsetY + 64 + hackEspecialY;
				}
				
				calculateLayerPosition(layer, x, y, layerClipBox);
			}

			dimensions.positionX = xPos;
			dimensions.positionY = yPos;
			dimensions.calculateFinalDrawSpace(outterClipBox);
		}
		
		if(hasBorder() == true)
		{
			int innerWidth, innerHeight;
			innerWidth = (int)dimensions.afterMinimizeOutterX - (int)getTotalOutterSpacingWidth();
			innerHeight = (int)dimensions.afterMinimizeOutterY - (int)getTotalOutterSpacingHeight();
			if(innerWidth < 0)
				innerWidth = 0;
			if(innerHeight < 0)
				innerHeight = 0;
			int innerPosX = xPos + (int)getSingleOutterSpacingWidth();
			int innerPosY = yPos + (int)getSingleOutterSpacingHeight();
			
			ILS.Dimensions.ClipBox borderClipBox = getComponentClipBox(outterClipBox, 0, 0,
													(uint)innerWidth, (uint)innerHeight);
			border.calculatePosition(innerPosX, innerPosY, borderClipBox);
		}
	}
}




