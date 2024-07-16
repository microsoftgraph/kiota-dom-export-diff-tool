using System.CommandLine;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Microsoft.KiotaDomExportDiffTool.Tests")]

namespace Microsoft.KiotaDomExportDiffTool;
static class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = DiffToolHost.GetRootCommand();
        var result = await rootCommand.InvokeAsync(args);
        // DisposeSubCommands(rootCommand);
        return result;
    }
}
