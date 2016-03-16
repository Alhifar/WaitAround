using System;
using Storm.ExternalEvent;
using Storm.StardewValley.Event;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Storm.StardewValley;
using Storm.StardewValley.Accessor;
using Storm.StardewValley.Wrapper;
using Microsoft.Xna.Framework;

namespace WaitAround
{
    [Mod]
    public class WaitAround : DiskResource
    {
        public Config ModConfig { get; private set; }
        private Keys menuKey { get; set; }
        private WaitMenu waitMenu;
        private bool drawingWaitMenu = false;

        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            ModConfig = new Config(this);
            menuKey = (Keys)Enum.Parse(typeof(Keys), ModConfig.menuKey.ToUpper());
            waitMenu = new WaitMenu(@event.Root, this);
            Console.WriteLine("WaitAround Initialization Completed");
        }

        [Subscribe]
        public void KeyPressedCallback(KeyPressedEvent @event)
        {
            Keys keyPressed = @event.Key;
            if (keyPressed == menuKey)
            {
                HandleWaitMenu(@event);
            }
        }

        [Subscribe]
        public void PostRender(PostRenderEvent @event)
        {
            if (@event.Root.ActiveClickableMenu != null && drawingWaitMenu)
            {
                waitMenu.Draw(@event.Root.SpriteBatch);
            }
            if (@event.Root.ActiveClickableMenu == null && drawingWaitMenu)
            {
                drawingWaitMenu = false;
            }
        }

        [Subscribe]
        public void TitleCallback(UpdateTitleScreenEvent @event)
        {
            drawingWaitMenu = false;
        }

        [Subscribe]
        public void LeftClickCallback(MouseButtonPressedEvent @event)
        {
            if(drawingWaitMenu && @event.Button == MouseButtonPressedEvent.MouseButton.Left)
            {
                waitMenu.ReceiveLeftClick(@event.State.X, @event.State.Y);
            }
        }

        private void HandleWaitMenu(StaticContextEvent @event)
        {
            if (@event.Root.CurrentLocation == null)
            {
                return;
            }

            if (@event.Root.ActiveClickableMenu != null && drawingWaitMenu)
            {
                @event.Root.ActiveClickableMenu = null;
                drawingWaitMenu = false;
            }
            else
            {
                waitMenu.root = @event.Root;
                // create our menu
                //set it as ActiveMenu per Proxy
                @event.Root.ActiveClickableMenu = @event.Proxy<ClickableMenuAccessor, ClickableMenu>(waitMenu);
                //set our draw to true
                drawingWaitMenu = true;
            }
        }
    }
}
