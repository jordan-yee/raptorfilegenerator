using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;

namespace TestLibrary.Tests
{
    [TestClass]
    public class TemplateDataTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTemplateParameterData_NullTemplateNameException_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateName = null;
            Dictionary<string, string>[] templateParameters = new Dictionary<string, string>[0];

            // act
            templateData.AddTemplateParameterData(templateName, templateParameters);

            // assert
            // An exception should be thrown by templateName being null.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTemplateParameterData_EmptyTemplateNameException_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateName = string.Empty;
            Dictionary<string, string>[] templateParameters = new Dictionary<string, string>[0];

            // act
            templateData.AddTemplateParameterData(templateName, templateParameters);

            // assert
            // An exception should be thrown by templateName being empty.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTemplateParameterData_NullTemplateParametersException_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateName = "template";
            Dictionary<string, string>[] templateParameters = null;

            // act
            templateData.AddTemplateParameterData(templateName, templateParameters);

            // assert
            // An exception should be thrown by templateName being null.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTemplateParameters_templateNameOrPathNullException_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateNameOrPath = null;

            // act
            templateData.GetTemplateParameters(templateNameOrPath);

            // assert
            // An exception should be thrown by templateNameOrPath being null.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTemplateParameters_templateNameOrPathEmptyException_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateNameOrPath = string.Empty;

            // act
            templateData.GetTemplateParameters(templateNameOrPath);

            // assert
            // An exception should be thrown by templateNameOrPath being empty.
        }

        [TestMethod]
        public void GetTemplateParameters_invalidTemplateNameReturnsEmptyArray_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateNameOrPath = "foo";

            // act
            Dictionary<string,string>[] result = templateData.GetTemplateParameters(templateNameOrPath);

            // assert
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void GetTemplateParameters_GetDataByTemplateName_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateName = "template";
            Dictionary<string, string>[] templateParameters = new Dictionary<string, string>[1];
            templateParameters[0] = new Dictionary<string, string>();
            templateParameters[0].Add("parameter", "value");
            templateData.AddTemplateParameterData(templateName, templateParameters);

            // act
            Dictionary<string, string>[] result = templateData.GetTemplateParameters(templateName);

            // assert
            Assert.IsTrue(result[0]["parameter"] == "value");
        }

        [TestMethod]
        public void GetTemplateParameters_GetDataByTemplatePath_Test()
        {
            // arrange
            TemplateData templateData = new TemplateData();
            string templateName = "template";
            string templatePath = @"C:\template.txt";
            Dictionary<string, string>[] templateParameters = new Dictionary<string, string>[1];
            templateParameters[0] = new Dictionary<string, string>();
            templateParameters[0].Add("parameter", "value");
            templateData.AddTemplateParameterData(templateName, templateParameters);

            // act
            Dictionary<string, string>[] result = templateData.GetTemplateParameters(templatePath);

            // assert
            Assert.IsTrue(result[0]["parameter"] == "value");
        }
    }
}
