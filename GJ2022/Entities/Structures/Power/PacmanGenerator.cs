using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Game.GameWorld;
using GJ2022.Game.Power;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems.Processing;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Entities.Structures.Power
{
    class PacmanGenerator : Structure, IProcessable
    {

        public PacmanGenerator(Vector<float> position) : base(position, Layers.LAYER_STRUCTURE)
        {
            World.AddPowernetInteractor((int)Position[0], (int)Position[1], PowernetInteractor);
            PowerProcessingSystem.Singleton.StartProcessing(this);
        }

        public override Renderable Renderable { get; set; } = new StandardRenderable("power.portgen0_0");

        //10 kW of power supply
        public virtual float PowerSupply { get; } = 10;

        //The powernet interactor
        public PowernetInteractor PowernetInteractor { get; } = new PowernetInteractor();

        public override bool Destroy()
        {
            World.RemovePowernetInteractor((int)Position[0], (int)Position[1], PowernetInteractor);
            PowerProcessingSystem.Singleton.StopProcessing(this);
            return base.Destroy();
        }

        public void Process(float deltaTime)
        {
            PowernetInteractor.Supply = PowerSupply * deltaTime;
            PowernetInteractor.AttachedPowernet.SendPower(PowerSupply * deltaTime);
        }
    }
}
