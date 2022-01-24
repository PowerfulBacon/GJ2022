using GJ2022.Entities.Blueprints;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Entities.Items;
using GJ2022.Game.GameWorld;
using GJ2022.Managers;
using GJ2022.Pathfinding;
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

        protected override Renderable Renderable { get; set; } = new CircleRenderable(Colour.Yellow);

        public bool Destroyed { get; set; } = false;

        private Item heldItem;

        private Item itemTarget;
        private Blueprint workTarget;
        private Line line;

        private List<Line> lines = new List<Line>();

        private Path followingPath = null;
        private int positionOnPath;

        public Pawn(Vector<float> position) : base(position, Layers.LAYER_PAWN)
        {
            PawnControllerSystem.Singleton.StartProcessing(this);
        }

        public void Process(float deltaTime)
        {
            //Target no longer exists
            if (workTarget != null && workTarget.Destroyed)
                workTarget = null;
            //Move towards the nearest blueprint and build it
            if (workTarget == null)
            {
                if (PawnControllerSystem.QueuedBlueprints.Count > 0)
                {
                    Vector<float> workTargetPosition = ListPicker.Pick(PawnControllerSystem.QueuedBlueprints.Keys);
                    workTarget = PawnControllerSystem.QueuedBlueprints[workTargetPosition].Values.ElementAt(0);
                    if (line == null)
                    {
                        line = Line.StartDrawingLine(Position, workTargetPosition, Colour.Cyan);
                    }
                    else
                    {
                        line.End = workTarget.Position;
                    }
                    //Locate materials
                    if (workTarget.HasMaterials())
                    {
                        PathfindingSystem.Singleton.RequestPath(
                            new PathfindingRequest(
                                Position,
                                workTargetPosition,
                                (Path path) =>
                                {
                                    foreach (Line l in lines)
                                    {
                                        l.StopDrawing();
                                    }
                                    lines.Clear();
                                    Log.WriteLine($"Located path with length {path.Points}");
                                    for (int i = 0; i < path.Points.Count - 1; i++)
                                    {
                                        lines.Add(Line.StartDrawingLine(path.Points[i].SetZ(10), path.Points[i + 1].SetZ(10)));
                                    }
                                    followingPath = path;
                                    positionOnPath = 0;
                                },
                                () => { workTarget = null; }
                            ));
                    }
                    else
                    {
                        //Look for required items
                        (Type, int) requiredItems = workTarget.GetRequiredMaterial() ?? (null, 0);
                        //Bug
                        if (requiredItems.Item1 == null)
                            throw new Exception("This shouldn't happen");
                        //Locate the item
                        Item locatedItem = StockpileManager.LocateItemInStockpile(requiredItems.Item1);
                        if (locatedItem == null)
                        {
                            //Item isn't in stockpile
                            workTarget = null;
                            return;
                        }
                        //Path towards the item
                        itemTarget = locatedItem;
                        PathfindingSystem.Singleton.RequestPath(
                            new PathfindingRequest(
                                Position,
                                itemTarget.Position,
                                (Path path) =>
                                {
                                    foreach (Line l in lines)
                                    {
                                        l.StopDrawing();
                                    }
                                    lines.Clear();
                                    Log.WriteLine($"Located path with length {path.Points}");
                                    for (int i = 0; i < path.Points.Count - 1; i++)
                                    {
                                        lines.Add(Line.StartDrawingLine(path.Points[i].SetZ(10), path.Points[i + 1].SetZ(10)));
                                    }
                                    followingPath = path;
                                    positionOnPath = 0;
                                },
                                () => { itemTarget = null; workTarget = null; }
                            ));
                    }
                }
                else
                {
                    line?.StopDrawing();
                    line = null;
                    return;
                }
            }
            if (followingPath == null || workTarget == null)
                return;
            Vector<float> nextPosition;
            if (positionOnPath < followingPath.Points.Count)
            {
                nextPosition = followingPath.Points[positionOnPath];
            }
            else
            {
                nextPosition = itemTarget?.Position ?? workTarget.Position;
            }
            //Move towards
            Position = Position.MoveTowards(nextPosition, 0.1f, deltaTime);
            //ugly line
            line.Start = Position;

            //If distance < build range, build it
            if (Position.IgnoreZ() == itemTarget?.Position.IgnoreZ())
            {
                //Pickup the item
                itemTarget.Location = this;
                //Null the item target
                heldItem = itemTarget;
                itemTarget = null;
                //Path towards the work target
                PathfindingSystem.Singleton.RequestPath(
                    new PathfindingRequest(
                        Position,
                        workTarget.Position.IgnoreZ(),
                        (Path path) =>
                        {
                            foreach (Line l in lines)
                            {
                                l.StopDrawing();
                            }
                            lines.Clear();
                            Log.WriteLine($"Located path with length {path.Points}");
                            for (int i = 0; i < path.Points.Count - 1; i++)
                            {
                                lines.Add(Line.StartDrawingLine(path.Points[i].SetZ(10), path.Points[i + 1].SetZ(10)));
                            }
                            followingPath = path;
                            positionOnPath = 0;
                        },
                        () =>
                        {
                            //Null the work target
                            workTarget = null;
                            //Drop held items
                            if(itemTarget != null)
                                itemTarget.Location = null;
                            heldItem = null;
                        }
                    ));
            }
            else if (Position.IgnoreZ() == workTarget.Position.IgnoreZ())
            {
                //Put materials into the work target
                if (heldItem != null)
                    workTarget.PutMaterials(heldItem);
                heldItem = null;
                if (workTarget.HasMaterials())
                    workTarget.Complete();
                else
                    workTarget = null;
            }
            else if (Position.IgnoreZ() == nextPosition.IgnoreZ())
            {
                positionOnPath++;
            }
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
