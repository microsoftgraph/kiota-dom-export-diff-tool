using Microsoft.Extensions.Logging;
using Moq;

namespace Microsoft.KiotaDomExportDiffTool.Tests;
public sealed class DomDiffExplanationServiceTests : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private static async Task<string> LoadTestFile(CancellationToken cancellationToken)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "resources", "0001-temp-second-export.patch");
        return await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
    }
    [Fact]
    public async Task TestName()
    {
        var testContent = await LoadTestFile(_cancellationTokenSource.Token);
        var logger = Mock.Of<ILogger>();
        var service = new DomDiffExplanationService(logger);
        var result = service.CleanupDiffHeaderAndFooter(testContent);
        Assert.DoesNotContain("+++", result);
        Assert.DoesNotContain(".windows.", result);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        GC.SuppressFinalize(this);
    }
}
