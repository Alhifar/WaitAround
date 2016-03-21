using System;
using Storm.ExternalEvent;
using Storm.StardewValley.Event;
using Microsoft.Xna.Framework.Input;
using Storm.StardewValley;
using Storm.StardewValley.Accessor;
using Storm.StardewValley.Wrapper;

namespace WaitAround
{
    [Mod]
    public class WaitAround : DiskResource
    {
        public Config ModConfig { get; private set; }
        public bool drawingWaitMenu = false;

        private static int LatestTime = 2550;
        private int _timeToWait;
        public int timeToWait
        {
            get { return _timeToWait; }
            set
            {
                if (value < 0)
                {
                    return;
                }
                int newTime = getTimeFromOffset(this.Root.TimeOfDay, value);
                if (newTime > LatestTime)
                {
                    _timeToWait = getOffsetFromTimes(this.Root.TimeOfDay, LatestTime);
                }
                else
                {
                    _timeToWait = value;
                }
            }
        }

        private StaticContext Root { get; set; }
        private Keys menuKey { get; set; }
        private WaitMenu waitMenu;

        [Subscribe]
        public void InitializeCallback(InitializeEvent @event)
        {
            ModConfig = new Config(this);
            menuKey = (Keys)Enum.Parse(typeof(Keys), ModConfig.menuKey.ToUpper());
            waitMenu = new WaitMenu(@event.Root, this);
            this.Root = @event.Root;
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
                waitMenu.Close();
            }
            else
            {
                waitMenu.Root = @event.Root;
                // create our menu
                //set it as ActiveMenu per Proxy
                @event.Root.ActiveClickableMenu = @event.Proxy<ClickableMenuAccessor, ClickableMenu>(waitMenu);
                //set our draw to true
                drawingWaitMenu = true;
            }
        }

        public static int getTimeFromOffset(int startTime, int offset)
        {
            int time = startTime;
            for (;offset > 0; offset -= 10)
            {
                time += 10;
                if (time % 100 == 60)
                {
                    time += 40;
                }
            }
            return time;
        }

        public static int getOffsetFromTimes(int startTime, int endTime)
        {
            int offset = 0;
            for (int i = startTime + 10; i - 10 < endTime; i += 10)
            {
                offset += 10;
                if(i%100 == 60)
                {
                    i += 40;
                }
            }
            return offset;
        }
    }
}
