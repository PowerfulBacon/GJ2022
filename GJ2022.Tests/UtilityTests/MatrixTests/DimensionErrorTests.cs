using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GJ2022.Tests.UtilityTests.MatrixTests
{
    [TestClass]
    public class DimensionErrorTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidMatrixDimensionError))]
        public void TestDimensionalError()
        {
            Matrix a = new Matrix(4, 1);
            Matrix b = new Matrix(2, 4);
            Matrix c = b * a;
        }

        [TestMethod]
        public void TestValidDimensions()
        {
            Matrix a = new Matrix(4, 1);
            Matrix b = new Matrix(2, 4);
            Matrix c = a * b;
            Assert.AreEqual(2, c.X);
            Assert.AreEqual(1, c.Y);
        }
    }
}
