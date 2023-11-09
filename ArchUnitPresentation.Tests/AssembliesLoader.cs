using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Assembly = System.Reflection.Assembly;
// ReSharper disable MemberCanBePrivate.Global

namespace ArchUnitPresentation.Tests;

public static class AssembliesLoader
{
    private static readonly object LockObject = new ();
    private static readonly IDictionary<string, Assembly> ArchUnitAssemblies = new Dictionary<string, Assembly>();

    public static Architecture BuildArchitectureFromAssembliesContaining(string pattern)
    {
        return new ArchLoader().LoadAssemblies(
            GetAssembliesContaining(pattern).ToArray()
        ).Build();
    }
    
    public static IEnumerable<Assembly> GetAllAppAssemblies()
    {
        var assembliesToServe = LoadAssemblies();
        foreach (var assembly in assembliesToServe)
        {
            yield return assembly.Value;
        }
    }
    
    public static IEnumerable<Assembly> GetAssembliesContaining(string pattern)
    {
        var assembliesToServe = LoadAssemblies();
        foreach (var assembly in assembliesToServe)
        {
            if (assembly.Key.Contains(pattern))
            {
                yield return assembly.Value;
            }
        }
    }
    private static IDictionary<string, Assembly> LoadAssemblies()
    {
        lock (LockObject)
        {
            // the scan is already done - return the result
            if (ArchUnitAssemblies.Count > 0)
            {
                return ArchUnitAssemblies;
            }
            ScanAssembly(Assembly.GetExecutingAssembly(), ArchUnitAssemblies);
            return ArchUnitAssemblies;
        }
    }

    private static void ScanAssembly(Assembly assemblyToScan, IDictionary<string, Assembly> archUnitAssemblies)
    {
        var assemblies = assemblyToScan.GetReferencedAssemblies();
        foreach (var assemblyName in assemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            var assemblyFullName = assemblyName.FullName;
            if (assemblyFullName.StartsWith("ArchUnitPresentation.") && !archUnitAssemblies.ContainsKey(assemblyFullName))
            {
                archUnitAssemblies.Add(assemblyFullName, assembly);
            }

            if (assembly.GetReferencedAssemblies().Length > 0 
                && assembly.GetReferencedAssemblies().Any(a => a.FullName.StartsWith("ArchUnitPresentation.")))
            {
                ScanAssembly(assembly, archUnitAssemblies);
            }
        }
    }
}