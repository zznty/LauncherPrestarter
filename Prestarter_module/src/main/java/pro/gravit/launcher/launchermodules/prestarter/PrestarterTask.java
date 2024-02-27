package pro.gravit.launcher.launchermodules.prestarter;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import pro.gravit.launchserver.LaunchServer;
import pro.gravit.launchserver.binary.tasks.LauncherBuildTask;
import pro.gravit.launchserver.binary.tasks.exe.BuildExeMainTask;
import pro.gravit.utils.Version;
import pro.gravit.utils.helper.IOHelper;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class PrestarterTask implements LauncherBuildTask, BuildExeMainTask {
    private final LaunchServer server;
    private final PrestarterModule module;

    private transient final Logger logger = LogManager.getLogger();

    public PrestarterTask(LaunchServer server, PrestarterModule module) {
        this.server = server;
        this.module = module;
    }

    @Override
    public String getName() {
        return "Prestarter.buildDotNet";
    }

    @Override
    public Path process(Path inputFile) throws IOException {
        Path projectPath = Paths.get(module.config.projectPath);
        if (!Files.exists(projectPath)) {
            throw new FileNotFoundException(projectPath.toString());
        }

        Path outputPath = server.launcherEXEBinary.nextPath(getName());
        buildDotNet(projectPath, getProperties(outputPath.getParent()));

        try (InputStream input = IOHelper.newInput(outputPath.getParent().resolve("Prestarter.exe"))) {
            try (OutputStream output = IOHelper.newOutput(outputPath)) {
                input.transferTo(output);
            }
        }

        return outputPath;
    }

    private Map<String, String> getProperties(Path outputDir) {
        var map = new HashMap<String, String>();

        map.put("AssemblyTitle", server.config.projectName);
        map.put("Version", Version.getVersion().getVersionString());
        map.put("Configuration", "Release");
        map.put("OutDir", outputDir.normalize().toString());

        map.put("LauncherUrl", server.config.netty.launcherURL);
        map.put("DownloadConfirmation", Boolean.toString(module.config.downloadConfirmation));
        map.put("UseGlobalJava", Boolean.toString(module.config.useGlobalJava));
        map.put("DownloadJava", module.config.downloadJava);

        var faviconPath = server.dir.resolve("favicon.ico");
        if (Files.exists(faviconPath)) {
            map.put("ApplicationIcon", faviconPath.normalize().toString());
        }

        return map;
    }

    private void buildDotNet(Path projectPath, Map<String, String> properties) throws IOException {
        var command = new ArrayList<>(List.of("dotnet", "build", projectPath.toString(), "-v", "q"));

        properties.forEach((key, value) -> command.add(String.format("-p:%s=%s", key, value)));

        Process process;
        try {
            process = new ProcessBuilder(command)
                    .inheritIO()
                    .start();
        } catch (IOException e) {
            throw new IOException("Failed to start dotnet process", e);
        }

        int exitCode;
        try {
            exitCode = process.waitFor();
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }

        if (exitCode != 0) {
            throw new RuntimeException(String.format("Build failed with exit code %d", exitCode));
        }
    }
}
