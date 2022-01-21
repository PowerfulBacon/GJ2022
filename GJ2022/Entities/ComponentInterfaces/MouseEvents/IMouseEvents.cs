using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.ComponentInterfaces.MouseEvents
{
    public interface IMouseEvent
    {

        float ScreenX { get; }

        float ScreenY { get; }

        float Width { get; }

        float Height { get; }

    }
}
