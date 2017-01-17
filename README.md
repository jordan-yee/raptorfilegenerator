# Raptor File Generator
### A .NET Core class library for generating custom text (code) files.

---
## Usage
### 1. Create text file template.
*Put any text you want in a file.*  
__template.txt:__
> Hello, World!

### 2. Embed other text files in your template via their file paths.
*By default, define embedded files by starting lines with '###' and following them with a file path.*  
__template.txt:__
> Hello, World!  
> \### C:\\file.txt

__file.txt (nested file):__
> This is a nested file.

### 3. Parameterize any text in your template.
*By default, parameters are defined by surrounding a parameter name in double curly braces: {{parameter}}.*  
__Template File:__
> Hello, {{name}}!  
> \### C:\\file.txt

### 4. Get the text of the generated templates and write it to a file, or do anything else you want with it.
__C# Code:__

	// Define the template path
	string templatePath = "C:\\template.txt";
	
	// Get the expanded template text
	TemplateExpander templateFile = new TemplateExpander(templatePath);
	string templateText = templateFile.TemplateText;
	
	// Define your parameters
	Dictionary<string, string> parameters = new Dictionary<string, string>();
	parameters.Add("name", "World");
	
	// Inject the parameters into the template
	ParameterInjector pi = new ParameterInjector(templateText, parameters);
	string result = pi.GetInjectedText();
	
__Result:__  
Hello, World!  
This is a nested file.