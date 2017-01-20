using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;

namespace TestLibrary
{
    [TestClass]
    public class FileDataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTemplateParameterData_NullTemplateNameException_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateName = null;
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            // act
            fileData.AddTemplateParameterData(templateName, templateParameters);

            // assert
            // An exception should be thrown by templateName being null.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTemplateParameterData_EmptyTemplateNameException_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateName = string.Empty;
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            // act
            fileData.AddTemplateParameterData(templateName, templateParameters);

            // assert
            // An exception should be thrown by templateName being empty.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTemplateParameterData_NullTemplateParametersException_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateName = "template";
            Dictionary<string, string> templateParameters = null;

            // act
            fileData.AddTemplateParameterData(templateName, templateParameters);

            // assert
            // An exception should be thrown by templateName being null.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTemplateParameters_templateNameOrPathNullException_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateNameOrPath = null;

            // act
            fileData.GetTemplateParameters(templateNameOrPath);

            // assert
            // An exception should be thrown by templateNameOrPath being null.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTemplateParameters_templateNameOrPathEmptyException_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateNameOrPath = string.Empty;

            // act
            fileData.GetTemplateParameters(templateNameOrPath);

            // assert
            // An exception should be thrown by templateNameOrPath being empty.
        }

        [TestMethod]
        public void GetTemplateParameters_templateNameNotFoundReturnsEmptyDictionary_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateNameOrPath = "foo";

            // act
            Dictionary<string,string> result = fileData.GetTemplateParameters(templateNameOrPath);

            // assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetTemplateParameters_GetDictionaryByTemplateName_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateName = "template";
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();
            templateParameters.Add("parameter", "value");
            fileData.AddTemplateParameterData(templateName, templateParameters);

            // act
            Dictionary<string, string> result = fileData.GetTemplateParameters(templateName);

            // assert
            Assert.IsTrue(result["parameter"] == "value");
        }

        [TestMethod]
        public void GetTemplateParameters_GetDictionaryByTemplatePath_Test()
        {
            // arrange
            FileData fileData = new FileData();
            string templateName = "template";
            string templatePath = @"C:\template.txt";
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();
            templateParameters.Add("parameter", "value");
            fileData.AddTemplateParameterData(templateName, templateParameters);

            // act
            Dictionary<string, string> result = fileData.GetTemplateParameters(templatePath);

            // assert
            Assert.IsTrue(result["parameter"] == "value");
        }
    }
}
