using System.Text.RegularExpressions;

namespace Microsoft.KiotaDomExportDiffTool;

public interface IDomExportEntry
{
    string ExplainToHuman();
}

public enum AccessModifier
{
    Public,
    Protected,
}

public partial record PropertyDomEntry(string ParentTypePath, bool isStatic, AccessModifier AccessModifier, string Name, string TypeName) : IDomExportEntry
{
    //Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters::|public|Search:string
    [GeneratedRegex(@"(?<parentTypePathName>[\w.]+)::(?:\|(?<static>static))?\|(?<access>(public|protected))\|(?<name>[\w]+):(?<typeName>[\w]+)")]
    private static partial Regex _regex();
    public static PropertyDomEntry? Parse(string content)
    {
        var match = _regex().Match(content);
        if (match.Success)
        {
            return new PropertyDomEntry(match.Groups["parentTypePathName"].Value,
                                        match.Groups["static"].Success,
                                        Enum.Parse<AccessModifier>(match.Groups["access"].Value, true),
                                        match.Groups["name"].Value,
                                        match.Groups["typeName"].Value);
        }
        return null;
    }
    public string ExplainToHuman() => $"{AccessModifier} property {Name} in type {ParentTypePath} with type {TypeName}";
}
