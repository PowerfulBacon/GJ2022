using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Pawns
{
    public class Human : Pawn
    {

        public Human(Vector<float> position) : base(position)
        {
            Renderable.AddOverlay("head", new StandardRenderable("human_head_male"), Layers.LAYER_PAWN + 0.01f);
            Renderable.AddOverlay("rarm", new StandardRenderable("human_rightarm"), Layers.LAYER_PAWN + 0.01f);
            Renderable.AddOverlay("larm", new StandardRenderable("human_leftarm"), Layers.LAYER_PAWN + 0.01f);
            Renderable.AddOverlay("rhand", new StandardRenderable("human_righthand"), Layers.LAYER_PAWN + 0.02f);
            Renderable.AddOverlay("lhand", new StandardRenderable("human_lefthand"), Layers.LAYER_PAWN + 0.02f);
            Renderable.AddOverlay("rleg", new StandardRenderable("human_rightleg"), Layers.LAYER_PAWN + 0.01f);
            Renderable.AddOverlay("lleg", new StandardRenderable("human_leftleg"), Layers.LAYER_PAWN + 0.01f);
            Renderable.UpdateDirection(Directions.EAST);
        }

        protected override Renderable Renderable { get; set; } = new StandardRenderable("human_body_male");

        protected override void AddEquipOverlay(InventorySlot targetSlot, IEquippable item)
        {
            Renderable.AddOverlay($"wear_{InventoryHelper.GetSlotAppend(targetSlot)}", new StandardRenderable($"{item.equipTexture}_{InventoryHelper.GetSlotAppend(targetSlot)}"), Layers.LAYER_PAWN + 0.08f);
        }
    }
}
