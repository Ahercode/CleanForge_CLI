namespace CleanForge.Templates;

public class ApplicationTemplates
{
    private readonly string _projectName;

    public ApplicationTemplates(string projectName)
    {
        _projectName = projectName;
    }

    public string GetIRepositoryCode() => $@"namespace {_projectName}.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}}";

    public string GetIApplicationDbContextCode() => $@"using Microsoft.EntityFrameworkCore;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Common.Interfaces;

public interface IApplicationDbContext
{{
    DbSet<Product> Products {{ get; }}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}}";

    public string GetPagedResultCode() => $@"namespace {_projectName}.Application.Common.Models;

public class PagedResult<T>
{{
    public IEnumerable<T> Items {{ get; set; }} = Enumerable.Empty<T>();
    public int Page {{ get; set; }}
    public int PageSize {{ get; set; }}
    public int TotalCount {{ get; set; }}
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}}";

    public string GetValidationBehaviourCode() => $@"using FluentValidation;
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

    public string GetLoggingBehaviourCode() => $@"using MediatR;
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

    public string GetApplicationDICode() => $@"using System.Reflection;
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

    public string GetCreateProductCommandCode() => $@"using MediatR;
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

    public string GetCreateProductValidatorCode() => $@"using FluentValidation;

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

    public string GetUpdateProductCommandCode() => $@"using MediatR;
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
            throw new KeyNotFoundException($""Product with ID {{request.Id}} not found."");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product, cancellationToken);
    }}
}}";

    public string GetUpdateProductValidatorCode() => $@"using FluentValidation;

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

    public string GetDeleteProductCommandCode() => $@"using MediatR;
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

    public string GetGetProductByIdQueryCode() => $@"using MediatR;
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

    public string GetGetAllProductsQueryCode() => $@"using MediatR;
using {_projectName}.Application.Common.Interfaces;
using {_projectName}.Application.Common.Models;
using {_projectName}.Domain.Entities;

namespace {_projectName}.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery : IRequest<PagedResult<Product>>
{{
    public int Page {{ get; init; }} = 1;
    public int PageSize {{ get; init; }} = 10;
}}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResult<Product>>
{{
    private readonly IRepository<Product> _repository;

    public GetAllProductsQueryHandler(IRepository<Product> repository)
    {{
        _repository = repository;
    }}

    public async Task<PagedResult<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {{
        var (items, totalCount) = await _repository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);

        return new PagedResult<Product>
        {{
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        }};
    }}
}}";
}
