//#define ATMOS_DEBUG
//#define PRESSURE_OVERLAY

using GJ2022.Atmospherics;
using GJ2022.Atmospherics.Block;
using GJ2022.Audio;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using System;
using System.Linq;

namespace GJ2022.Entities.Turfs
{

    public class Turf : Entity, IDestroyable, IProcessable
    {

        //Position of the turf
        public int X { get; private set; }
        public int Y { get; private set; }

        public bool Destroyed { get; set; } = false;

        //Atmospheric block of this turf. Null represents space.
        public AtmosphericBlock Atmosphere { get; set; } = null;

        public bool AllowAtmosphericFlow { get; private set; } = false;

        public bool Solid { get; private set; } = false;

        public Turf() : base() { }

        [Obsolete]
        public Turf(int x, int y) : base(new Vector<float>(x, y), Layers.LAYER_TURF)
        {
            Initialize(new Vector<float>(x, y));
        }

        public override void Initialize(Vector<float> initializePosition)
        {
            X = (int)initializePosition.X;
            Y = (int)initializePosition.Y;
            //Set the position to update the renderable
            Position = new Vector<float>(X, Y);
            //Destroy the old turf
            Turf oldTurf = World.Current.GetTurf(X, Y);
            //Set the new turf
            oldTurf?.Destroy(true);
            World.Current.SetTurf(X, Y, this);
            //Set the direction
            Direction = Directions.NONE;
            //Atmos flow blocking
            if (!AllowAtmosphericFlow)
                World.Current.AddAtmosphericBlocker(X, Y, false);
            //Tell the atmos system a turf was created / changed at this location
            if (oldTurf == null)
                AtmosphericsSystem.Singleton.OnTurfCreated(this);
            else
                AtmosphericsSystem.Singleton.OnTurfChanged(oldTurf, this);
#if ATMOS_DEBUG
            //Create the atmos indicator
            attachedTextObject = new Rendering.Text.TextObject("0", Colour.White, new Vector<float>(X, Y), Rendering.Text.TextObject.PositionModes.WORLD_POSITION, 0.3f);
            AtmosphericsSystem.Singleton.StartProcessing(this);
#endif
#if PRESSURE_OVERLAY
            Renderable.AddOverlay("pressure", overlay, Layers.LAYER_TURF + 0.1f);
            AtmosphericsSystem.Singleton.StartProcessing(this);
#endif
        }

        //Set destroyed
        public bool Destroy(bool changed)
        {
#if PRESSURE_OVERLAY || ATMOS_DEBUG
            AtmosphericsSystem.Singleton.StopProcessing(this);
#endif
            //Atmos flow blocking
            if (!AllowAtmosphericFlow)
                World.Current.RemoveAtmosphericBlock(X, Y, false);
            //If we weren't changed, destroy the turf
            if (!changed)
                AtmosphericsSystem.Singleton.OnTurfDestroyed(this);
            return Destroy();
        }

        public override bool Destroy()
        {
            //Dereference
            World.Current.SetTurf(X, Y, null);
            //Set destroyed
            Destroyed = true;
            return base.Destroy();
        }

        public void AtmosphericPressureChangeReact(Vector<float> flowPoint, float force)
        {
            if (force > 1)
            {
                new AudioSource().PlaySound("effects/space_wind.wav", 0, 0);
            }
        }

        public virtual void OnAtmopshereContentsChanged(AtmosphericBlock block)
        {
            if (Renderable == null)
                return;
            //check if overlays changed
            lock (Renderable)
            {
                //Renderable.ClearOverlays();
                if (block != null)
                {
                    foreach (PressurisedGas gas in block.ContainedAtmosphere.AtmosphericContents.Values.ToList())
                    {
                        if (gas != null && !Renderable.HasOverlay($"atmosphere_{gas.gas}"))
                            Renderable.AddOverlay($"atmosphere_{gas.gas}", new StandardRenderable(gas.gas.OverlayTexture, true), Layers.LAYER_GAS);
                    }
                }
                else
                {
                    Renderable.ClearOverlays();
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

#if PRESSURE_OVERLAY
        StandardRenderable overlay = new StandardRenderable("white", true);
#endif

        public void Process(float deltaTime)
        {
#if PRESSURE_OVERLAY
            float p = Atmosphere?.ContainedAtmosphere?.KiloPascalPressure ?? 0;
            float pressureRed = (float)(2.0f / (1 + Math.Pow(Math.E, -p * 0.01f))) - 1.0f;
            overlay.SetColour(new Colour(1 - pressureRed, pressureRed, 0, 0.3f));
#endif
#if ATMOS_DEBUG
            attachedTextObject.Text = Atmosphere != null
                ? $"{Math.Round(Atmosphere.ContainedAtmosphere.KiloPascalPressure, 2)}@{Math.Round(Atmosphere.ContainedAtmosphere.KelvinTemperature, 0)}k"
                : $"N/A";
#endif
        }

        public override void SetProperty(string name, object property)
        {
            switch (name)
            {
                case "Solid":
                    Solid = (bool)property;
                    return;
                case "AllowAtmosphericFlow":
                    AllowAtmosphericFlow = (bool)property;
                    return;
            }
            base.SetProperty(name, property);
        }

    }

}
