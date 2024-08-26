using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Microsoft.KiotaDomExportDiffTool;

internal class ExplainDiffHandler : ICommandHandler
{
    public required Option<string> Diff
    {
        get; init;
    }
    public required Option<string> Path
    {
        get; init;
    }
    public required Option<bool> FailOnRemoval
    {
        get; init;
    }
    public required Option<LogLevel> LogLevelOption
    {
        get; init;
    }
    public int Invoke(InvocationContext context)
    {
        throw new InvalidOperationException("this command is async only");
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        string diffValue = context.ParseResult.GetValueForOption(Diff) ?? string.Empty;
        string pathValue = context.ParseResult.GetValueForOption(Path) ?? string.Empty;
        bool failOnRemovalValue = context.ParseResult.GetValueForOption(FailOnRemoval);
        CancellationToken cancellationToken = context.BindingContext.GetService(typeof(CancellationToken)) is CancellationToken token ? token : CancellationToken.None;

        var (loggerFactory, logger) = GetLoggerAndFactory<DomDiffExplanationService>(context);
        using (loggerFactory)
        {
            if (string.IsNullOrEmpty(diffValue) && string.IsNullOrEmpty(pathValue))
            {
                logger.LogCritical("You must provide either a diff or a path to a patch file");
                return 1;
            }
            if (!string.IsNullOrEmpty(diffValue) && !string.IsNullOrEmpty(pathValue))
            {
                logger.LogCritical("You must provide either a diff or a path to a patch file, not both");
                return 1;
            }
            var diffExplanationService = new DomDiffExplanationService(logger);
            if (!string.IsNullOrEmpty(diffValue))
            {
                logger.LogDebug("Explaining diff");
            }
            else
            {
                logger.LogDebug("Explaining patch file");
                diffValue = await File.ReadAllTextAsync(pathValue, cancellationToken).ConfigureAwait(false);
            }
            var result = diffExplanationService.ExplainDiff(diffValue);
            var sb = new StringBuilder();
            foreach (var diff in result)
            {
                sb.AppendLine(diff.ToString());
            }
            var explanationResult = sb.ToString();
            Console.WriteLine(explanationResult);
            await WriteExplanationToFileAsync(explanationResult).ConfigureAwait(false);
            await WriteSummaryToGitHubOutput(result).ConfigureAwait(false);

            if (Array.Exists(result, static x => x.Kind is DifferenceKind.Removal) && failOnRemovalValue)
            {
                logger.LogCritical("A removal was detected and the process was configured to fail on removals");
                return 1;
            }
            return 0;
        }
    }
    private static async Task WriteExplanationToFileAsync(string explanationResult)
    {
        var directory = Directory.GetCurrentDirectory();
        var explanationFilePath = System.IO.Path.Combine(directory, "explanations.txt");
        await File.WriteAllTextAsync(explanationFilePath, explanationResult).ConfigureAwait(false);
    }
    private static async Task WriteSummaryToGitHubOutput(Difference[] results)
    {
        // https://docs.github.com/actions/reference/workflow-commands-for-github-actions#setting-an-output-parameter
        // ::set-output deprecated as mentioned in https://github.blog/changelog/2022-10-11-github-actions-deprecating-save-state-and-set-output-commands/
        var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT", EnvironmentVariableTarget.Process);
        var removalCount = results.Count(static x=> x.Kind is DifferenceKind.Removal);
        var additionCount = results.Count(static x => x.Kind is DifferenceKind.Addition);
        if (!string.IsNullOrWhiteSpace(githubOutputFile))
        {
            using var textWriter = new StreamWriter(githubOutputFile!, true, Encoding.UTF8);
            await textWriter.WriteLineAsync("explanations<<EOF").ConfigureAwait(false);
            await textWriter.WriteLineAsync().ConfigureAwait(false); //empty line
            await textWriter.WriteLineAsync("**This PR introduces the following changes to the Public API**").ConfigureAwait(false);
            await textWriter.WriteLineAsync().ConfigureAwait(false); //empty line
            await textWriter.WriteLineAsync("| Change Type| Count |").ConfigureAwait(false);
            await textWriter.WriteLineAsync("|--------|--------|").ConfigureAwait(false);
            await textWriter.WriteLineAsync($"| **Additions**| **{additionCount}** :white_check_mark:|").ConfigureAwait(false);
            await textWriter.WriteLineAsync($"| **Removals**| **{removalCount}** {(removalCount > 0 ? ":x:" : ":white_check_mark:")}|").ConfigureAwait(false);
            await textWriter.WriteLineAsync().ConfigureAwait(false);//end table
            if (removalCount > 0)
            {
                await textWriter.WriteLineAsync(":bangbang: **IMPORTANT** :bangbang: Please confirm if the **{removalCount}** removals are expected by checking the action logs.").ConfigureAwait(false);
            }
            await textWriter.WriteLineAsync("EOF").ConfigureAwait(false);
            await textWriter.WriteLineAsync("hasExplanations=true").ConfigureAwait(false);
        }
    }
    protected (ILoggerFactory, ILogger<T>) GetLoggerAndFactory<T>(InvocationContext context, string logFileRootPath = "")
    {
        LogLevel logLevel = context.ParseResult.GetValueForOption(LogLevelOption);
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole()
#if DEBUG
                .AddDebug()
#endif
                .SetMinimumLevel(logLevel);
        });
        var logger = loggerFactory.CreateLogger<T>();
        return (loggerFactory, logger);
    }
}
