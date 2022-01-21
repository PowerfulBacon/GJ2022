using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.ComponentInterfaces.MouseEvents
{
    public interface IMouseEvent
    {

        float WorldX { get; }

        float WorldY { get; }

        float Width { get; }

        float Height { get; }

    }
}
