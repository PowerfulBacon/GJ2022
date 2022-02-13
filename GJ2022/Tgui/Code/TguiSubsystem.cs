using GJ2022.Subsystems;
using GLFW;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GJ2022.Tgui.Code
{
    public class TguiSubsystem : Subsystem
    {

        public const int TGUI_WINDOW_SOFT_LIMIT = 5;
        public const int TGUI_WINDOW_HARD_LIMIT = 10;

        public static TguiSubsystem Singleton { get; } = new TguiSubsystem();

        public override int sleepDelay => 900;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NONE;

        public string BaseHtml { get; private set; }

        public TguiWebServer WebServer { get; } = new TguiWebServer(5050);

        //List of TGUI windows
        public Dictionary<string, TguiWindow> TguiWindows { get; } = new Dictionary<string, TguiWindow>();

        protected TguiSubsystem() : base()
        { }

        public override void Fire(Window window)
        { }

        public override void InitSystem()
        {
            try
            {
                BaseHtml = File.ReadAllText("Tgui/tgui/public/tgui.html");
                new TguiWindow("0").Initialize();
            }
            catch (System.Exception e)
            {
                Log.WriteLine(e, LogType.ERROR);
            }
        }

        protected override void AfterWorldInit()
        { }

        public static string TguiWindowId(int i) => $"tgui-window-{i}";

        public static string TguiCreateMessage(string type, object[] payload) => $"%7b%22type%22%3a%22{type}%22%2c%22payload%22%3a{HttpUtility.UrlEncode(JsonConvert.SerializeObject(payload))}%7d";

        /**
         * public
         *
         * Requests a usable tgui window from the pool.
         * Returns null if pool was exhausted.
         *
         * required user mob
         * return datum/tgui
         */
        public TguiWindow RequestPooledWindow()
        {
            string windowId;
            TguiWindow window;
            for (int i = 0; i < TGUI_WINDOW_HARD_LIMIT; i++)
            {
                windowId = TguiWindowId(i);
                //Create missing window objects
                if (TguiWindows.ContainsKey(windowId))
                    window = TguiWindows[windowId];
                else
                    window = new TguiWindow(windowId, true);
                //Skip windows with aquired locks
                if (window.Locked)
                    continue;
                //Return ready window
                if (window.Status == TguiWindow.TguiWindowState.TGUI_WINDOW_READY)
                    return window;
                //Locate window
                if (window.Status == TguiWindow.TguiWindowState.TGUI_WINDOW_CLOSED)
                {
                    window.Status = TguiWindow.TguiWindowState.TGUI_WINDOW_LOADING;
                    return window;
                }
            }
            Log.WriteLine("TGUI Warning: Pool Exhausted", LogType.WARNING);
            return null;
        }

    }
}
