using System.Linq;
using System.Reflection;
using Prestarter.Configuration;
using Prestarter.Downloaders;

namespace Prestarter
{
    internal class Config
    {
        public static Config Current { get; } = new();
        public string Title => $"{Project} Launcher Prestarter v{Version}";
        public string Project { get; }
        public string Version { get; }

        public string LauncherDownloadUrl { get; }
        
        public bool DownloadQuestionEnabled { get; }
        
        public bool UseGlobalJava { get; }
        public IRuntimeDownloader JavaDownloader { get; }

#if !DEBUG
        private Config()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes();

            var titleAttrbibute = attributes.OfType<AssemblyTitleAttribute>().FirstOrDefault();
            Project = titleAttrbibute?.Title;

            var versionAttribute = attributes.OfType<AssemblyInformationalVersionAttribute>().FirstOrDefault();
            Version = versionAttribute?.InformationalVersion;

            var prestarterConfigurationAttribute = attributes.OfType<PrestarterConfigurationAttribute>().FirstOrDefault();
            LauncherDownloadUrl = prestarterConfigurationAttribute?.LauncherUrl;

            var downloaderConfigurationAttribute = attributes.OfType<JavaDownloaderConfigurationAttribute>().FirstOrDefault();
            DownloadQuestionEnabled = downloaderConfigurationAttribute?.DownloadConfirmation ?? DownloadQuestionEnabled;
            UseGlobalJava = downloaderConfigurationAttribute?.UseGlobalJava ?? UseGlobalJava;

            if (downloaderConfigurationAttribute?.JavaDownloaders == null)
                return;

            if (downloaderConfigurationAttribute.JavaDownloaders.Length == 1)
            {
                JavaDownloader = RuntimeDownloaderFactory.GetById(downloaderConfigurationAttribute.JavaDownloaders[0]);
                return;
            }

            JavaDownloader = new CompositeDownloader(downloaderConfigurationAttribute.JavaDownloaders
                    .Select(RuntimeDownloaderFactory.GetById)
                    .ToArray());
        }
#else
        private Config()
        {
            Project = "Minecraft";
            Version = "0.0.0-dev";
            LauncherDownloadUrl = "https://demo.gravit-support.ru/updates/Launcher.jar";
            DownloadQuestionEnabled = true;
            UseGlobalJava = true;
            JavaDownloader = new CompositeDownloader(new AdoptiumJavaDownloader(), new OpenJFXDownloader());
        }
#endif
    }
}
