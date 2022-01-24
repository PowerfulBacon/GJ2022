using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;
using System.Linq;

namespace GJ2022.Pathfinding
{
    public class PathfindingProcessData
    {

        public Vector<int> End { get; set; }
        //List of points already processed
        public HashSet<Vector<int>> ProcessedPoints { get; } = new HashSet<Vector<int>>();
        //Hashset of pathnodes
        private Dictionary<Vector<int>, PathNode> PathNodePositionRefs { get; } = new Dictionary<Vector<int>, PathNode>();
        //Dictionary of the cost of things in the process queue
        private SortedDictionary<float, List<PathNode>> ProcessQueue { get; } = new SortedDictionary<float, List<PathNode>>();

        public void UpdateNode(PathNode node, float oldScore)
        {
            //Remove the old node
            ProcessQueue[oldScore].Remove(node);
            if (ProcessQueue[oldScore].Count == 0)
                ProcessQueue.Remove(oldScore);
            //Add the new node
            if (ProcessQueue.ContainsKey(node.NodeScore))
                ProcessQueue[node.NodeScore].Add(node);
            else
                BinaryInsert(node);
        }

        public bool HasNext()
        {
            return ProcessQueue.Count > 0;
        }

        public void AddNode(PathNode node)
        {
            //Add to reference list
            PathNodePositionRefs.Add(node.Position, node);
            //Add to process queue
            if (ProcessQueue.ContainsKey(node.NodeScore))
                ProcessQueue[node.NodeScore].Add(node);
            else
                BinaryInsert(node);
        }

        private void BinaryInsert(PathNode node)
        {
            //just kidding
            ProcessQueue.Add(node.NodeScore, new List<PathNode>() { node });
        }

        /// <summary>
        /// Gets a node at a position or returns null
        /// </summary>
        public PathNode GetNode(Vector<int> position)
        {
            if (PathNodePositionRefs.ContainsKey(position))
                return PathNodePositionRefs[position];
            return null;
        }

        public PathNode PopProcessQueue()
        {
            //Take the first element from the processing queue
            List<PathNode> firstElements = ProcessQueue.Values.First();
            //Lazyremove the first element
            PathNode processing = firstElements.First();
            //Remove the processing from the first element
            firstElements.RemoveAt(0);
            //Add the current to the processed list
            ProcessedPoints.Add(processing.Position);
            //Remove the first elements list if its empty
            if (firstElements.Count == 0)
                ProcessQueue.Remove(processing.NodeScore);
            //Remove from the reference list
            PathNodePositionRefs.Remove(processing.Position);
            return processing;
        }

        //Range of the pathfinding search (It would be unreasonable to search an infinite grid)
        public int MinimumX { get; set; }
        public int MinimumY { get; set; }
        public int MaximumX { get; set; }
        public int MaximumY { get; set; }

    }
}
