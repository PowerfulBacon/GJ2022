using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tests.UtilityTests
{
    [TestClass]
    public class PositionBasedBinaryListUnitTests
    {

        [TestMethod]
        public void TestElementsInRange()
        {
            PositionBasedBinaryList<int> randomList = new PositionBasedBinaryList<int>();
            randomList.Add(0, 0, 1);
            randomList.Add(3, 3, 2);
            randomList.Add(2, 2, 3);
            randomList.Add(2, 5, 4);
            randomList.Add(2, 8, 5);
            randomList.Add(10, 10, 6);
            randomList.Add(5, -10, 7);
            randomList.Add(-8, 4, 8);
            Assert.IsTrue(randomList.ElementsInRange(0, 0, 10, 10));
            Assert.IsTrue(randomList.ElementsInRange(0, 0, 0, 0));
            Assert.IsTrue(randomList.ElementsInRange(-9, 3, -7, 5));
            Assert.IsFalse(randomList.ElementsInRange(50, 50, 60, 60));
            Assert.IsFalse(randomList.ElementsInRange(-10, 50, 10, 50));
            Assert.IsFalse(randomList.ElementsInRange(4, 0, 6, 10));
        }

        [TestMethod]
        public void TestBinaryListAddition()
        {
            BinaryList<int> byteList = new BinaryList<int>();
            byteList.Add(3, 3);
            byteList.Add(2, 2);
            byteList.Add(6, 6);
            byteList.Add(5, 5);
            byteList.Add(1, 1);
            byteList.Add(100, 100);
            byteList.Add(-3, -3);
            byteList.Add(7, 7);
            //Check
            int[] validResults = new int[] { -3, 1, 2, 3, 5, 6, 7, 100 };
            for (int i = 0; i < validResults.Length; i++)
            {
                Assert.AreEqual(validResults[i], byteList._ElementAt(i), "Binary list insertion is out of order");
            }
            Assert.AreEqual(3, byteList.ElementWithKey(3), "Failed to locate element with key 3");
            Assert.AreEqual(6, byteList.ElementWithKey(6), "Failed to locate element with key 6");
            Assert.AreEqual(100, byteList.ElementWithKey(100), "Failed to locate element with key 100");
            Assert.AreEqual(default(int), byteList.ElementWithKey(101), "Successfully located an invalid element");
        }

        [TestMethod]
        public void TestBinaryListAddingAndRemoving()
        {
            BinaryList<int> byteList = new BinaryList<int>();
            byteList.Add(3, 3);
            byteList.Add(2, 2);
            byteList.Add(6, 6);
            byteList.Remove(3);
            byteList.Add(5, 5);
            byteList.Add(1, 1);
            byteList.Remove(2);
            byteList.Add(100, 100);
            byteList.Add(-3, -3);
            byteList.Add(7, 7);
            byteList.Remove(6);
            byteList.Remove(6);
            byteList.Remove(101);
            byteList.Remove(-101);
            byteList.Remove(99);
            int[] validResults = new int[] { -3, 1, 5, 7, 100 };
            for (int i = 0; i < validResults.Length; i++)
            {
                Assert.AreEqual(validResults[i], byteList._ElementAt(i), "Binary list insertion is out of order");
            }
            Assert.AreEqual(5, byteList.ElementWithKey(5), "Failed to locate element with key 3");
            Assert.AreEqual(7, byteList.ElementWithKey(7), "Failed to locate element with key 6");
            Assert.AreEqual(default(int), byteList.ElementWithKey(6), "Successfully located an invalid element");
            Assert.AreEqual(default(int), byteList.ElementWithKey(101), "Successfully located an invalid element");
            byteList.Remove(-3);
            byteList.Remove(100);
            byteList.Remove(5);
            byteList.Remove(7);
            byteList.Remove(1);
            Assert.AreEqual(0, byteList.Length(), "There should be nothing in the list");
        }

    }
}
