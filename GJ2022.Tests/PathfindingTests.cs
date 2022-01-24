using GJ2022.Subsystems;
using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tests
{
    [TestClass]
    public class PathfindingTests
    {

        [TestMethod]
        public void TestPathfinding()
        {
            PathfindingSystem.ProcessPathImmediate(new Vector<int>(0, 0), new Vector<int>(5, 10));
            PathfindingSystem.ProcessPathImmediate(new Vector<int>(0, 0), new Vector<int>(-5, -2));
            PathfindingSystem.ProcessPathImmediate(new Vector<int>(-3, -3), new Vector<int>(3, 3));
            PathfindingSystem.ProcessPathImmediate(new Vector<float>(-3.4f, -3.2f), new Vector<float>(3.6f, 3.1f));
            PathfindingSystem.ProcessPathImmediate(new Vector<int>(10, 10), new Vector<int>(0, 0));
        }

    }
}
