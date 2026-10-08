# Browser Select

Browser Select is a utility to dynamically select the browser you want instead of just having one default for all links. Similar to the prompt in android to choose a browser when a link in a non-browser app is clicked/touched. It may not be useful for everyone but it helps when you use multiple browsers for different things (e.g. one with proxy and one without) and open many links from other applications (e.g. Messengers).

This is an actively maintained fork of [zumoshi/BrowserSelect](https://github.com/zumoshi/BrowserSelect) with Windows **64-bit** installer builds via GitHub Actions, maintained by [snipeTR](https://github.com/snipeTR). Available in English and Turkish (Settings → Language).

![screenshot1](https://raw.githubusercontent.com/zumoshi/BrowserSelect/master/screenshots/photo_2016-07-11_13-44-19.png)

Instead of having to copy the link, open the desired (non-default) browser then pasting the link, all you need to do is to click on the link and this prompt will open allowing you to choose the browser you want. It automatically detects installed browsers. It does not require administrative rights and can be installed as a restricted user.

![screenshot 2](https://raw.githubusercontent.com/zumoshi/BrowserSelect/master/screenshots/photo_2015-10-12_16-46-14.jpg)

You may click on the desired browser or press one of the shortcuts (its index or the first letter of its name), for example for chrome you can press 2, g or c.
you may also press Esc (or click the X) to not open the URL.

To install, download the installer below then set BrowserSelect as the default browser.

![select default browser](https://raw.githubusercontent.com/zumoshi/BrowserSelect/master/screenshots/photo_2015-10-12_16-43-08.jpg)

BrowserSelect has been tested on windows 7, windows 8.1 and windows 10/11. Requires **.NET Framework 4.8**.

# Download

Windows 64-bit installer: [BrowserSelect 1.4.1.0 (x64) Setup](https://github.com/snipeTR/BrowserSelect/releases/download/v1.4.1.0-build.4/BrowserSelect-1.4.1.0-x64-Setup.exe)

Latest release: [github.com/snipeTR/BrowserSelect/releases/latest](https://github.com/snipeTR/BrowserSelect/releases/latest)

CI builds (MSBuild + NSIS) run on GitHub Actions on every push to `master`. When the version in `BrowserSelect/Properties/AssemblyInfo.cs` changes, a draft release with the installer is created automatically (no manual run needed); published releases are available on the Releases page.

# Related links

[AlternativeTo](http://alternativeto.net/software/browser-select/)

Reviews: [DSTech](http://dipendrashekhawat.com/choose-specific-browser-every-time-you-open-a-link/)
[TrishTech](http://www.trishtech.com/2016/07/use-different-browsers-for-different-links-with-browserselect/)
[DonationCoder](http://www.donationcoder.com/forum/index.php?topic=42860.msg401447)

Upstream / mirrors (may be outdated):
[zumoshi/BrowserSelect](https://github.com/zumoshi/BrowserSelect/releases)
[SoftPedia](http://www.softpedia.com/get/Internet/Browsers/Browser-Select.shtml)

# ToDo

Just a list of some ideas that can be integrated into BrowserSelect.
- [x] Make Settings persist across updates
- [x] Shift-Click to open link in incognito/private mode
- [x] Option to display running browsers only
- [x] More Auto-Select rule options
    - [x] based on the source application
    - [x] based on file extension
    - [x] based on URL path
    - [x] based on keywords
    - [x] ignoring the URL as an option
    - [x] custom flags to browsers as an option (e.g. incognito mode or disable CSRF)
- [x] export/import for rules/settings
- [x] Sorting browsers on the list
- [x] Custom Shortcuts
- [x] Ignoring the rules if Alt key is held down when clicking a link
- [ ] an API to invoke BrowserSelect
- [x] Bugfix for when Browser was launched with Maximize window state (browser select will launch maximized)
- [ ] A browser extension to launch the correct browser based on the rules even if a link is clicked inside a browser
- [x] support for portable browsers (adding browsers using a browse button rather than registry)
- [ ] support for non-browser apps as an option (e.g. download managers)
- [ ] themes ? or at least an optional transparent Aero glass mode
- [x] Ability to choose custom icons for browsers
- [ ] display the unshortened version of adf.ly or goo.gl links when selecting the browser
- [x] Localization (English and Turkish; texts in `BrowserSelect/Localization/Strings*.resx`, language selection in Settings)
- [ ] handling of other link types (e.g. `mail:` in case you have both outlook and thunderbird installed [or maybe as a sister app])
- [x] update checker (not as a popup or messagebox, a tiny icon somewhere on the main form that appears when you don't have the last version)
- [x] add file associations (e.g. .url files, or .html files)

# Changelog

v1.4.3.0
- Turkish translation (`Strings.tr.resx`): select *Türkçe (TR)* in Settings → Language, restart BrowserSelect
- Texts of the Settings, browser Add/Edit and About windows, the About/Settings buttons and the messages moved to `Strings.resx`, so they follow the selected language (English stays the default)
- About: credits snipeTR as the maintainer of this fork (with a link to the fork)
- About → Donate: bitcoin address and QR code of snipeTR (`bc1q3jqugh66ctwzqr7tqjafunlpaaejqgt265rwjq`) next to the original author's; both addresses have a Copy Address button and open the installed wallet when clicked

v1.4.2.0
- Localization infrastructure: UI texts come from a single English file (`BrowserSelect/Localization/Strings.resx`), see `BrowserSelect/Localization/README.md`
- Language drop-down at the bottom left of the Settings window (lists English and every installed translation; saved in the settings and applied at startup)
- The selected language is included in settings export/import
- GitHub Actions: every push to master builds the installer; a draft release is created automatically when the version changes

v1.4.1.0-build.4 [08/10/26]
- Fixed BrowserSelect opening maximized when the link was clicked in an application running maximized
- Auto Select rules can now match on Domain, URL, Path, Keyword, (file) Extension, Source App (the application the link was clicked in) or a Regex
- Rules can pass custom arguments to the browser (e.g. `--incognito`) and can ignore a URL entirely ("ignore URL (do nothing)")
- Rules pointing to a browser that is no longer installed now show the selection dialogue instead of crashing
- Delete button for rules (next to Move Down) in Settings
- Holding Alt while clicking a link skips the rules and shows the browser list (can be disabled in Settings)
- Portable browsers (or any program) can be added with Settings > Browsers > Add... (browse for the executable)
- Settings > Browsers > Edit... sets a custom icon (.ico/.exe/.dll/.png/.jpg/...), custom shortcut keys and extra arguments per browser
- Browsers can be sorted manually (up/down buttons), alphabetically or by most used
- Option to display only the browsers that are currently running
- Export/Import of rules and settings to a JSON file (Settings > Options)
- File associations: BrowserSelect can open .htm/.html/.shtml/.xht/.xhtml files and .url Internet Shortcuts (Settings > Default Browser > File types..., also registered by the installer). The "Always" button on a local file creates an Extension rule
- Windows x64 NSIS installer built and published via GitHub Actions

v1.4.1 [24/08/19]
- Fixed couldn't hide chrome profiles separately (#52)
- Improved startup speed by caching browsers (#40)
(special thanks to [kthejoker](https://github.com/kthejoker) for his pull request)

v1.4.0 [12/06/18]
- Fixed Opera (post-blink) private mode (#35)
- Chrome profiles are now listed as separate options (#29)
(special thanks to [kueswol](https://github.com/kueswol) for his pull request)

v1.3.9 [06/04/18]
- Fixed Edge private mode (#34)
- Added Alt as an alternative to shift for open in private/incognito mode (#33)

v1.3.8 [20/10/17]
- Fixed pattern generator for single part domains (e.g. localhost) (issue #27)
- Fixed unintended unescaping of URL's (issue #28)

v1.3.7 [16/08/17]
- Fixed issues with clipping on high dpi screens (#24)

v1.3.6 [11/06/17]
- BrowserSelect's window now shows up in the monitor with the mouse cursor instead of the default one (#22)

v1.3.5 [16/12/16]
- fixed crash on startup caused by incompatible/incomplete registry keys (issues #17,#20,#21)

v1.3.4 [02/09/16]
- fixed Always button adding rules with the wrong pattern for second-level domains (e.g. *.com.au for news.com.au)
- Shift Clicking on browsers now opens the URL in incognito/private browsing
- added an update checker (adds a yellow "New" icon to the main window to indicate a new version is available)[disabled by default]

v1.3.3 [03/08/16]
- fixed a crash on malformed (without protocol) URL's
- added donate button in about page

v1.3.2 [28/07/16]
- bugfix to bring IE to the foreground if it is already open

v1.3.1 [14/07/16]
- bugfix for Auto rule creation of domains with subdomains

v1.3 [11/07/16]
- Added an "Always" button under browser icons that adds a rule for *.domain.tld
- Added a help button in the main form
- made about form closable by Esc key
- added a help form for the settings page
- changed how filters are executed to allow simpler use of a match-all pattern
- added browser select to the list of options when adding rules
- added an apply button to the settings page for Rules
- polished the rule adding interface
- some code Formating/Indenting/Restructuring

v1.2.1 [14/06/16]
- bugfix for InternetExplorer to open links in a new tab instead of a new window

v1.2 [08/06/16]
- you can now add URL patterns to select the Browser based on URL automatically.

v1.1 [18/05/16]
- added option to select browsers that are displayed on the list (and remove/hide some)

v1.0.2 [15/01/16]
- added option to set browser select as the default browser in settings

v1.0.1 [27/10/15]
- added edge browser for windows 10 (it wouldn't show up due edge being a Universal App)
