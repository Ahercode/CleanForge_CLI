using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var projectName = GetArgValue(args, "--name") ?? "CleanForgeProject";
        var outputPath = GetArgValue(args, "--output") ?? "./";
        
        Console.WriteLine($"Creating Clean Architecture project: {projectName}");
        Console.WriteLine("Features: MediatR + FluentValidation + CQRS");
        
        var scaffold = new CleanArchScaffold(projectName, outputPath);
        scaffold.Generate();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
    _____  ______      _____ _      _____ 
  / ____| |  ____|    / ____| |    |_   _|
 | |      | |___     | |    | |      | |  
 | |      |  __/     | |    | |      | |  
 | |___|  | |        | |____| |____ _| |_ 
  \_____\ |_|         \_____|______|_____|
                                                         
            Clean Forge CLI
    ");
        Console.ResetColor();
        
        Console.WriteLine("\n✓ Project created successfully!");
        Console.WriteLine($"\nNext steps:");
        Console.WriteLine($"  cd {outputPath}/{projectName}");
        Console.WriteLine($"  dotnet restore");
        Console.WriteLine($"  dotnet run --project {projectName}.Api");
    }
    
    static string? GetArgValue(string[] args, string key)
    {
        var idx = Array.IndexOf(args, key);
        return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : null;
    }
}