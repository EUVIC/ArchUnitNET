using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitPresentation.Tests;

public class ImplementationModuleTests
{
    private const string ServicesNamespace = @"^ArchUnitPresentation\..+\.Impl\.Services"; 
    private const string RepositoriesNamespace = @"^ArchUnitPresentation\..+\.Impl\.Repositories";

    private static readonly Architecture Architecture =
        AssembliesLoader.BuildArchitectureFromAssembliesContaining(".Impl");
    
    private readonly IObjectProvider<IType> typesInImplModule = Types().That()
        .DoNotHaveName("ServiceRegistrations").As("All types except Service Registrations Extensions");

    [Fact]
    public void AllTypesInImplementationModuleShouldBeInternal()
    {
        Types().That().Are(typesInImplModule).Should()
            .BeInternal()
            .Check(Architecture);
    }

    [Fact]
    public void AllServiceClassesShouldResideInServicesNamespace()
    {
        Classes().That().HaveNameEndingWith("Service").Should()
            .ResideInNamespace(ServicesNamespace, true)
            .Check(Architecture);
    }

    [Fact]
    public void AllServicesShouldBeSealed()
    {
        Classes().That()
            .ResideInNamespace(ServicesNamespace, true).Should()
            .BeSealed()
            .Check(Architecture);
    }
    
    [Fact]
    public void AllServicesShouldImplementTheirRespectiveInterfaces()
    {
        Classes().That()
            .ResideInNamespace(ServicesNamespace, true).Should()
            .FollowCustomCondition(
                service =>
                {
                    var serviceFullName = service.FullName;
                    var expectedInterfaceFullName = serviceFullName
                        .Replace(".Impl.", ".Contracts.")
                        .Replace(service.Name, $"I{service.Name}");
                    return service.ImplementsInterface(expectedInterfaceFullName);
                },
                "Services should implemented their respective interfaces", 
                "Service does not implement proper interface")
            .Check(Architecture);
    }
    
    [Fact]
    public void AllRepositoriesShouldBeSealed()
    {
        Classes().That()
            .ResideInNamespace(RepositoriesNamespace, true).Should()
            .BeSealed()
            .Check(Architecture);
    }
    
    [Fact]
    public void AllRepositoryClassesShouldResideInRepositoriesNamespace()
    {
        Classes().That().HaveNameEndingWith("Repository").Should()
            .ResideInNamespace(RepositoriesNamespace, true)
            .Check(Architecture);
    }
    
    [Fact]
    public void RepositoryClassCannotDependOnServices()
    {
        var serviceLayer = Classes().That()
            .ResideInNamespace(ServicesNamespace, true);
        
        Classes().That().ResideInNamespace(RepositoriesNamespace, true).Should()
            .NotDependOnAny(serviceLayer)
            .Check(Architecture);
    }
}