using System;
using System.IO;
using CleanForge.Templates;

class CleanArchScaffold
{
    private readonly string _projectName;
    private readonly string _outputPath;
    private readonly string _solutionPath;

    private readonly ProjectTemplates _projectTemplates;
    private readonly DomainTemplates _domainTemplates;
    private readonly ApplicationTemplates _applicationTemplates;
    private readonly InfrastructureTemplates _infrastructureTemplates;
    private readonly ApiTemplates _apiTemplates;

    public CleanArchScaffold(string projectName, string outputPath)
    {
        _projectName = projectName;
        _outputPath = outputPath;
        _solutionPath = Path.Combine(outputPath, projectName);

        _projectTemplates = new ProjectTemplates(projectName);
        _domainTemplates = new DomainTemplates(projectName);
        _applicationTemplates = new ApplicationTemplates(projectName);
        _infrastructureTemplates = new InfrastructureTemplates(projectName);
        _apiTemplates = new ApiTemplates(projectName);
    }

    public void Generate()
    {
        CreateDirectoryStructure();
        CreateProjectFiles();
        CreateSolutionFile();
        CreateSourceFiles();
        Console.WriteLine("\nGenerating files...");
    }

    private void CreateDirectoryStructure()
    {
        var dirs = new[]
        {
            _solutionPath,
            $"{_solutionPath}/{_projectName}.Domain",
            $"{_solutionPath}/{_projectName}.Domain/Entities",
            $"{_solutionPath}/{_projectName}.Domain/Common",
            $"{_solutionPath}/{_projectName}.Application",
            $"{_solutionPath}/{_projectName}.Application/Products",
            $"{_solutionPath}/{_projectName}.Application/Products/Commands",
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/CreateProduct",
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/UpdateProduct",
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/DeleteProduct",
            $"{_solutionPath}/{_projectName}.Application/Products/Queries",
            $"{_solutionPath}/{_projectName}.Application/Products/Queries/GetProductById",
            $"{_solutionPath}/{_projectName}.Application/Products/Queries/GetAllProducts",
            $"{_solutionPath}/{_projectName}.Application/Common",
            $"{_solutionPath}/{_projectName}.Application/Common/Behaviours",
            $"{_solutionPath}/{_projectName}.Application/Common/Interfaces",
            $"{_solutionPath}/{_projectName}.Application/Common/Models",
            $"{_solutionPath}/{_projectName}.Infrastructure",
            $"{_solutionPath}/{_projectName}.Infrastructure/Data",
            $"{_solutionPath}/{_projectName}.Infrastructure/Repositories",
            $"{_solutionPath}/{_projectName}.Api",
            $"{_solutionPath}/{_projectName}.Api/Controllers",
            $"{_solutionPath}/{_projectName}.Api/Middleware",
            $"{_solutionPath}/{_projectName}.Api/Properties",
        };

        foreach (var dir in dirs)
        {
            Directory.CreateDirectory(dir);
        }
    }

    private void CreateProjectFiles()
    {
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Domain/{_projectName}.Domain.csproj",
            _projectTemplates.GetDomainCsproj());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/{_projectName}.Application.csproj",
            _projectTemplates.GetApplicationCsproj());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/{_projectName}.Infrastructure.csproj",
            _projectTemplates.GetInfrastructureCsproj());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/{_projectName}.Api.csproj",
            _projectTemplates.GetApiCsproj());
    }

    private void CreateSolutionFile()
    {
        var domainGuid = Guid.NewGuid().ToString("B").ToUpper();
        var applicationGuid = Guid.NewGuid().ToString("B").ToUpper();
        var infrastructureGuid = Guid.NewGuid().ToString("B").ToUpper();
        var apiGuid = Guid.NewGuid().ToString("B").ToUpper();

        var slnContent = _projectTemplates.GetSolutionFile(domainGuid, applicationGuid, infrastructureGuid, apiGuid);
        File.WriteAllText($"{_solutionPath}/{_projectName}.sln", slnContent);
    }

    private void CreateSourceFiles()
    {
        CreateDomainFiles();
        CreateApplicationFiles();
        CreateInfrastructureFiles();
        CreateApiFiles();
    }

    private void CreateDomainFiles()
    {
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Domain/Common/BaseEntity.cs",
            _domainTemplates.GetBaseEntityCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Domain/Entities/Product.cs",
            _domainTemplates.GetProductEntityCode());
    }

    private void CreateApplicationFiles()
    {
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Interfaces/IRepository.cs",
            _applicationTemplates.GetIRepositoryCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Interfaces/IApplicationDbContext.cs",
            _applicationTemplates.GetIApplicationDbContextCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Models/PagedResult.cs",
            _applicationTemplates.GetPagedResultCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Behaviours/ValidationBehaviour.cs",
            _applicationTemplates.GetValidationBehaviourCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Behaviours/LoggingBehaviour.cs",
            _applicationTemplates.GetLoggingBehaviourCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/DependencyInjection.cs",
            _applicationTemplates.GetApplicationDICode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/CreateProduct/CreateProductCommand.cs",
            _applicationTemplates.GetCreateProductCommandCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/CreateProduct/CreateProductCommandValidator.cs",
            _applicationTemplates.GetCreateProductValidatorCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/UpdateProduct/UpdateProductCommand.cs",
            _applicationTemplates.GetUpdateProductCommandCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/UpdateProduct/UpdateProductCommandValidator.cs",
            _applicationTemplates.GetUpdateProductValidatorCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/DeleteProduct/DeleteProductCommand.cs",
            _applicationTemplates.GetDeleteProductCommandCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Queries/GetProductById/GetProductByIdQuery.cs",
            _applicationTemplates.GetGetProductByIdQueryCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Queries/GetAllProducts/GetAllProductsQuery.cs",
            _applicationTemplates.GetGetAllProductsQueryCode());
    }

    private void CreateInfrastructureFiles()
    {
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/Data/AppDbContext.cs",
            _infrastructureTemplates.GetAppDbContextCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/Repositories/Repository.cs",
            _infrastructureTemplates.GetRepositoryCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/DependencyInjection.cs",
            _infrastructureTemplates.GetInfrastructureDICode());
    }

    private void CreateApiFiles()
    {
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/Program.cs",
            _apiTemplates.GetProgramCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/Controllers/ProductsController.cs",
            _apiTemplates.GetProductsControllerCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/Middleware/GlobalExceptionHandler.cs",
            _apiTemplates.GetGlobalExceptionHandlerCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/appsettings.json",
            _apiTemplates.GetAppSettingsCode());

        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/Properties/launchSettings.json",
            _apiTemplates.GetLaunchSettingsCode());
    }
}
