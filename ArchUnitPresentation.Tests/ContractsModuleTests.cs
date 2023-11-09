using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitPresentation.Tests;

public class ContractsModuleTests
{
    private static readonly Architecture Architecture =
        AssembliesLoader.BuildArchitectureFromAssembliesContaining(".Contracts");

    [Fact]
    public void AllTypesInContractsModuleShouldBePublic()
    {
        Types().That().ResideInNamespace(@"^ArchUnitPresentation\..+", true).Should()
            .BePublic()
            .Check(Architecture);
    }

    [Fact]
    public void AllDtosShouldBeInModelsNamespace()
    {
        Classes().That().HaveNameEndingWith("Dto").Should()
            .ResideInNamespace(@"^ArchUnitPresentation\..+\.Contracts\.Models", true)
            .Check(Architecture);
    }

    [Fact]
    public void AllServiceInterfacesShouldBeInServicesNamespace()
    {
        Interfaces().That().HaveNameEndingWith("Service").Should()
            .ResideInNamespace(@"^ArchUnitPresentation\..+\.Contracts\.Services", true)
            .Check(Architecture);
    }
}