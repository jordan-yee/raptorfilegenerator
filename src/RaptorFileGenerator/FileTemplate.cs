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

        public string NestedFileLinePrefix
        {
            get
            {
                return _nestedFileLinePrefix;
            }

            set
            {
                _nestedFileLinePrefix = value;
            }
        }

        public string TemplateText
        {
            get
            {
                return _templateText;
            }
        }

        public FileTemplate(string templatePath)
        {
            _templatePath = templatePath;
            _templateText = File.ReadAllText(templatePath);
            _templateText = ExpandTemplateText(_templateText);
        }

        private string ExpandTemplateText(string templateText)
        {
            List<string> allLines = new List<string>();

            using(StringReader reader = new StringReader(templateText)) {
                List<string> currentSectionLines = new List<string>();

                string currentLine;
                while ((currentLine = reader.ReadLine()) != null) {

                    if (currentLine.StartsWith(_nestedFileLinePrefix)) {
                        // Add the current section to the list
                        allLines.AddRange(currentSectionLines);
                        currentSectionLines.Clear();

                        // Recursively get the text from the nested file
                        string nestedFilePath = GetNestedFilePath(currentLine);
                        string nestedFileText = File.ReadAllText(nestedFilePath);
                        string expandedNestedFileText = ExpandTemplateText(nestedFileText);
                        currentSectionLines.Add(expandedNestedFileText);
                    }
                    else {
                        currentSectionLines.Add(currentLine);
                    }

                }

                // Add remainder of current section to the list
                allLines.AddRange(currentSectionLines);
            }

            string allLinesJoined = string.Join(Environment.NewLine, allLines);
            return allLinesJoined;
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
