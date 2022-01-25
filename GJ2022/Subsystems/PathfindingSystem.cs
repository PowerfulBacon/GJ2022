using GJ2022.Game.GameWorld;
using GJ2022.Pathfinding;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
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
        private enum ConnectingDirections
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
        };

        //Little delay
        public override int sleepDelay => 20;

        //No processing, but fires
        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_UPDATE;

        public override void Fire(Window window)
        {
            throw new NotImplementedException("Pathfinding system does not fire.");
        }

        public override void InitSystem() { }

        protected override void AfterWorldInit() { }

        public void RequestPath(PathfindingRequest request)
        {
            new Task(() => ProcessPath(request)).Start();
        }

        /// <summary>
        /// For testing only
        /// </summary>
        public static void ProcessPathImmediate(Vector<int> start, Vector<int> end)
        {
            Singleton.ProcessPath(new PathfindingRequest(start, end, (Path located) =>
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
                    return;
                }
                //Otherwise add surrounding nodes
                AddSurroundingNodes(processing, ref processData);
                i++;
            }
            request.failedDelegate?.Invoke();;
        }

        private void AddSurroundingNodes(PathNode current, ref PathfindingProcessData processData)
        {
            //Get the valid directions you can travel in from this point.
            ConnectingDirections validDirections = GetValidDirections(processData, current.Position);
            //Process north node
            if ((validDirections & ConnectingDirections.NORTH) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(0, 1));
            if ((validDirections & ConnectingDirections.EAST) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(1, 0));
            if ((validDirections & ConnectingDirections.SOUTH) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(0, -1));
            if ((validDirections & ConnectingDirections.WEST) != 0)
                CheckAddNode(current, ref processData, new Vector<int>(-1, 0));
            if ((validDirections & ConnectingDirections.NORTH_EAST) == ConnectingDirections.NORTH_EAST && !World.IsSolid(current.Position + new Vector<int>(1, 1)))
                CheckAddNode(current, ref processData, new Vector<int>(1, 1));
            if ((validDirections & ConnectingDirections.SOUTH_EAST) == ConnectingDirections.SOUTH_EAST && !World.IsSolid(current.Position + new Vector<int>(1, -1)))
                CheckAddNode(current, ref processData, new Vector<int>(1, -1));
            if ((validDirections & ConnectingDirections.SOUTH_WEST) == ConnectingDirections.SOUTH_WEST && !World.IsSolid(current.Position + new Vector<int>(-1, -1)))
                CheckAddNode(current, ref processData, new Vector<int>(-1, -1));
            if ((validDirections & ConnectingDirections.NORTH_WEST) == ConnectingDirections.NORTH_WEST && !World.IsSolid(current.Position + new Vector<int>(-1, 1)))
                CheckAddNode(current, ref processData, new Vector<int>(-1, 1));
        }

        private void CheckAddNode(PathNode current, ref PathfindingProcessData processData, Vector<int> offset)
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
        private ConnectingDirections GetValidDirections(PathfindingProcessData processData, Vector<int> position)
        {
            //Default valid connecting directions
            ConnectingDirections connectingDirections = 0;
            //Check north
            if (!processData.ProcessedPoints.Contains(position + new Vector<int>(0, 1)) && position[1] < processData.MaximumY && !World.IsSolid(position + new Vector<int>(0, 1)))
                connectingDirections |= ConnectingDirections.NORTH;
            //Check east
            if (!processData.ProcessedPoints.Contains(position + new Vector<int>(1, 0)) && position[0] < processData.MaximumX && !World.IsSolid(position + new Vector<int>(1, 0)))
                connectingDirections |= ConnectingDirections.EAST;
            //Check south
            if (!processData.ProcessedPoints.Contains(position + new Vector<int>(0, -1)) && position[1] > processData.MinimumY && !World.IsSolid(position + new Vector<int>(0, -1)))
                connectingDirections |= ConnectingDirections.SOUTH;
            //Check west
            if (!processData.ProcessedPoints.Contains(position + new Vector<int>(-1, 0)) && position[0] > processData.MinimumX && !World.IsSolid(position + new Vector<int>(-1, 0)))
                connectingDirections |= ConnectingDirections.WEST;
            //Return valid directions
            return connectingDirections;
        }

    }
}
