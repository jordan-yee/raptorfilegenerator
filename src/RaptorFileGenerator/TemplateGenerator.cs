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

        public string ExpandTemplateText(string templatePath)
        {
            string[] templateLines = File.ReadAllLines(templatePath);
            string[] expandedTemplateLines = ExpandNestedFileLines(templateLines);
            string allLinesJoined = string.Join(Environment.NewLine, expandedTemplateLines);

            return allLinesJoined;
        }

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
                expandedLine = ExpandTemplateText(nestedFilePath);
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
    }
}
