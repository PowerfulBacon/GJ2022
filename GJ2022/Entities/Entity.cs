using GJ2022.Rendering;
using GJ2022.Utility.MathConstructs;
using System;

namespace GJ2022.Entities
{
    public abstract class Entity : Renderable
    {

        public Entity(Vector position) : base(position)
        {
            //If this contains an initialize behaviour it will be added, otherwise it will be null and skipped
            //InitializeSubsystem.Singleton.Initialize(this as IInitializeBehaviour);
        }

    }
}
