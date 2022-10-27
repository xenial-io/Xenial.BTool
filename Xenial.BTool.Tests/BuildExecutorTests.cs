using System.IO.Abstractions;

using Xenial.BTool;
using Xenial.BTool.Tests;

namespace Xenial.Tests.BTool;

public sealed class BuildExecutorTests
{
    [IgnoreUnixFact]
    public void Run()
    {
        var executor = new BuildExecutor(new BuildLocator(new FileSystem()));

        executor.Run(Array.Empty<string>());
    }

    [IgnoreUnixFact]
    public Task RunAsync()
    {
        var executor = new BuildExecutor(new BuildLocator(new FileSystem()));

        return executor.RunAsync(Array.Empty<string>());
    }
}
