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
        DisposeSubCommands(rootCommand);
        return result;
    }
    private static void DisposeSubCommands(this Command command)
    {
        if (command.Handler is IDisposable disposableHandler)
            disposableHandler.Dispose();
        foreach (var subCommand in command.Subcommands)
        {
            DisposeSubCommands(subCommand);
        }
    }
}
