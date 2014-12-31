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
										Border
	------------------------------------------------------------------------------------
	Border implementation for XNA using ILS library
	----------------------------------------------------------------------------------*/
	public class Border : ILS.Container<Border>, ILS.IBorder, Gestores.IObjetoIdentificable
	{
		// public variables
		public String id { get; protected set; }
		public BorderFlyweight flyweight { get; protected set; }
		public Container centralSpace { get; protected set; }
		public Sprite background { get; protected set; }
		public Sprite topSprite { get; protected set; }
		public Sprite bottomSprite { get; protected set; }
		public Sprite leftSprite { get; protected set; }
		public Sprite rightSprite { get; protected set; }
		public ushort borderNumber { get; set; }
		public float backgroundTransparency { get; set; }


		// constructor
		public Border(String newID, BorderFlyweight newFlyweight, ILS.Layer newParent = null)
			: base(null, newParent)
		{
			if(newID == null || newFlyweight == null)
				throw new ArgumentNullException();
			
			id = newID;
			flyweight = newFlyweight;
			borderNumber = 0;
			backgroundTransparency = 0.0f;

			centralSpace = new Container();
			centralSpace.contentSpacingX = 0;
			centralSpace.contentSpacingY = 0;
			centralSpace.sizeSettings.minInnerWidth = (uint)getCurrentBorder()[1].Width;
			centralSpace.sizeSettings.minInnerHeight = (uint)getCurrentBorder()[3].Height;
			topSprite = null;
			bottomSprite = null;
			leftSprite = null;
			rightSprite = null;

			background = new Sprite();
			background.innerComponent = getCurrentBorder()[4];
			background.displayModeWidth = flyweight.backgroundMode;
			background.displayModeHeight = flyweight.backgroundMode;

			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			updateContent();
		}


		public Border(Border border, ILS.Layer newParent = null)
			: base(null, newParent)
		{
			if(border == null)
				throw new ArgumentNullException();
			
			flyweight = border.flyweight;
			borderNumber = border.borderNumber;
			backgroundTransparency = border.backgroundTransparency;

			centralSpace = new Container();
			topSprite = null;
			bottomSprite = null;
			leftSprite = null;
			rightSprite = null;

			background = (Sprite)border.background.clone();

			layout.axisPriority = ILS.Layout.AxisPriority.VerticalFirst;
			updateContent();
		}
		

		// public functions
		public static Border cargarObjeto(Dictionary<String, String> campos, Dictionary<String, List<String>> listas)
		{
			Border border;
			
			border = new Border(campos["id"], Gestores.Mundo.Instancia.borderFlyweights[campos["flyweight"]]);
			border.contentSpacingX = Convert.ToUInt32(campos["contentSpacingX"]);
			border.contentSpacingY = Convert.ToUInt32(campos["contentSpacingY"]);
			border.backgroundTransparency = Gestores.Mundo.parseFloat(campos["backgroundTransparency"]);

			return border;
		}


		public override ILS.BaseComponent clone(ILS.Layer newParent = null)
		{
			return new Border(this, newParent);
		}


		public override void minimize(uint outterConstraintWidth, uint outterConstraintHeight)
		{
			centralSpace.sizeSettings.minInnerWidth = outterConstraintWidth;
			centralSpace.sizeSettings.minInnerHeight = outterConstraintHeight;
			topSprite.sizeSettings.minInnerWidth = bottomSprite.sizeSettings.minInnerWidth = outterConstraintWidth;
			leftSprite.sizeSettings.minInnerHeight = rightSprite.sizeSettings.minInnerHeight = outterConstraintHeight;
			
			background.sizeSettings.minInnerWidth = outterConstraintWidth + flyweight.getTotalBorderWidth();
			background.sizeSettings.minInnerHeight = outterConstraintHeight + flyweight.getTotalBorderHeight();
			background.sizeSettings.minInnerWidth -= flyweight.getTotalInnerSpacing();
			background.sizeSettings.minInnerHeight -= flyweight.getTotalInnerSpacing();

			base.minimize(0, 0);
		}


		public override void draw(Object renderSurface)
		{
			drawBackground(renderSurface);
			base.draw(renderSurface);
		}


		public override void updateContent()
		{
			clearComponents(true);
			requestedContentUpdate = false;

			background = new Sprite();
			background.innerComponent = getCurrentBorder()[4];
			background.displayModeWidth = flyweight.backgroundMode;
			background.displayModeHeight = flyweight.backgroundMode;

			addBorder();
		}

		
		public override uint getSingleBorderWidth()
		{
			return flyweight.getSingleBorderWidth();
		}


		public override uint getSingleBorderHeight()
		{
			return flyweight.getSingleBorderHeight();
		}


		public override uint getTotalBorderWidth()
		{
			return flyweight.getTotalBorderWidth();
		}


		public override uint getTotalBorderHeight()
		{
			return flyweight.getTotalBorderHeight();
		}


		// protected functions
		protected void drawBackground(Object renderSurface)
		{
			background.opacity = 1.0f - backgroundTransparency;
			background.dimensions.positionX = getDrawPositionX() + (int)flyweight.innerMargin;
			background.dimensions.positionY = getDrawPositionY() + (int)flyweight.innerMargin;
			background.dimensions.drawSpace.width = (uint)(getFinalWidth() - 2 * flyweight.innerMargin);
			background.dimensions.drawSpace.height = (uint)(getFinalHeight() - 2 * flyweight.innerMargin);
			background.draw(renderSurface);
		}


		protected void addBorder()
		{
			addBorderTop();
			addBorderMiddle();
			addBorderBottom();

			topSprite.displayModeWidth = flyweight.borderMode;
			bottomSprite.displayModeWidth = flyweight.borderMode;
			leftSprite.displayModeHeight = flyweight.borderMode;
			rightSprite.displayModeHeight = flyweight.borderMode;
		}


		protected void addBorderTop()
		{
			Sprite sprite;
			Container top = new Container();
			addComponent(top);

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[0];
			top.addComponent(sprite);

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[1];
			top.addComponent(sprite);
			topSprite = sprite;

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[2];
			top.addComponent(sprite);
		}


		protected void addBorderMiddle()
		{
			Sprite sprite;
			Container middle = new Container();
			addComponent(middle);

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[3];
			middle.addComponent(sprite);
			leftSprite = sprite;

			middle.addComponent(centralSpace);

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[5];
			middle.addComponent(sprite);
			rightSprite = sprite;
		}


		protected void addBorderBottom()
		{
			Sprite sprite;
			Container bottom = new Container();
			addComponent(bottom);

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[6];
			bottom.addComponent(sprite);

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[7];
			bottom.addComponent(sprite);
			bottomSprite = sprite;

			sprite = new Sprite();
			sprite.innerComponent = getCurrentBorder()[8];
			bottom.addComponent(sprite);
		}


		public List<Texture2D> getCurrentBorder()
		{
			if (borderNumber >= flyweight.borders.Count || borderNumber < 0)
				return null;
			return flyweight.borders[borderNumber];
		}
	}



} // end namespace ILSXNA
/*--------------------------------------------------------------------------------*/


