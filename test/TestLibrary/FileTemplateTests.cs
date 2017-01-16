using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;
using System.Text.RegularExpressions;

namespace TestLibrary
{
    [TestClass]
    public class FileTemplateTests
    {
        [TestMethod]
        public void TestFileDefinitionCreation()
        {
            // arrange
            string basicFileTemplatePath = @"C:\Users\jyee_\Documents\GitHub\RaptorFileGenerator\test\TestLibrary\TestFilesTemplates\BasicFileTemplate.txt";
            Regex expectedText = new Regex("Test Template");

            // act
            FileTemplate file = new FileTemplate(basicFileTemplatePath);
            string expandedTemplateText = file.TemplateText;

            // assert
            StringAssert.Matches(expandedTemplateText, expectedText);
        }
    }
}
