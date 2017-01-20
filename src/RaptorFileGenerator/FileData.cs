using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public void AddTemplateParameterData<T>(string templateName, IEnumerable<T> templateParameters, bool convertPropertyNamesToCamelCase = true)
        {
            List<Dictionary<string, string>> templateParameterDictionaries = new List<Dictionary<string, string>>();

            foreach (T parameterObject in templateParameters) {
                Dictionary<string, string> parameterDictionary = ConvertObjectToDictionary(parameterObject, convertPropertyNamesToCamelCase);
                templateParameterDictionaries.Add(parameterDictionary);
            }

            AddTemplateParameterData(templateName, templateParameterDictionaries.ToArray());
        }

        private Dictionary<string, string> ConvertObjectToDictionary<T>(T parameterObject, bool convertPropertyNamesToCamelCase)
        {
            PropertyInfo[] objectProperties = typeof(T).GetProperties();

            Dictionary<string, string> objectAsDictionary;
            if (convertPropertyNamesToCamelCase) {
                objectAsDictionary = objectProperties.ToDictionary(pi => FirstCharacterToLower(pi.Name),
                    pi => pi.GetValue(parameterObject).ToString());
            }
            else {
                objectAsDictionary = objectProperties.ToDictionary(pi => pi.Name,
                    pi => pi.GetValue(parameterObject).ToString());
            }

            return objectAsDictionary;
        }

        private string FirstCharacterToLower(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;
            if (name.Length == 1)
                return name.ToLowerInvariant();
            else
                return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }

        public void AddTemplateParameterData(string templateName, Dictionary<string, string>[] templateParameters)
        {
            if (string.IsNullOrEmpty(templateName)) {
                throw new ArgumentException("Parameter cannot be null or empty.", "templateName");
            }

            if (templateParameters == null) {
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
