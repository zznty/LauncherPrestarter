using System;

namespace Prestarter.Configuration;

[AttributeUsage(AttributeTargets.Assembly)]
public class JavaDownloaderConfigurationAttribute(bool downloadConfirmation, bool useGlobalJava, string javaDownloaders) : Attribute
{
    public bool DownloadConfirmation { get; } = downloadConfirmation;
    public bool UseGlobalJava { get; } = useGlobalJava;
    public string[] JavaDownloaders { get; } = javaDownloaders.Split([' '], StringSplitOptions.RemoveEmptyEntries);
}
