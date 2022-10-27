var sln = "./Xenial.BTool.sln";

Target("restore", () => RunAsync("dotnet", $"restore {sln}"));

Target("build", DependsOn("restore"), 
    () => RunAsync("dotnet", $"build {sln} --no-restore -c Release")
);

Target("test", DependsOn("build"), 
    () => RunAsync("dotnet", $"test {sln} --no-build --no-restore -c Release --logger:\"console;verbosity=normal\"")
);

Target("pack",DependsOn("test"),  
    () => RunAsync("dotnet", $"pack {sln} --no-build --no-restore -c Release")
);

Target("release", () => ReleaseHelper.Release());

Target("default", DependsOn("test"));

await RunTargetsAndExitAsync(args);