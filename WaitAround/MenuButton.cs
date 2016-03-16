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
        public Action callbackFunction { set; get; }
        public Rectangle buttonRect { set; get; }
        public Texture2D buttonTex { set; get; }

        public MenuButton(int width, int height, int x, int y, Texture2D buttonTex, Action callbackFunction)
        {
            this.buttonRect = new Rectangle(x, y, width, height);
            this.buttonTex = buttonTex;
            this.callbackFunction = callbackFunction;
        }

    }
}
