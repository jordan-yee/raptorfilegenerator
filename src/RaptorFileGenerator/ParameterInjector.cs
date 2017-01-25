using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// TODO: Extract ParameterInjectorOptions, allowing them to be set by other classes using this class.
public class ParameterInjector
{
    private string _input;
    private Dictionary<string, string> _parameters;
    private string _startingParameterDelimitter = "{{";
    private string _endingParameterDelimitter = "}}";

    public ParameterInjector(string input, Dictionary<string, string> parameters)
    {
        _input = input;
        _parameters = parameters;

        ValidateInput();
        ValidateParameters();
    }

    public string Input
    {
        get
        {
            return _input;
        }

        set
        {
            _input = value;
        }
    }

    public Dictionary<string, string> Parameters
    {
        get
        {
            return _parameters;
        }

        set
        {
            _parameters = value;
        }
    }

    public string StartingParameterDelimitter
    {
        get
        {
            return _startingParameterDelimitter;
        }

        set
        {
            if(string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentNullException("StartingParameterDelimitter",
                    "The parameter cannot be set to null, empty, or whitespace.");
            }
            _startingParameterDelimitter = Regex.Escape(value);
        }
    }

    public string EndingParameterDelimitter
    {
        get
        {
            return _endingParameterDelimitter;
        }

        set
        {
            if(string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentNullException("StartingParameterDelimitter",
                    "The parameter cannot be set to null, empty, or whitespace.");
            }
            _endingParameterDelimitter = Regex.Escape(value);
        }
    }

    public string GetInjectedText()
    {
        ValidateAllParameters();

        string injectedText = _input;

        string[] parameterNames = GetParameterNamesFromDictionary();
        foreach (string parameterName in parameterNames) {
            injectedText = InjectParameterValue(injectedText, parameterName);
        }

        return injectedText;
    }

    private void ValidateAllParameters()
    {
        ValidateInput();
        ValidateParameters();
        ValidateStartingParameterDelimitter();
        ValidateEndingParameterDelimitter();
    }

    private void ValidateInput()
    {
        if(_input == null) {
            throw new ArgumentNullException("Input");
        }
    }

    private void ValidateParameters()
    {
        if(_parameters == null) {
            throw new ArgumentNullException("Parameters");
        }
    }

    private void ValidateStartingParameterDelimitter()
    {
        if(_parameters == null) {
            throw new ArgumentNullException("StartingParameterDelimitter");
        }
    }

    private void ValidateEndingParameterDelimitter()
    {
        if(_parameters == null) {
            throw new ArgumentNullException("EndingParameterDelimitter");
        }
    }

    private string[] GetParameterNamesFromDictionary()
    {
        List<string> keys = new List<string>();
        foreach (string name in _parameters.Keys) {
            keys.Add(name);
        }

        return keys.ToArray();
    }

    private string InjectParameterValue(string text, string parameterName)
    {
        string parameterPattern = _startingParameterDelimitter + " *" + parameterName + " *" + _endingParameterDelimitter;
        string parameterReplacementValue = _parameters[parameterName];
        text = Regex.Replace(text, parameterPattern, parameterReplacementValue);

        return text;
    }
}