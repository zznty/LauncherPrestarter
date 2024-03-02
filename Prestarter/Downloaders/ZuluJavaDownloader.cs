﻿using Prestarter.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Prestarter.Downloaders;

internal class ZuluJavaDownloader : IRuntimeDownloader
{
    private const string x64Name = "Zulu JRE Full (x86_64)";
    private const string x86Name = "Zulu JRE Full (x86)";

    public void Download(string javaPath, IUIReporter reporter)
    {
        var bitness = Environment.Is64BitOperatingSystem ? "64" : "32";
        reporter.SetStatus("Запрос к Azul API");
        reporter.SetProgressBarState(ProgressBarState.Marqee);
        var url = $"https://api.azul.com/zulu/download/community/v1.0/bundles/latest/?jdk_version=21&bundle_type=jre&features=headfull&javafx=true&ext=zip&os=windows&arch=x86&hw_bitness={bitness}";
        var result = Prestarter.SharedHttpClient.GetAsync(url).Result;
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception($"Произошла ошибка во время инициализации: сервер вернул код {result.StatusCode}");
        }

        reporter.SetStatus("Обработка ответа от Azul API");
        var azulApiResult = result.Content.ReadAsStringAsync().Result;

        var parsed = new JsonParser().Parse(azulApiResult);
        var downloadUrl = (parsed as Dictionary<string, object>)?["url"] as string;
        if (downloadUrl == null)
        {
            throw new Exception("Произошла ошибка во время обработки ответа");
        }

        var zipPath = Path.Combine(javaPath, "java.zip");
        reporter.SetStatus("Скачивание Zulu Full JRE");
        reporter.SetProgress(0);
        reporter.SetProgressBarState(ProgressBarState.Progress);
        using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            Prestarter.SharedHttpClient.Download(downloadUrl, file, reporter.SetProgress);
        }
        reporter.SetProgressBarState(ProgressBarState.Marqee);
        if (File.Exists(javaPath))
        {
            reporter.SetStatus("Удаление старой Java");
            Directory.Delete(javaPath, true);
        }
        reporter.SetStatus("Распаковка");
        Directory.CreateDirectory(javaPath);
        DownloaderHelper.UnpackZip(zipPath, javaPath, true);
        File.Delete(zipPath);
    }

    public string GetName() => Environment.Is64BitOperatingSystem ? x64Name : x86Name;

    public string GetDirectoryPrefix() => "zulu";
}
