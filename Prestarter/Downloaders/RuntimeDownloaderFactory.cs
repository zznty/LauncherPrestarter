using System.Collections.Generic;
using System.Linq;
using Prestarter.Properties;

namespace Prestarter.Downloaders;

internal static class RuntimeDownloaderFactory
{
    private static readonly IRuntimeDownloader[] Downloaders = [
        new AdoptiumJavaDownloader(),
        new BellsoftJavaDownloader(),
        new ZuluJavaDownloader(),
        new OpenJFXDownloader()
    ];

    public static IRuntimeDownloader GetById(string id) 
    {
        return Downloaders.FirstOrDefault(x => x.GetDirectoryPrefix() == id) ?? 
            throw new KeyNotFoundException(string.Format(Resources.RuntimeDownloaderFactory_DownloaderNotFound, id));
    }
}
