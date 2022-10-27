using Shouldly;

using System.IO.Abstractions.TestingHelpers;
using System.Runtime.InteropServices;

namespace Xenial.BTool.Tests;

public sealed class BuildLocatorTests
{
    [Fact]
    public void LocateBuildScript()
    {
        var root = OperatingSystem.IsWindows()
            ? @"c:\demo\"
            : "/var/demo/";

        var ext = OperatingSystem.IsWindows()
            ? @".bat"
            : ".sh";

        var s = Path.DirectorySeparatorChar;

        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @$"{root}myfile.txt", new MockFileData("Testing is meh.") },
            { @$"{root}jQuery.js", new MockFileData("some js") },
            { @$"{root}sub{s}image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @$"{root}sub{s}sub{s}image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @$"{root}sub{s}sub{s}sub{s}image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @$"{root}sub{s}b{ext}", new MockFileData("")}
        });

        var locator = new BuildLocator(fileSystem);

        var path = locator.LocateBuildScript(@$"{root}sub{s}sub{s}sub");
        path.ShouldBe(@$"{root}sub{s}b{ext}");
    }

    [IgnoreUnixFact]
    public void LocateBuildScriptInvariant()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
            { @"c:\demo\jQuery.js", new MockFileData("some js") },
            { @"c:\demo\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\sub\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\B.BaT", new MockFileData("")}
        });

        var locator = new BuildLocator(fileSystem);

        var path = locator.LocateBuildScript(@"c:\demo\sub\sub\sub");
        path.ShouldBe(@"c:\demo\sub\B.BaT");
    }

    [Fact]
    public void NotExitingReturnsNull()
    {
        var root = OperatingSystem.IsWindows()
            ? @"c:\demo\"
            : "/var/demo/";

        var s = Path.DirectorySeparatorChar;

        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @$"{root}myfile.txt", new MockFileData("Testing is meh.") },
            { @$"{root}jQuery.js", new MockFileData("some js") },
            { @$"{root}sub{s}image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @$"{root}sub{s}sub{s}image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @$"{root}sub{s}sub{s}sub{s}image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
        });

        var locator = new BuildLocator(fileSystem);

        var path = locator.LocateBuildScript(@$"{root}sub{s}sub{s}sub");
        path.ShouldBeNull();
    }
}
