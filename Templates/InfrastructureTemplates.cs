namespace CleanForge.Templates;

public class InfrastructureTemplates
{
    private readonly string _projectName;

    public InfrastructureTemplates(string projectName)
    {
        _projectName = projectName;
    }

    public string GetAppDbContextCode() => $@"using Microsoft.EntityFrameworkCore;
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

    public string GetRepositoryCode() => $@"using Microsoft.EntityFrameworkCore;
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

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {{
        var totalCount = await _dbSet.CountAsync(cancellationToken);
        var items = await _dbSet
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }}
}}";

    public string GetInfrastructureDICode() => $@"using Microsoft.EntityFrameworkCore;
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
}
