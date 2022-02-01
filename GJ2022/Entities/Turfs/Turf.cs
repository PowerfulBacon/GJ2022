//#define ATMOS_DEBUG

using GJ2022.Atmospherics;
using GJ2022.Atmospherics.Block;
using GJ2022.Audio;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System;

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

        public Turf(int x, int y) : base(new Vector<float>(x, y), Layers.LAYER_TURF)
        {
            X = x;
            Y = y;
            //Destroy the old turf
            Turf oldTurf = World.GetTurf(x, y);
            //Set the new turf
            oldTurf?.Destroy(true);
            World.SetTurf(x, y, this);
            //TEMP:
            if (oldTurf == null)
                AtmosphericsSystem.Singleton.OnTurfCreated(this);
            else
                AtmosphericsSystem.Singleton.OnTurfChanged(oldTurf, this);
            //Set the direction
            Direction = Directions.NONE;
#if ATMOS_DEBUG
            //Create the atmos indicator
            attachedTextObject = new Rendering.Text.TextObject(creationStatus ? "C0" : "M0", creationStatus ? Colour.White : Colour.Green, new Vector<float>(X, Y), Rendering.Text.TextObject.PositionModes.WORLD_POSITION, 0.3f);
            AtmosphericsSystem.Singleton.StartProcessing(this);
#endif
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

        public void AtmosphericPressureChangeReact(Vector<float> flowPoint, float force)
        {
            if (force > 1)
            {
                new AudioSource().PlaySound("effects/space_wind.wav", 0, 0);
            }
        }

        public virtual void OnAtmopshereContentsChanged(AtmosphericBlock block)
        {
            Renderable.ClearOverlays();
            if (block != null)
            {
                foreach (PressurisedGas gas in block.ContainedAtmosphere.AtmosphericContents.Values)
                {
                    Renderable.AddOverlay($"atmosphere_{gas.gas}", new StandardRenderable(gas.gas.OverlayTexture, true), Layers.LAYER_GAS);
                }
            }
        }

        public virtual void OnAtmosphericBlockChanged(AtmosphericBlock block)
        {
            OnAtmopshereContentsChanged(block);
#if ATMOS_DEBUG
            if(block != null)
                Renderable.AddOverlay("atmosphere", new StandardRenderable("area_stockpile", true), Layers.LAYER_USER_INTERFACE);
#endif
        }

        public void Process(float deltaTime)
        {
            attachedTextObject.Text = Atmosphere != null
                ? $"{Math.Round(Atmosphere.ContainedAtmosphere.KiloPascalPressure, 2)}@{Math.Round(Atmosphere.ContainedAtmosphere.KelvinTemperature, 0)}k"
                : $"N/A";
        }
    }

}
