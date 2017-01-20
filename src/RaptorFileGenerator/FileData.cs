using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RaptorFileGenerator
{
    public class FileData
    {
        private Dictionary<string, Dictionary<string, string>[]> _fileData;

        public FileData()
        {
            _fileData = new Dictionary<string, Dictionary<string, string>[]>();
        }

        public void AddTemplateParameterData(string templateName, Dictionary<string, string>[] templateParameters)
        {
            if (string.IsNullOrEmpty(templateName)) {
                throw new ArgumentException("Parameter cannot be null or empty.", "templateName");
            }

            if(templateParameters == null) {
                throw new ArgumentNullException("templateParameters");
            }

            _fileData.Add(templateName, templateParameters);
        }

        public Dictionary<string, string>[] GetTemplateParameters(string templateNameOrPath)
        {
            if (string.IsNullOrEmpty(templateNameOrPath)) {
                throw new ArgumentException("Parameter cannot be null or empty.", "templateNameOrPath");
            }

            string templateName = Path.GetFileNameWithoutExtension(templateNameOrPath);

            if (!_fileData.ContainsKey(templateName)) {
                return new Dictionary<string, string>[0];
            }

            return _fileData[templateName];
        }
    }
}
