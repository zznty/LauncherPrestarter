using System;
using System.Collections.Generic;
using System.Linq;

namespace Prestarter.Downloaders;

internal static class RuntimeDownloaderFactory
{
    private static readonly IRuntimeDownloader[] Downloaders = [
        new AdoptiumJavaDownloader(),
        new BellsoftJavaDownloader(),
        new OpenJFXDownloader()
    ];

    public static IRuntimeDownloader GetById(string id) 
    {
        return Downloaders.FirstOrDefault(x => x.GetDirectoryPrefix() == id) ?? 
            throw new KeyNotFoundException($"Не удалось найти {id}");
    }
}
