using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RaptorFileGenerator
{
    public class TemplateGenerator
    {
        private const string DEFAULT_FILE_LINE_PREFIX = "###";
        private string _nestedFileLinePrefix;
        private FileData _fileData;

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

        public FileData FileData
        {
            get
            {
                return _fileData;
            }

            set
            {
                _fileData = value == null ? new FileData() : value;
            }
        }

        #region Constructors
        public TemplateGenerator()
        {
            CommonConstructor(null, null);
        }

        public TemplateGenerator(FileData fileData, string nestedFileLinePrefix = DEFAULT_FILE_LINE_PREFIX)
        {
            CommonConstructor(fileData, nestedFileLinePrefix);
        }

        private void CommonConstructor(FileData fileData, string nestedFileLinePrefix)
        {
            NestedFileLinePrefix = nestedFileLinePrefix;
            FileData = fileData;
        }
        #endregion

        public string GenerateTemplateText(string templatePath)
        {
            string[] templateLines = File.ReadAllLines(templatePath);
            templateLines = InjectTemplateParameters(templatePath, templateLines);
            string[] expandedTemplateLines = ExpandNestedFileLines(templateLines);
            string allLinesJoined = string.Join(Environment.NewLine, expandedTemplateLines);

            return allLinesJoined;
        }

        #region InjectTemplateParameters
        private string[] InjectTemplateParameters(string templatePath, string[] templateLines)
        {
            Dictionary<string, string>[] templateParameterSets = FileData.GetTemplateParameters(templatePath);

            // If template has no parameters, return the lines as is.
            if (templateParameterSets.Length == 0) {
                return templateLines;
            }

            List<string> templateTextForEachParameterSet = GetInjectedTemplateTextForEachParameterSet(templateLines, templateParameterSets);
            List<string> injectedTemplateTextLines = GetAllTemplateTextInstanceLines(templateTextForEachParameterSet);

            return injectedTemplateTextLines.ToArray();
        }

        private List<string> GetInjectedTemplateTextForEachParameterSet(string[] templateLines, Dictionary<string, string>[] templateParameterSets)
        {
            List<string> templateTextForEachParameterSet = new List<string>();

            foreach (Dictionary<string, string> parameterSet in templateParameterSets) {
                string templateTextForCurrentParameterSet = GetTemplateTextForParameterSet(templateLines, parameterSet);
                templateTextForEachParameterSet.Add(templateTextForCurrentParameterSet);
            }

            return templateTextForEachParameterSet;
        }

        private string GetTemplateTextForParameterSet(string[] templateLines, Dictionary<string, string> parameterSet)
        {
            string templateText = string.Join(Environment.NewLine, templateLines);
            ParameterInjector parameterInjector = new ParameterInjector(templateText, parameterSet);
            string injectedTemplateText = parameterInjector.GetInjectedText();

            return injectedTemplateText;
        }

        private static List<string> GetAllTemplateTextInstanceLines(List<string> templateTextForEachParameterSet)
        {
            List<string> injectedTemplateTextLines = new List<string>();

            foreach (string templateTextInstance in templateTextForEachParameterSet) {
                string[] templateTextInstanceLines = templateTextInstance.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                injectedTemplateTextLines.AddRange(templateTextInstanceLines);
            }

            return injectedTemplateTextLines;
        }
        #endregion

        #region ExpandNestedFileLines
        private string[] ExpandNestedFileLines(string[] templateLines)
        {
            List<string> expandedLines = new List<string>();

            foreach (string line in templateLines) {
                string expandedLine = GetLineOrNestedFileContents(line);
                expandedLines.Add(expandedLine);
            }

            return expandedLines.ToArray();
        }

        private string GetLineOrNestedFileContents(string line)
        {
            string expandedLine = line;

            if (line.StartsWith(NestedFileLinePrefix)) {
                string nestedFilePath = GetNestedFilePath(line);
                expandedLine = GenerateTemplateText(nestedFilePath);
            }

            return expandedLine;
        }

        private string GetNestedFilePath(string currentLine)
        {
            Regex nestedFilePathRegex = new Regex($"^{NestedFileLinePrefix} *(.+)$");
            Match nestedFilePathMatch = nestedFilePathRegex.Match(currentLine);
            Group nestedFilePathGroup = nestedFilePathMatch.Groups[1];
            string nestedFilePath = nestedFilePathGroup.Value.TrimEnd();

            return nestedFilePath;
        }
        #endregion
    }
}
