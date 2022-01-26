using GJ2022.Entities.Blueprints;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Game.GameWorld;
using GJ2022.Managers.Stockpile;
using GJ2022.Pathfinding;
using GJ2022.PawnBehaviours;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.RenderSystems.Renderables;
using GJ2022.Subsystems;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Entities.Pawns
{
    public class Pawn : Entity, IProcessable
    {

        //The renderable attached to our pawn
        protected override Renderable Renderable { get; set; } = new CircleRenderable(Colour.Yellow);

        //TODO: Draw debug pathfinding lines
        public static bool DrawLines = false;

        //Is this pawn destroyed?
        public bool Destroyed { get; set; } = false;

        //The AI controller
        public PawnBehaviour behaviourController;

        //The intended destination
        private Entity entityTargetDestination;
        //Positional destination
        //Entity target destination takes precidence over this
        private Vector<float> targetDestinationPosition;
        //The path we are currently following
        private Path followingPath = null;
        //The position we currently are along this path
        private int positionOnPath;

        public Pawn(Vector<float> position) : base(position, Layers.LAYER_PAWN)
        {
            PawnControllerSystem.Singleton.StartProcessing(this);
        }

        public void MoveTowardsEntity(Entity target)
        {
            if (target == entityTargetDestination)
                return;
            //Set our new target destination
            entityTargetDestination = target;
            //Nullify the path we were following (calculate a new one)
            followingPath = null;
        }

        public void MoveTowardsPosition(Vector<float> position)
        {
            if (targetDestinationPosition == position)
                return;
            //Set our new target destination
            targetDestinationPosition = position;
            //Nullify the entity target destination
            entityTargetDestination = null;
            //Nullify the path we were following (calculate a new one)
            followingPath = null;
        }

        public void Process(float deltaTime)
        {
            //Hazard reaction, prepare to panic
            if (HazardReact())
                return;
        }

        private bool HazardReact()
        {
            //Detect unhandled hazards
            //behaviourController.PawnActionIntercept();
            return false;
        }

        public override bool Destroy()
        {
            base.Destroy();
            PawnControllerSystem.Singleton.StopProcessing(this);
            Destroyed = true;
            return true;
        }

    }
}
