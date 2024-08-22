namespace Microsoft.KiotaDomExportDiffTool.Tests;

public sealed class DomExportEntryTests
{
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters::|public|Search:string", "Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters", "Search", "string")]
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters::|public|Search:string[]", "Graphdotnetv4.Users.Item.MailFolders.Item.Messages.Item.Extensions.Count.CountRequestBuilder.CountRequestBuilderGetQueryParameters", "Search", "string[]")]
    [InlineData("Microsoft.Graph.Chats.GetAllRetainedMessages.getAllRetainedMessagesGetResponse::|public|Value:List<global.Microsoft.Graph.Models.ChatMessage>", "Microsoft.Graph.Chats.GetAllRetainedMessages.getAllRetainedMessagesGetResponse", "Value", "List<global.Microsoft.Graph.Models.ChatMessage>")]
    [Theory]
    public void ParsesDomProperty(string value, string expectedParentTypePath, string propertyName, string expectedType)
    {
        var result = PropertyDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal(expectedParentTypePath, result.ParentTypePath);
        Assert.False(result.isStatic);
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.Equal(propertyName, result.Name);
        Assert.Equal(expectedType, result.TypeName);
    }
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration; cancellationToken?:CancellationToken):Stream", "Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder", "DeleteAsync", "requestConfiguration", "RequestConfiguration", "cancellationToken", "CancellationToken", "Stream", true, true)]
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder::|public|DeleteAsync(requestConfiguration?:RequestConfiguration; cancellationToken?:CancellationToken):[string]", "Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder", "DeleteAsync", "requestConfiguration", "RequestConfiguration", "cancellationToken", "CancellationToken", "[string]", true, true)]
    [InlineData("Graphdotnetv4.Users.Item.InferenceClassification.Overrides.Count.CountRequestBuilder::|public|constructor(pathParameters:Dictionary<string, object>; requestAdapter:IRequestAdapter):void", "Graphdotnetv4.Users.Item.InferenceClassification.Overrides.Count.CountRequestBuilder", "constructor", "pathParameters", "Dictionary<string, object>", "requestAdapter", "IRequestAdapter", "void", false, false)]
    [InlineData("Microsoft.Graph.Drives.Item.Items.Item.Workbook.Names.Item.RangeNamespace.EntireColumn.entireColumnRequestBuilder::|public|GetAsync(requestConfiguration?:Action<RequestConfiguration<DefaultQueryParameters>>; cancellationToken?:CancellationToken):global.Microsoft.Graph.Models.WorkbookRange", "Microsoft.Graph.Drives.Item.Items.Item.Workbook.Names.Item.RangeNamespace.EntireColumn.entireColumnRequestBuilder", "GetAsync", "requestConfiguration", "Action<RequestConfiguration<DefaultQueryParameters>>", "cancellationToken", "CancellationToken", "global.Microsoft.Graph.Models.WorkbookRange", true, true)]//C#
    [InlineData("exportNamespace.me.GetRequestBuilder::|public|ToGetRequestInformation(ctx:context.Context; requestConfiguration?:*GetRequestBuilderGetRequestConfiguration):*RequestInformation", "exportNamespace.me.GetRequestBuilder", "ToGetRequestInformation", "ctx", "context.Context", "requestConfiguration", "*GetRequestBuilderGetRequestConfiguration", "*RequestInformation", false, true)]//Golang
    [InlineData("exportNamespace.me.get.GetRequestBuilder::|public|constructor(path_parameters:Union[str, Dict[str, Any]]; request_adapter:RequestAdapter):None", "exportNamespace.me.get.GetRequestBuilder", "constructor", "path_parameters", "Union[str, Dict[str, Any]]", "request_adapter", "RequestAdapter", "None", false, false)]//python
    [InlineData("exportnamespace.me.MeRequestBuilder::|public|toPostRequestInformation(user:User; requestConfiguration?:java.util.function.Consumer<GetRequestConfiguration>):RequestInformation", "exportnamespace.me.MeRequestBuilder", "toPostRequestInformation", "user", "User", "requestConfiguration", "java.util.function.Consumer<GetRequestConfiguration>", "RequestInformation", false, true)]//java
    [Theory]
    public void ParsesDomMethod(string value, string expectedParentPath, string expectedMethodName, string firstParameterName, string firstParameterType, string secondParameterName, string secondParameterType, string expectedReturnType, bool isFirstParamOptional, bool isSecondParamOptional)
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
        Assert.Equal(isFirstParamOptional, result.Parameters[0].isOptional);
        Assert.Equal(secondParameterName, result.Parameters[1].Name);
        Assert.Equal(secondParameterType, result.Parameters[1].TypeName);
        Assert.Equal(isSecondParamOptional, result.Parameters[1].isOptional);
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
    [InlineData("Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration~~>IBackedModel; IParsable", "Graphdotnetv4.Users.Item.MailFolders.Item.ChildFolders.Item.Messages.Item.Value.ContentRequestBuilder.ContentRequestBuilderDeleteRequestConfiguration", "IBackedModel", "IParsable")]
    [InlineData("exportNamespace.models.microsoft.graph.userable~~>*i878a80d2330e89d26896388a3f487eef27b0a0e6c010c493bf80be1452208f91.AdditionalDataHolder; *i878a80d2330e89d26896388a3f487eef27b0a0e6c010c493bf80be1452208f91.Parsable", "exportNamespace.models.microsoft.graph.userable", "*i878a80d2330e89d26896388a3f487eef27b0a0e6c010c493bf80be1452208f91.AdditionalDataHolder", "*i878a80d2330e89d26896388a3f487eef27b0a0e6c010c493bf80be1452208f91.Parsable")]
    [Theory]
    public void ParsesDomImplementation(string value, string expectedType, string expectedInterfaceType, string secondExpectedInterfaceType)
    {
        var result = ImplementationDomEntry.Parse(value);
        Assert.NotNull(result);
        Assert.Equal(expectedType, result.CurrentTypePath);
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
