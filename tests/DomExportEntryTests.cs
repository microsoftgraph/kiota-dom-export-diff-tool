namespace Microsoft.KiotaDomExportDiffTool.Tests;

public sealed class DomExportEntryTests
{
    [Fact]
    public void ParsesDomProperty()
    {
        var value = "Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters::|public|Search:string";
        var result = PropertyDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters", result.ParentTypePath);
        Assert.False(result.isStatic);
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal("Search", result.Name);
        Assert.Equal("string", result.TypeName);
    }
}
