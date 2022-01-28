using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Entities.ComponentInterfaces.MouseEvents
{
    interface IMouseRightPress : IMouseEvent
    {

        void OnRightPressed(GLFW.Window window);

    }
}
