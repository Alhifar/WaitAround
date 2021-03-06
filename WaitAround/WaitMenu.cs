﻿using System;
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
        public StaticContext Root { get; set; }
        private WaitAround Mod { get; set; }
        private Rectangle MenuRect { get; set; }
        private List<MenuButton> Buttons { get; set; }

        public WaitMenu(StaticContext root, WaitAround mod)
        {
            this.Root = root;
            this.Mod = mod;
            this.Buttons = new List<MenuButton>();

            Texture2D upArrowTex = getTextureFromTileSheet(this.Root.MouseCursors, 12, 64, 64);
            Texture2D downArrowTex = getTextureFromTileSheet(this.Root.MouseCursors, 11, 64, 64);
            Texture2D okButtonTex = getTextureFromTileSheet(this.Root.MouseCursors, 46, 64, 64);

            Buttons.Add(new MenuButton(64, 64, 0, -1 * ((64 + 10 + 64 + 10 + 64) / 2), new Vector2(-0.25f, 0.5f), this.MenuRect, upArrowTex, upButton));
            Buttons.Add(new MenuButton(64, 64, 0, (-1 * ((64 + 10 + 64 + 10 + 64) / 2)) + 64 + 10, new Vector2(-0.25f, 0.5f), this.MenuRect, downArrowTex, downButton));
            Buttons.Add(new MenuButton(64, 64, 0, (-1 * ((64 + 10 + 64 + 10 + 64) / 2)) + 64 + 10 + 64 + 10, new Vector2(-0.25f, 0.5f), this.MenuRect, okButtonTex, enterButton));
        }

        public Texture2D getTextureFromTileSheet(Texture2D tileSheet, int tilePosition, int width, int height)
        {
            Rectangle sourceRectangle = StardewValley.Game1.getSourceRectForStandardTileSheet(tileSheet, tilePosition, width, height);
            Texture2D newTexture = new Texture2D(Root.Graphics.GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
            Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
            tileSheet.GetData(0, sourceRectangle, data, 0, data.Length);
            newTexture.SetData(data);
            return newTexture;
        }

        private void upButton(MenuButton menuButton)
        {
            Mod.timeToWait += 10;
        }

        private void downButton(MenuButton menuButton)
        {
            Mod.timeToWait -= 10;
        }

        private void enterButton(MenuButton menuButton)
        {
            Root.TimeOfDay = WaitAround.getTimeFromOffset(Root.TimeOfDay, Mod.timeToWait);
            Mod.timeToWait = 0;
            this.Close();
        }

        public void Close()
        {
            Root.ActiveClickableMenu = null;
            Mod.drawingWaitMenu = false;
            Mod.timeToWait = 0;
        }

        public override void Draw(SpriteBatch b)
        {
            SpriteBatch b2 = new SpriteBatch(b.GraphicsDevice);
            b2.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);
            MenuRect = drawBaseMenu(b2, 450, 300, 550, 300);
            foreach (MenuButton button in Buttons)
            {
                button.Draw(b2, MenuRect);
            }
            String titleString = "How long do you want to wait?";
            b2.DrawString(Root.DialogueFont, titleString, new Vector2(MenuRect.X + (MenuRect.Width / 2) - (Root.DialogueFont.MeasureString(titleString).X / 2), MenuRect.Y + 15), Color.Black);
            String timeString = String.Format("{0:00}:{1:00}", Math.Floor(Mod.timeToWait / 60.0), Mod.timeToWait % 60);
            b2.DrawString(this.Root.DialogueFont, timeString, new Vector2((MenuRect.Width / 2) - (this.Root.DialogueFont.MeasureString(timeString).X) + MenuRect.X, (MenuRect.Height / 2) - (this.Root.DialogueFont.MeasureString(timeString).Y) + MenuRect.Y), Color.Black, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
            b2.Draw(Root.MouseCursors, new Vector2(Root.OldMouseState.X, Root.OldMouseState.Y), StardewValley.Game1.getSourceRectForStandardTileSheet(Root.MouseCursors, 0, 16, 16), Color.White, 0.0f, Vector2.Zero, Root.PixelZoom + (Root.DialogueButtonScale / 150), SpriteEffects.None, 0f);
            b2.End();
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

            foreach (MenuButton button in Buttons)
            {
                if (mouseRect.Intersects(button.buttonRect))
                {
                    button.callbackFunction(button);
                }
            }
        }

        public override void ReceiveRightClick(int x, int y, bool playSound = true)
        {
            return;
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
        private Rectangle drawBaseMenu(SpriteBatch b, int leftRightPadding, int upperLowerPadding, int minWidth, int minHeight)
        {
            Texture2D MenuTiles = this.Root.Content.Load<Texture2D>("MenuTiles");
            var font = Root.SmallFont;
            var viewport = Root.Viewport;
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
            Texture2D menu = new Texture2D(Root.Graphics.GraphicsDevice, width, height);
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

            return new Rectangle((int) screenLoc.X, (int) screenLoc.Y, width, height);
        }
    }
}
