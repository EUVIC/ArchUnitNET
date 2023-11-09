using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using ArchUnitPresentation.Services.Impl.Services;
using Microsoft.Extensions.DependencyInjection;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitPresentation.Tests;

public class DependencyInjectionTests
{
    private static readonly Architecture Architecture =
        AssembliesLoader.BuildArchitectureFromAssembliesContaining(".Impl");
    
    private readonly IObjectProvider<Class> serviceRegistrations = Classes().That()
        .HaveName("ServiceRegistrations").As("Services Registrations Extensions");

    [Fact]
    public void ServiceRegistrationsShouldHaveSingleInjectionMethod()
    {
        Classes().That().Are(serviceRegistrations).Should()
            .BePublic()
            .AndShould()
            .FollowCustomCondition(
                testClass =>
                {
                    if (testClass.Members.Count(member => member.Visibility == Visibility.Public) != 1)
                    {
                        return false;
                    }
                    if (testClass.Members.FirstOrDefault(member => member.IsStatic == true 
                        && member.Name.StartsWith("Register")) is not MethodMember singleStaticMethodStartingWithRegister)
                    {
                        return false;
                    }

                    if (!singleStaticMethodStartingWithRegister.Parameters.Any())
                    {
                        return false;
                    }

                    var returnTypeMatches =
                        singleStaticMethodStartingWithRegister.ReturnType.FullName == typeof(IServiceCollection).FullName;
                    var parameterTypeMatches =
                        singleStaticMethodStartingWithRegister.Parameters.First().FullName == typeof(IServiceCollection).FullName; 

                    return returnTypeMatches && parameterTypeMatches;
                },
                "Service Registration should have single public static method for dependency registrations", 
                "Service Registrations does not meet the requirements"
                )
            .Check(Architecture);
    }
}