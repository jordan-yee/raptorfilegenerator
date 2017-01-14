﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RaptorFileGenerator
{
    public class FileDefinition
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

        public FileDefinition(string templatePath)
        {
            _templatePath = templatePath;
            _templateText = File.ReadAllText(templatePath);
            _templateText = ExpandTemplateText(_templateText);
        }

        private string ExpandTemplateText(string templateText)
        {
            List<string> sections = new List<string>();

            using(StringReader reader = new StringReader(templateText)) {
                List<string> currentSectionLines = new List<string>();

                string currentLine;
                while ((currentLine = reader.ReadLine()) != null) {
                    if (currentLine.StartsWith(_nestedFileLinePrefix)) {
                        // Add the current section to the list
                        sections.Add(string.Concat(currentSectionLines));
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
                sections.Add(string.Concat(currentSectionLines));
            }

            return string.Concat(sections);
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
