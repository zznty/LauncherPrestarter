using Prestarter.Helpers;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Prestarter.Properties;

namespace Prestarter
{
    internal class Prestarter
    {
        public static readonly HttpClient SharedHttpClient = new HttpClient();

        public IUIReporter reporter;

        public Prestarter(IUIReporter reporter)
        {
            this.reporter = reporter;
        }

        private static JavaStatus CheckJavaUpdateDate(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return JavaStatus.NotInstalled;
                }
                var text = File.ReadAllText(path);
                var parsed = DateTime.Parse(text, CultureInfo.InvariantCulture);
                var now = DateTime.Now;
                if (parsed.AddDays(30) < now)
                {
                    return JavaStatus.NeedUpdate;
                }
                return JavaStatus.Ok;
            }
            catch (Exception)
            {
                return JavaStatus.NotInstalled;
            }
        }

        private enum JavaStatus
        {
            Ok,
            NotInstalled, 
            NeedUpdate
        }

        private string VerifyAndDownloadJava(string basePath)
        {
            var javaPath = Config.Current.UseGlobalJava
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "GravitLauncherStore", "Java", Config.Current.JavaDownloader.GetDirectoryPrefix())
                : Path.Combine(basePath, "jre-full");
            Directory.CreateDirectory(javaPath);

            var dateFilePath = Path.Combine(javaPath, "date-updated");
            var javaStatus = CheckJavaUpdateDate(dateFilePath);
            
            if (javaStatus == JavaStatus.Ok)
            {
                return javaPath;
            }
            
            if (Config.Current.DownloadQuestionEnabled)
            {
                if (javaStatus == JavaStatus.NeedUpdate)
                {
                    var dialog = MessageBox.Show(Resources.Prestarter_JavaUpdatePrompt, Config.Current.Title, MessageBoxButtons.YesNoCancel);
                    if (dialog == DialogResult.No)
                    {
                        return javaPath;
                    }

                    if (dialog == DialogResult.Cancel)
                    {
                        return null;
                    }
                }
                else
                {
                    var dialog = MessageBox.Show(string.Format(Resources.Prestarter_JavaDownloadPrompt, Config.Current.Project, Config.Current.JavaDownloader.GetName()), 
                        Config.Current.Title, MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        return null;
                    }
                }
            }

            reporter.ShowForm();
            Config.Current.JavaDownloader.Download(javaPath, reporter);

            File.WriteAllText(dateFilePath, DateTime.Now.ToString(CultureInfo.InvariantCulture));
            return javaPath;
        }

        public void Run()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var basePath = Path.Combine(appData, Config.Current.Project);
            Directory.CreateDirectory(basePath);

            var javaPath = VerifyAndDownloadJava(basePath);
            if (javaPath == null)
            {
                return;
            }

            reporter.SetStatus(Resources.Prestarter_Step_LauncherSearch);
            var launcherPath = Path.Combine(basePath, "Launcher.jar");

            if (Config.Current.LauncherDownloadUrl == null)
            {
                launcherPath = Assembly.GetExecutingAssembly().Location;
            }
            else if (!File.Exists(launcherPath))
            {
                reporter.ShowForm();
                reporter.SetStatus(Resources.Prestarter_Step_LauncherDownload);
                reporter.SetProgress(0);
                reporter.SetProgressBarState(ProgressBarState.Progress);
                using (var file = new FileStream(launcherPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    SharedHttpClient.Download(Config.Current.LauncherDownloadUrl, file, value => reporter.SetProgress(value));
                }
                reporter.SetProgressBarState(ProgressBarState.Marqee);
            }

            reporter.SetStatus(Resources.Prestarter_Step_Launch);
            
            var args = new StringBuilder();
            foreach (var e in Environment.GetCommandLineArgs())
            {
                args.Append('\"').Append(e).Append("\" ");
            }
            
            StartJvm(javaPath, launcherPath, args.ToString());
        }

        private static void StartJvm(string javaPath, string launcherPath, string args)
        {
            var process = new Process
            {
                StartInfo = new()
                {
                    FileName = Path.Combine(javaPath, "bin", "java.exe"),
                    Arguments = $"-Dlauncher.noJavaCheck=true -jar \"{launcherPath}\" {args}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            var logBuilder = new StringBuilder();

            logBuilder.Append("Starting ").AppendLine(process.StartInfo.FileName);
            logBuilder.Append("Args ").AppendLine(process.StartInfo.Arguments);

            process.OutputDataReceived += (_, e) => logBuilder.Append("0: ").AppendLine(e.Data);
            process.ErrorDataReceived += (_, e) => logBuilder.Append("1: ").AppendLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (!process.WaitForExit(500)) return;
            
            using var log = new EventLog("Application")
            {
                Source = "Application"
            };

            log.WriteEntry(logBuilder.ToString(), EventLogEntryType.Error);

            throw new(Resources.Prestarter_StartJvm_ProcessExitedEarly);
        }
    }
}
