using Shouldly;

using System.IO.Abstractions.TestingHelpers;
using System.Runtime.InteropServices;

namespace Xenial.BTool.Tests;

public sealed class BuildLocatorTests
{
    [Fact]
    public void LocateBuildScript()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
            { @"c:\demo\jQuery.js", new MockFileData("some js") },
            { @"c:\demo\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\sub\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\b.bat", new MockFileData("")}
        });

        var locator = new BuildLocator(fileSystem);

        var path = locator.LocateBuildScript(@"c:\demo\sub\sub\sub");
        path.ShouldBe(@"c:\demo\sub\b.bat");
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
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
            { @"c:\demo\jQuery.js", new MockFileData("some js") },
            { @"c:\demo\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
            { @"c:\demo\sub\sub\sub\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
        });

        var locator = new BuildLocator(fileSystem);

        var path = locator.LocateBuildScript(@"c:\demo\sub\sub\sub");
        path.ShouldBeNull();
    }
}

public sealed class IgnoreUnixFact : FactAttribute
{
    public IgnoreUnixFact()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Skip = "Ignore on Unix";
        }
    }
}