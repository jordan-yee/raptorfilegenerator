using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RaptorFileGenerator
{
    public class FileTemplate
    {
        private string _templatePath;
        private string _templateText;
        private string _nestedFileLinePrefix = "###";

        public string TemplateText
        {
            get
            {
                return _templateText;
            }
        }

        public FileTemplate(string templatePath, string nestedFileLinePrefix = "###")
        {
            _templatePath = templatePath;
            _templateText = ExpandTemplateText(templatePath);
        }

        private string ExpandTemplateText(string templatePath)
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

            if (line.StartsWith(_nestedFileLinePrefix)) {
                string nestedFilePath = GetNestedFilePath(line);
                expandedLine = ExpandTemplateText(nestedFilePath);
            }

            return expandedLine;
        }

        private string GetNestedFilePath(string currentLine)
        {
            Regex nestedFilePathRegex = new Regex($"^{_nestedFileLinePrefix} *(.+)$");
            Match nestedFilePathMatch = nestedFilePathRegex.Match(currentLine);
            Group nestedFilePathGroup = nestedFilePathMatch.Groups[1];
            string nestedFilePath = nestedFilePathGroup.Value.TrimEnd();

            return nestedFilePath;
        }
    }
}
