using GJ2022.Atmospherics.Block;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Turfs
{

    public abstract class Turf : Entity, IDestroyable, IProcessable
    {

        //Position of the turf
        public int X { get; }
        public int Y { get; }

        public bool Destroyed { get; set; } = false;

        //Atmospheric block of this turf. Null represents space.
        public AtmosphericBlock Atmosphere { get; set; } = null;

        bool creationStatus = false;

        public Turf(int x, int y) : base(new Vector<float>(x, y), Layers.LAYER_TURF)
        {
            X = x;
            Y = y;
            //Destroy the old turf
            Turf oldTurf = World.GetTurf(x, y);
            creationStatus = oldTurf == null;
            //Set the new turf
            oldTurf?.Destroy(true);
            World.SetTurf(x, y, this);
            //Create the atmos indicator
            attachedTextObject = new Rendering.Text.TextObject(creationStatus ? "C0" : "M0", creationStatus ? Colour.White : Colour.Green, new Vector<float>(X, Y), Rendering.Text.TextObject.PositionModes.WORLD_POSITION, 0.3f);
            //TEMP:
            if (oldTurf == null)
                AtmosphericsSystem.Singleton.OnTurfCreated(this);
            else
                AtmosphericsSystem.Singleton.OnTurfChanged(oldTurf, this);
            //Set the direction
            Direction = Directions.NONE;
            AtmosphericsSystem.Singleton.StartProcessing(this);
        }

        //Set destroyed
        public bool Destroy(bool changed)
        {
            //If we weren't changed, destroy the turf
            if (!changed)
                AtmosphericsSystem.Singleton.OnTurfDestroyed(this);
            return Destroy();
        }

        public override bool Destroy()
        {
            //Dereference
            World.SetTurf(X, Y, null);
            //Set destroyed
            Destroyed = true;
            return base.Destroy();
        }

        public abstract bool AllowAtmosphericFlow { get; }

        //Temp


        public void AtmosphericPressureChangeReact(Vector<float> flowPoint, float force)
        {

        }

        public virtual void OnAtmosphereChanged(AtmosphericBlock block)
        {
            Renderable.ClearOverlays();
            if(block != null)
                Renderable.AddOverlay("atmosphere", new StandardRenderable("area_stockpile", true), Layers.LAYER_USER_INTERFACE);
        }

        public void Process(float deltaTime)
        {
            if(Atmosphere != null)
                attachedTextObject.Text = $"{Atmosphere.ContainedAtmosphere.KiloPascalPressure} kPa";
            else
                attachedTextObject.Text = $"0 kPa";
        }
    }

}
