﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items.StockParts.Cells;
using GJ2022.Game;
using GJ2022.Game.GameWorld;
using GJ2022.Game.Power;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures.Power
{
    public class AreaPowerController : Structure, IPowerGridConnected, IProcessable
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
            World.AddPowernetInteractor((int)position[0], (int)position[1], PowernetInteractor);
            Position += offset;
            insertedCell = new StandardCell(this);
            insertedCell.TakePower(insertedCell.MaxCharge);
            textObjectOffset = new Vector<float>(0, -0.3f);
            attachedTextObject = new TextObject($"{insertedCell?.Charge}", Colour.White, Position + textObjectOffset, TextObject.PositionModes.WORLD_POSITION, 0.4f);
            UpdateOverlays();
        }

        //Display offset, will screw up the positional checks of this
        private Vector<float> offset;

        //Set up the renderable
        public override Renderable Renderable { get; set; } = new StandardRenderable("power.apc0");

        //Create the powernet interactor
        public PowernetInteractor PowernetInteractor { get; } = new PowernetInteractor();

        public override bool Destroy()
        {
            Position -= offset;
            World.RemoveAreaPowerController((int)Position[0], (int)Position[1], this);
            World.RemovePowernetInteractor((int)Position[0], (int)Position[1], PowernetInteractor);
            return base.Destroy();
        }

        int overlayState = 0;

        public void UpdateOverlays()
        {
            attachedTextObject.Text = $"{insertedCell?.Charge}";
            int newOverlayState;
            if (PowernetInteractor.AttachedPowernet == null || insertedCell == null)
                newOverlayState = 1;
            else
                newOverlayState = 2;
            if (newOverlayState == overlayState)
                return;
            overlayState = newOverlayState;
            Renderable.ClearOverlays();
            switch (overlayState)
            {
                case 1:
                    Renderable.AddOverlay("power", new StandardRenderable("power.apco3-0"), Layers.LAYER_STRUCTURE + 0.01f);
                    break;
                case 2:
                    Renderable.AddOverlay("power", new StandardRenderable("power.apco3-2"), Layers.LAYER_STRUCTURE + 0.01f);
                    break;
            }
        }

        public void Process(float deltaTime)
        {
            UpdateOverlays();
            //Check if we are attached to a powernet
            if (PowernetInteractor.AttachedPowernet == null)
            {
                return;
            }
            //Check we actually contain a cell
            if (insertedCell == null)
            {
                PowernetInteractor.Demand = 0;
                return;
            }
            //Charge our cell if we can
            PowernetInteractor.Demand = insertedCell.ChargeRate;
            insertedCell.GivePower(PowernetInteractor.AttachedPowernet.ReceievePower(insertedCell.ChargeRate * deltaTime));
        }

    }
}
