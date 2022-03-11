using GJ2022.EntityLoading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tests
{
    [TestClass]
    public class EntityDefTest
    {

        [TestMethod]
        public void TestDefs()
        {
            //Reset for testing purposes
            Log.ExceptionCount = 0;
            EntityLoader.LoadEntities();
            if (Log.ExceptionCount > 0)
                Assert.Fail();
        }

    }
}
