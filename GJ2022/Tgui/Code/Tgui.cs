using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tgui.Code
{
    public class Tgui
    {

        // The object which owns this UI
        public object Source { get; }
        //The title of this UI
        public string Title { get; }
        //The attached window
        public TguiWindow Window { get; private set; }
        //The interface (template) to be used for this UI
        public string Interface { get; }
        //Is the UI autoupdating
        public bool AutoUpdate { get; set; }
        //Has the UI been initialized yet?
        public bool Initialized { get; private set; }
        // Stops further updates when close() is called
        public bool Closing { get; private set; } = false;
        //The status / visibility of the UI
        //TODO
        public bool NeedsUpdate { get; private set; } = false;
        //Time opened at
        public double OpenedAt { get; private set; }

        /**
         * public
         *
         * Create a new UI.
         *
         * required user mob The mob who opened/is using the UI.
         * required src_object datum The object or datum which owns the UI.
         * required interface string The interface used to render the UI.
         * optional title string The title of the UI.
         * optional ui_x int Deprecated: Window width.
         * optional ui_y int Deprecated: Window height.
         *
         * return datum/tgui The requested UI.
         */
        public Tgui(object source, string interfaceName, string title = null)
        {
            //Log
            Log.WriteLine($"New {interfaceName}", LogType.DEBUG);
            //Update stuff
            Source = source;
            Interface = interfaceName;
            Title = title ?? source.ToString();
        }

        /**
         * public
         *
         * Open this UI (and initialize it with data).
         *
         * return bool - TRUE if a new pooled window is opened, FALSE in all other situations including if a new pooled window didn't open because one already exists.
         */
        public bool Open()
        {
            if (Window != null)
                return false;
            //ProcessStatus();
            //if(status < UI_UPDATE) return false;
            Window = TguiSubsystem.Singleton.RequestPooledWindow();
            if (Window == null)
                return false;
            //Set opened at time
            OpenedAt = GLFW.Glfw.Time;
            //Aquire lock
            Window.AcquireLock(this);
            //Check if ready
            if (!Window.IsReady)
                Window.Initialize();
            else
                throw new NotImplementedException();
            throw new NotImplementedException();
        }

    }
}
