﻿using Prestarter.Helpers;
using System;
using System.IO;
using Prestarter.Properties;

namespace Prestarter.Downloaders
{
    internal class OpenJFXDownloader : IRuntimeDownloader
    {
        private const string x64Name = "OpenJFX 21 (x86_64)";
        private const string x86Name = "OpenJFX 17 (x86)";

        private const string x64Url = "https://download2.gluonhq.com/openjfx/22/openjfx-22_windows-x64_bin-sdk.zip";
        private const string x86Url = "https://download2.gluonhq.com/openjfx/17.0.8/openjfx-17.0.8_windows-x86_bin-sdk.zip";

        public void Download(string javaPath, IUIReporter reporter)
        {
            var url = Environment.Is64BitOperatingSystem ? x64Url : x86Url;
            var name = GetName();
            string zipPath = Path.Combine(javaPath, "openjfx.zip");
            reporter.SetStatus(string.Format(Resources.JavaDownloader_Downloading, name));
            reporter.SetProgress(0);
            reporter.SetProgressBarState(ProgressBarState.Progress);
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Prestarter.SharedHttpClient.Download(url, file, reporter.SetProgress);
            }
            reporter.SetProgressBarState(ProgressBarState.Marqee);
            reporter.SetStatus(string.Format(Resources.JavaDownloader_Unpacking, name));
            DownloaderHelper.UnpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
        }

        public string GetName() => Environment.Is64BitOperatingSystem ? x64Name : x86Name;

        public string GetDirectoryPrefix() => "openjfx";
    }
}
