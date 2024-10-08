param (
	[string]
	$fileNameToDiff = "kiota-dom-export.txt",
    [string]
    $initialCommitSha = "",
    [string]
    $finalCommitSha = ""
)
try
{
    git --version | Out-Null
}
catch [System.Management.Automation.CommandNotFoundException]
{
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
} else {
    if (Test-Path Env:GITHUB_WORKSPACE) {
        #ignore ownership errors when running from a container
        git config --global --add safe.directory $Env:GITHUB_WORKSPACE
    }
    if (Test-Path Env:GITHUB_EVENT_NAME) {
        # Disable detached head so we get a patch
        if ("push" -eq $Env:GITHUB_EVENT_NAME -and (Test-Path Env:GITHUB_REF)) {
            $branchName = $Env:GITHUB_REF -replace "refs/heads/", ""
            git checkout $branchName
        } elseif ("pull_request" -eq $Env:GITHUB_EVENT_NAME -and (Test-Path Env:GITHUB_HEAD_REF)) {
            $branchName = $Env:GITHUB_HEAD_REF
            git fetch origin $branchName
            git checkout $branchName
        }
    }
    if ($initialCommitSha -eq "" -or $finalCommitSha -eq "") {
        $result = git format-patch -1 HEAD --minimal -U0 -w $relativePath
    } else {
        $result = git format-patch $initialCommitSha..$finalCommitSha --minimal -U0 -w $relativePath
    }

    if ((Test-Path $result) -and (Test-Path Env:GITHUB_OUTPUT)) {
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