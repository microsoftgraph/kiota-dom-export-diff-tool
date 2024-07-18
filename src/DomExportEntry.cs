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
    [GeneratedRegex(@"(?<parentTypePathName>[\w.]+)::(?:\|(?<static>static))?\|(?<access>(public|protected))\|(?<name>[\w]+):(?<typeName>[\w\[\]]+)")]
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
    [GeneratedRegex(@"(?<name>[\w]+)(?<isOptional>\?)?:(?<typeName>[\w\[\]]+)")]
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
    [GeneratedRegex(@"(?<parentTypePathName>[\w.]+)::(?:\|(?<static>static))?\|(?<access>(public|protected))\|(?<name>[\w]+)\((?<parameters>.*)?\):(?<returnTypeName>[\w\[\]]+)")]
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

public partial record InheritanceDomEntry(string CurrentTypePath, string ParentTypePath) : IDomExportEntry
{
    //Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration-->RequestConfiguration
    [GeneratedRegex(@"(?<currentType>[\w.]+)-->(?<parentType>[\w.]+)")]
    private static partial Regex _regex();
    public static InheritanceDomEntry? Parse(string content)
    {
        var match = _regex().Match(content);
        if (match.Success)
        {
            return new InheritanceDomEntry(match.Groups["currentType"].Value, match.Groups["parentType"].Value);
        }
        return null;
    }
    public string ExplainToHuman()
    {
        return $"{CurrentTypePath} inherits from {ParentTypePath}";
    }
}

public partial record ImplementationDomEntry(string CurrentTypePath, string[] InterfaceTypePaths) : IDomExportEntry
{
    //Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration~~>RequestConfiguration
    [GeneratedRegex(@"(?<currentType>[\w.]+)~~>(?<interfaceTypes>[\w.,]+)")]
    private static partial Regex _regex();
    public static ImplementationDomEntry? Parse(string content)
    {
        var match = _regex().Match(content);
        if (match.Success)
        {
            return new ImplementationDomEntry(match.Groups["currentType"].Value, match.Groups["interfaceTypes"].Value.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
        return null;
    }
    public string ExplainToHuman()
    {
        return $"{CurrentTypePath} implements {string.Join(", ", InterfaceTypePaths.Order(StringComparer.OrdinalIgnoreCase))}";
    }
}

public partial record IndexerDomEntry(string ParentTypePath, string ParameterName, string ParameterTypePath, string ReturnTypePath) : IDomExportEntry
{
    //Graphdotnetv4.Users.usersRequestBuilder::[UserId:string]:Graphdotnetv4.Users.Item.UserItemRequestBuilder
    [GeneratedRegex(@"(?<parentTypePath>[\w.]+)::\[(?<parameterName>[\w]+):(?<parameterTypePath>[\w.]+)\]:(?<returnTypePath>[\w.]+)")]
    private static partial Regex _regex();
    public static IndexerDomEntry? Parse(string content)
    {
        var match = _regex().Match(content);
        if (match.Success)
        {
            return new IndexerDomEntry(match.Groups["parentTypePath"].Value, match.Groups["parameterName"].Value, match.Groups["parameterTypePath"].Value, match.Groups["returnTypePath"].Value);
        }
        return null;
    }
    public string ExplainToHuman()
    {
        return $"{ParentTypePath} has an indexer with parameter {ParameterName} of type {ParameterTypePath} and return type {ReturnTypePath}";
    }
}

public partial record EnumMemberDomEntry(string ParentTypePath, string MemberName, int MemberIndex) : IDomExportEntry
{
    //Graphdotnetv4.Models.bodyType::0000-text
    [GeneratedRegex(@"(?<parentTypePath>[\w.]+)::(?<memberOrder>\d{4})-(?<memberName>[\w]+)")]
    private static partial Regex _regex();
    public static EnumMemberDomEntry? Parse(string content)
    {
        var match = _regex().Match(content);
        if (match.Success && int.TryParse(match.Groups["memberOrder"].Value, out var memberIndex))
        {
            return new EnumMemberDomEntry(match.Groups["parentTypePath"].Value, match.Groups["memberName"].Value, memberIndex);
        }
        return null;
    }
    public string ExplainToHuman()
    {
        return $"{ParentTypePath} has an enum member {MemberName} with order {MemberIndex}";
    }
}
