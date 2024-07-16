
using Microsoft.Extensions.Logging;

namespace Microsoft.KiotaDomExportDiffTool;

public class DomDiffExplanationService
{
    private readonly ILogger Logger;
    private readonly bool FailOnRemoval;
    public DomDiffExplanationService(ILogger logger, bool failOnRemoval = false)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
        FailOnRemoval = failOnRemoval;
    }
    internal void ExplainDiff(string diffValue)
    {
        var diffBody = CleanupDiffHeaderAndFooter(diffValue);
        throw new NotImplementedException();
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
}
