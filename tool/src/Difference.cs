namespace Microsoft.KiotaDomExportDiffTool;

public enum DifferenceKind
{
    Addition,
    Removal,
}

public record Difference(DifferenceKind Kind, IDomExportEntry Entry)
{
    public override string ToString() => $"{Kind} of {Entry.ExplainToHuman()}";
}
