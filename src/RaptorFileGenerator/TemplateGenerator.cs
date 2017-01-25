using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RaptorFileGenerator.Text;

namespace RaptorFileGenerator
{
    public class TemplateGenerator
    {
        private const string DEFAULT_FILE_LINE_PREFIX = "###";

        // All user-definable parameters
        private string _nestedFileLinePrefix;
        private TemplateData _templateData;
        // TODO: ParameterInjectorOptions
        // TODO: Template root directory (TemplateOptions w/ nestedFileLinePrefix)

        public string NestedFileLinePrefix
        {
            get
            {
                return _nestedFileLinePrefix;
            }

            set
            {
                _nestedFileLinePrefix = string.IsNullOrWhiteSpace(value) ? DEFAULT_FILE_LINE_PREFIX : value;
            }
        }

        public TemplateData TemplateData
        {
            get
            {
                return _templateData;
            }

            set
            {
                _templateData = value == null ? new TemplateData() : value;
            }
        }

        #region Constructors
        public TemplateGenerator()
        {
            CommonConstructor(null, null);
        }

        public TemplateGenerator(TemplateData templateData, string nestedFileLinePrefix = DEFAULT_FILE_LINE_PREFIX)
        {
            CommonConstructor(templateData, nestedFileLinePrefix);
        }

        private void CommonConstructor(TemplateData templateData, string nestedFileLinePrefix)
        {
            NestedFileLinePrefix = nestedFileLinePrefix;
            TemplateData = templateData;
        }
        #endregion

        public string GenerateTemplateText(string templatePath)
        {
            // Generate this template
            string templateText = File.ReadAllText(templatePath);
            Dictionary<string, string>[] templateParameterSets = _templateData.GetTemplateParameters(templatePath);
            TemplateInstancer instancer = new TemplateInstancer(templateText, templateParameterSets);
            templateText = instancer.GetCombinedTemplateInstances();

            // Recursively generate nested templates
            string expandedTemplateText = ExpandNestedFileLines(templateText);

            return expandedTemplateText;
        }

        #region ExpandNestedFileLines
        private string ExpandNestedFileLines(string templateText)
        {
            string[] templateLines = LineConverter.GetLinesFromText(templateText);
            List<string> expandedLines = new List<string>();

            foreach (string line in templateLines) {
                string expandedLine = GetLineOrNestedFileContents(line);
                expandedLines.Add(expandedLine);
            }

            string expandedTemplateText = LineConverter.GetTextFromLines(expandedLines.ToArray());

            return expandedTemplateText;
        }

        private string GetLineOrNestedFileContents(string line)
        {
            string expandedLine = line;

            if (line.StartsWith(NestedFileLinePrefix)) {
                string nestedFilePath = GetNestedFilePath(line);

                // Recurse:
                expandedLine = GenerateTemplateText(nestedFilePath);
            }

            return expandedLine;
        }

        private string GetNestedFilePath(string currentLine)
        {
            // TODO: Detect if complete file path is given, or if path is relative to template root directory.
            Regex nestedFilePathRegex = new Regex($"^{NestedFileLinePrefix} *(.+)$");
            Match nestedFilePathMatch = nestedFilePathRegex.Match(currentLine);
            Group nestedFilePathGroup = nestedFilePathMatch.Groups[1];
            string nestedFilePath = nestedFilePathGroup.Value.TrimEnd();

            return nestedFilePath;
        }
        #endregion
    }
}
