using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GJ2022.Tests.UtilityTests.MatrixTests
{
    [TestClass]
    public class GeneralMatrixTests
    {

        [TestMethod]
        public void TestEmptyMatrix()
        {
            Matrix matrix = new Matrix(2, 2);
            Assert.AreEqual(0, matrix[1, 1]);
            Assert.AreEqual(0, matrix[2, 1]);
            Assert.AreEqual(0, matrix[1, 2]);
            Assert.AreEqual(0, matrix[2, 2]);
        }

        [TestMethod]
        public void TestPresetMatrix()
        {
            Matrix matrix = new Matrix(new float[,] {
                { 1, 2 },
                { 3, 4 }
            });
            Assert.AreEqual(1, matrix[1, 1]);
            Assert.AreEqual(2, matrix[2, 1]);
            Assert.AreEqual(3, matrix[1, 2]);
            Assert.AreEqual(4, matrix[2, 2]);
        }

        [TestMethod]
        public void CheckReferenceEquality()
        {
            Matrix a = new Matrix(2, 2);
            Matrix b = new Matrix(2, 2);
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void CheckEqualityOperator()
        {
            Matrix a = new Matrix(2, 2);
            Matrix b = new Matrix(2, 2);
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public unsafe void TestOpenGlReferenceFormatting()
        {
            Matrix a = new Matrix(new float[,] {
                { 1, 2 },
                { 3, 4 }
            });
            float[] results = new float[] { 1, 3, 2, 4 };
            //Get reference
            float* matrixPointer = a.GetPointer();
            for (int i = 0; i < 4; i++)
            {
                float value = *matrixPointer;
                Assert.AreEqual(results[i], value);
                matrixPointer++;
            }
        }

        /// <summary>
        /// Other tests rely on this, so we need to check
        /// that toString actually works.
        /// </summary>
        [TestMethod]
        public void CheckToString()
        {
            Matrix a = new Matrix(2, 3);
            Log.WriteLine(a);
        }

    }
}
