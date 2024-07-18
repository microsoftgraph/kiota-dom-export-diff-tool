namespace Microsoft.KiotaDomExportDiffTool.Tests;

public sealed class DomExportEntryTests
{
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters::|public|Search:string", "string")]
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters::|public|Search:[string]", "[string]")]
    [Theory]
    public void ParsesDomProperty(string value, string expectedType)
    {
        var result = PropertyDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters", result.ParentTypePath);
        Assert.False(result.isStatic);
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal("Search", result.Name);
        Assert.Equal(expectedType, result.TypeName);
    }
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration, cancellationToken?:CancellationToken):Stream", "Stream")]
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration, cancellationToken?:CancellationToken):[string]", "[string]")]
    [Theory]
    public void ParsesDomMethod(string value, string expectedReturnType)
    {
        var result = MethodDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder", result.ParentTypePath);
        Assert.False(result.isStatic);
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal("DeleteAsync", result.Name);
        Assert.Equal(expectedReturnType, result.ReturnTypeName);
        Assert.Equal(2, result.Parameters.Length);
        Assert.Equal("requestConfiguration", result.Parameters[0].Name);
        Assert.Equal("RequestConfiguration", result.Parameters[0].TypeName);
        Assert.True(result.Parameters[0].isOptional);
        Assert.Equal("cancellationToken", result.Parameters[1].Name);
        Assert.Equal("CancellationToken", result.Parameters[1].TypeName);
        Assert.True(result.Parameters[1].isOptional);
    }
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration-->RequestConfiguration", "RequestConfiguration")]
    [Theory]
    public void ParsesDomInheritance(string value, string expectedParentType)
    {
        var result = InheritanceDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration", result.CurrentTypePath);
        Assert.Equal(expectedParentType, result.ParentTypePath);
    }
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration~~>RequestConfiguration", "RequestConfiguration")]
    [Theory]
    public void ParsesDomImplementation(string value, string expectedInterfaceType)
    {
        var result = ImplementationDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration", result.CurrentTypePath);
        Assert.Contains(expectedInterfaceType, result.InterfaceTypePaths, StringComparer.OrdinalIgnoreCase);
    }
    [InlineData("Graphdotnetv4.Users.usersRequestBuilder::[UserId:string]:Graphdotnetv4.Users.Item.UserItemRequestBuilder", "UserId", "string", "Graphdotnetv4.Users.Item.UserItemRequestBuilder")]
    [Theory]
    public void ParsesDomIndexer(string value, string expectedParameterName, string expectedParameterType, string expectedReturnType)
    {
        var result = IndexerDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.usersRequestBuilder", result.ParentTypePath);
        Assert.Equal(expectedParameterName, result.ParameterName);
        Assert.Equal(expectedParameterType, result.ParameterTypePath);
        Assert.Equal(expectedReturnType, result.ReturnTypePath);
    }
}
