# Set the base image as the .NET 8.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Copy everything and publish the release (publish implicitly restores and builds)
WORKDIR /app
COPY . ./
RUN dotnet publish ./src/kiota-dom-export-diff-tool.csproj -c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Vincent Biret <vincent.biret@microsoft.com>"
LABEL repository="https://github.com/baywet/kiota-dom-export-diff-tool"
LABEL homepage="https://github.com/baywet/kiota-dom-export-diff-tool"

# Label as GitHub action
LABEL com.github.actions.name="Kiota dom export diff tool"
# Limit to 160 characters
LABEL com.github.actions.description="Analyzes the differences between two Kiota DOM exports for source breaking changes in the public API surface"
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="orange"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/runtime:8.0
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "/kiota-dom-export-diff-tool.dll" ]