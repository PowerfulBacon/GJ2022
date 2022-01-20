using GJ2022.Utility.MathConstructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GJ2022.Tests.UtilityTests.ColourTests
{

    [TestClass]
    public class TestColours
    {

        [TestMethod]
        public void TestColourStruct()
        {
            Colour colour = new Colour(1, 0, 0);
            Assert.AreEqual(1f, colour.red);
            Assert.AreEqual(0f, colour.green);
            Assert.AreEqual(0f, colour.blue);
            Assert.AreEqual(1f, colour.alpha);
        }

        [TestMethod]
        public void TestColourComparison()
        {
            Colour red1 = new Colour(1, 0, 0);
            Colour red2 = new Colour(1, 0, 0);
            Assert.AreEqual(red1, red2);
            Assert.AreEqual(Colour.Red, red1);
            Assert.AreEqual(Colour.Red, red2);
        }

        [TestMethod]
        public void TestColourMultiplication()
        {
            Colour red1 = new Colour(0.3f, 0, 0);
            red1 *= 2;
            Assert.AreEqual(0.6f, red1.red);
            Assert.AreEqual(0, red1.blue);
            Assert.AreEqual(0, red1.green);
        }

        [TestMethod]
        public void TestNormalize()
        {
            Colour test = new Colour(5, 3, 0.2f);
            Colour normalized = test.GetNormalized();
            Assert.AreEqual(1, normalized.red);
            Assert.AreEqual(0.6f, normalized.green);
            Assert.AreEqual(0.2f / 5f, normalized.blue);
        }

        [TestMethod]
        public void TestIndependance()
        {
            Colour a = new Colour(1, 0, 0);
            Colour b = a;
            a.red = 0.5f;
            Assert.AreEqual(1, b.red);
            Assert.AreEqual(0.5f, a.red);
        }

    }
}
