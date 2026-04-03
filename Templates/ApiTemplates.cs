namespace CleanForge.Templates;

public class ApiTemplates
{
    private readonly string _projectName;

    public ApiTemplates(string projectName)
    {
        _projectName = projectName;
    }

    public string GetGlobalExceptionHandlerCode() => $@"using System.Net;
using System.Text.Json;
using FluentValidation;

namespace {_projectName}.Api.Middleware;

public class GlobalExceptionHandler
{{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {{
        _next = next;
        _logger = logger;
    }}

    public async Task InvokeAsync(HttpContext context)
    {{
        try
        {{
            await _next(context);
        }}
        catch (Exception ex)
        {{
            await HandleExceptionAsync(context, ex);
        }}
    }}

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {{
        var (statusCode, message) = exception switch
        {{
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                new ErrorResponse
                {{
                    Message = ""Validation failed."",
                    Errors = validationEx.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList()
                }}
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new ErrorResponse {{ Message = exception.Message }}
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse {{ Message = ""Unauthorized access."" }}
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse {{ Message = ""An unexpected error occurred."" }}
            )
        }};

        _logger.LogError(exception, ""Exception caught by global handler: {{Message}}"", exception.Message);

        context.Response.ContentType = ""application/json"";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {{
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }});

        await context.Response.WriteAsync(json);
    }}
}}

public class ErrorResponse
{{
    public string Message {{ get; set; }} = string.Empty;
    public List<string> Errors {{ get; set; }} = new();
}}";

    public string GetProgramCode() => $@"using {_projectName}.Api.Middleware;
using {_projectName}.Application;
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

// Global exception handling
app.UseMiddleware<GlobalExceptionHandler>();

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

    public string GetProductsControllerCode() => $@"using MediatR;
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
    /// Get all products (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {{
        var query = new GetAllProductsQuery {{ Page = page, PageSize = pageSize }};
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
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

    public string GetAppSettingsCode() => $"{{\n  \"ConnectionStrings\": {{\n    \"DefaultConnection\": \"Server=(localdb)\\\\mssqllocaldb;Database={_projectName};Trusted_Connection=True;\"\n  }},\n  \"Logging\": {{\n    \"LogLevel\": {{\n      \"Default\": \"Information\",\n      \"Microsoft\": \"Warning\",\n      \"Microsoft.Hosting.Lifetime\": \"Information\"\n    }}\n  }},\n  \"AllowedHosts\": \"*\"\n}}";

    public string GetLaunchSettingsCode() => $@"{{
  ""profiles"": {{
    ""{_projectName}.Api"": {{
      ""commandName"": ""Project"",
      ""dotnetRunMessages"": true,
      ""launchBrowser"": true,
      ""launchUrl"": ""swagger"",
      ""applicationUrl"": ""https://localhost:5001;http://localhost:5000"",
      ""environmentVariables"": {{
        ""ASPNETCORE_ENVIRONMENT"": ""Development""
      }}
    }}
  }}
}}";
}
