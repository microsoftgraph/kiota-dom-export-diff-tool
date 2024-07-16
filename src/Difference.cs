namespace Microsoft.KiotaDomExportDiffTool;

public enum DifferenceKind
{
    Addition,
    Removal,
}

public record Difference(DifferenceKind Kind, IDomExportEntry Entry);
