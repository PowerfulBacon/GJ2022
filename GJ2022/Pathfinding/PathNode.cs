using GJ2022.Utility.MathConstructs;

namespace GJ2022.Pathfinding
{
    public class PathNode
    {

        //Position of this node
        public Vector<int> Position { get; }

        //The node which we originate from (the one that has the shortest distance)
        public PathNode SourceNode { get; set; }

        public int NodeDepth
        {
            get
            {
                return (SourceNode?.NodeDepth + 1) ?? 0;
            }
        }

        //The score of the node
        public float NodeScore
        {
            get
            {
                return (SourceNode?.NodeScore ?? 0) + NodeEndScore;
            }
        }

        public void RecursivePrint()
        {
            SourceNode?.RecursivePrint();
            Log.WriteLine(Position);
        }

        //Score 
        public float NodeEndScore { get; set; }

        public PathNode(Vector<int> position, float endScore)
        {
            Position = position;
            NodeEndScore = endScore;
        }

        /// <summary>
        /// Checks a node to see if it can be a new source node.
        /// If the new source node has a lower score than our old source node, add it.
        /// </summary>
        /// <param name="newNode"></param>
        public bool CheckSourceNode(PathNode newNode)
        {
            if (SourceNode == null || SourceNode.NodeScore < newNode.NodeScore)
            {
                SourceNode = newNode;
                return true;
            }
            return false;
        }

    }
}
