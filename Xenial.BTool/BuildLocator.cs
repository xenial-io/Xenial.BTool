using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Xenial.BTool;

public sealed record BuildLocator(IFileSystem FileSystem)
{
    private string[] patterns => OperatingSystem.IsWindows() switch
    {
        true => new[]
        {
            "b.bat",
            "b.cmd",
            "b.ps1",
            "build.bat",
            "build.cmd",
            "build.ps1",
        },
        false => (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux()) switch
        {
            true => new[]
            {
                "b.sh",
                "build.sh",
            },
            false => Array.Empty<string>()
        }
    };

    public string? LocateBuildScript(string? cd = null)
    {
        cd = cd ?? Environment.CurrentDirectory;
        var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName(cd);

        string? Locate(IDirectoryInfo? directoryInfo)
        {
            if(directoryInfo is null)
            {
                return null;
            }
            foreach (var file in directoryInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly))
            {
                var comparisonType = OperatingSystem.IsWindows() switch
                {
                    true => StringComparison.InvariantCultureIgnoreCase,
                    false => StringComparison.InvariantCulture,
                };
                foreach (var pattern in patterns)
                {
                    if (file.Name.Equals(pattern, comparisonType))
                    {
                        return file.FullName;
                    }
                }
            }
            return Locate(directoryInfo.Parent);
        }

        return Locate(directoryInfo);
    }
}
