using System;
using System.IO;

class CleanArchScaffold
{
    private readonly string _projectName;
    private readonly string _outputPath;
    private readonly string _solutionPath;
    
    public CleanArchScaffold(string projectName, string outputPath)
    {
        _projectName = projectName;
        _outputPath = outputPath;
        _solutionPath = Path.Combine(outputPath, projectName);
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
            $"{_solutionPath}/{_projectName}.Infrastructure",
            $"{_solutionPath}/{_projectName}.Infrastructure/Data",
            $"{_solutionPath}/{_projectName}.Infrastructure/Repositories",
            $"{_solutionPath}/{_projectName}.Api",
            $"{_solutionPath}/{_projectName}.Api/Controllers",
        };
        
        foreach (var dir in dirs)
        {
            Directory.CreateDirectory(dir);
        }
    }
    
    private void CreateProjectFiles()
    {
        // Domain project
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Domain/{_projectName}.Domain.csproj",
            GetDomainCsproj());
        
        // Application project
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/{_projectName}.Application.csproj",
            GetApplicationCsproj());
        
        // Infrastructure project
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/{_projectName}.Infrastructure.csproj",
            GetInfrastructureCsproj());
        
        // API project
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/{_projectName}.Api.csproj",
            GetApiCsproj());
    }
    
    private void CreateSolutionFile()
    {
        var slnContent = $@"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Domain"", ""{_projectName}.Domain\{_projectName}.Domain.csproj"", ""{{GUID-DOMAIN}}""
EndProject
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Application"", ""{_projectName}.Application\{_projectName}.Application.csproj"", ""{{GUID-APPLICATION}}""
EndProject
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Infrastructure"", ""{_projectName}.Infrastructure\{_projectName}.Infrastructure.csproj"", ""{{GUID-INFRASTRUCTURE}}""
EndProject
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Api"", ""{_projectName}.Api\{_projectName}.Api.csproj"", ""{{GUID-API}}""
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Debug|Any CPU = Debug|Any CPU
        Release|Any CPU = Release|Any CPU
    EndGlobalSection
EndGlobal";
        
        File.WriteAllText($"{_solutionPath}/{_projectName}.sln", slnContent);
    }
    
    private void CreateSourceFiles()
    {
        // Domain Layer
        CreateDomainFiles();
        
        // Application Layer
        CreateApplicationFiles();
        
        // Infrastructure Layer
        CreateInfrastructureFiles();
        
        // API Layer
        CreateApiFiles();
    }
    
    private void CreateDomainFiles()
    {
        // BaseEntity
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Domain/Common/BaseEntity.cs",
            GetBaseEntityCode());
        
        // Product Entity
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Domain/Entities/Product.cs",
            GetProductEntityCode());
    }
    
    private void CreateApplicationFiles()
    {
        // Common Interfaces
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Interfaces/IRepository.cs",
            GetIRepositoryCode());
        
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Interfaces/IApplicationDbContext.cs",
            GetIApplicationDbContextCode());
        
        // MediatR Pipeline Behaviours
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Behaviours/ValidationBehaviour.cs",
            GetValidationBehaviourCode());
        
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Common/Behaviours/LoggingBehaviour.cs",
            GetLoggingBehaviourCode());
        
        // DependencyInjection
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/DependencyInjection.cs",
            GetApplicationDICode());
        
        // Commands with Validators
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/CreateProduct/CreateProductCommand.cs",
            GetCreateProductCommandCode());
        
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/CreateProduct/CreateProductCommandValidator.cs",
            GetCreateProductValidatorCode());
        
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/UpdateProduct/UpdateProductCommand.cs",
            GetUpdateProductCommandCode());
        
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/UpdateProduct/UpdateProductCommandValidator.cs",
            GetUpdateProductValidatorCode());
        
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Commands/DeleteProduct/DeleteProductCommand.cs",
            GetDeleteProductCommandCode());
        
        // Queries
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Queries/GetProductById/GetProductByIdQuery.cs",
            GetGetProductByIdQueryCode());
        
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Application/Products/Queries/GetAllProducts/GetAllProductsQuery.cs",
            GetGetAllProductsQueryCode());
    }
    
    private void CreateInfrastructureFiles()
    {
        // AppDbContext
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/Data/AppDbContext.cs",
            GetAppDbContextCode());
        
        // Repository
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/Repositories/Repository.cs",
            GetRepositoryCode());
        
        // DependencyInjection
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Infrastructure/DependencyInjection.cs",
            GetInfrastructureDICode());
    }
    
    private void CreateApiFiles()
    {
        // Program.cs
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/Program.cs",
            GetProgramCode());
        
        // ProductsController
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/Controllers/ProductsController.cs",
            GetProductsControllerCode());
        
        // appsettings.json
        File.WriteAllText(
            $"{_solutionPath}/{_projectName}.Api/appsettings.json",
            GetAppSettingsCode());
    }
    
    // Project file templates
    private string GetDomainCsproj() => @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>";
    
    private string GetApplicationCsproj() => $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""MediatR"" Version=""12.2.0"" />
    <PackageReference Include=""FluentValidation"" Version=""11.9.0"" />
    <PackageReference Include=""FluentValidation.DependencyInjectionExtensions"" Version=""11.9.0"" />
    <PackageReference Include=""Microsoft.Extensions.Logging.Abstractions"" Version=""8.0.0"" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include=""..\{_projectName}.Domain\{_projectName}.Domain.csproj"" />
  </ItemGroup>
</Project>";
    
    private string GetInfrastructureCsproj() => $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""Microsoft.EntityFrameworkCore"" Version=""8.0.0"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.InMemory"" Version=""8.0.0"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.SqlServer"" Version=""8.0.0"" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include=""..\{_projectName}.Domain\{_projectName}.Domain.csproj"" />
    <ProjectReference Include=""..\{_projectName}.Application\{_projectName}.Application.csproj"" />
  </ItemGroup>
</Project>";
    
    private string GetApiCsproj() => $@"<Project Sdk=""Microsoft.NET.Sdk.Web"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""Microsoft.AspNetCore.OpenApi"" Version=""8.0.0"" />
    <PackageReference Include=""Swashbuckle.AspNetCore"" Version=""6.5.0"" />
    <PackageReference Include=""MediatR"" Version=""12.2.0"" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include=""..\{_projectName}.Infrastructure\{_projectName}.Infrastructure.csproj"" />
  </ItemGroup>
</Project>";
    
    // Source code templates
    private string GetBaseEntityCode() => $@"namespace {_projectName}.Domain.Common;

public abstract class BaseEntity
{{
    public Guid Id {{ get; set; }} = Guid.NewGuid();
    public DateTime CreatedAt {{ get; set; }} = DateTime.UtcNow;
    public DateTime? UpdatedAt {{ get; set; }}
}}";
    
    private string GetProductEntityCode() => $@"using {_projectName}.Domain.Common;

namespace {_projectName}.Domain.Entities;

public class Product : BaseEntity
{{
    public string Name {{ get; set; }} = string.Empty;
    public string Description {{ get; set; }} = string.Empty;
    public decimal Price {{ get; set; }}
    public int Stock {{ get; set; }}
}}";
    
    private string GetIRepositoryCode() => $@"namespace {_projectName}.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}}";

    private string GetIApplicationDbContextCode() => $@"using Microsoft.EntityFrameworkCore;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Common.Interfaces;

public interface IApplicationDbContext
{{
    DbSet<Product> Products {{ get; }}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}}";

    private string GetValidationBehaviourCode() => $@"using FluentValidation;
using MediatR;

namespace {_projectName}.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {{
        _validators = validators;
    }}

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {{
        if (_validators.Any())
        {{
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }}

        return await next();
    }}
}}";

    private string GetLoggingBehaviourCode() => $@"using MediatR;
using Microsoft.Extensions.Logging;

namespace {_projectName}.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {{
        _logger = logger;
    }}

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {{
        var requestName = typeof(TRequest).Name;
        
        _logger.LogInformation(""Handling {{RequestName}}"", requestName);

        var response = await next();

        _logger.LogInformation(""Handled {{RequestName}}"", requestName);

        return response;
    }}
}}";

    private string GetApplicationDICode() => $@"using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using {_projectName}.Application.Common.Behaviours;

namespace {_projectName}.Application;

public static class DependencyInjection
{{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {{
        services.AddMediatR(cfg => {{
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        }});

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }}
}}";
    
    private string GetCreateProductCommandCode() => $@"using MediatR;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<Guid>
{{
    public string Name {{ get; init; }} = string.Empty;
    public string Description {{ get; init; }} = string.Empty;
    public decimal Price {{ get; init; }}
    public int Stock {{ get; init; }}
}}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{{
    private readonly IRepository<Product> _repository;

    public CreateProductCommandHandler(IRepository<Product> repository)
    {{
        _repository = repository;
    }}

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {{
        var product = new Product
        {{
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock
        }};

        var created = await _repository.AddAsync(product, cancellationToken);
        return created.Id;
    }}
}}";

    private string GetCreateProductValidatorCode() => $@"using FluentValidation;

namespace {_projectName}.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{{
    public CreateProductCommandValidator()
    {{
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(""Product name is required."")
            .MaximumLength(200).WithMessage(""Product name must not exceed 200 characters."");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(""Product description is required."")
            .MaximumLength(1000).WithMessage(""Product description must not exceed 1000 characters."");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(""Product price must be greater than 0."");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage(""Product stock cannot be negative."");
    }}
}}";
    
    private string GetUpdateProductCommandCode() => $@"using MediatR;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest
{{
    public Guid Id {{ get; init; }}
    public string Name {{ get; init; }} = string.Empty;
    public string Description {{ get; init; }} = string.Empty;
    public decimal Price {{ get; init; }}
    public int Stock {{ get; init; }}
}}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{{
    private readonly IRepository<Product> _repository;

    public UpdateProductCommandHandler(IRepository<Product> repository)
    {{
        _repository = repository;
    }}

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {{
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product == null)
            throw new Exception($""Product with ID {{request.Id}} not found."");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product, cancellationToken);
    }}
}}";

    private string GetUpdateProductValidatorCode() => $@"using FluentValidation;

namespace {_projectName}.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{{
    public UpdateProductCommandValidator()
    {{
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(""Product ID is required."");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(""Product name is required."")
            .MaximumLength(200).WithMessage(""Product name must not exceed 200 characters."");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(""Product description is required."")
            .MaximumLength(1000).WithMessage(""Product description must not exceed 1000 characters."");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(""Product price must be greater than 0."");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage(""Product stock cannot be negative."");
    }}
}}";
    
    private string GetDeleteProductCommandCode() => $@"using MediatR;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{{
    private readonly IRepository<Product> _repository;

    public DeleteProductCommandHandler(IRepository<Product> repository)
    {{
        _repository = repository;
    }}

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {{
        await _repository.DeleteAsync(request.Id, cancellationToken);
    }}
}}";
    
    private string GetGetProductByIdQueryCode() => $@"using MediatR;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<Product?>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
{{
    private readonly IRepository<Product> _repository;

    public GetProductByIdQueryHandler(IRepository<Product> repository)
    {{
        _repository = repository;
    }}

    public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {{
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }}
}}";
    
    private string GetGetAllProductsQueryCode() => $@"using MediatR;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery : IRequest<IEnumerable<Product>>;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{{
    private readonly IRepository<Product> _repository;

    public GetAllProductsQueryHandler(IRepository<Product> repository)
    {{
        _repository = repository;
    }}

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {{
        return await _repository.GetAllAsync(cancellationToken);
    }}
}}";
    
    private string GetAppDbContextCode() => $@"using Microsoft.EntityFrameworkCore;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Infrastructure.Data;

public class AppDbContext : DbContext, IApplicationDbContext
{{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {{ }}
    
    public DbSet<Product> Products {{ get; set; }}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {{
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>(entity =>
        {{
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        }});
    }}
}}";
    
    private string GetRepositoryCode() => $@"using Microsoft.EntityFrameworkCore;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Infrastructure.Data;

namespace {_projectName}.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    
    public Repository(AppDbContext context)
    {{
        _context = context;
        _dbSet = context.Set<T>();
    }}
    
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {{
        return await _dbSet.FindAsync(new object[] {{ id }}, cancellationToken);
    }}
    
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {{
        return await _dbSet.ToListAsync(cancellationToken);
    }}
    
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {{
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }}
    
    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {{
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }}
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {{
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {{
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }}
    }}
}}";
    
    private string GetInfrastructureDICode() => $@"using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Infrastructure.Data;
using {_projectName}.Infrastructure.Repositories;

namespace {_projectName}.Infrastructure;

public static class DependencyInjection
{{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {{
        var connectionString = configuration.GetConnectionString(""DefaultConnection"");
        
        // Use InMemory for demo, switch to SQL Server for production
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(""CleanArchDb""));
        
        // For SQL Server, uncomment:
        // services.AddDbContext<AppDbContext>(options =>
        //     options.UseSqlServer(connectionString));
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }}
}}";
    
    private string GetProgramCode() => $@"using {_projectName}.Application;
using {_projectName}.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Application layer (MediatR + FluentValidation)
builder.Services.AddApplication();

// Add Infrastructure layer (EF Core + Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{{
    app.UseSwagger();
    app.UseSwaggerUI();
}}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();";
    
    private string GetProductsControllerCode() => $@"using MediatR;
using Microsoft.AspNetCore.Mvc;
using {_projectName}.Application.Products.Commands.CreateProduct;
using {_projectName}.Application.Products.Commands.UpdateProduct;
using {_projectName}.Application.Products.Commands.DeleteProduct;
using {_projectName}.Application.Products.Queries.GetProductById;
using {_projectName}.Application.Products.Queries.GetAllProducts;

namespace {_projectName}.Api.Controllers;

[ApiController]
[Route(""api/[controller]"")]
public class ProductsController : ControllerBase
{{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {{
        _mediator = mediator;
    }}

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {{
        var query = new GetAllProductsQuery();
        var products = await _mediator.Send(query, cancellationToken);
        return Ok(products);
    }}

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet(""{{id}}"")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {{
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query, cancellationToken);
        return product == null ? NotFound() : Ok(product);
    }}

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {{
        var productId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new {{ id = productId }}, productId);
    }}

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut(""{{id}}"")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {{
        if (id != command.Id) 
            return BadRequest(""ID mismatch"");
            
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }}

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete(""{{id}}"")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {{
        var command = new DeleteProductCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }}
}}";
    
        private string GetAppSettingsCode() => $"{{\n  \"ConnectionStrings\": {{\n    \"DefaultConnection\": \"Server=(localdb)\\\\mssqllocaldb;Database={_projectName};Trusted_Connection=True;\"\n  }},\n  \"Logging\": {{\n    \"LogLevel\": {{\n      \"Default\": \"Information\",\n      \"Microsoft\": \"Warning\",\n      \"Microsoft.Hosting.Lifetime\": \"Information\"\n    }}\n  }},\n  \"AllowedHosts\": \"*\"\n}}";

}
    