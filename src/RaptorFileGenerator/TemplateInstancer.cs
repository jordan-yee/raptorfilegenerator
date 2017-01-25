using System;
using System.Collections.Generic;
using RaptorFileGenerator.Text;

public class TemplateInstancer
{
    private Dictionary<string, string>[] _parameterSets;
    private string _templateText;
    private readonly List<string> _templateInstances = new List<string>();

    private bool _generateSingleInstanceIfNoParameterSetsAreGiven;

    public string[] TemplateInstances
    {
        get
        {
            return _templateInstances.ToArray();
        }
    }

    public TemplateInstancer(string templateText, Dictionary<string, string>[] parameterSets,
        bool generateSingleInstanceIfNoParameterSetsAreGiven = true)
    {
        _templateText = templateText;
        _parameterSets = parameterSets;
        _generateSingleInstanceIfNoParameterSetsAreGiven = generateSingleInstanceIfNoParameterSetsAreGiven;
        InitializeTemplateInstances();
    }

    private void InitializeTemplateInstances()
    {
        if (_parameterSets == null || _parameterSets.Length == 0) {
            if (_generateSingleInstanceIfNoParameterSetsAreGiven) {
                _templateInstances.Add(_templateText);
            }
        }
        else if (_parameterSets != null) {
            CreateTemplateInstanceForEachParameterSet();
        }
    }

    private void CreateTemplateInstanceForEachParameterSet()
    {
        foreach (Dictionary<string, string> parameterSet in _parameterSets) {
            ParameterInjector parameterInjector = new ParameterInjector(_templateText, parameterSet);
            string templateInstance = parameterInjector.GetInjectedText();
            _templateInstances.Add(templateInstance);
        }
    }

    public string GetCombinedTemplateInstances()
    {
        string combinedTemplateInstances = string.Join(Environment.NewLine, _templateInstances);

        return combinedTemplateInstances;
    }
}