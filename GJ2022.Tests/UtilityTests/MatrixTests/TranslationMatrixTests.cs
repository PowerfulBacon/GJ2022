using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GJ2022.Tests.UtilityTests.MatrixTests
{
    [TestClass]
    public class TranslationMatrixTests
    {
        [TestMethod]
        public void TestNoTranslation()
        {
            //Start at the origin
            Matrix origin = new Matrix(1, 4);
            origin[1, 4] = 1;
            //Where we should end up
            Matrix result = new Matrix(new float[,] {
                { 0 },
                { 0 },
                { 0 },
                { 1 }
                });
            //See where we end up
            Assert.AreEqual(result, Matrix.GetTranslationMatrix(0, 0, 0) * origin);

        }

        [TestMethod]
        public void TestForwardTranslation()
        {
            //Start at the origin
            Matrix origin = new Matrix(1, 4);
            origin[1, 4] = 1;
            //Where we should end up
            Matrix result = new Matrix(new float[,] {
                { 1 },
                { 1 },
                { 1 },
                { 1 }
                });
            //See where we end up
            Assert.AreEqual(result, Matrix.GetTranslationMatrix(1, 1, 1) * origin);
        }

        [TestMethod]
        public void TestBackwardsTranslation()
        {

        }
    }
}
