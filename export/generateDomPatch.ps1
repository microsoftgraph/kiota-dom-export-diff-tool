param (
    [string]
    $fileNameToDiff = "kiota-dom-export.txt",
    [string]
    $initialCommitSha = "",
    [string]
    $finalCommitSha = ""
)
try {
    git --version | Out-Null
}
catch [System.Management.Automation.CommandNotFoundException] {
    Write-Error "git is not installed"
    exit 1
}
if (Test-Path Env:fileNameToDiff) {
    $fileNameToDiff = $env:fileNameToDiff
}
if (Test-Path Env:initialCommitSha) {
    $initialCommitSha = $env:initialCommitSha
}
if (Test-Path Env:finalCommitSha) {
    $finalCommitSha = $env:finalCommitSha
}
if ($null -eq $fileNameToDiff -or "" -eq $fileNameToDiff) {
    $fileNameToDiff = "kiota-dom-export.txt"
}
$relativePath = Get-ChildItem -Recurse -Filter $fileNameToDiff | Select-Object -First 1
if ($null -eq $relativePath) {
    Write-Warning "No file found with the name $fileNameToDiff"
}
else {
    if (Test-Path Env:GITHUB_WORKSPACE) {
        #ignore ownership errors when running from a container
        git config --global --add safe.directory $Env:GITHUB_WORKSPACE
    }
    git fetch
    if (Test-Path Env:GITHUB_EVENT_NAME) {
        # Disable detached head so we get a patch
        if ("push" -eq $Env:GITHUB_EVENT_NAME -and (Test-Path Env:GITHUB_REF)) {
            $branchName = $Env:GITHUB_REF -replace "refs/heads/", ""
            git checkout $branchName
        }
        elseif ("pull_request" -eq $Env:GITHUB_EVENT_NAME -and (Test-Path Env:GITHUB_HEAD_REF)) {
            if ((Test-Path Env:GITHUB_BASE_REF) -and $initialCommitSha -eq "") {
                # get the base branch commit and set it as the initial commit sha if none has been provided
                $targetBranch = $Env:GITHUB_BASE_REF
                git switch $targetBranch
                $initialCommitSha = git rev-parse HEAD
                write-host "Setting initial commit sha as $initialCommitSha from head of $targetBranch"
            }
            # checkout the PR branch
            $branchName = $Env:GITHUB_HEAD_REF
            git switch $branchName
        }
    }
    if ($initialCommitSha -eq "" -and $finalCommitSha -eq "") {
        # neither are set, use the previous commit
        write-host "Running git format-patch with previous commit sha as initial commit sha"
        $result = git format-patch -1 HEAD --minimal -U0 -w $relativePath
    }
    elseif ($initialCommitSha -ne "" -and $finalCommitSha -ne "") {
        # both are set, use them
        write-host "Running git format-patch with the initial commit sha $initialCommitSha and final commit sha $finalCommitSha"
        $result = git format-patch $initialCommitSha..$finalCommitSha -U0 --binary $relativePath
    }
    elseif ($initialCommitSha -ne "") {
        # only the initial commit sha is set(this will diff the current checkout with the initial commit)
        write-host "Running git format-patch with initial commit sha $initialCommitSha"
        $result = git format-patch $initialCommitSha -U0 --binary $relativePath
    }
    else {
        # only the final commit sha is set(this will diff the current checkout commit with the final commit provided)
        write-host "Running git format-patch with final commit sha $finalCommitSha and current commit sha as initial commit sha"
        $initialCommitSha = git rev-parse HEAD
        $result = git format-patch $initialCommitSha..$finalCommitSha -U0 --binary $relativePath
    }
    write-host "Patch file generated at $result"
    if ($result -and (Test-Path $result) -and (Test-Path Env:GITHUB_OUTPUT)) {
        write-host "Patch file generated at $result"
        "isFilePresent=true" | Out-File -FilePath $env:GITHUB_OUTPUT -Append
        "patchFilePath=$result" | Out-File -FilePath $env:GITHUB_OUTPUT -Append
        exit 0
    }
}
if (Test-Path Env:GITHUB_OUTPUT) {
    write-host "No patch file generated"
    "isFilePresent=false" | Out-File -FilePath $env:GITHUB_OUTPUT -Append
}