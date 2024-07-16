# Kiota DOM export diff tool

## Generating a valid diff for evaluation

### Diff method

```PowerShell
$result = git diff --minimal -U0 -w
```

### Patch method

```bash
git format-patch -1 HEAD --minimal -U0 -w kiota-dom-export.txt
```
