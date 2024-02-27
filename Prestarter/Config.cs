using System.Linq;
using System.Reflection;
using Prestarter.Configuration;
using Prestarter.Downloaders;

namespace Prestarter
{
    internal class Config
    {
        public static string Title => $"{Project} Launcher Prestarter v{Version}";
        public static string Project { get; } = "Minecraft";
        public static string Version { get; } = "0.0.0-dev";

        public static string LauncherDownloadUrl { get; } = "https://demo.gravit-support.ru/updates/Launcher.jar";
        
        public static bool DownloadQuestionEnabled { get; } = true;
        
        public static bool UseGlobalJava { get; } = true;
        public static IRuntimeDownloader JavaDownloader { get; } = new CompositeDownloader(new AdoptiumJavaDownloader(), new OpenJFXDownloader());

        static Config()
        {
            foreach (var attribute in Assembly.GetExecutingAssembly().GetCustomAttributes())
            {
                switch (attribute)
                {
                    case AssemblyTitleAttribute titleAttribute:
                    {
                        Project = titleAttribute.Title;
                        break;
                    }
                    case AssemblyInformationalVersionAttribute versionAttribute:
                    {
                        Version = versionAttribute.InformationalVersion;
                        break;
                    }
                    case PrestarterConfigurationAttribute prestarterConfigurationAttribute:
                    {
                        LauncherDownloadUrl = prestarterConfigurationAttribute.LauncherUrl;
                        break;
                    }
                    case JavaDownloaderConfigurationAttribute downloaderConfigurationAttribute:
                    {
                        DownloadQuestionEnabled = downloaderConfigurationAttribute.DownloadConfirmation;
                        UseGlobalJava = downloaderConfigurationAttribute.UseGlobalJava;

                        if (downloaderConfigurationAttribute.JavaDownloaders.Length == 1) 
                        {
                            JavaDownloader = RuntimeDownloaderFactory.GetById(downloaderConfigurationAttribute.JavaDownloaders[0]);
                            break;
                        }

                        JavaDownloader = new CompositeDownloader(downloaderConfigurationAttribute.JavaDownloaders
                                .Select(RuntimeDownloaderFactory.GetById)
                                .ToArray());
                        break;
                    }
                }
            }
        }
    }
}
