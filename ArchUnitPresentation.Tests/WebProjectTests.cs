using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Microsoft.AspNetCore.Mvc;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitPresentation.Tests;

public class WebProjectTests
{
    private const string ControllersNamespace = "ArchUnitPresentation.Web.Controllers";
    
    private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
        typeof(Program).Assembly
    ).Build();
    
    private readonly IObjectProvider<Class> controllers = Classes().That()
        .ResideInNamespace(ControllersNamespace).As("Controllers Namespace");

    [Fact]
    public void OnlyControllersShouldBeInControllersNamespace()
    {
        Classes().That().Are(controllers).Should()
            .HaveNameEndingWith("Controller")
            .Check(Architecture);
    }

    [Fact]
    public void ControllersShouldInheritFromControllerBase()
    {
        Classes().That().Are(controllers).Should()
            .BeAssignableTo(typeof(ControllerBase))
            .Check(Architecture);
    }
    
    [Fact]
    public void ControllersShouldBePublic()
    {
        Classes().That().Are(controllers).Should()
            .BePublic()
            .Check(Architecture);
    }
    
    [Fact]
    public void ControllersShouldHaveApiControlerAndRouteAttributes()
    {
        Classes().That().Are(controllers).Should().FollowCustomCondition(
            condition: testClass => testClass.HasAttribute<ApiControllerAttribute>() &&
                         testClass.HasAttribute<RouteAttribute>(), 
            description: "Controllers should have ApiController and Route attributes", 
            failDescription: "Controller does not have required attributes"
            )
            .Check(Architecture);
    }
    
    [Fact]
    public void ControllersShouldBeOnlyInProperControllersNamespace()
    {
        Classes().That().HaveNameEndingWith("Controller").Should()
            .ResideInNamespace(ControllersNamespace)
            .Check(Architecture);
    }
}