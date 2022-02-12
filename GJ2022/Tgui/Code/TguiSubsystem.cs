using GJ2022.Subsystems;
using GLFW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tgui.Code
{
    public class TguiSubsystem : Subsystem
    {

        public static TguiSubsystem Singleton { get; } = new TguiSubsystem();

        public override int sleepDelay => 900;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NONE;

        public string BaseHtml { get; }

        private TguiSubsystem() : base()
        {
            BaseHtml = File.ReadAllText("tgui/public/tgui.html");
        }

        public override void Fire(Window window)
        {
            throw new NotImplementedException();
        }

        public override void InitSystem()
        {
            throw new NotImplementedException();
        }

        protected override void AfterWorldInit()
        {
            throw new NotImplementedException();
        }
    }
}
