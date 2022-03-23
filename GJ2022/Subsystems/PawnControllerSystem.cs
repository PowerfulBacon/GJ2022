using GJ2022.Entities.Blueprints;
using GJ2022.Entities.Pawns;
using GJ2022.Game.Construction;
using GJ2022.Game.GameWorld;
using GJ2022.UserInterface;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System.Collections.Generic;

namespace GJ2022.Subsystems
{
    public class PawnControllerSystem : Subsystem
    {

        public static PawnControllerSystem Singleton { get; } = new PawnControllerSystem();

        public override int sleepDelay => 100;

        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_FIRE;

        public Pawn SelectedPawn { get; private set; }

        public void SelectPawn(Pawn target)
        {
            SelectedPawn = target;
            UserInterfaceCreator.SelectorTextObject.Text = $"Selected Pawn: {target}";
        }

        public override void Fire(Window window)
        { }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

    }
}
