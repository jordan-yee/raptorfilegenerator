using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;

namespace TestLibrary
{
    [TestClass]
    public class ExampleTests
    {
        [TestMethod]
        public void ExampleTest()
        {
            // arrange
            int value = 5;
            int otherValue = 5;
            int expectedResult = 10;

            // act
            int result = value + otherValue;

            // assert
            Assert.AreEqual(result, expectedResult);
        }
    }
}
