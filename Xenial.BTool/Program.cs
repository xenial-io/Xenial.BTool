using System.IO.Abstractions;

using Xenial.BTool;

var executor = new BuildExecutor(new BuildLocator(new FileSystem()));

await executor.RunAsync(args);