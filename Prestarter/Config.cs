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

#if !DEBUG
        static Config()
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
#endif
    }
}
