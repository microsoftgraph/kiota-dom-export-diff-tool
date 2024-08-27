
$comments = gh api --method GET -H "Accept: application/vnd.github+json" -H "X-GitHub-Api-Version: 2022-11-28" "/repos/$($Env:GITHUB_REPOSITORY)/issues/$($Env:PR_NUMBER)/comments" 
            | ConvertFrom-Json
            | Where-Object {$_.user.login -eq "github-actions[bot]" -and $_.body.StartsWith("<!--publicapi-->")}
foreach($comment in $comments) {
    Write-Host "Deleting comment $($comment.id)"
    gh api --method DELETE -H "Accept: application/vnd.github+json" -H "X-GitHub-Api-Version: 2022-11-28" "/repos/$($Env:GITHUB_REPOSITORY)/issues/comments/$($comment.id)"
}

$comment = $Env:COMMENT;
if(Test-Path $Env:COMMENT) {# If the comment is a file path, read the content
    $comment = Get-Content $Env:COMMENT
}

gh pr comment $Env:PR_NUMBER -b "<!--publicapi-->$comment"