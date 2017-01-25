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
    public class TemplateInstancerTests
    {
        [TestMethod]
        public void GetCombinedTemplateInstancesTest()
        {
            // arrange
            string templateText = "Text {{parameter}}";
            Dictionary<string, string>[] parameterSets = new Dictionary<string, string>[2];
            parameterSets[0] = new Dictionary<string, string>();
            parameterSets[0].Add("parameter", "p1");
            parameterSets[1] = new Dictionary<string, string>();
            parameterSets[1].Add("parameter", "p2");
            string expectedOutput = $"Text p1{Environment.NewLine}Text p2";
            TemplateInstancer instancer = new TemplateInstancer(templateText, parameterSets);

            // act
            string result = instancer.GetCombinedTemplateInstances();

            // assert
            StringAssert.Matches(result, new Regex(expectedOutput));
        }

        [TestMethod]
        public void TemplateInstancesTest()
        {
            // arrange
            string templateText = "Text {{parameter}}";
            Dictionary<string, string>[] parameterSets = new Dictionary<string, string>[2];
            parameterSets[0] = new Dictionary<string, string>();
            parameterSets[0].Add("parameter", "p1");
            parameterSets[1] = new Dictionary<string, string>();
            parameterSets[1].Add("parameter", "p2");
            string[] expectedOutput = new string[] { "Text p1", "Text p2" };
            TemplateInstancer instancer = new TemplateInstancer(templateText, parameterSets);

            // act
            string[] result = instancer.TemplateInstances;

            // assert
            CollectionAssert.AreEqual(expectedOutput, result);
        }

        [TestMethod]
        public void NullOrEmptyParameterSetsReturnsSingleInstanceOfTemplateTextByDefault()
        {
            // arrange
            string templateText = "Template Text";
            Dictionary<string, string>[] nullParameterSets = null;
            Dictionary<string, string>[] emptyParameterSets = new Dictionary<string, string>[0];
            string[] expectedOutput = new string[] { "Template Text" };
            TemplateInstancer nullInstancer = new TemplateInstancer(templateText, nullParameterSets);
            TemplateInstancer emptyInstancer = new TemplateInstancer(templateText, emptyParameterSets);

            // act
            string[] nullParameterSetsResult = nullInstancer.TemplateInstances;
            string nullParameterSetsCombinedResult = nullInstancer.GetCombinedTemplateInstances();
            string[] emptyParameterSetsResult = emptyInstancer.TemplateInstances;
            string emptyParameterSetsCombinedResult = emptyInstancer.GetCombinedTemplateInstances();

            // assert
            CollectionAssert.AreEqual(expectedOutput, nullParameterSetsResult);
            StringAssert.Matches(nullParameterSetsCombinedResult, new Regex(templateText));
            CollectionAssert.AreEqual(expectedOutput, emptyParameterSetsResult);
            StringAssert.Matches(emptyParameterSetsCombinedResult, new Regex(templateText));
        }

        [TestMethod]
        public void NullOrEmptyParameterSetsReturnsEmptyWithOption()
        {
            // arrange
            string templateText = "Template Text";
            Dictionary<string, string>[] nullParameterSets = null;
            Dictionary<string, string>[] emptyParameterSets = new Dictionary<string, string>[0];
            string[] expectedOutput = new string[0];
            TemplateInstancer nullInstancer = new TemplateInstancer(templateText, nullParameterSets, false);
            TemplateInstancer emptyInstancer = new TemplateInstancer(templateText, emptyParameterSets, false);

            // act
            string[] nullParameterSetsResult = nullInstancer.TemplateInstances;
            string nullParameterSetsCombinedResult = nullInstancer.GetCombinedTemplateInstances();
            string[] emptyParameterSetsResult = emptyInstancer.TemplateInstances;
            string emptyParameterSetsCombinedResult = emptyInstancer.GetCombinedTemplateInstances();

            // assert
            CollectionAssert.AreEqual(expectedOutput, nullParameterSetsResult);
            StringAssert.Matches(nullParameterSetsCombinedResult, new Regex(string.Empty));
            CollectionAssert.AreEqual(expectedOutput, emptyParameterSetsResult);
            StringAssert.Matches(emptyParameterSetsCombinedResult, new Regex(string.Empty));
        }
    }
}
