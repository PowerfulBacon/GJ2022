using GJ2022.Entities.Turfs;
using GJ2022.Game.GameWorld;
using GJ2022.Game.GameWorld.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tests
{
    [TestClass]
    public class RegionTests
    {

        [TestMethod]
        public void TestValidPathThatGetsBlocked()
        {
            //Generate a wall
            Turf solidTurf = new Turf();
            solidTurf.SetProperty("Solid", true);
            //Reset the world
            World.Current = new World();
            World.Current.SetTurf(12, 0, solidTurf);
            World.Current.SetTurf(12, 1, solidTurf);
            World.Current.SetTurf(12, 2, solidTurf);
            World.Current.SetTurf(12, 3, solidTurf);
            World.Current.SetTurf(12, 5, solidTurf);
            World.Current.SetTurf(12, 6, solidTurf);
            World.Current.SetTurf(12, 7, solidTurf);
            //Create a world region list
            WorldRegionList wrl = new WorldRegionList();
            wrl.GenerateWorldSection(0, 0);
            wrl.GenerateWorldSection(8, 0);
            wrl.GenerateWorldSection(16, 0);
            //Test accessability
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(5, 5)), "There should exist a valid path from region (0, 0) to (5, 5)");
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(10, 5)), "There should exist a valid path from region (0, 0) to (10, 5)");
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(14, 5)), "There should exist a valid path from region (0, 0) to (14, 5)");
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(18, 5)), "There should exist a valid path from region (0, 0) to (18, 5)");
            Assert.IsTrue(wrl.regions.Get(14, 5).HasPath(wrl.regions.Get(18, 5)), "There should exist a valid path from region (14, 5) to (18, 5)");
            //Check this
            Assert.IsTrue(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(12, 4), 12, 4));
            //Block off the wall
            World.Current.SetTurf(12, 4, solidTurf);
            wrl.SetNodeSolid(12, 4);
            Log.WriteLine("World has been divided");
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(5, 5)), "There should still exist a valid path from region (0, 0) to (5, 5)");
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(10, 5)), "There should still exist a valid path from region (0, 0) to (10, 5)");
            Assert.IsFalse(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(14, 5)), "There should no longer exist a valid path from region (0, 0) to (14, 5)");
            Assert.IsFalse(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(18, 5)), "There should no longer exist a valid path from region (0, 0) to (18, 5)");
            Assert.IsTrue(wrl.regions.Get(14, 5).HasPath(wrl.regions.Get(18, 5)), "There should still exist a valid path from region (14, 5) to (18, 5)");
        }

        [TestMethod]
        public void TestVerticalWall()
        {
            //Reset the world
            World.Current = new World();
            //Generate a wall
            Turf solidTurf = new Turf();
            solidTurf.SetProperty("Solid", true);
            World.Current.SetTurf(13, 0, solidTurf);
            World.Current.SetTurf(13, 1, solidTurf);
            World.Current.SetTurf(13, 2, solidTurf);
            World.Current.SetTurf(13, 3, solidTurf);
            World.Current.SetTurf(13, 4, solidTurf);
            World.Current.SetTurf(13, 5, solidTurf);
            World.Current.SetTurf(13, 6, solidTurf);
            World.Current.SetTurf(13, 7, solidTurf);
            //Create a world region list
            WorldRegionList wrl = new WorldRegionList();
            wrl.GenerateWorldSection(0, 0);
            wrl.GenerateWorldSection(8, 0);
            wrl.GenerateWorldSection(16, 0);
            //Test accessability
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(5, 5)), "There should exist a valid path from region (0, 0) to (5, 5)");
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(10, 5)), "There should exist a valid path from region (0, 0) to (10, 5)");
            Assert.IsFalse(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(14, 5)), "There should not exist a valid path from region (0, 0) to (14, 5)");
            Assert.IsFalse(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(18, 5)), "There should not exist a valid path from region (0, 0) to (18, 5)");
            Assert.IsTrue(wrl.regions.Get(14, 5).HasPath(wrl.regions.Get(18, 5)), "There should exist a valid path from region (14, 5) to (18, 5)");
        }

        [TestMethod]
        public void TestRegionCreation()
        {
            //Generate a wall
            Turf solidTurf = new Turf();
            solidTurf.SetProperty("Solid", true);
            //Reset the world
            World.Current = new World();
            World.Current.SetTurf(4, 0, solidTurf);
            World.Current.SetTurf(4, 1, solidTurf);
            World.Current.SetTurf(4, 2, solidTurf);
            World.Current.SetTurf(4, 3, solidTurf);
            World.Current.SetTurf(4, 4, solidTurf);
            World.Current.SetTurf(4, 5, solidTurf);
            World.Current.SetTurf(4, 6, solidTurf);
            World.Current.SetTurf(4, 7, solidTurf);
            WorldRegionList wrl = new WorldRegionList();
            wrl.GenerateWorldSection(0, 0);
            Region left = null;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (left == null)
                        left = wrl.regions.Get(x, y);
                    Assert.AreEqual(left, wrl.regions.Get(x, y), "Expected left region to be the same");
                }
            }
            for (int y = 0; y < 8; y++)
            {
                Assert.AreEqual(null, wrl.regions.Get(4, y), "Expected center line to be null");
            }
            for (int x = 5; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Assert.AreNotEqual(left, wrl.regions.Get(x, y), "Expected right region to be different");
                }
            }
        }

        [TestMethod]
        public void TestRegionUpdateRequirements()
        {
            //Generate a wall
            Turf solidTurf = new Turf();
            solidTurf.SetProperty("Solid", true);
            //Reset the world
            World.Current = new World();
            World.Current.SetTurf(4, 0, solidTurf);
            World.Current.SetTurf(4, 1, solidTurf);
            World.Current.SetTurf(4, 2, solidTurf);
            World.Current.SetTurf(4, 3, solidTurf);
            World.Current.SetTurf(4, 5, solidTurf);
            World.Current.SetTurf(4, 6, solidTurf);
            World.Current.SetTurf(4, 7, solidTurf);
            WorldRegionList wrl = new WorldRegionList();
            wrl.GenerateWorldSection(0, 0);
            Assert.IsTrue(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(0, 0), 4, 4));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(0, 0), 3, 1));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(0, 0), 5, 5));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(0, 0), 4, 6));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(0, 0), 3, 3));
            Assert.IsTrue(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(0, 0), 5, 4));
            Assert.IsTrue(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(0, 0), 3, 4));
            wrl.GenerateWorldSection(8, 0);
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(12, 4), 12, 4));
            World.Current.SetTurf(12, 0, solidTurf);
            World.Current.SetTurf(12, 1, solidTurf);
            World.Current.SetTurf(12, 2, solidTurf);
            World.Current.SetTurf(12, 3, solidTurf);
            World.Current.SetTurf(12, 5, solidTurf);
            World.Current.SetTurf(12, 6, solidTurf);
            World.Current.SetTurf(12, 7, solidTurf);
            Assert.IsTrue(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(12, 4), 12, 4));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(11, 1), 11, 1));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(8, 0), 13, 5));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(8, 0), 12, 6));
            Assert.IsFalse(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(8, 0), 11, 3));
            Assert.IsTrue(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(8, 0), 13, 4));
            Assert.IsTrue(wrl.NodeSolidRequiresUpdate(wrl.regions.Get(8, 0), 11, 4));
        }

        [TestMethod]
        public void TestRegionWorldSection()
        {
            WorldRegionList wrl = new WorldRegionList();
            //Test pathing within a region
            wrl.GenerateWorldSection(0, 0);
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(7, 7)), "Failed to locate valid path within own region");
            //Test pathing to an adjacent region
            wrl.GenerateWorldSection(8, 0);
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(8, 0)), "Failed to locate valid path within adjacent region");
            //Test invalid pathing
            wrl.GenerateWorldSection(24, 0);
            Assert.IsFalse(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(24, 0)), "Failed to identify that a path shouldn't exist between 2 non-connected regions");
            //Test connection
            wrl.GenerateWorldSection(16, 0);
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(24, 0)), "Failed to identify completed path between a line of 4 regions");
            //Test 2 dimension functionality
            wrl.GenerateWorldSection(24, 16);
            wrl.GenerateWorldSection(24, 8);
            Assert.IsTrue(wrl.regions.Get(0, 0).HasPath(wrl.regions.Get(24, 16)), "Failed to identify completed path in a 2D grid");
        }

        [TestMethod]
        public void TestSharedParentImplementation()
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

                (8+16, 9+16, 1),
                (8+16, 10+16, 2),
            };

            bool failed = false;

            WorldRegionList wrl = new WorldRegionList();

            foreach ((int, int, int) testValue in testValues)
            {
                int result = wrl.GetSharedParentLevel(testValue.Item1, testValue.Item2);
                if (result != testValue.Item3)
                {
                    Log.WriteLine($"Fail: ({testValue.Item1}, {testValue.Item2}): Expected: {testValue.Item3} Actual: {result}");
                    failed = true;
                }
            }

            if (failed)
                Assert.Fail("Failed for at least 1 point");

        }

    }
}
