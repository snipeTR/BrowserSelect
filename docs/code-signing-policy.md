# Code signing policy

Free code signing provided by SignPath.io (https://about.signpath.io/), certificate by SignPath Foundation (https://signpath.org/)

## What is signed

- The Windows x64 NSIS installer `BrowserSelect-<version>-x64-Setup.exe` published on the [Releases page](https://github.com/snipeTR/BrowserSelect/releases), and optionally the `BrowserSelect.exe` it contains.
- Every signed file is built from the source code in this repository by the public GitHub Actions workflow [`.github/workflows/build-release.yml`](../.github/workflows/build-release.yml), running on GitHub-hosted `windows-latest` runners.
- Only release builds from the `master` branch or release tags are signed. No binaries built elsewhere (locally, on self-hosted runners or from forks) are signed.
- Every signing request is approved manually by an approver (see below) before the signature is applied.

## Team roles

BrowserSelect currently has a single maintainer.

### Authors

- [snipeTR](https://github.com/snipeTR) (committer)

### Reviewers

- [snipeTR](https://github.com/snipeTR)

### Approvers

- [snipeTR](https://github.com/snipeTR)

Upstream credit: BrowserSelect was originally written by Bor691 ([zumoshi/BrowserSelect](https://github.com/zumoshi/BrowserSelect)). The original author is credited here as upstream only and has no role in signing releases of this repository.

## Privacy policy

This program will not transfer any information to other networked systems unless specifically requested by the user or the person installing or operating it.

Details of the network connections BrowserSelect can make:

- **Update check (off by default).** Automatic update checking is disabled after installation. It is only used when the user turns on *Settings → Update checker → enable* or clicks *check now* there. When enabled, BrowserSelect checks at most once every 7 days at startup by sending a HEAD request to `https://github.com/snipeTR/BrowserSelect/releases/latest` and reading the version from the release tag it redirects to. To stop it, clear the *enable* check box in *Settings → Update checker*.
- **Update download (only after confirmation).** Only when a newer version was found and the user answers *Yes* to "Do you want to download and run the new version now?", BrowserSelect asks GitHub's public releases API (`https://api.github.com/repos/snipeTR/BrowserSelect/releases/latest`) for the installer and downloads `BrowserSelect-<version>-x64-Setup.exe` from GitHub to `%TEMP%`, then starts it.
- These requests send no personal data and no telemetry. The only data sent is what any HTTPS request contains (e.g. the IP address seen by GitHub) and a `User-Agent: BrowserSelect/<version>` header that the GitHub API requires. No usage data, URLs, rules or settings are transmitted.
- **Links opened by the user.** Help links, the releases page and links in the About window are opened in the user's default browser only when the user clicks them. If .NET Framework 4.8 is missing, the installer offers to open Microsoft's download page in the browser, only if the user agrees.
- Opening a link with BrowserSelect simply passes it to the browser the user (or one of the user's own rules) selected; BrowserSelect itself does not contact that address.

## Security

- The maintainer uses two-factor authentication (2FA) on GitHub and on SignPath.

## Uninstall

BrowserSelect can be removed at any time via Windows *Settings → Apps → Installed apps* (or *Apps & features*); this runs the standard NSIS uninstaller (`Uninstall.exe` in the installation folder).
