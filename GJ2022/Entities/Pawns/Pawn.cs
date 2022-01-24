using GJ2022.Entities.Abstract;
using GJ2022.Entities.Blueprints;
using GJ2022.Entities.ComponentInterfaces;
using GJ2022.Pathfinding;
using GJ2022.Rendering.RenderSystems;
using GJ2022.Rendering.RenderSystems.Interfaces;
using GJ2022.Rendering.RenderSystems.LineRenderer;
using GJ2022.Rendering.Textures;
using GJ2022.Subsystems;
using GJ2022.Utility.Helpers;
using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Entities.Pawns
{
    public class Pawn : Entity, ICircleRenderable, IProcessable, IMovable
    {

        public Pawn(Vector<float> position) : base(position)
        {
            PawnControllerSystem.Singleton.StartProcessing(this);
            CircleRenderSystem.Singleton.StartRendering(this);
        }

        public Colour Colour { get; } = Colour.Yellow;

        public RenderSystem<ICircleRenderable, CircleRenderSystem> RenderSystem => CircleRenderSystem.Singleton;

        private Blueprint workTarget;
        private Line line;

        private List<Line> lines = new List<Line>();

        private Path followingPath = null;
        private int positionOnPath;

        public void Process(float deltaTime)
        {
            //Target no longer exists
            if (workTarget != null && workTarget.IsDestroyed())
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
                    //Pathfind
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
                nextPosition = workTarget.Position;
            }
            //Move towards
            Position.MoveTowards(nextPosition, 0.1f, deltaTime);
            UpdatePosition();
            //ugly line
            line.Start = Position;

            //If distance < build range, build it
            if (Position.IgnoreZ() == workTarget.Position.IgnoreZ())
            {
                workTarget.Complete();
            }
            else if (Position.IgnoreZ() == nextPosition.IgnoreZ())
            {
                positionOnPath++;
            }
        }

        public Vector<float> GetPosition()
        {
            return Position;
        }

        public RendererTextureData GetRendererTextureData()
        {
            return TextureCache.GetTexture(TextureCache.ERROR_ICON_STATE);
        }

        private Dictionary<object, int> renderableBatchIndex = new Dictionary<object, int>();

        public void SetRenderableBatchIndex(object associatedSet, int index)
        {
            if (renderableBatchIndex.ContainsKey(associatedSet))
                renderableBatchIndex[associatedSet] = index;
            else
                renderableBatchIndex.Add(associatedSet, index);
        }

        /// <summary>
        /// Returns the renderable batch index in the provided set.
        /// Returns -1 if failed.
        /// </summary>
        public int GetRenderableBatchIndex(object associatedSet)
        {
            if (renderableBatchIndex.ContainsKey(associatedSet))
                return renderableBatchIndex[associatedSet];
            else
                return -1;
        }

        private bool destroyed = false;

        public bool Destroy()
        {
            CircleRenderSystem.Singleton.StopRendering(this);
            PawnControllerSystem.Singleton.StopProcessing(this);
            destroyed = true;
            return true;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }

        public void UpdatePositionBatch()
        {
            //Update position in renderer
            if (renderableBatchIndex.Count > 0)
                (renderableBatchIndex.Keys.ElementAt(0) as RenderBatchSet<ICircleRenderable, CircleRenderSystem>)?.UpdateBatchData(this, 0);
        }

        public void OnMoved(Vector<float> previousPosition)
        {
            return;
        }

    }
}
