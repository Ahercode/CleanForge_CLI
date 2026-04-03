namespace CleanForge.Templates;

public class DomainTemplates
{
    private readonly string _projectName;

    public DomainTemplates(string projectName)
    {
        _projectName = projectName;
    }

    public string GetBaseEntityCode() => $@"namespace {_projectName}.Domain.Common;

public abstract class BaseEntity
{{
    public Guid Id {{ get; set; }} = Guid.NewGuid();
    public DateTime CreatedAt {{ get; set; }} = DateTime.UtcNow;
    public DateTime? UpdatedAt {{ get; set; }}
}}";

    public string GetProductEntityCode() => $@"using {_projectName}.Domain.Common;

namespace {_projectName}.Domain.Entities;

public class Product : BaseEntity
{{
    public string Name {{ get; set; }} = string.Empty;
    public string Description {{ get; set; }} = string.Empty;
    public decimal Price {{ get; set; }}
    public int Stock {{ get; set; }}
}}";
}
