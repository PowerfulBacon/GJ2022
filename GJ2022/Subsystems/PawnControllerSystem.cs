using GJ2022.Entities.Blueprints;
using GJ2022.Entities.Pawns;
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

        public static Dictionary<Vector<float>, Dictionary<int, Blueprint>> QueuedBlueprints { get; set; } = new Dictionary<Vector<float>, Dictionary<int, Blueprint>>();

        public Pawn SelectedPawn { get; private set; }

        public void SelectPawn(Pawn target)
        {
            SelectedPawn = target;
            UserInterfaceCreator.SelectorTextObject.Text = $"Selected Pawn: {target}";
        }

        public static void QueueBlueprint(Vector<float> position, Blueprint blueprint, int layer)
        {
            //Check if the blueprint is redundant
            //TODO
            /*if (World.GetTurf((int)position.X, (int)position.Y)?== blueprint.BlueprintDetail.CreatedDef)
            {
                blueprint.Destroy();
                return;
            }*/
            //Check for existing blurprints
            if (QueuedBlueprints.ContainsKey(position) && QueuedBlueprints[position].ContainsKey(layer))
            {
                if (QueuedBlueprints[position][layer].BlueprintDetail.Priority <= blueprint.BlueprintDetail.Priority)
                    QueuedBlueprints[position][layer].Destroy();
                else
                {
                    //Destroy ourselves
                    blueprint.Destroy();
                    return;
                }
            }
            //Make a new blueprint at this location
            if (!QueuedBlueprints.ContainsKey(position))
            {
                QueuedBlueprints.Add(position, new Dictionary<int, Blueprint>());
            }
            QueuedBlueprints[position].Add(layer, blueprint);
        }

        public override void Fire(Window window)
        { }

        public override void InitSystem()
        { }

        protected override void AfterWorldInit()
        { }

    }
}
