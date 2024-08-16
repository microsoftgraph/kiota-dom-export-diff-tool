FROM mcr.microsoft.com/powershell

RUN mkdir /scripts
COPY ./clearOldComments.ps1 ./scripts

RUN apt update &&\
    apt install -y curl gpg &&\
    apt clean;
RUN curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg | gpg --dearmor -o /usr/share/keyrings/githubcli-archive-keyring.gpg;
RUN echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" | tee /etc/apt/sources.list.d/github-cli.list > /dev/null;
RUN apt update &&\
    apt install -y gh &&\
    apt clean;

LABEL maintainer="Vincent Biret <vincent.biret@microsoft.com>"
LABEL repository="https://github.com/baywet/kiota-dom-export-diff-tool"
LABEL homepage="https://github.com/baywet/kiota-dom-export-diff-tool"

# Label as GitHub action
LABEL com.github.actions.name="Comments rotation script"
# Limit to 160 characters
LABEL com.github.actions.description="Rotates comments in a pull request"
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="orange"

ENTRYPOINT [ "pwsh", "-c", "/scripts/clearOldComments.ps1" ]