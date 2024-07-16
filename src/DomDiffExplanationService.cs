
using Microsoft.Extensions.Logging;

namespace Microsoft.KiotaDomExportDiffTool;

public class DomDiffExplanationService
{
    private readonly ILogger Logger;
    public DomDiffExplanationService(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
    }
    internal Difference[] ExplainDiff(string diffValue)
    {
        var diffBody = CleanupLocationLines(CleanupDiffHeaderAndFooter(diffValue));
        return SplitLines(diffBody).AsParallel().Select(ParseLine).OfType<Difference>().ToArray();
    }
    private static readonly Func<string, IDomExportEntry?>[] parsers = [
        PropertyDomEntry.Parse,
        MethodDomEntry.Parse,
    ];
    private Difference? ParseLine(string line)
    {
        var kind = line[0] switch
        {
            '+' => DifferenceKind.Addition,
            '-' => DifferenceKind.Removal,
            _ => throw new InvalidOperationException($"Unrecognized difference kind: {line}"),
        };
        foreach (var parser in parsers)
        {
            var result = parser(line);
            if (result is not null)
                return new Difference(kind, result);
        }
        Logger.LogWarning("Unrecognized line: {Line}", line);
        return null;
    }
    private const string DiffHeaderCloseTag = "+++ ";
    private const string DiffFooterOpenTag = "--";
    private const char LineReturnChar = '\n';
    internal string CleanupDiffHeaderAndFooter(string diffValue)
    {
        if (string.IsNullOrEmpty(diffValue))
        {
            return string.Empty;
        }
        var headerCloserIndex = diffValue.IndexOf(DiffHeaderCloseTag);
        if (headerCloserIndex >= 0)
        {
            headerCloserIndex = diffValue.IndexOf(LineReturnChar, headerCloserIndex);
        }
        else
        {
            headerCloserIndex = 0;
        }
        var footerOpenerIndex = diffValue.LastIndexOf(DiffFooterOpenTag);
        if (footerOpenerIndex >= 0)
        {
            footerOpenerIndex = diffValue.LastIndexOf(LineReturnChar, footerOpenerIndex);
        }
        else
        {
            footerOpenerIndex = diffValue.Length;
        }
        return diffValue[headerCloserIndex..footerOpenerIndex];
    }
    private static string[] SplitLines(string diffValue) =>
        diffValue.Split(LineReturnChar, StringSplitOptions.RemoveEmptyEntries);
    internal string CleanupLocationLines(string diffValue) =>
        string.Join(LineReturnChar, SplitLines(diffValue)
            .Where(static x => !x.StartsWith("@@", StringComparison.OrdinalIgnoreCase))
            .Select(static x => x.Trim()));
}
