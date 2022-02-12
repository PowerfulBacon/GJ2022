using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.Items.StockParts.Cells;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures.Power
{
    public class AreaPowerController : Structure
    {

        //The cell inside this APC
        private Cell insertedCell;

        public AreaPowerController(Vector<float> position, Directions direction) : base(position, Layers.LAYER_STRUCTURE)
        {
            float offsetAmount = 0.7f;
            switch (direction)
            {
                case Directions.EAST:
                    offset = new Vector<float>(offsetAmount, 0);
                    break;
                case Directions.WEST:
                    offset = new Vector<float>(-offsetAmount, 0);
                    break;
                case Directions.NORTH:
                    offset = new Vector<float>(0, offsetAmount);
                    break;
                case Directions.SOUTH:
                    offset = new Vector<float>(0, -offsetAmount);
                    break;
            }
            World.AddAreaPowerController((int)position[0], (int)position[1], this);
            Position += offset;
            insertedCell = new StandardCell(this);
            UpdateOverlays();
        }

        //Display offset, will screw up the positional checks of this
        private Vector<float> offset;

        public override Renderable Renderable { get; set; } = new StandardRenderable("power.apc0");

        public override bool Destroy()
        {
            Position -= offset;
            World.RemoveAreaPowerController((int)Position[0], (int)Position[1], this);
            return base.Destroy();
        }

        public void UpdateOverlays()
        {
            Renderable.ClearOverlays();
            Renderable.AddOverlay("power", new StandardRenderable("power.apco3-0"), Layers.LAYER_STRUCTURE + 0.01f);
        }

    }
}
