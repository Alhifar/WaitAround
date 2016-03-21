using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Storm.StardewValley.Proxy;
using Storm.StardewValley.Wrapper;

namespace WaitAround
{
    class MenuButton
    {
        private static int highestID = 0;
        public int id { get; private set; }
        public Action<MenuButton> callbackFunction { set; get; }
        public Dictionary<String, String> callbackArgs;
        public Texture2D buttonTex { set; get; }
        public int relativeX { set; get; }
        public int relativeY { set; get; }
        public Rectangle buttonRect { set; get; }

        public MenuButton(int width, int height, int x, int y, Rectangle parentMenu, Texture2D buttonTex, Action<MenuButton> callbackFunction)
        {
            this.id = highestID + 1;
            highestID = id;

            this.relativeX = x;
            this.relativeY = y;

            this.buttonRect = new Rectangle(0, 0, width, height);
            this.setAbsoluteButtonPosition(parentMenu);

            this.buttonTex = buttonTex;

            this.callbackFunction = callbackFunction;
            this.callbackArgs = new Dictionary<String, String>();
        }
        public void Draw(SpriteBatch b, Rectangle parentMenu)
        {
            this.setAbsoluteButtonPosition(parentMenu);
            b.Draw(this.buttonTex, this.buttonRect, new Rectangle(0, 0, this.buttonTex.Width, this.buttonTex.Height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f );
        }

        private void setAbsoluteButtonPosition(Rectangle parentMenu)
        {
            int finalButtonX = parentMenu.X + this.relativeX;
            if (this.relativeX < 0)
            {
                finalButtonX = parentMenu.X + parentMenu.Width + this.relativeX;
            }

            int finalButtonY = parentMenu.Y + this.relativeY;
            if (this.relativeY < 0)
            {
                finalButtonY = parentMenu.Y + parentMenu.Height + this.relativeY;
            }
            this.buttonRect = new Rectangle(finalButtonX, finalButtonY, this.buttonRect.Width, this.buttonRect.Height);
        }

    }
}
