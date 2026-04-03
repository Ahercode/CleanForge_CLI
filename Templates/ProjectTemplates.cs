namespace CleanForge.Templates;

public class ProjectTemplates
{
    private readonly string _projectName;

    public ProjectTemplates(string projectName)
    {
        _projectName = projectName;
    }

    public string GetDomainCsproj() => @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>";

    public string GetApplicationCsproj() => $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""MediatR"" Version=""12.2.0"" />
    <PackageReference Include=""FluentValidation"" Version=""11.9.0"" />
    <PackageReference Include=""FluentValidation.DependencyInjectionExtensions"" Version=""11.9.0"" />
    <PackageReference Include=""Microsoft.Extensions.Logging.Abstractions"" Version=""8.0.0"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore"" Version=""8.0.0"" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include=""..\{_projectName}.Domain\{_projectName}.Domain.csproj"" />
  </ItemGroup>
</Project>";

    public string GetInfrastructureCsproj() => $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
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

    public string GetApiCsproj() => $@"<Project Sdk=""Microsoft.NET.Sdk.Web"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
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

    public string GetSolutionFile(string domainGuid, string applicationGuid, string infrastructureGuid, string apiGuid) =>
        $@"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Domain"", ""{_projectName}.Domain\{_projectName}.Domain.csproj"", ""{domainGuid}""
EndProject
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Application"", ""{_projectName}.Application\{_projectName}.Application.csproj"", ""{applicationGuid}""
EndProject
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Infrastructure"", ""{_projectName}.Infrastructure\{_projectName}.Infrastructure.csproj"", ""{infrastructureGuid}""
EndProject
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{_projectName}.Api"", ""{_projectName}.Api\{_projectName}.Api.csproj"", ""{apiGuid}""
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Debug|Any CPU = Debug|Any CPU
        Release|Any CPU = Release|Any CPU
    EndGlobalSection
    GlobalSection(ProjectConfigurationPlatforms) = postSolution
        {domainGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {domainGuid}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {domainGuid}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {domainGuid}.Release|Any CPU.Build.0 = Release|Any CPU
        {applicationGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {applicationGuid}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {applicationGuid}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {applicationGuid}.Release|Any CPU.Build.0 = Release|Any CPU
        {infrastructureGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {infrastructureGuid}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {infrastructureGuid}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {infrastructureGuid}.Release|Any CPU.Build.0 = Release|Any CPU
        {apiGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        {apiGuid}.Debug|Any CPU.Build.0 = Debug|Any CPU
        {apiGuid}.Release|Any CPU.ActiveCfg = Release|Any CPU
        {apiGuid}.Release|Any CPU.Build.0 = Release|Any CPU
    EndGlobalSection
EndGlobal";
}
