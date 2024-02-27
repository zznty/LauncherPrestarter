using System;

namespace Prestarter.Configuration;

[AttributeUsage(AttributeTargets.Assembly)]
public class PrestarterConfigurationAttribute(string launcherUrl) : Attribute
{
    public string LauncherUrl { get; } = launcherUrl;
}
