using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitPresentation.Tests;

public static class AttributeExtensions
{
    public static bool HasAttribute<T>(
        this IHasAttributes a)
    {
        return a.Attributes.Any<ArchUnitNET.Domain.Attribute>(attribute => attribute.FullNameMatches(typeof(T).FullName));
    }
}