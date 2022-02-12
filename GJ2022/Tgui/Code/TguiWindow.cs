using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tgui.Code
{
    public class TguiWindow
    {

        public enum TguiWindowState
        {
            TGUI_WINDOW_CLOSED = 0,
            TGUI_WINDOW_LOADING = 1,
            TGUI_WINDOW_READY = 2,
        }

        public string ID { get; }
        public bool Pooled { get; private set; }
        public int PoolIndex { get; private set; }
        public bool IsBrowser { get; }
        public TguiWindowState Status { get; private set; }
        public bool Locked { get; private set; }

        public bool FatallyErrored { get; private set; } = false;

        // Variables passed to initialize method (and saved for later)
        public string[] InlineAssets { get; private set; }
        public bool Fancy { get; private set; }

        public TguiWindow(string id, bool pooled = false)
        {
            ID = id;
            Pooled = pooled;
            if (pooled)
                PoolIndex = int.Parse(id.Substring(13));
        }

        public void Initialize(string[] inline_assets = null, string inline_html = "", bool fancy = false)
        {
            Log.WriteLine($"{ID}/initialize", LogType.DEBUG);
            InlineAssets = inline_assets ?? new string[0];
            Fancy = fancy;
            Status = TguiWindowState.TGUI_WINDOW_LOADING;
            FatallyErrored = false;
            // Build window options
            string options = $"file={ID}.html;can_minimize=0;auto_format=0;";
            // Remove titlebar and resize handles for a fancy winedow
            if (fancy)
                options += "titlebar=0;can_resize=0;";
            else
                options += "titlebar=1;can_resize=1;";
            // Generate page html
            string html = TguiSubsystem.Singleton.BaseHtml;
            html = html.Replace("[tgui:windowId]", ID);
            //Open the windw
        }

    }
}
