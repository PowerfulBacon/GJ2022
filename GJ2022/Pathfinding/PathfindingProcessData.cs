using GJ2022.Utility.MathConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Pathfinding
{
    public class PathfindingProcessData
    {

        //List of points already processed
        public HashSet<Vector<int>> ProcessedPoints { get; } = new HashSet<Vector<int>>();
        //Dictionary of the cost of things in the process queue
        public Dictionary<float, List<Vector<int>>> ProcessQueue { get; } = new Dictionary<float, List<Vector<int>>>();

        //Range of the pathfinding search (It would be unreasonable to search an infinite grid)
        public int MinimumX { get; set; }
        public int MinimumY { get; set; }
        public int MaximumX { get; set; }
        public int MaximumY { get; set; }

    }
}
