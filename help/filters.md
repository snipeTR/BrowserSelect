# Auto Select rules (filters)

🇹🇷 [Türkçe](filters.tr.md)

With **Settings → Auto Select Filters** you can add rules so that BrowserSelect opens a link in a browser
automatically instead of showing the browser list. Every rule has five columns:

**Match**: what part of the link the pattern is compared to:

| Match | Pattern is matched against | Examples |
|---|---|---|
| `Domain` (default) | the host name | `google.com`, `*.google.com`, `website.*`, `*` |
| `URL` | the whole link, `*` and `?` wildcards (the `scheme://` part is optional) | `github.com/myorg/*`, `https://*/login*` |
| `Path` | the path (and query) of the link; a leading `/` is added if missing | `/watch*`, `/docs/*` |
| `Keyword` | comma (or `;`) separated words, matches if the link contains any of them | `zoom, meet, webex` |
| `Extension` | the file extension of the link or of the opened file (comma or space separated) | `pdf, zip`, `html` |
| `Source App` | the application the link was clicked in (file name with or without `.exe`, or full path; wildcards allowed) | `outlook.exe`, `slack`, `teams*` |
| `Regex` | a .NET regular expression (case-insensitive) matched against the whole link | `^https://(www\.)?example\.(com\|org)/` |

All comparisons are case-insensitive.

**Pattern**: what links match the rule. For `Domain` rules you may use:
- a domain name (a URL without the path e.g. `google.com , www.mywebsite.org`)
- a wildcard pattern ( `*.local , *.us , *.google.com , website.*` )
- an asterisk ( `*` ) which will match everything

**Browser**: what happens when the pattern matches:
- a browser (or browser profile) to open the link with automatically,
- `display BrowserSelect` to always show the browser list for this pattern (excludes it from a broader rule),
- `ignore URL (do nothing)` to not open the link at all.

**Private**: open the link in a private/incognito window.

**Arguments**: custom command line flags passed to the browser, e.g. `--incognito`,
`--disable-web-security --user-data-dir="C:\temp\chrome"` or `-P work` for a Firefox profile.

Private rules and arguments that open a new window (`--new-window`, `--incognito`, `-private-window`, `--app=...`,
`--kiosk`, ...) skip *Settings → Options → Avoid full-screen windows*: the link gets a new window anyway, so no
existing browser window is brought to the front.

Order and priority
---

- Exact patterns are checked first, then wildcard patterns, and the match-all `*` (or the regex `.*`) last.
- Rules with the same priority are checked from top to bottom; use **Move Up** / **Move Down** to reorder them.
- The first matching rule wins. If no rule matches, the browser list is shown.
- An invalid rule (e.g. a broken regular expression) never blocks a link; it is simply skipped.

Editing rules
---

- **Delete** (right of Move Down) or the <kbd>Del</kbd> key removes the selected rule.
- Changes are saved only after **Apply**.
- The settings window shows which application opened the current link ("Link opened from: ..."); that is the
  name to use in `Source App` rules.
- **Options → Export... / Import...** saves the rules (and other settings) to a `.json` file and loads them back.

*Note*: you can open BrowserSelect manually from the Start menu to change the filters. BrowserSelect always opens
itself when launched from the Start menu regardless of the filters.

*Tip*: hold **Alt** while clicking a link to skip all rules and pick the browser manually
(can be disabled in Settings → Options → "Hold Alt on a link to skip rules").

*Tip*: the **Always** button in the browser list creates a `Domain` rule for the current site automatically.

Examples
---

1. open all websites in Firefox but companywebsite.com in IE:
    - rule 1: `*` -> `Firefox`
    - rule 2: `*.companywebsite.com` -> `IE`
2. open Google websites (`mail.google.com, contacts.google.com, ...`) in Chrome, GitHub in Firefox and ask for other websites:
    - rule 1: `*.google.com` -> `Chrome`
    - rule 2: `github.com` -> `Firefox`

    note: you may add a third rule with `*` for `display BrowserSelect` but it is not necessary; BrowserSelect
    asks by default when no rule matches.
3. ask which browser for `.us` domains, open all other websites with Firefox:
    - rule 1: `*.us` -> `display BrowserSelect`
    - rule 2: `*` -> `Firefox`
4. open every link clicked in Outlook with Edge, and every Zoom/Meet link in Chrome:
    - rule 1: `Source App` `outlook.exe` -> `Edge`
    - rule 2: `Keyword` `zoom.us, meet.google.com` -> `Google Chrome`
5. open only your organization's GitHub repositories in a new Chrome window, with custom flags:
    - rule 1: `URL` `github.com/myorg/*` -> `Google Chrome` with Arguments `--new-window`
6. open PDF links in Edge, and YouTube videos in a private window:
    - rule 1: `Extension` `pdf` -> `Edge`
    - rule 2: `Path` `/watch*` -> `Firefox` with Private ticked
7. never open tracking links:
    - rule 1: `Regex` `^https?://(www\.)?tracker\.example\.com/` -> `ignore URL (do nothing)`
