using System.CommandLine;

namespace Microsoft.KiotaDomExportDiffTool.Tests;

public class HandlerTests
{
    [Fact]
    public async Task FailsOnNoArguments()
    {
        var rootCommand = DiffToolHost.GetRootCommand();
        var result = await rootCommand.InvokeAsync([]);
        Assert.Equal(1, result);
    }
    [Fact]
    public async Task FailsOnBothArguments()
    {
        var rootCommand = DiffToolHost.GetRootCommand();
        var result = await rootCommand.InvokeAsync(["--diff", "diff", "--path", "path"]);
        Assert.Equal(1, result);
    }
}
