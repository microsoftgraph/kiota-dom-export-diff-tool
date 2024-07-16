﻿using System.CommandLine;
using System.CommandLine.Invocation;
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
            var diffExplanationService = new DomDiffExplanationService(logger, failOnRemovalValue);
            if (!string.IsNullOrEmpty(diffValue))
            {
                logger.LogDebug("Explaining diff");
                diffExplanationService.ExplainDiff(diffValue);
            }
            else
            {
                logger.LogDebug("Explaining patch file");
                var patchFileContent = await File.ReadAllTextAsync(pathValue, cancellationToken).ConfigureAwait(false);
                diffExplanationService.ExplainDiff(patchFileContent);
            }
            await Task.Delay(0, cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
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
