using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BrowserSelect.Localization;

namespace BrowserSelect
{
    /// <summary>Information about the link being opened, used to evaluate Auto Select rules.</summary>
    public class LinkContext
    {
        public string Url;
        public Uri Uri;
        /// <summary>Full path of the application that opened the link (may be null).</summary>
        public string SourceApp;

        public LinkContext(string url, string sourceApp = null)
        {
            Uri = new UriBuilder(url).Uri;
            Url = Uri.AbsoluteUri;
            SourceApp = sourceApp;
        }
    }

    /// <summary>An Auto Select rule (stored as a delimited string in Settings.AutoBrowser).</summary>
    public class AutoMatchRule
    {
        private const string Separator = "[#!][$~][?_]";

        // special "browser" targets
        public const string DisplayBrowserSelect = "display BrowserSelect";
        public const string IgnoreUrl = "ignore URL (do nothing)";

        // what part of the link a rule's pattern is matched against
        public const string MatchDomain = "Domain";
        public const string MatchUrl = "URL";
        public const string MatchPath = "Path";
        public const string MatchKeyword = "Keyword";
        public const string MatchExtension = "Extension";
        public const string MatchSourceApp = "Source App";
        public const string MatchRegex = "Regex";

        public static readonly string[] MatchTypes =
        {
            MatchDomain, MatchUrl, MatchPath, MatchKeyword, MatchExtension, MatchSourceApp, MatchRegex
        };

        private string _matchType = MatchDomain;

        public string Pattern { get; set; }
        public string Browser { get; set; }
        public bool IsPrivate { get; set; }

        /// <summary>One of <see cref="MatchTypes"/>; defaults to Domain (the original behavior).</summary>
        public string MatchType
        {
            get { return _matchType; }
            set
            {
                var known = MatchTypes.FirstOrDefault(x => string.Equals(x, (value ?? "").Trim(),
                    StringComparison.OrdinalIgnoreCase));
                _matchType = known ?? MatchDomain;
            }
        }

        /// <summary>Extra command line flags passed to the browser (e.g. --incognito).</summary>
        public string Arguments { get; set; } = "";

        public static implicit operator AutoMatchRule(System.String s)
        {
            var ss = (s ?? "").Split(new[] { Separator }, StringSplitOptions.None);
            return new AutoMatchRule()
            {
                Pattern = ss.Length > 0 ? ss[0] : "",
                Browser = ss.Length > 1 ? ss[1] : "",
                IsPrivate = ss.Length > 2 &&
                    (ss[2] == "1" || string.Equals(ss[2], "true", StringComparison.OrdinalIgnoreCase)),
                MatchType = ss.Length > 3 ? ss[3] : MatchDomain,
                Arguments = ss.Length > 4 ? ss[4] : ""
            };
        }

        public override string ToString()
        {
            return Pattern + Separator + Browser + Separator + (IsPrivate ? "1" : "0") +
                Separator + MatchType + Separator + (Arguments ?? "");
        }

        public string error()
        {
            if (!string.IsNullOrEmpty(Pattern) && string.IsNullOrEmpty(Browser))
                return L10n.T("Rule_NoBrowser", Pattern);
            else if (string.IsNullOrEmpty(Pattern) && !string.IsNullOrEmpty(Browser))
                return Strings.Rule_EmptyPattern;
            else if (MatchType == MatchRegex && !IsValidRegex(Pattern))
                return L10n.T("Rule_InvalidRegex", Pattern);
            else
                return "";
        }

        public bool valid()
        {
            if (string.IsNullOrEmpty(Browser) || string.IsNullOrEmpty(Pattern))
                return false;
            return MatchType != MatchRegex || IsValidRegex(Pattern);
        }

        private static bool IsValidRegex(string pattern)
        {
            try
            {
                new Regex(pattern ?? "");
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Lower values are evaluated first: exact patterns, then wildcard patterns, then the match-all "*"
        /// (rules with the same priority keep their order from the rule list).
        /// </summary>
        public int Priority
        {
            get
            {
                var p = Pattern ?? "";
                if (MatchType == MatchRegex)
                    return p == ".*" ? 2 : 1;
                return (p.Contains("*") ? 1 : 0) + (p == "*" ? 1 : 0);
            }
        }

        /// <summary>Returns the first rule (by priority, then list order) that matches the link, or null.</summary>
        public static AutoMatchRule FindMatch(IEnumerable<AutoMatchRule> rules, LinkContext link)
        {
            return rules.Where(r => r.valid())
                .OrderBy(r => r.Priority)
                .FirstOrDefault(r => r.Matches(link));
        }

        public bool Matches(LinkContext link)
        {
            var pattern = (Pattern ?? "").Trim();
            if (pattern.Length == 0)
                return false;
            try
            {
                switch (MatchType)
                {
                    case MatchUrl:
                        return MatchUrlPattern(link, pattern);
                    case MatchPath:
                        return MatchPathPattern(link.Uri, pattern);
                    case MatchKeyword:
                        return MatchKeywords(link, pattern);
                    case MatchExtension:
                        return MatchExtensions(link.Uri, pattern);
                    case MatchSourceApp:
                        return MatchSource(link.SourceApp, pattern);
                    case MatchRegex:
                        return Regex.IsMatch(link.Url, pattern, RegexOptions.IgnoreCase,
                            TimeSpan.FromSeconds(1));
                    default:
                        return Program.DoesDomainMatchPattern(link.Uri.Host, pattern);
                }
            }
            catch (Exception)
            {
                // a broken rule must never prevent the link from opening
                return false;
            }
        }

        private static IEnumerable<string> SplitList(string pattern, bool splitOnSpaces = false)
        {
            var separators = splitOnSpaces ? new[] { ',', ';', ' ', '\t' } : new[] { ',', ';' };
            return pattern.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).Where(x => x.Length > 0);
        }

        /// <summary>Case-insensitive wildcard (* and ?) match of the whole string.</summary>
        public static bool WildcardMatch(string input, string pattern)
        {
            if (input == null)
                return false;
            var regex = "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            return Regex.IsMatch(input, regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        private static bool MatchUrlPattern(LinkContext link, string pattern)
        {
            if (WildcardMatch(link.Url, pattern))
                return true;
            // allow patterns without a scheme, e.g. github.com/myorg/*
            if (!pattern.Contains("://"))
            {
                var withoutScheme = link.Url.Substring(link.Uri.Scheme.Length).TrimStart(':', '/');
                return WildcardMatch(withoutScheme, pattern);
            }
            return false;
        }

        private static bool MatchPathPattern(Uri uri, string pattern)
        {
            if (!pattern.StartsWith("/") && !pattern.StartsWith("*"))
                pattern = "/" + pattern;
            return WildcardMatch(uri.AbsolutePath, pattern) || WildcardMatch(uri.PathAndQuery, pattern) ||
                   WildcardMatch(Uri.UnescapeDataString(uri.AbsolutePath), pattern);
        }

        private static bool MatchKeywords(LinkContext link, string pattern)
        {
            var url = link.Url;
            string decoded;
            try
            {
                decoded = Uri.UnescapeDataString(url);
            }
            catch
            {
                decoded = url;
            }
            return SplitList(pattern).Any(k =>
                url.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 ||
                decoded.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public static string GetExtension(Uri uri)
        {
            try
            {
                return Path.GetExtension(Uri.UnescapeDataString(uri.AbsolutePath)).TrimStart('.');
            }
            catch (ArgumentException)
            {
                return "";
            }
        }

        private static bool MatchExtensions(Uri uri, string pattern)
        {
            var ext = GetExtension(uri);
            if (ext.Length == 0)
                return false;
            return SplitList(pattern, true).Any(p => WildcardMatch(ext, p.TrimStart('*').TrimStart('.')));
        }

        private static bool MatchSource(string sourceApp, string pattern)
        {
            if (string.IsNullOrEmpty(sourceApp))
                return false;
            var file = sourceApp.Substring(sourceApp.LastIndexOfAny(new[] { '\\', '/' }) + 1);
            var name = Path.GetFileNameWithoutExtension(file);
            return SplitList(pattern).Any(p => p.Contains("\\")
                ? WildcardMatch(sourceApp, p)
                : WildcardMatch(file, p) || WildcardMatch(name, p));
        }
    }
}
