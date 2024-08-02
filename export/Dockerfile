FROM mcr.microsoft.com/powershell

WORKDIR /app
COPY ./generateDomPatch.ps1 ./

LABEL maintainer="Vincent Biret <vincent.biret@microsoft.com>"
LABEL repository="https://github.com/baywet/kiota-dom-export-diff-tool"
LABEL homepage="https://github.com/baywet/kiota-dom-export-diff-tool"

# Label as GitHub action
LABEL com.github.actions.name="Kiota dom export diff script"
# Limit to 160 characters
LABEL com.github.actions.description="Creates a diff between two Kiota generated DOMs"
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="orange"

ENTRYPOINT [ "pwsh", "-c", "./generateDomPatch.ps1" ]