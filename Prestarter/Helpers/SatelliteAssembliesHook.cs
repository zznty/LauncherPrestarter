using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Prestarter.Helpers;

public static class SatelliteAssembliesHook
{
    private static readonly ConcurrentDictionary<CultureInfo, Assembly> SatelliteCache = [];
    
    public static void Install()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
    }

    private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
    {
        var name = new AssemblyName(args.Name);
        
        return name.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase)
            ? LocateSatelliteAssembly(name, name.CultureInfo)
            : null;
    }

    private static Assembly LocateSatelliteAssembly(AssemblyName requestedName, CultureInfo cultureInfo)
    {
        if (SatelliteCache.TryGetValue(cultureInfo, out var assembly))
            return assembly;
        
        using var stream =
            typeof(SatelliteAssembliesHook).Assembly.GetManifestResourceStream($"{requestedName.CultureName}/{requestedName.Name}.dll");
        
        if (stream != null)
        {
            using var memStream = new MemoryStream();
            stream.CopyTo(memStream);

            assembly = Assembly.Load(memStream.ToArray());
        }
        else
        {
            cultureInfo = cultureInfo.Parent;
            if (cultureInfo.Equals(CultureInfo.InvariantCulture))
                return null;
            
            assembly = LocateSatelliteAssembly(requestedName, cultureInfo);
        }

        if (assembly != null)
            SatelliteCache.TryAdd(cultureInfo, assembly);
        
        return assembly;
    } 
}