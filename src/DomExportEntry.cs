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

public partial record ParameterDomEntry(string Name, string TypeName, bool isOptional) : IDomExportEntry
{
    //requestConfiguration?:RequestConfiguration
    [GeneratedRegex(@"(?<name>[\w]+)(?<isOptional>\?)?:(?<typeName>[\w]+)")]
    private static partial Regex _regex();
    public static ParameterDomEntry? Parse(string content)
    {
        var match = _regex().Match(content);
        if (match.Success)
        {
            return new ParameterDomEntry(match.Groups["name"].Value, match.Groups["typeName"].Value, match.Groups["isOptional"].Success);
        }
        return null;
    }
    public string ExplainToHuman()
    {
        return $"{Name} of type {TypeName} {(isOptional ? "is optional" : "is required")}";
    }
}
public partial record MethodDomEntry(string ParentTypePath, bool isStatic, AccessModifier AccessModifier, string Name, string ReturnTypeName, ParameterDomEntry[] Parameters) : IDomExportEntry
{
    //Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration, cancellationToken?:CancellationToken):Stream
    [GeneratedRegex(@"(?<parentTypePathName>[\w.]+)::(?:\|(?<static>static))?\|(?<access>(public|protected))\|(?<name>[\w]+)\((?<parameters>.*)?\):(?<returnTypeName>[\w]+)")]
    private static partial Regex _regex();
    public static MethodDomEntry? Parse(string content)
    {
        var match = _regex().Match(content);
        if (match.Success)
        {
            return new MethodDomEntry(match.Groups["parentTypePathName"].Value,
                                        match.Groups["static"].Success,
                                        Enum.Parse<AccessModifier>(match.Groups["access"].Value, true),
                                        match.Groups["name"].Value,
                                        match.Groups["returnTypeName"].Value,
                                        match.Groups["parameters"].Value
                                                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                            .Select(static x => ParameterDomEntry.Parse(x.Trim()))
                                                            .OfType<ParameterDomEntry>()
                                                            .ToArray());
        }
        return null;
    }
    public string ExplainToHuman()
    {
        return $"{AccessModifier} method {Name} in type {ParentTypePath} with return type {ReturnTypeName} and parameters {string.Join(", ", Parameters.Select(static x => x.ExplainToHuman()))}";
    }
}
