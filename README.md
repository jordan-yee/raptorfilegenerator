# Raptor File Generator
### A .NET Core class library for generating custom text (code) files.

---
## Usage
### 1. Create text file template.
*Put any text you want in a file.*  
__template.txt:__
> Hello, World!

### 2. Embed other text files in your template via their file paths.
*By default, define embedded files using lines that start with '###' followed by a file path.*  
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
	
	// Define template parameters
	Dictionary<string, string>[] parameters = new Dictionary<string, string>[1];
	parameters[0] = new Dictionary<string, string>();
	parameters[0].Add("name", "World");
	
	FileData fileData = new FileData();
    fileData.AddTemplateParameterData("template", parameters);
	
	// Get the expanded template text
	TemplateGenerator templateGenerator = new TemplateGenerator(fileData);
	string templateText = templateGenerator.GenerateTemplateText(templatePath);
	
__Result:__  
Hello, World!  
This is a nested file.

### 5. Assign multiple parameter sets to a template's FileData to repeat that template for each parameter set.
__C# Code:__

	// Define the template path
	string templatePath = "C:\\template.txt";
	
	// Define template parameters
	Dictionary<string, string>[] parameters = new Dictionary<string, string>[2];
	parameters[0] = new Dictionary<string, string>();
	parameters[0].Add("name", "World");
	parameters[1] = new Dictionary<string, string>();
	parameters[2].Add("name", "Other World");
	
	FileData fileData = new FileData();
    fileData.AddTemplateParameterData("template", parameters);
	
	// Get the expanded template text
	TemplateGenerator templateGenerator = new TemplateGenerator(fileData);
	string templateText = templateGenerator.GenerateTemplateText(templatePath);
	
__Result:__  
Hello, World!  
Hello, Other World!  
This is a nested file.

### 6. Alternatively assign template parameter values using POCOs
__C# Code:__

	// Define the template path
	string templatePath = "C:\\template.txt";
	
	// Define template parameters
	List<ParameterSets> parameters= new ParameterSet[2];
	parameters.Add(new ParameterSet() { Name = "World", });
	parameters.Add(new ParameterSet() { Name = "Other World" });
	
	FileData fileData = new FileData();
	// Note: The 3rd parameter is optional--whether to convert parameter objects' property names to camelCase
    fileData.AddTemplateParameterData("template", parameters, true);
	
	// Get the expanded template text
	TemplateGenerator templateGenerator = new TemplateGenerator(fileData);
	string templateText = templateGenerator.GenerateTemplateText(templatePath);
	
__Result:__  
Hello, World!  
Hello, Other World!  
This is a nested file.