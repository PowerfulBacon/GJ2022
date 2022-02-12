using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Items.StockParts.Cells
{
    public abstract class Cell : Entity
    {

        public Cell(Vector<float> position) : base(position, Layers.LAYER_ITEM)
        {
            Charge = MaxCharge;
        }

        public Cell(Entity location) : base(location, Layers.LAYER_ITEM)
        {
            Charge = MaxCharge;
        }

        public abstract float MaxCharge { get; }

        public float Charge { get; private set; }

        public void UpdateOverlays()
        {
            Renderable.ClearOverlays();
            Renderable.AddOverlay("charge", new StandardRenderable("power.cell-o1", true), Layers.LAYER_ITEM + 0.01f);
        }

        /// <summary>
        /// Attempts to put power in the cell, returns the amount actually put
        /// </summary>
        public float GivePower(float amount)
        {
            float powerGiven = Math.Min(amount, MaxCharge - Charge);
            Charge += powerGiven;
            return powerGiven;
        }

        /// <summary>
        /// Attempts to take power from the cell.
        /// Returns the amount of power taken successfully.
        /// </summary>
        public float TakePower(float amount)
        {
            float powerTaken = Math.Min(amount, Charge);
            Charge -= powerTaken;
            return powerTaken;
        }

    }
}
