using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Components;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.EntityLoading;
using GJ2022.Game;
using GJ2022.Game.GameWorld;
using GJ2022.Game.Power;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Rendering.Text;
using GJ2022.Subsystems.Processing;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures.Power
{
    public class AreaPowerController : Structure, IPowerGridConnected, IProcessable
    {

        //The cell inside this APC
        private Entity insertedCell;

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
            World.AddAreaPowerController((int)position.X, (int)position.Y, this);
            World.AddPowernetInteractor((int)position.X, (int)position.Y, PowernetInteractor);
            Position += offset;
            //TODO: Move this to a definition file
            insertedCell = EntityCreator.CreateEntity<Entity>("Cell_Standard", position);
            insertedCell.Location = this;
            textObjectOffset = new Vector<float>(0, -0.3f);
            attachedTextObject = new TextObject($"{insertedCell?.SendSignalSynchronously(Signal.SIGNAL_GET_STORED_POWER)}", Colour.White, Position + textObjectOffset, TextObject.PositionModes.WORLD_POSITION, 0.4f);
            UpdateOverlays(0);
            PowerProcessingSystem.Singleton.StartProcessing(this);
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
            World.RemoveAreaPowerController((int)Position.X, (int)Position.Y, this);
            World.RemovePowernetInteractor((int)Position.X, (int)Position.Y, PowernetInteractor);
            PowerProcessingSystem.Singleton.StopProcessing(this);
            return base.Destroy();
        }

        int overlayState = 0;

        public void UpdateOverlays(float chargeRate)
        {
            attachedTextObject.Text = $"{insertedCell?.SendSignalSynchronously(Signal.SIGNAL_GET_STORED_POWER)}";
            int newOverlayState;
            bool maxCharge = Convert.ToSingle(insertedCell.SendSignalSynchronously(Signal.SIGNAL_GET_POWER_DEMAND, 1)) == 0;
            if (PowernetInteractor.AttachedPowernet == null || insertedCell == null || (chargeRate <= 0 && !maxCharge))
                newOverlayState = 1;
            else if (maxCharge)
                newOverlayState = 2;
            else
                newOverlayState = 3;
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
                case 3:
                    Renderable.AddOverlay("power", new StandardRenderable("power.apco3-1"), Layers.LAYER_STRUCTURE + 0.01f);
                    break;
            }
        }

        public void Process(float deltaTime)
        {
            //Check if we are attached to a powernet
            if (PowernetInteractor.AttachedPowernet == null)
            {
                UpdateOverlays(0);
                return;
            }
            //Check we actually contain a cell
            if (insertedCell == null)
            {
                UpdateOverlays(0);
                PowernetInteractor.Demand = 0;
                return;
            }
            //Charge our cell if we can
            float powerDemand = Convert.ToSingle(insertedCell.SendSignalSynchronously(Signal.SIGNAL_GET_POWER_DEMAND, deltaTime));
            PowernetInteractor.Demand = powerDemand;
            float powerDelta = PowernetInteractor.AttachedPowernet.ReceievePower(powerDemand);
            insertedCell.SendSignal(Signal.SIGNAL_ITEM_GIVE_POWER, powerDelta);
            UpdateOverlays(powerDelta);
        }

    }
}
