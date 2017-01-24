using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaptorFileGenerator;
using RaptorFileGenerator.Text;
using System.Text.RegularExpressions;

namespace TestLibrary.Tests
{
    [TestClass]
    public class LineConverterTests
    {
        [TestMethod]
        public void CanRetrieveOriginialTextFromObject()
        {
            // arrange
            string originalText = "Original Text";
            LineConverter lineConverter = new LineConverter(originalText);

            // act
            string retrievedText = lineConverter.Text;

            // assert
            StringAssert.Matches(originalText, new Regex(retrievedText));
        }

        [TestMethod]
        public void CanRetrieveOriginalLinesFromObject()
        {
            // arrange
            string[] originalLines = new string[] { "Line 1", "Line 2" };
            LineConverter lineConverter = new LineConverter(originalLines);

            // act
            string[] retrievedLines = lineConverter.Lines;

            // assert
            CollectionAssert.AreEqual(originalLines, retrievedLines);
        }

        [TestMethod]
        public void CanRetrieveLinesFromText()
        {
            // arrange
            string[] textVariations = new string[]
            {
                "Line 1\nLine 2",
                "Line 1\r\nLine 2"
            };
            string[] expectedLines = new string[] { "Line 1", "Line 2" };

            foreach (string text in textVariations) {
                // act
                // Text set via constructor
                LineConverter lineConverter = new LineConverter(text);
                string[] retrievedLines = lineConverter.Lines;

                // Text set via property
                LineConverter lineConverter2 = new LineConverter("Foo");
                lineConverter2.Text = text;
                string[] retrievedLines2 = lineConverter2.Lines;

                // assert
                CollectionAssert.AreEqual(expectedLines, retrievedLines);
                CollectionAssert.AreEqual(expectedLines, retrievedLines2);
            }
        }

        [TestMethod]
        public void CanRetrieveTextFromLines()
        {
            // arrange
            string[] lines = new string[] { "Line 1", "Line 2" };
            string expectedText = $"Line 1{Environment.NewLine}Line 2";
            LineConverter[] lineConverterVariations = new LineConverter[2];

            // Lines set via constructor
            lineConverterVariations[0] = new LineConverter(lines);

            // Lines set via parameter
            lineConverterVariations[1] = new LineConverter("foo");
            lineConverterVariations[1].Lines = lines;

            foreach (LineConverter lineConverter in lineConverterVariations) {
                // act
                string retrievedText = lineConverter.Text;

                // assert
                StringAssert.Matches(retrievedText, new Regex(expectedText));
            }
        }

        [TestMethod]
        public void GetLinesFromText_StaticMethodTest()
        {
            // arrange
            string[] textVariations = new string[]
            {
                "Line 1\nLine 2",
                "Line 1\r\nLine 2"
            };
            string[] expectedLines = new string[] { "Line 1", "Line 2" };

            foreach (string text in textVariations) {
                // act
                string[] retrievedLines = LineConverter.GetLinesFromText(text);

                // assert
                CollectionAssert.AreEqual(expectedLines, retrievedLines);
            }

            // null test
            Equals(LineConverter.GetLinesFromText(null), null);
        }

        [TestMethod]
        public void GetTextFromLines_StaticMethodTest()
        {
            // arrange
            string[] lines = new string[] { "Line 1", "Line 2" };
            string expectedText = $"Line 1{Environment.NewLine}Line 2";

            // act
            string retrievedText = LineConverter.GetTextFromLines(lines);

            // assert
            StringAssert.Matches(retrievedText, new Regex(expectedText));

            // null test
            Equals(LineConverter.GetTextFromLines(null), null);
        }

        [TestMethod]
        public void CanSetTextAsNull()
        {
            // assert
            string nullString = null;
            LineConverter lineConverter = new LineConverter(nullString);
            LineConverter lineConverter2 = new LineConverter("Foo");
            lineConverter2.Text = nullString;

            // act
            string retrievedText = lineConverter.Text;
            string[] retrievedLines = lineConverter.Lines;
            string retrievedText2 = lineConverter2.Text;
            string[] retrievedLines2 = lineConverter2.Lines;

            // assert
            Equals(retrievedText, null);
            Equals(retrievedLines, null);
            Equals(retrievedText2, null);
            Equals(retrievedLines2, null);
        }

        [TestMethod]
        public void CanSetLinesAsNull()
        {
            // assert
            string[] nullStringArray = null;
            LineConverter lineConverter = new LineConverter(nullStringArray);
            LineConverter lineConverter2 = new LineConverter("Foo");
            lineConverter2.Lines = nullStringArray;

            // act
            string retrievedText = lineConverter.Text;
            string[] retrievedLines = lineConverter.Lines;
            string retrievedText2 = lineConverter2.Text;
            string[] retrievedLines2 = lineConverter2.Lines;

            // assert
            Equals(retrievedText, null);
            Equals(retrievedLines, null);
            Equals(retrievedText2, null);
            Equals(retrievedLines2, null);
        }
    }
}
