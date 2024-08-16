# Kiota DOM export diff tool

## Generating a valid diff for evaluation

### Debugging

```PowerShell
dotnet run --project .\src\kiota-dom-export-diff-tool.csproj -- explain --path (Resolve-Path .\tests\resources\0001-temp-second-export.patch) -f
```

### Diff method

```PowerShell
$result = git diff --minimal -U0 -w
```

### Patch method

```bash
git format-patch -1 HEAD --minimal -U0 -w kiota-dom-export.txt
```
