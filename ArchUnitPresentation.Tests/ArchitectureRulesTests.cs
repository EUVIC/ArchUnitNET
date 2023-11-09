using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

namespace ArchUnitPresentation.Tests;

using static ArchUnitNET.Fluent.Slices.SliceRuleDefinition;

public class ArchitectureRulesTests
{
    
    private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
        AssembliesLoader.GetAllAppAssemblies().ToArray()
    ).Build();
    
    [Fact]
    public void ModulesShouldNotHaveCycleReferences()
    {
        Slices().Matching("ArchUnitPresentation.(*)").Should()
            .BeFreeOfCycles()
            .Check(Architecture);
    }
}