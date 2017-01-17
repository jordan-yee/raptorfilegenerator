using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;
using System.Text.RegularExpressions;
using System.IO;

namespace TestLibrary
{
    [TestClass]
    public class TemplateExpanderTests
    {
        [TestMethod]
        public void TestBasicFileCreation()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "BasicFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "BasicFileOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateExpander file = new TemplateExpander(templatePath);
            string expandedTemplateText = file.TemplateText;

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        [TestMethod]
        public void TestNestedFileCreation()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "NestedFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "NestedFileOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateExpander file = new TemplateExpander(templatePath);
            string expandedTemplateText = file.TemplateText;

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        private string ReadTextFromFile(string filePath)
        {
            string fileContents = "Test file contents";

            try {
                fileContents = File.ReadAllText(filePath);
            }
            catch (Exception) {
                Assert.Fail("Failed to read a test data file. Make sure test file paths are correct.");
            }

            return fileContents;
        }

        [TestMethod]
        public void BadFilePathThrowsException()
        {
            // arrange
            string badTemplatePath = "foo";

            // act
            bool exceptionWasThrown = false;
            try {
                TemplateExpander file = new TemplateExpander(badTemplatePath);
            }
            catch (Exception) {
                exceptionWasThrown = true;
            }

            // assert
            Assert.IsTrue(exceptionWasThrown);
        }

        [TestMethod]
        public void NullFilePathThrowsException()
        {
            // arrange
            string badTemplatePath = null;

            // act
            bool exceptionWasThrown = false;
            try {
                TemplateExpander file = new TemplateExpander(badTemplatePath);
            }
            catch (Exception) {
                exceptionWasThrown = true;
            }

            // assert
            Assert.IsTrue(exceptionWasThrown);
        }
    }
}
