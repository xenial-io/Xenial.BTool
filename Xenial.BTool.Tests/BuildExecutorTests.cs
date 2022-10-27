using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenial.BTool;

namespace Xenial.Tests.BTool;

public sealed class BuildExecutorTests
{
    [Fact]
    public void Run()
    {
        var executor = new BuildExecutor(new BuildLocator(new FileSystem()));

        executor.Run(Array.Empty<string>());
    }

    [Fact]
    public Task RunAsync()
    {
        var executor = new BuildExecutor(new BuildLocator(new FileSystem()));

        return executor.RunAsync(Array.Empty<string>());
    }
}
