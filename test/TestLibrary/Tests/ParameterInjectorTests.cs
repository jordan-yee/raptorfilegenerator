using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;
using System.Text.RegularExpressions;

namespace TestLibrary.Tests
{
    [TestClass]
    public class ParameterInjectorTests
    {
        [TestMethod]
        public void ReplaceAParameterTest()
        {
            // arrange
            string input = "Hello, {{name}}!";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", "World");
            string expectedOutput = "Hello, World!";
            ParameterInjector pi = new ParameterInjector(input, parameters);

            // act
            string result = pi.GetInjectedText();

            // assert
            Regex expectedOutputPattern = new Regex(expectedOutput);
            StringAssert.Matches(result, expectedOutputPattern);
        }

        [TestMethod]
        public void ReplaceARepeatedParameterTest()
        {
            // arrange
            string input = "Hello, {{name}}! {{name}}";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", "World");
            string expectedOutput = "Hello, World! World";
            ParameterInjector pi = new ParameterInjector(input, parameters);

            // act
            string result = pi.GetInjectedText();

            // assert
            Regex expectedOutputPattern = new Regex(expectedOutput);
            StringAssert.Matches(result, expectedOutputPattern);
        }

        [TestMethod]
        public void AlternateParameterDelimittersTest()
        {
            // arrange
            string input = "Hello, [[name]]!";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", "World");
            string expectedOutput = "Hello, World!";
            ParameterInjector pi = new ParameterInjector(input, parameters);
            pi.StartingParameterDelimitter = "[[";
            pi.EndingParameterDelimitter = "]]";

            // act
            string result = pi.GetInjectedText();

            // assert
            Regex expectedOutputPattern = new Regex(expectedOutput);
            StringAssert.Matches(result, expectedOutputPattern);
        }

        [TestMethod]
        public void InitializeWithNullValuesExceptionTest()
        {
            // arrange
            string input = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            // act
            bool nullExceptionWasThrown = false;
            try {
                ParameterInjector pi = new ParameterInjector(null, parameters);
                pi = new ParameterInjector(input, null);
            }
            catch (ArgumentNullException) {
                nullExceptionWasThrown = true;
            }

            // assert
            Assert.IsTrue(nullExceptionWasThrown);
        }

        [TestMethod]
        public void NullInputExceptionTest()
        {
            // arrange
            string input = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ParameterInjector pi = new ParameterInjector(input, parameters);

            // act
            bool nullExceptionWasThrown = false;
            try {
                pi.Input = null;
                pi.GetInjectedText();
            }
            catch (ArgumentNullException) {
                nullExceptionWasThrown = true;
            }

            // assert
            Assert.IsTrue(nullExceptionWasThrown);
        }

        [TestMethod]
        public void NullParameterExceptionTest()
        {
            // arrange
            string input = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ParameterInjector pi = new ParameterInjector(input, parameters);

            // act
            bool nullExceptionWasThrown = false;
            try {
                pi.Parameters = null;
                pi.GetInjectedText();
            }
            catch (ArgumentNullException) {
                nullExceptionWasThrown = true;
            }

            // assert
            Assert.IsTrue(nullExceptionWasThrown);
        }

        [TestMethod]
        public void NullStartingParameterDelimitterExceptionTest()
        {
            // arrange
            string input = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ParameterInjector pi = new ParameterInjector(input, parameters);

            // act
            bool nullExceptionWasThrown = false;
            try {
                pi.StartingParameterDelimitter = null;
                pi.GetInjectedText();
            }
            catch (ArgumentNullException) {
                nullExceptionWasThrown = true;
            }

            // assert
            Assert.IsTrue(nullExceptionWasThrown);
        }

        [TestMethod]
        public void NullEndingParameterDelimitterExceptionTest()
        {
            // arrange
            string input = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ParameterInjector pi = new ParameterInjector(input, parameters);

            // act
            bool nullExceptionWasThrown = false;
            try {
                pi.EndingParameterDelimitter = null;
                pi.GetInjectedText();
            }
            catch (ArgumentNullException) {
                nullExceptionWasThrown = true;
            }

            // assert
            Assert.IsTrue(nullExceptionWasThrown);
        }

        [TestMethod]
        public void EmptyInputAndParametersTest()
        {
            // arrange
            string input = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ParameterInjector pi = new ParameterInjector(input, parameters);

            // act
            string result = pi.GetInjectedText();

            // assert
            Regex expectedOutputPattern = new Regex("");
            StringAssert.Matches("", expectedOutputPattern);
        }
    }
}
