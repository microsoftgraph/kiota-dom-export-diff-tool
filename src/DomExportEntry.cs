namespace Microsoft.KiotaDomExportDiffTool;

public interface IDomExportEntry
{
    string Serialize();
    string ExplainToHuman();
}
