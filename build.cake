#addin Cake.Coveralls
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=coveralls.net"

var target = Argument("target", "Default");

var coverallsToken = EnvironmentVariable("COVERALLS_REPO_TOKEN");
var hasCoveralls = !string.IsNullOrEmpty(coverallsToken);

Task("Clean")
    .Does(() => {
        EnsureDirectoryExists("./artifacts");
        CleanDirectory("./artifacts");
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => {
        DotNetCoreRestore();
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild("./src/**/project.json");
        DotNetCoreBuild("./test/**/project.json");
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        OpenCover(tool => {
                tool.DotNetCoreTest("./test/CsBenc.Tests");
            },
            new FilePath("./artifacts/result.xml"),
            new OpenCoverSettings());

        DotNetCoreTest("./test/CsBenc.Check");
    });

Task("Report")
    .IsDependentOn("Test")
    .WithCriteria(() => BuildSystem.IsLocalBuild)
    .Does(() => {
        ReportGenerator("./artifacts/result.xml", "./artifacts/CoverageReport");
    });

Task("Coveralls")
    .IsDependentOn("Test")
    .WithCriteria(() => BuildSystem.IsRunningOnAppVeyor && hasCoveralls)
    .Does(() => {
        CoverallsNet("./artifacts/result.xml", CoverallsNetReportType.OpenCover);
    });

Task("Default")
    .IsDependentOn("Coveralls")
    .IsDependentOn("Report")
    .Does(() => {
        Information("Running build");
    });

RunTarget(target);