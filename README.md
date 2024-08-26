# Kiota DOM export diff tool

This tool set is used by the Microsoft Graph Developer Experience team to automate validation of source breaking changes on pre-packaged service libraries. It is not meant for public consumption or use in any other context.

## Structure

The tool set is composed of multiple GitHub actions:

- [Export](./export): creates a diff for the DOM export file.
- [Tool](./tool): analyzes the diff and provide a success/fail depending on whether source breaking changes are detected. Also provides an "explanation" of the changes.
- [Comment](./comment): adds a comment to the pull request, and removes any previously added comment if any.

## Example workflow

```yaml
name: Validate Public API surface changes

on:
  workflow_dispatch:
  push:
  pull_request:
    branches: [ 'feature/*', 'dev' ,'master' ]

permissions:
  contents: read
  pull-requests: write
  issues: write

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: microsoftgraph/kiota-dom-export-diff-tool/export@main
        id: generatePatch
      - uses: microsoftgraph/kiota-dom-export-diff-tool/tool@main
        if: ${{ steps.generatePatch.outputs.patchFilePath != '' }}
        with:
          path: ${{ steps.generatePatch.outputs.patchFilePath }}
          fail-on-removal: true
        id: diff
      - uses: microsoftgraph/kiota-dom-export-diff-tool/comment@main
        if: ${{ always() && steps.generatePatch.outputs.patchFilePath != '' && steps.diff.outputs.hasExplanations != '' && github.event_name == 'pull_request' }}
        continue-on-error: true
        with:
          comment: ${{ steps.diff.outputs.explanations }}
          prNumber: ${{ github.event.pull_request.number }}
          GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
      - name: Upload patch file as artifact
        if: always()
        uses: actions/upload-artifact@v4
        continue-on-error: true
        with:
          name: patch
          path: '*.patch'
```
