using System.CommandLine;

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
