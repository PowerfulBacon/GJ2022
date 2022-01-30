using GJ2022.Game.GameWorld;
using GJ2022.Pathfinding;
using GJ2022.PawnBehaviours;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class PathfindingSystem : Subsystem
    {

        public static PathfindingSystem Singleton { get; } = new PathfindingSystem();

        //The size of the border around the square that connects the start and the end.
        //A value of 30 means that we can search 30 to the left, right, up and down of points that take us further the end.
        //(The bounds of which the path can move)
        private const int SEARCH_OFFSET_RANGE = 30;

        //Enum that represents directions that a point can travel in
        public enum ConnectingDirections
        {
            NORTH = 1 << 0,
            EAST = 1 << 1,
            SOUTH = 1 << 2,
            WEST = 1 << 3,
            //To make it print nicer
            NORTH_EAST = NORTH | EAST,
            NORTH_WEST = NORTH | WEST,
            SOUTH_EAST = SOUTH | EAST,
            SOUTH_WEST = SOUTH | WEST,
            EAST_WEST = EAST | WEST,
            NORTH_SOUTH = NORTH | SOUTH,
            NORTH_EAST_SOUTH = NORTH | EAST | SOUTH,
            EAST_SOUTH_WEST = EAST | SOUTH | WEST,
            SOUTH_WEST_NORTH = NORTH | SOUTH | WEST,
            WEST_NORTH_EAST = NORTH | EAST | WEST,
            ALL = NORTH | EAST | SOUTH | WEST,
            NONE = 0,
        };

        //Little delay
        public override int sleepDelay => 1000;

        //No processing, but fires
        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        public override void Fire(Window window)
        {
            Log.WriteLine($"Currently processing {runningAmt} paths.");
        }

        public override void InitSystem() { }

        protected override void AfterWorldInit() { }

        public static volatile int runningAmt = 0;

        public void RequestPath(PathfindingRequest request)
        {
            runningAmt++;
            Task.Run(() => ProcessPath(request));
        }

        /// <summary>
        /// For testing only
        /// </summary>
        public static void ProcessPathImmediate(Vector<int> start, Vector<int> end)
        {
            Singleton.ProcessPath(new PathfindingRequest(start, end, PawnHazards.ALL, (Path located) =>
            {
                foreach (Vector<int> node in located.Points)
                {
                    Log.WriteLine(node);
                }
            }));
        }

        /// <summary>
        /// Process a path
        /// </summary>
        private void ProcessPath(PathfindingRequest request)
        {
            //If the end of the request is blocked, don't both
            if (World.IsSolid(request.End))
            {
                request.failedDelegate?.Invoke();
                return;
            }
            //Create the process data
            PathfindingProcessData processData = new PathfindingProcessData();
            processData.End = request.End;
            //Create the search borders
            processData.MinimumX = Math.Min(request.Start[0], request.End[0]) - SEARCH_OFFSET_RANGE;
            processData.MaximumX = Math.Max(request.Start[0], request.End[0]) + SEARCH_OFFSET_RANGE;
            processData.MinimumY = Math.Min(request.Start[1], request.End[1]) - SEARCH_OFFSET_RANGE;
            processData.MaximumY = Math.Max(request.Start[1], request.End[1]) + SEARCH_OFFSET_RANGE;
            //Begin processing the start point (It will propogate out from here)
            PathNode sourceNode = new PathNode(request.Start, CalculateDistance(request.Start, request.End));
            processData.AddNode(sourceNode);
            //Do processing
            int i = 0;
            while (processData.HasNext())
            {
                PathNode processing = processData.PopProcessQueue();
                //If the processing is the end, we have won.
                if (processing.Position == request.End)
                {
                    Path path = new Path(processing);
                    request.foundDelegate?.Invoke(path);
                    runningAmt--;
                    return;
                }
                //Otherwise add surrounding nodes
                AddSurroundingNodes(processing, request.ignoringHazards, ref processData);
                i++;
                Thread.Yield();
            }
            runningAmt--;
            request.failedDelegate?.Invoke();
        }

        private void AddSurroundingNodes(PathNode current, PawnHazards ignoringHazards, ref PathfindingProcessData processData)
        {
            //Get the valid directions you can travel in from this point.
            ConnectingDirections validDirections = GetValidDirections(processData, ignoringHazards, current.Position, current.SourceNode?.Position ?? current.Position);
            //Process north node
            if ((validDirections & ConnectingDirections.NORTH) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(0, 1), ConnectingDirections.NORTH);
            if ((validDirections & ConnectingDirections.EAST) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(1, 0), ConnectingDirections.EAST);
            if ((validDirections & ConnectingDirections.SOUTH) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(0, -1), ConnectingDirections.SOUTH);
            if ((validDirections & ConnectingDirections.WEST) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(-1, 0), ConnectingDirections.WEST);
            if ((validDirections & ConnectingDirections.NORTH_EAST) == ConnectingDirections.NORTH_EAST && !World.IsSolid(current.Position + new Vector<int>(1, 1)))
                CheckAddNode(current, ref processData, new Vector<int>(1, 1), ConnectingDirections.NORTH_EAST);
            if ((validDirections & ConnectingDirections.SOUTH_EAST) == ConnectingDirections.SOUTH_EAST && !World.IsSolid(current.Position + new Vector<int>(1, -1)))
                CheckAddNode(current, ref processData, new Vector<int>(1, -1), ConnectingDirections.SOUTH_EAST);
            if ((validDirections & ConnectingDirections.SOUTH_WEST) == ConnectingDirections.SOUTH_WEST && !World.IsSolid(current.Position + new Vector<int>(-1, -1)))
                CheckAddNode(current, ref processData, new Vector<int>(-1, -1), ConnectingDirections.SOUTH_WEST);
            if ((validDirections & ConnectingDirections.NORTH_WEST) == ConnectingDirections.NORTH_WEST && !World.IsSolid(current.Position + new Vector<int>(-1, 1)))
                CheckAddNode(current, ref processData, new Vector<int>(-1, 1), ConnectingDirections.NORTH_WEST);
        }

        private void CheckAddNode(PathNode current, ref PathfindingProcessData processData, Vector<int> offset, ConnectingDirections outputDir)
        {
            //Get the position of the next connecting node
            Vector<int> targetPosition = current.Position + offset;
            PathNode targetNode = processData.GetNode(targetPosition);
            //If it doesn't exist, add it
            if (targetNode == null)
            {
                PathNode pathNode = new PathNode(targetPosition, CalculateDistance(targetPosition, processData.End));
                pathNode.SourceNode = current;
                processData.AddNode(pathNode);
                processData.SetScannedDirection(current.Position, outputDir);
            }
            //if it does exist, check if its more efficient to pass through this node.
            else
            {
                float oldNodeScore = targetNode.NodeScore;
                //The score of the node was updated so we need to change it in the process data
                if (targetNode.CheckSourceNode(current))
                    processData.UpdateNode(targetNode, oldNodeScore);
            }
        }

        private float CalculateDistance(Vector<int> start, Vector<int> end)
        {
            //Number of moves we need to perform to go from A to B assuming no obstacles
            //Since diagonal movement is a single move, (0, 0) to (20, 30) is 20 diagonal and 10 moves,
            //The total number of moves is always just the larger of the 2 delta values.
            int deltaX = Math.Abs(start[0] - end[0]);
            int deltaY = Math.Abs(start[1] - end[1]);
            return Math.Max(deltaX, deltaY);
        }

        /// <summary>
        /// Returns a bitflag containing all possible directions you can move in
        /// 0000 = Nowhere
        /// 1000 = north (ConnectingDirections.NORTH)
        /// 1100 = north east (ConnectingDirections.NORTH | ConnectingDirections.EAST)
        /// etc.
        /// </summary>
        private ConnectingDirections GetValidDirections(PathfindingProcessData processData, PawnHazards ignoringHazards, Vector<int> position, Vector<int> source)
        {
            //Default valid connecting directions
            ConnectingDirections connectingDirections = 0;
            //Check north
            if (!IsPointChecked(processData, position + new Vector<int>(0, 1), ConnectingDirections.NORTH_SOUTH, ignoringHazards)
                    && position[1] < processData.MaximumY
                    && !World.IsSolid(position + new Vector<int>(0, 1))
                    && GravityCheck(ignoringHazards, position, source, ConnectingDirections.NORTH))
                connectingDirections |= ConnectingDirections.NORTH;
            //Check east
            if (!IsPointChecked(processData, position + new Vector<int>(1, 0), ConnectingDirections.EAST_WEST, ignoringHazards)
                    && position[0] < processData.MaximumX
                    && !World.IsSolid(position + new Vector<int>(1, 0))
                    && GravityCheck(ignoringHazards, position, source, ConnectingDirections.EAST))
                connectingDirections |= ConnectingDirections.EAST;
            //Check south
            if (!IsPointChecked(processData, position + new Vector<int>(0, -1), ConnectingDirections.NORTH_SOUTH, ignoringHazards)
                    && position[1] > processData.MinimumY
                    && !World.IsSolid(position + new Vector<int>(0, -1))
                    && GravityCheck(ignoringHazards, position, source, ConnectingDirections.SOUTH))
                connectingDirections |= ConnectingDirections.SOUTH;
            //Check west
            if (!IsPointChecked(processData, position + new Vector<int>(-1, 0), ConnectingDirections.EAST_WEST, ignoringHazards)
                    && position[0] > processData.MinimumX
                    && !World.IsSolid(position + new Vector<int>(-1, 0))
                    && GravityCheck(ignoringHazards, position, source, ConnectingDirections.WEST))
                connectingDirections |= ConnectingDirections.WEST;
            //Return valid directions
            return connectingDirections;
        }

        private bool IsPointChecked(PathfindingProcessData processData, Vector<int> position, ConnectingDirections sourceDirection, PawnHazards ignoringHazards)
        {
            //Ignoring gravity, just check if we processed it already
            if ((ignoringHazards & PawnHazards.HAZARD_GRAVITY) != 0)
                return processData.ProcessedPoints.ContainsKey(position);
            //If we aren't ignoring gravity check what way we came from
            if (!processData.ProcessedPoints.ContainsKey(position))
                return false;
            //If this location has gravity, then we checked it
            if (World.HasGravity(position))
                return true;
            //Check if we have scanned the incomming direction
            ConnectingDirections scannedDirections = processData.ProcessedPoints[position];
            return (scannedDirections & sourceDirection) != 0;
        }

        private bool GravityCheck(PawnHazards ignoringHazards, Vector<int> position, Vector<int> source, ConnectingDirections direction)
        {
            //Ignore gravity
            if ((ignoringHazards & PawnHazards.HAZARD_GRAVITY) != 0)
                return true;
            //Place has gravity
            if (World.HasGravity(position))
                return true;
            //We can move in a straight line without gravity
            int delta_x = position[0] - source[0];
            int delta_y = position[1] - source[1];
            //If the direction is north or south and we want to move vertically, allow it
            if (delta_x == 0 && (direction & ConnectingDirections.NORTH_SOUTH) != 0)
                return true;
            if (delta_y == 0 && (direction & ConnectingDirections.EAST_WEST) != 0)
                return true;
            //Cannot move in this direction
            return false;
        }

    }
}
