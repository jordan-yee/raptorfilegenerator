using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;
using System.Text.RegularExpressions;
using System.IO;
using TestLibrary.TestObjects;

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
            string expandedTemplateText = file.GenerateTemplateText(templatePath);

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
            string expandedTemplateText = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void BadFilePathThrowsException()
        {
            // arrange
            string badTemplatePath = "foo";

            // act
            TemplateGenerator file = new TemplateGenerator();
            file.GenerateTemplateText(badTemplatePath);

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
            file.GenerateTemplateText(badTemplatePath);

            // assert
            // Should throw an exception do to a null file path.
        }

        [TestMethod]
        public void ChangeNestedFilePrefix()
        {
            // arrange
            string nestedFileLinePrefix = "TEMPLATE";
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "AlternatePrefixNestedFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "AlternatePrefixNestedFileOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateGenerator file = new TemplateGenerator(null, nestedFileLinePrefix);
            string expandedTemplateText = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        [TestMethod]
        public void ConstructorArgumentsCanBeNull()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "BasicFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "BasicFileOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateGenerator file = new TemplateGenerator(null, null);
            string expandedTemplateText = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        [TestMethod]
        public void PublicPropertiesCanBeSetNull()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "BasicFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "BasicFileOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateGenerator file = new TemplateGenerator();
            file.NestedFileLinePrefix = null;
            file.FileData = null;
            string expandedTemplateText = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        [TestMethod]
        public void NestedFileLinePrefix_NullToDefault_NestedFileCreation()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "NestedFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "NestedFileOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateGenerator file = new TemplateGenerator();
            file.NestedFileLinePrefix = null;
            string expandedTemplateText = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        [TestMethod]
        public void NestedFileLinePrefix_WhiteSpaceToDefault_NestedFileCreation()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "NestedFileTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "NestedFileOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(expectedOutputText);

            // act
            TemplateGenerator file = new TemplateGenerator();
            file.NestedFileLinePrefix = " ";
            string expandedTemplateText = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }

        [TestMethod]
        public void GenerateTemplateFileWithTemplateParameters()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "NestedFileWithParametersTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "NestedFileWithParametersOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(Regex.Escape(expectedOutputText));

            FileData fileData = new FileData();
            Dictionary<string, string>[] parametersForNestedFileWithParameters = new Dictionary<string, string>[1];
            parametersForNestedFileWithParameters[0] = new Dictionary<string, string>();
            parametersForNestedFileWithParameters[0].Add("parameter", "test parameter");

            Dictionary<string, string>[] parametersForSubNestedFileWithParameters = new Dictionary<string, string>[1];
            parametersForSubNestedFileWithParameters[0] = new Dictionary<string, string>();
            parametersForSubNestedFileWithParameters[0].Add("parameter", "sub test parameter");

            fileData.AddTemplateParameterData("NestedFileWithParametersTemplate", parametersForNestedFileWithParameters);
            fileData.AddTemplateParameterData("SubNestedFileWithParametersTemplate", parametersForSubNestedFileWithParameters);

            TemplateGenerator file = new TemplateGenerator(fileData);

            // act
            string result = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(result, expectedText);
        }

        [TestMethod]
        public void GenerateTemplateFileWithMultipleParameterSets()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "MultipleParameterSetsTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "MultipleParameterSetsOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(Regex.Escape(expectedOutputText));

            Dictionary<string, string>[] parameters = new Dictionary<string, string>[2];
            parameters[0] = new Dictionary<string, string>();
            parameters[0].Add("name", "James");
            parameters[1] = new Dictionary<string, string>();
            parameters[1].Add("name", "John");

            FileData fileData = new FileData();
            fileData.AddTemplateParameterData("MultipleParameterSetsTemplate", parameters);

            TemplateGenerator file = new TemplateGenerator(fileData);

            // act
            string result = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(result, expectedText);
        }

        [TestMethod]
        public void GenerateTemplateFileWithObjectDefinedParameterSets()
        {
            // arrange
            string templatePath = Path.Combine(FilePaths.TestFileTemplateDirectory, "MultipleParameterSetsTemplate.txt");
            string expectedOutputFilePath = Path.Combine(FilePaths.ExpectedTemplateOutputDirectory, "MultipleParameterSetsOutput.txt");
            string expectedOutputText = ReadTextFromFile(expectedOutputFilePath);
            Regex expectedText = new Regex(Regex.Escape(expectedOutputText));

            Person[] people = new Person[2];
            people[0] = new Person() { Name = "James", };
            people[1] = new Person() { Name = "John" };

            FileData fileData = new FileData();
            fileData.AddTemplateParameterData("MultipleParameterSetsTemplate", people);

            TemplateGenerator file = new TemplateGenerator(fileData);

            // act
            string result = file.GenerateTemplateText(templatePath);

            // assert
            StringAssert.Matches(result, expectedText);
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
    }
}
