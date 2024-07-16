
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
        throw new NotImplementedException();
    }
}
