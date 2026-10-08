**Match**: what part of the link the pattern is compared to:

| Match | Pattern is matched against | Examples |
|---|---|---|
| `Domain` (default) | the host name | `google.com`, `*.google.com`, `website.*`, `*` |
| `URL` | the whole link, `*` and `?` wildcards (the `scheme://` part is optional) | `github.com/myorg/*`, `https://*/login*` |
| `Path` | the path (and query) of the link | `/watch*`, `/docs/*` |
| `Keyword` | comma separated words, matches if the link contains any of them | `zoom, meet, webex` |
| `Extension` | the file extension of the link or of the opened file | `pdf, zip`, `html` |
| `Source App` | the application the link was clicked in (file name or full path, wildcards allowed) | `outlook.exe`, `slack`, `teams*` |
| `Regex` | a .NET regular expression (case-insensitive) matched against the whole link | `^https://(www\.)?example\.(com\|org)/` |

**Pattern**: what links match the rule. For `Domain` rules you may use:
- a domain name (a URL without the path e.g. `google.com , www.mywebsite.org`)
- a wildcard pattern ( `*.local , *.us , *.google.com , website.*` )
- an asterisk ( `*` ) which will match everything

**Browser**: what happens when the pattern matches. which is either a browser to be opened automatically, "display browser select" to exclude a pattern from auto-selection, or "ignore URL (do nothing)" to not open the link at all.

**Private**: open the link in a private/incognito window.

**Arguments**: custom command line flags passed to the browser, e.g. `--incognito`, `--disable-web-security --user-data-dir="C:\temp\chrome"` or `-P work` for a Firefox profile.

Rules are evaluated from top to bottom (use Move Up / Move Down to reorder). Exact patterns are checked before wildcard patterns and the match-all `*` is checked last. The first matching rule wins.

The settings window shows which application opened the current link ("Link opened from: ..."), which is the name to use in `Source App` rules.

*Note*: you can open BrowserSelect manually from startmenu to change the Filters. BrowserSelect always opens itself when launched from startmenu regardless of Filters added.

*Tip*: hold **Alt** while clicking a link to skip all rules and pick the browser manually (can be disabled in Settings > Options).


Examples
---

1. open all websites in firefox but companywebsite.com in IE:
    - rule 1: `*` -> `Firefox`
    - rule 2: `*.companywebsite.com` -> `IE`
2. open google websites (`mail.google.com, contacts.google.com, ...`) in chrome, github in firefox and ask for other website:
    - rule 1: `*.google.com` -> `chrome`
    - rule 2: `github.com` -> `firefox`
    note: you may add a third rule with `*` for display browser select but it is not nessesary, Browser select defaults to asking when no rule matches
3. ask which browser for `.us` domains, open all other websites with firefox:
    - rule 1: `*.us` -> `display browser select`
    - rule 2: `*` -> `firefox`
4. open every link clicked in Outlook with Edge, and every Zoom/Meet link in Chrome:
    - rule 1: `Source App` `outlook.exe` -> `Edge`
    - rule 2: `Keyword` `zoom.us, meet.google.com` -> `Google Chrome`
5. open only your organization's GitHub repositories in a separate Chrome profile window, with custom flags:
    - rule 1: `URL` `github.com/myorg/*` -> `Google Chrome` with Arguments `--new-window`
