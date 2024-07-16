using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Logging;
namespace Microsoft.KiotaDomExportDiffTool;
public static partial class DiffToolHost
{
    public static RootCommand GetRootCommand()
    {
        var rootCommand = new RootCommand();
        rootCommand.AddCommand(ExplainDiff());
        return rootCommand;
    }
    private static Command ExplainDiff()
    {
        var diffOption = new Option<string>(["--diff"], "The diff to explain")
        {
            IsRequired = false
        };
        var pathOption = new Option<string>(["--path"], "The path to the patch file to explain")
        {
            IsRequired = false
        };
        var failOnRemoval = new Option<bool>(["--fail-on-removal", "-f"], "Fails if a removal is detected")
        {
            IsRequired = false,
        };
        var command = new Command("explain", "Parses the result of a git diff and returns an explanation of the changes")
        {
            diffOption,
            pathOption,
            failOnRemoval
        };
        var logLevelOption = GetLogLevelOption();
        command.Handler = new ExplainDiffHandler
        {
            Diff = diffOption,
            Path = pathOption,
            FailOnRemoval = failOnRemoval,
            LogLevelOption = logLevelOption
        };
        return command;
    }
    internal static Option<LogLevel> GetLogLevelOption()
    {
#if DEBUG
        static LogLevel DefaultLogLevel() => LogLevel.Debug;
#else
        static LogLevel DefaultLogLevel() => LogLevel.Warning;
#endif
        var logLevelOption = new Option<LogLevel>("--log-level", DefaultLogLevel, "The log level to use when logging messages to the main output.");
        logLevelOption.AddAlias("--ll");
        AddEnumValidator(logLevelOption, "log level");
        return logLevelOption;
    }
    private static void AddEnumValidator<T>(Option<T> option, string parameterName) where T : struct, Enum
    {
        option.AddValidator(input =>
        {
            ValidateEnumValue<T>(input, parameterName);
        });
    }
    private static void ValidateEnumValue<T>(OptionResult input, string parameterName) where T : struct, Enum
    {
        if (input.Tokens.Any() && !Enum.TryParse<T>(input.Tokens[0].Value, true, out var _))
        {
            var validOptionsList = Enum.GetValues<T>().Select(static x => x.ToString()).Aggregate(static (x, y) => x + ", " + y);
            input.ErrorMessage = $"{input.Tokens[0].Value} is not a supported generation {parameterName}, supported values are {validOptionsList}";
        }
    }
}
