using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Pathfinding
{
    public class PathfindingRequest
    {

        public delegate void PathFoundDelegate(Path path);
        public delegate void PathFailedDelegate();

        //Start position of the pathfinding request
        public Vector Start { get; }
        //End position of the pathfinding request
        public Vector End { get; }

        //Delegate to be called when the path is found
        public PathFoundDelegate foundDelegate { get; }
        //Delegate to be called when no valid path exists
        public PathFailedDelegate failedDelegate { get; }

        //Constructor
        public PathfindingRequest(Vector start, Vector end, PathFoundDelegate foundDelegate, PathFailedDelegate failedDelegate = null)
        {
            Start = start;
            End = end;
            this.foundDelegate = foundDelegate;
            this.failedDelegate = failedDelegate;
        }
    }
}
