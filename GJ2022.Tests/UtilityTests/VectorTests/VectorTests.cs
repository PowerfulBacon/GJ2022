using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GJ2022.Tests.UtilityTests.VectorTests
{
    [TestClass]
    public class VectorTests
    {

        [TestMethod]
        public void TestVectorLength()
        {
            Vector<float> vector = new Vector<float>(3, 4, 0);
            Assert.AreEqual(5, vector.Length());
        }

        [TestMethod]
        public void TestVectorEquivilance()
        {
            Vector<float> a = new Vector<float>(1, 2, 3);
            Vector<float> b = new Vector<float>(1, 2, 3);
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestDictionaryKey()
        {
            Vector<float> a = new Vector<float>(1, 1, 1);
            Dictionary<Vector<float>, bool> testDict = new Dictionary<Vector<float>, bool>();
            testDict.Add(a, true);
            Assert.IsTrue(testDict.ContainsKey(a), "Could not located same struct");
            Assert.IsTrue(testDict.ContainsKey(new Vector<float>(1, 1, 1)), "Could not locate identical struct");
            Assert.IsTrue(testDict.ContainsKey(new Vector<float>((int)1.4, 1, 1)), "Could not locate integer converted struct");
            Assert.IsFalse(testDict.ContainsKey(new Vector<float>(2, 1, 1)), "Hash collisions detected");
            Assert.IsFalse(a.GetHashCode() == new Vector<float>(2, 1, 1).GetHashCode(), "Hash collision detected.");
        }

        /// <summary>
        /// Tests the dot product of A with B
        /// (1, 1, 1) dot (2, 2, 2) = 2 + 2 + 2 = 6
        /// </summary>
        [TestMethod]
        public void TestVectorDotProduct()
        {
            Vector<float> a = new Vector<float>(1, 1, 1);
            Vector<float> b = new Vector<float>(2, 2, 2);
            Assert.AreEqual(6, Vector<float>.DotProduct(a, b));
        }

        /// <summary>
        /// Test multiplication of vectors.
        ///  - Multiplying 2 vectors
        ///  - Multiplying vector by an integer scalar
        ///  - Multiplying vector by a floating point scalar
        /// </summary>
        [TestMethod]
        public void TestVectorMultiplication()
        {
            Vector<float> a = new Vector<float>(1, 1, 1);
            Vector<float> b = new Vector<float>(2, 2, 2);
            Assert.AreEqual(new Vector<float>(2, 2, 2), a * b);
            Assert.AreEqual(new Vector<float>(2, 2, 2), a * 2);
            Assert.AreEqual(new Vector<float>(5, 5, 5), b * 2.5f);
        }

        [TestMethod]
        public void TestVectorAddition()
        {
            Vector<float> a = new Vector<float>(1, 1, 1);
            Vector<float> b = new Vector<float>(2, 2, 2);
            Assert.AreEqual(new Vector<float>(3, 3, 3), a + b);
        }

    }
}
