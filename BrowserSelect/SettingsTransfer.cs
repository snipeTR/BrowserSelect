using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using BrowserSelect.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserSelect
{
    /// <summary>Export/import of rules and settings to/from a human readable JSON file.</summary>
    public static class SettingsTransfer
    {
        private const string FormatName = "BrowserSelect-settings";
        private const int FormatVersion = 1;

        public static void Export(string path)
        {
            var s = Settings.Default;
            var root = new JObject
            {
                ["format"] = FormatName,
                ["version"] = FormatVersion,
                ["exported"] = DateTime.Now.ToString("o"),
                ["rules"] = new JArray(s.AutoBrowser.Cast<string>().Select(x => (AutoMatchRule)x).Select(r =>
                    new JObject
                    {
                        ["match"] = r.MatchType,
                        ["pattern"] = r.Pattern,
                        ["browser"] = r.Browser,
                        ["private"] = r.IsPrivate,
                        ["arguments"] = r.Arguments ?? ""
                    })),
                ["hiddenBrowsers"] = new JArray(s.HideBrowsers.Cast<string>()),
                ["customBrowsers"] = JArray.FromObject(BrowserCustomizations.LoadCustomBrowsers()),
                ["browserOverrides"] = JObject.FromObject(BrowserCustomizations.LoadOverrides()),
                ["browserOrder"] = new JArray(BrowserCustomizations.LoadOrder()),
                ["sortMode"] = s.SortMode,
                ["showRunningOnly"] = s.ShowRunningOnly,
                ["altIgnoresRules"] = s.AltIgnoresRules,
                ["checkForUpdates"] = s.check_update != "nope"
            };
            File.WriteAllText(path, root.ToString(Formatting.Indented));
        }

        /// <summary>
        /// Imports a file created by <see cref="Export"/>. Every section present in the file replaces the
        /// current value; sections missing from the file are left unchanged. Returns the number of rules imported.
        /// </summary>
        public static int Import(string path)
        {
            JObject root;
            try
            {
                root = JObject.Parse(File.ReadAllText(path));
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("The file is not a valid BrowserSelect settings file.", ex);
            }
            if ((string)root["format"] != FormatName)
                throw new InvalidDataException("The file is not a valid BrowserSelect settings file.");

            var s = Settings.Default;
            int ruleCount = 0;

            var rules = root["rules"] as JArray;
            if (rules != null)
            {
                var collection = new StringCollection();
                foreach (var r in rules.OfType<JObject>())
                {
                    var rule = new AutoMatchRule
                    {
                        MatchType = (string)r["match"],
                        Pattern = (string)r["pattern"] ?? "",
                        Browser = (string)r["browser"] ?? "",
                        IsPrivate = (bool?)r["private"] ?? false,
                        Arguments = (string)r["arguments"] ?? ""
                    };
                    if (rule.valid())
                    {
                        collection.Add(rule.ToString());
                        ruleCount++;
                    }
                }
                s.AutoBrowser = collection;
            }

            var hidden = root["hiddenBrowsers"] as JArray;
            if (hidden != null)
            {
                var collection = new StringCollection();
                collection.AddRange(hidden.Values<string>().Where(x => !string.IsNullOrEmpty(x)).ToArray());
                s.HideBrowsers = collection;
            }

            var custom = root["customBrowsers"] as JArray;
            if (custom != null)
                s.CustomBrowsers = custom.ToString(Formatting.None);

            var overrides = root["browserOverrides"] as JObject;
            if (overrides != null)
                s.BrowserOverrides = overrides.ToString(Formatting.None);

            var order = root["browserOrder"] as JArray;
            if (order != null)
                s.BrowserOrder = order.ToString(Formatting.None);

            var sortMode = (string)root["sortMode"];
            if (sortMode != null && BrowserCustomizations.SortModes.Contains(sortMode))
                s.SortMode = sortMode;

            if (root["showRunningOnly"] != null)
                s.ShowRunningOnly = (bool)root["showRunningOnly"];
            if (root["altIgnoresRules"] != null)
                s.AltIgnoresRules = (bool)root["altIgnoresRules"];
            if (root["checkForUpdates"] != null)
            {
                bool check = (bool)root["checkForUpdates"];
                if (!check)
                    s.check_update = "nope";
                else if (s.check_update == "nope")
                    s.check_update = "0";
            }

            s.Save();
            return ruleCount;
        }
    }
}
