using GJ2022.PawnBehaviours;
using GJ2022.Utility.MathConstructs;

namespace GJ2022.Pathfinding
{
    public class PathfindingRequest
    {

        public delegate void PathFoundDelegate(Path path);
        public delegate void PathFailedDelegate();

        //Start position of the pathfinding request
        public Vector<int> Start { get; }
        //End position of the pathfinding request
        public Vector<int> End { get; }

        //Delegate to be called when the path is found
        public PathFoundDelegate foundDelegate { get; }
        //Delegate to be called when no valid path exists
        public PathFailedDelegate failedDelegate { get; }

        //Hazard ignore flags
        //Not ignoring gravity will result in gravity being taken into account
        public PawnHazards ignoringHazards { get; set; }

        //Constructor
        public PathfindingRequest(Vector<int> start, Vector<int> end, PawnHazards ignoringHazards, PathFoundDelegate foundDelegate, PathFailedDelegate failedDelegate = null)
        {
            Start = start.IgnoreZ();
            End = end.IgnoreZ();
            this.ignoringHazards = ignoringHazards;
            this.foundDelegate = foundDelegate;
            this.failedDelegate = failedDelegate;
        }
    }
}
