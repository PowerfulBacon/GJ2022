using GJ2022.Game.GameWorld;
using GJ2022.Pathfinding;
using GJ2022.Utility.MathConstructs;
using GLFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Subsystems
{
    public class PathfindingSystem : Subsystem
    {

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
        };

        //Little delay
        public override int sleepDelay => 20;

        //No processing, but fires
        public override SubsystemFlags SubsystemFlags => SubsystemFlags.NO_PROCESSING;

        public override void Fire(Window window)
        {
            throw new NotImplementedException();
        }

        public override void InitSystem() { }

        protected override void AfterWorldInit() { }

        private void ProcessPath(Vector<int> start, Vector<int> end)
        {
            //Create the process data
            PathfindingProcessData processData = new PathfindingProcessData();
            //Create the search borders
            processData.MinimumX = Math.Min(start[0], end[0]) - SEARCH_OFFSET_RANGE;
            processData.MaximumX = Math.Max(start[0], end[0]) + SEARCH_OFFSET_RANGE;
            processData.MinimumY = Math.Min(start[1], end[1]) - SEARCH_OFFSET_RANGE;
            processData.MaximumY = Math.Max(start[1], end[1]) + SEARCH_OFFSET_RANGE;
            //Begin processing the start point (It will propogate out from here)
            processData.ProcessQueue.Add(
                CalculateScore(start, start, end),
                new List<Vector<int>>() { start }
                );
            //Do processing
            while (processData.ProcessQueue.Count > 0)
            {
                //Take the first element from the processing queue
                List<Vector<int>> firstElements = processData.ProcessQueue.Values.First();
                //Lazyremove the first element
                //TODO continue from here
            }
        }

        /// <summary>
        /// Calculate the score of a point
        /// </summary>
        private float CalculateScore(Vector<int> point, Vector<int> start, Vector<int> end)
        {
            return CalculateDistance(start, point) + CalculateDistance(point, end);
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
