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
    [Fact]
    public void ParsesDomMethod()
    {
        var value = "Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration, cancellationToken?:CancellationToken):Stream";
        var result = MethodDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder", result.ParentTypePath);
        Assert.False(result.isStatic);
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal("DeleteAsync", result.Name);
        Assert.Equal("Stream", result.ReturnTypeName);
        Assert.Equal(2, result.Parameters.Length);
        Assert.Equal("requestConfiguration", result.Parameters[0].Name);
        Assert.Equal("RequestConfiguration", result.Parameters[0].TypeName);
        Assert.True(result.Parameters[0].isOptional);
        Assert.Equal("cancellationToken", result.Parameters[1].Name);
        Assert.Equal("CancellationToken", result.Parameters[1].TypeName);
        Assert.True(result.Parameters[1].isOptional);
    }
}
