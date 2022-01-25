using GJ2022.Rendering.RenderSystems.Renderables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.UserInterface.Components
{
    public abstract class UserInterfaceComponent
    {

        //The renderable attached to thie UI component.
        public abstract Renderable Renderable { get; }

        //Positional stuff

    }
}
