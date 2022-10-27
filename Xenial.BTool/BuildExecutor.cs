using static SimpleExec.Command;
namespace Xenial.BTool;

public sealed record BuildExecutor(BuildLocator BuildLocator)
{
    public Task RunAsync(string[] args, string? cd = null)
    {
        var located = BuildLocator.LocateBuildScript(cd);
        if (located is not null)
        {
            cd = BuildLocator.FileSystem.Path.GetDirectoryName(located);
            return SimpleExec.Command.RunAsync(located, workingDirectory: cd, args: string.Join(" ", args));
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Cannot locate build script");
        Console.ResetColor();

        return Task.CompletedTask;
    }

    public void Run(string[] args, string? cd = null)
    {
        var located = BuildLocator.LocateBuildScript(cd);
        if (located is not null)
        {
            cd = BuildLocator.FileSystem.Path.GetDirectoryName(located);
            SimpleExec.Command.Run(located, workingDirectory: cd, args: string.Join(" ", args));
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Cannot locate build script");
        Console.ResetColor();
    }
}
