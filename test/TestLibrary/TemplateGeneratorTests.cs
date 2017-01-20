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
    public class TemplateGeneratorTests
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
            TemplateGenerator file = new TemplateGenerator();
            string expandedTemplateText = file.ExpandTemplateText(templatePath);

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
            TemplateGenerator file = new TemplateGenerator();
            string expandedTemplateText = file.ExpandTemplateText(templatePath);

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
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void BadFilePathThrowsException()
        {
            // arrange
            string badTemplatePath = "foo";

            // act
            TemplateGenerator file = new TemplateGenerator();
            file.ExpandTemplateText(badTemplatePath);

            // assert
            // Should throw an exception do to a bad file path.
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NullFilePathThrowsException()
        {
            // arrange
            string badTemplatePath = null;

            // act
            TemplateGenerator file = new TemplateGenerator();
            file.ExpandTemplateText(badTemplatePath);

            // assert
            // Should throw an exception do to a null file path.
        }

        [TestMethod]
        public void ChangeNestedFilePrefix()
        {
            // arrange
            string nestedFileLinePrefix = "TEMPLATE";
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "AlternatePrefixNestedFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "AlternatePrefixNestedFileTemplateOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateGenerator file = new TemplateGenerator(nestedFileLinePrefix);
            string expandedTemplateText = file.ExpandTemplateText(templatePath);

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }
    }
}
