using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tests
{
    [TestClass]
    public class SuperTest
    {

        [TestMethod]
        public void DoTest()
        {
            List<(int, int, int)> testValues = new List<(int, int, int)>() {
                //First row
                (0, 0, 0),
                (1, 0, 1),
                (2, 0, 2),
                (3, 0, 2),
                (4, 0, 3),
                (5, 0, 3),
                (6, 0, 3),
                (7, 0, 3),

                (0, 1, 1),
                (1, 1, 0),
                (2, 1, 2),
                (3, 1, 2),
                (4, 1, 3),
                (5, 1, 3),
                (6, 1, 3),
                (7, 1, 3),

                (0, 2, 2),
                (1, 2, 2),
                (2, 2, 0),
                (3, 2, 1),
                (4, 2, 3),
                (5, 2, 3),
                (6, 2, 3),
                (7, 2, 3),

                (0, 3, 2),
                (1, 3, 2),
                (2, 3, 1),
                (3, 3, 0),
                (4, 3, 3),
                (5, 3, 3),
                (6, 3, 3),
                (7, 3, 3),

                (0, 4, 3),
                (1, 4, 3),
                (2, 4, 3),
                (3, 4, 3),
                (4, 4, 0),
                (5, 4, 1),
                (6, 4, 2),
                (7, 4, 2),

                (0, 5, 3),
                (1, 5, 3),
                (2, 5, 3),
                (3, 5, 3),
                (4, 5, 1),
                (5, 5, 0),
                (6, 5, 2),
                (7, 5, 2),

                (0, 6, 3),
                (1, 6, 3),
                (2, 6, 3),
                (3, 6, 3),
                (4, 6, 2),
                (5, 6, 2),
                (6, 6, 0),
                (7, 6, 1),

                (0, 7, 3),
                (1, 7, 3),
                (2, 7, 3),
                (3, 7, 3),
                (4, 7, 2),
                (5, 7, 2),
                (6, 7, 1),
                (7, 7, 0),

                (7, 8, 4),
                (11, 8, 2),
                (9, 13, 3),
            };

            bool failed = false;

            foreach ((int, int, int) testValue in testValues)
            {
                if (GetResult(testValue.Item1, testValue.Item2) != testValue.Item3)
                {
                    Log.WriteLine($"Fail: ({testValue.Item1}, {testValue.Item2}): Expected: {testValue.Item3} Actual: {GetResult(testValue.Item1, testValue.Item2)}");
                    failed = true;
                }
            }

            if (failed)
                Assert.Fail("Failed for at least 1 point");

        }

        /// <summary>
        /// Recursive algorithm to calculate the parent level of regions.
        /// Recursion depth should never pass 1 in theory, however hits 2 for (1,0) and (0,1)
        /// </summary>
        private int GetResult(int x, int y)
        {
            if (x == y)
                return 0;
            int logx = (int)Math.Ceiling(Math.Log(x + 1, 2));
            int logy = (int)Math.Ceiling(Math.Log(y + 1, 2));
            int powx = (int)Math.Pow(2, logx - 1);
            int powy = (int)Math.Pow(2, logy - 1);
            int maxpow = Math.Max(powx, powy);
            //Bottom Left
            if (y >= maxpow && x < maxpow)
            {
                return logy;
            }
            //Top Right
            else if (x >= maxpow && y < maxpow)
            {
                return logx;
            }
            //Bottom Right
            else
            {
                return GetResult(x - powx, y - powy);
            }
        }

    }
}
