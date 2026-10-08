using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using BrowserSelect.Properties;

namespace BrowserSelect
{
    class UpdateChecker
    {
        /// <summary>GitHub repository the update check looks at (published releases only, drafts are ignored by GitHub)</summary>
        public const string Repository = "snipeTR/BrowserSelect";
        public const string ReleasesUrl = "https://github.com/" + Repository + "/releases";
        public const string LatestReleaseUrl = ReleasesUrl + "/latest";
        /// <summary>help folder of the repository (help/filters.md, help/filters.tr.md, ...)</summary>
        public const string HelpUrl = "https://github.com/" + Repository + "/blob/master/help/";

        public String CVer => current_version;
        public String LVer => last_version;
        public Boolean Checked => init;
        public Boolean Updated => new_version();
        
        private string current_version = "x";
        private string last_version = "x";
        private bool init = false;
        private bool newer = false;

        public void check()
        {
            if (new_version())
                Settings.Default.last_version = last_version;
            else if (init)
                // checked successfully and nothing newer -> clear a stale "update available" flag
                Settings.Default.last_version = "nope";
            if (Settings.Default.check_update != "nope")
                Settings.Default.check_update = Program.time().ToString();
            Settings.Default.Save();
        }

        /// <summary>
        /// extracts the version from a release tag, e.g. "v1.4.3.0-build.8" -> 1.4.3.0, "1.4.0" -> 1.4.0
        /// </summary>
        public static Version ParseVersion(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return null;
            var m = Regex.Match(tag, @"\d+(\.\d+){1,3}");
            Version v;
            return m.Success && Version.TryParse(m.Value, out v) ? v : null;
        }

        /// <summary>1.4.3 and 1.4.3.0 should compare as equal</summary>
        private static Version Normalize(Version v)
        {
            return new Version(v.Major, v.Minor, Math.Max(v.Build, 0), Math.Max(v.Revision, 0));
        }

        string get_last_version()
        {
            // request to releases/latest redirects the user to /releases/tag/<tag>.
            // the tag contains the version number (v<version>-build.<n>), so we can get the latest version
            // from the Location header and make a HEAD request instead of get to save bandwidth
            var req = (HttpWebRequest)WebRequest.Create(LatestReleaseUrl);
            // make webrequest use tls 1.2 instead of ssl3 (#43)
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            req.Method = "HEAD";
            req.AllowAutoRedirect = false;
            using (var res = req.GetResponse())
            {
                // without any published release GitHub redirects to /releases (no tag) -> parse fails -> check failed
                return res.Headers["Location"].Split('/').Last();
            }
        }

        void get_versions()
        {
            try
            {
                var latest = ParseVersion(get_last_version());
                var current = ParseVersion(Application.ProductVersion);
                if (latest == null || current == null)
                    return;
                last_version = latest.ToString();
                current_version = current.ToString();
                newer = Normalize(latest) > Normalize(current);
                init = true;
            }
            catch (Exception) { }
        }

        bool new_version()
        {
            if (!init)
                get_versions();
            return init && newer;
        }
    }
}
