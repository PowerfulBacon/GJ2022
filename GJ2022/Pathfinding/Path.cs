using GJ2022.Utility.MathConstructs;
using System.Collections.Generic;

namespace GJ2022.Pathfinding
{
    public class Path
    {

        //List of points this path covers
        public List<Vector<int>> Points { get; } = new List<Vector<int>>();

        public Path(PathNode endNode)
        {
            RecursiveAdd(endNode);
        }

        private void RecursiveAdd(PathNode node)
        {
            if (node.SourceNode != null)
                RecursiveAdd(node.SourceNode);
            Points.Add(node.Position);
        }

    }
}
