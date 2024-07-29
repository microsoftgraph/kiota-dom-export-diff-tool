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
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration; cancellationToken?:CancellationToken):Stream", "Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder", "DeleteAsync", "requestConfiguration", "RequestConfiguration", "cancellationToken", "CancellationToken", "Stream", true)]
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration; cancellationToken?:CancellationToken):[string]", "Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder", "DeleteAsync","requestConfiguration", "RequestConfiguration", "cancellationToken", "CancellationToken", "[string]", true)]
    [InlineData("Graphdotnetv4.Users.Item.InferenceClassification.Overrides.Count.CountRequestBuilder::|public|constructor(pathParameters:Dictionary<string, object>; requestAdapter:IRequestAdapter):void", "Graphdotnetv4.Users.Item.InferenceClassification.Overrides.Count.CountRequestBuilder", "constructor", "pathParameters", "Dictionary<string, object>", "requestAdapter", "IRequestAdapter", "void", false)]
    [Theory]
    public void ParsesDomMethod(string value, string expectedParentPath, string expectedMethodName, string firstParameterName, string firstParameterType, string secondParameterName, string secondParameterType, string expectedReturnType, bool areParametersOptional)
    {
        var result = MethodDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal(expectedParentPath, result.ParentTypePath);
        Assert.False(result.isStatic);
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(expectedMethodName, result.Name);
        Assert.Equal(expectedReturnType, result.ReturnTypeName);
        Assert.Equal(2, result.Parameters.Length);
        Assert.Equal(firstParameterName, result.Parameters[0].Name);
        Assert.Equal(firstParameterType, result.Parameters[0].TypeName);
        Assert.Equal(areParametersOptional, result.Parameters[0].isOptional);
        Assert.Equal(secondParameterName, result.Parameters[1].Name);
        Assert.Equal(secondParameterType, result.Parameters[1].TypeName);
        Assert.Equal(areParametersOptional, result.Parameters[1].isOptional);
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
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration~~>IBackedModel; IParsable", "IBackedModel", "IParsable")]
    [Theory]
    public void ParsesDomImplementation(string value, string expectedInterfaceType, string secondExpectedInterfaceType)
    {
        var result = ImplementationDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration", result.CurrentTypePath);
        Assert.Contains(expectedInterfaceType, result.InterfaceTypePaths, StringComparer.OrdinalIgnoreCase);
        Assert.Contains(secondExpectedInterfaceType, result.InterfaceTypePaths, StringComparer.OrdinalIgnoreCase);
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
    [InlineData("Graphdotnetv4.Models.bodyType::0000-text", "Graphdotnetv4.Models.bodyType", "text", 0)]
    [Theory]
    public void ParsesDomEnumMember(string value, string expectedPath, string expectedMemberName, int expectedMemberIndex)
    {
        var result = EnumMemberDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal(expectedPath, result.ParentTypePath);
        Assert.Equal(expectedMemberName, result.MemberName);
        Assert.Equal(expectedMemberIndex, result.MemberIndex);
    }
}
