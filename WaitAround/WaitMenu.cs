using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Storm.StardewValley.Proxy;
using Storm.StardewValley.Wrapper;
using System.IO;

namespace WaitAround
{
    class WaitMenu : ClickableMenuDelegate
    {
        public StaticContext root { get; set; }
        private WaitAround mod { get; set; }
        private Vector2 menuTopLeft { get; set; }
        private List<MenuButton> buttons { get; set; }
        public WaitMenu(StaticContext root, WaitAround mod)
        {
            this.root = root;
            this.mod = mod;
            this.buttons = new List<MenuButton>();
            string imagePath = Path.Combine(mod.PathOnDisk, "images");

            Texture2D testButtonTex = root.LoadResource(Path.Combine(imagePath, "testButton.png"));
            buttons.Add(new MenuButton(15, 15, 5, 5, testButtonTex, testButton));
        }

        private void drawButton(SpriteBatch b, MenuButton button)
        {
            Rectangle finalButtonRect = new Rectangle((int) menuTopLeft.X + button.buttonRect.X, (int) menuTopLeft.Y + button.buttonRect.Y, button.buttonRect.Width, button.buttonRect.Height);
            b.Draw(button.buttonTex, finalButtonRect, Color.White);
        }
        private void testButton()
        {
            Console.Write("WaitAround: Pushed Test Button");
        }
        public override void Draw(SpriteBatch b)
        {
            menuTopLeft = drawBaseMenu(b, 300, 150, 500, 450);
            foreach (MenuButton button in buttons)
            {
                drawButton(b, button);
            }
        }

        public override void EmergencyShutDown()
        {
            throw new NotImplementedException();
        }

        public override void PerformHoverAction(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override bool ReadyToClose()
        {
            return true;
        }

        public override void ReceiveGamePadButton(Buttons b)
        {
            throw new NotImplementedException();
        }

        public override void ReceiveKeyPress(Keys key)
        {
            throw new NotImplementedException();
        }

        public override void ReceiveLeftClick(int x, int y, bool playSound = true)
        {
            Rectangle mouseRect = new Rectangle(x, y, 1, 1);

            foreach (MenuButton button in buttons)
            {
                Rectangle finalButtonRect = new Rectangle((int)menuTopLeft.X + button.buttonRect.X, (int)menuTopLeft.Y + button.buttonRect.Y, button.buttonRect.Width, button.buttonRect.Height);
                if (mouseRect.Intersects(finalButtonRect))
                {
                    button.callbackFunction();
                }
            }
        }

        public override void ReceiveRightClick(int x, int y, bool playSound = true)
        {
            throw new NotImplementedException();
        }

        public override void ReceiveScrollWheelAction(int direction)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime time)
        {
            throw new NotImplementedException();
        }

        //Borrowed from NPCLocations by Kemenor
        private Vector2 drawBaseMenu(SpriteBatch b, int leftRightPadding, int upperLowerPadding, int minWidth, int minHeight)
        {
            Texture2D MenuTiles = this.root.Content.Load<Texture2D>("MenuTiles");
            var font = root.SmallFont;
            var viewport = root.Viewport;
            var textColor = Color.Black;

            //calculate the dimensions of the menu
            int width = Math.Max(viewport.Width - leftRightPadding * 2, 1);
            int height = Math.Max(viewport.Height - upperLowerPadding * 2, 1);
            if(width < minWidth && minWidth <= viewport.Width)
            {
                width = minWidth;
                leftRightPadding = (viewport.Width - width) / 2;
            }
            if (height < minHeight && minHeight <= viewport.Height)
            {
                height = minHeight;
                upperLowerPadding = (viewport.Height - height) / 2;
            }

            //Texture2D for the menu
            Texture2D menu = new Texture2D(root.Graphics.GraphicsDevice, width, height);
            //get the upper left corner of the menu
            Vector2 screenLoc = new Vector2(leftRightPadding, upperLowerPadding);

            //fill menu with dump data so it shows
            var data = new uint[width * height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0xffffffff;
            }
            menu.SetData<uint>(data);

            //draw the ugly menu
            Vector2 menubar = screenLoc - new Vector2(32, 32);
            b.Draw(menu, screenLoc, new Color(232, 207, 128));

            Rectangle upperLeft = new Rectangle(0, 0, 64, 64);
            Rectangle upperRight = new Rectangle(192, 0, 64, 64);
            Rectangle lowerLeft = new Rectangle(0, 192, 64, 64);
            Rectangle lowerRight = new Rectangle(192, 192, 64, 64);
            Rectangle upperBar = new Rectangle(128, 0, 64, 64);
            Rectangle leftBar = new Rectangle(0, 128, 64, 64);
            Rectangle rightBar = new Rectangle(192, 128, 64, 64);
            Rectangle lowerBar = new Rectangle(128, 192, 64, 64);

            int menuHeight = height + 2 * 32;
            int menuWidth = width + 2 * 32;

            int rightUpperCorner = menuWidth - 64;
            int leftLowerCorner = menuHeight - 64;


            //Draw upperbar
            for (int i = 64; i < rightUpperCorner - 64; i += 64)
            {
                b.Draw(MenuTiles, menubar + new Vector2(i, 0), upperBar, Color.White);
            }
            int leftOver = rightUpperCorner % 64;
            Rectangle leftOverRect = new Rectangle(upperBar.X, upperBar.Y, leftOver, upperBar.Height);
            b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner - leftOver, 0), leftOverRect, Color.White);

            //draw left bar
            for (int i = 64; i < leftLowerCorner - 64; i += 64)
            {
                b.Draw(MenuTiles, menubar + new Vector2(0, i), leftBar, Color.White);
            }
            leftOver = leftLowerCorner % 64;
            leftOverRect = new Rectangle(leftBar.X, leftBar.Y, leftBar.Width, leftOver);
            b.Draw(MenuTiles, menubar + new Vector2(0, leftLowerCorner - leftOver), leftOverRect, Color.White);

            //draw right bar
            for (int i = 64; i < leftLowerCorner - 64; i += 64)
            {
                b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner, i), rightBar, Color.White);
            }
            leftOver = leftLowerCorner % 64;
            leftOverRect = new Rectangle(rightBar.X, rightBar.Y, rightBar.Width, leftOver);
            b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner, leftLowerCorner - leftOver), leftOverRect, Color.White);

            //draw lower Bar
            for (int i = 64; i < rightUpperCorner - 64; i += 64)
            {
                b.Draw(MenuTiles, menubar + new Vector2(i, leftLowerCorner), lowerBar, Color.White);
            }
            leftOver = rightUpperCorner % 64;
            leftOverRect = new Rectangle(lowerBar.X, lowerBar.Y, leftOver, lowerBar.Height);
            b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner - leftOver, leftLowerCorner), leftOverRect, Color.White);

            //draw upper left corner
            b.Draw(MenuTiles, menubar, upperLeft, Color.White);
            //draw upper right corner
            b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner, 0), upperRight, Color.White);
            //draw lower left corner
            b.Draw(MenuTiles, menubar + new Vector2(0, leftLowerCorner), lowerLeft, Color.White);
            //draw lower right Corner
            b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner, leftLowerCorner), lowerRight, Color.White);

            return screenLoc;
        }
    }
}
