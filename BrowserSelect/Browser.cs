using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BrowserSelect
{
    public class Browser
    {
        public string name;
        public string exec;
        public string icon;
        public string additionalArgs = "";

        /// <summary>user defined shortcut characters (see BrowserCustomizations), empty = automatic</summary>
        [JsonIgnore]
        public string customShortcut = "";
        /// <summary>user defined extra arguments for a detected browser (see BrowserCustomizations)</summary>
        [JsonIgnore]
        public string extraArgs = "";
        /// <summary>the browser's own icon (before a custom icon is applied)</summary>
        [JsonIgnore]
        public string defaultIcon;
        /// <summary>true for browsers added manually (portable browsers)</summary>
        [JsonIgnore]
        public bool isCustom;

        public string Identifier => $"{exec} {additionalArgs}";

        public Image string2Icon()
        {
            byte[] byteArray = Convert.FromBase64String(this.icon);
            Bitmap newIcon;
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                newIcon = new Bitmap(stream);
            }
          
            return newIcon; 
        }

        public string private_arg
        {
            get
            {
                var file = Path.GetFileName(exec).ToLowerInvariant();
                if (file.Contains("chrome") || file.Contains("chromium") || file.Contains("brave"))
                    return "--incognito";
                if (file.Contains("firefox"))
                    return "-private-window";
                if (file.Contains("msedge") || file.Contains("edge"))
                    return "--inprivate";
                if (file.Contains("opera"))
                    return "-newprivatetab";
                if (file.Contains("iexplore"))
                    return "-private";
                return "-private-window";
            }
        }

        public List<char> shortcuts => !string.IsNullOrWhiteSpace(customShortcut)
            ? customShortcut.ToLowerInvariant().Where(c => !char.IsWhiteSpace(c) && c != ',').Distinct().ToList()
            : Regex.Replace(name, @"[^A-Za-z\s]", "").Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Substring(0, 1).ToLower()[0]).ToList();
        public override string ToString()
        {
            return name;
        }
        public static implicit operator Browser(string s)
        {
            return BrowserFinder.find().First(b => b.name == s);
        }
    }
    static class BrowserFinder
    {
        /// <summary>returns the browser with the given (display) name, or null if it is not installed.</summary>
        public static Browser FindByName(string name)
        {
            return find().FirstOrDefault(b => b.name == name);
        }

        public static string icon2String(Icon myIcon)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                myIcon.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                bytes = ms.ToArray();
            }

            string iconString = Convert.ToBase64String(bytes);

            return iconString;
        }

        /// <summary>
        /// returns all browsers: detected ones (cached in settings unless update is true) plus the
        /// manually added (portable) browsers, with the user's customizations applied and sorted
        /// according to the selected sort mode.
        /// </summary>
        public static List<Browser> find(bool update = false)
        {
            var browsers = findDetected(update);
            foreach (var custom in BrowserCustomizations.LoadCustomBrowsers())
                if (!browsers.Any(b => string.Equals(b.Identifier, custom.Identifier,
                        StringComparison.OrdinalIgnoreCase)))
                    browsers.Add(custom);
            BrowserCustomizations.Apply(browsers);
            return BrowserCustomizations.Sort(browsers);
        }

        private static List<Browser> findDetected(bool update)
        {
            List<Browser> browsers = new List<Browser>();
            if (Properties.Settings.Default.BrowserList != "" && !update)
            {
                browsers = JsonConvert.DeserializeObject<List<Browser>>(Properties.Settings.Default.BrowserList);
            }
            else
            {
                //special case , firefox+firefox developer both installed
                //(only works if firefox installed in default directory)
                var ff_path = Path.Combine(
                    Program.ProgramFilesx86(),
                    @"Mozilla Firefox\firefox.exe");
                if (File.Exists(ff_path))
                    browsers.Add(new Browser()
                    {
                        name = "FireFox",
                        exec = ff_path,
                        icon = icon2String(IconExtractor.fromFile(ff_path))
                    });
                //special case , Edge
                var edge_path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    @"SystemApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\MicrosoftEdge.exe");
                if (File.Exists(edge_path))
                    browsers.Add(new Browser()
                    {
                        name = "Edge",
                        // #34
                        exec = "shell:AppsFolder\\Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge",
                        icon = icon2String(IconExtractor.fromFile(edge_path))
                    });

                //gather browsers from registry
                using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                    browsers.AddRange(find(hklm));
                using (RegistryKey hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
                    browsers.AddRange(find(hkcu));

                if (Environment.Is64BitOperatingSystem)
                {
                    using (RegistryKey hklm64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                        browsers.AddRange(find(hklm64));
                    using (RegistryKey hkcu64 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                        browsers.AddRange(find(hkcu64));
                }

                AddStandaloneChromium(browsers);

                //remove myself
                browsers = browsers.Where(x => Path.GetFileName(x.exec).ToLower() !=
                     Path.GetFileName(Application.ExecutablePath).ToLower()).ToList();
                //remove duplicates
                browsers = browsers.GroupBy(browser => browser.exec)
                    .Select(group => group.First()).ToList();

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                AddChromiumProfiles(browsers, new[] { "Microsoft Edge" },
                    Path.Combine(localAppData, @"Microsoft\Edge\User Data"), "Edge Profile.ico");
                AddChromiumProfiles(browsers, new[] { "Google Chrome" },
                    Path.Combine(localAppData, @"Google\Chrome\User Data"), "Google Profile.ico");
                AddChromiumProfiles(browsers, new[] { "Brave", "Brave Browser" },
                    Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data"), "Brave Profile.ico");
                AddChromiumProfiles(browsers, new[] { "Chromium" },
                    Path.Combine(localAppData, @"Chromium\User Data"), "Google Profile.ico");

                System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(browsers));
                Properties.Settings.Default.BrowserList = JsonConvert.SerializeObject(browsers);
                Properties.Settings.Default.Save();
            }

            return browsers;
        }

        private static void AddStandaloneChromium(List<Browser> browsers)
        {
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var candidates = new[]
            {
                Path.Combine(programFiles, @"Chromium\Application\chrome.exe"),
                Path.Combine(programFiles, @"Chromium\chrome.exe"),
                Path.Combine(programFilesX86, @"Chromium\Application\chrome.exe"),
                Path.Combine(programFilesX86, @"Chromium\chrome.exe"),
                Path.Combine(localAppData, @"Chromium\Application\chrome.exe"),
                Path.Combine(localAppData, @"Chromium\chrome.exe")
            };

            foreach (string path in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!File.Exists(path) || browsers.Any(x =>
                    string.Equals(x.exec, path, StringComparison.OrdinalIgnoreCase)))
                    continue;

                browsers.Add(new Browser
                {
                    name = "Chromium",
                    exec = path,
                    icon = icon2String(IconExtractor.fromFile(path))
                });
            }
        }

        private static void AddChromiumProfiles(
            List<Browser> browsers, string[] browserNames, string userDataDirectory, string profileIconFile)
        {
            Browser baseBrowser = browsers.FirstOrDefault(x => browserNames.Contains(x.name,
                StringComparer.OrdinalIgnoreCase));
            if (baseBrowser == null || !Directory.Exists(userDataDirectory))
                return;

            var profiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string localStatePath = Path.Combine(userDataDirectory, "Local State");
            try
            {
                if (File.Exists(localStatePath))
                {
                    JObject localState = JObject.Parse(File.ReadAllText(localStatePath));
                    JObject infoCache = localState.SelectToken("profile.info_cache") as JObject;
                    if (infoCache != null)
                    {
                        foreach (JProperty profile in infoCache.Properties())
                        {
                            if (!Directory.Exists(Path.Combine(userDataDirectory, profile.Name)))
                                continue;
                            string name = (string)profile.Value["name"];
                            profiles[profile.Name] = string.IsNullOrWhiteSpace(name) ? profile.Name : name;
                        }
                    }
                }
            }
            catch
            {
                // Profile directory discovery below is the fallback when Local State is unavailable.
            }

            string[] profileDirectories;
            try
            {
                profileDirectories = Directory.GetDirectories(userDataDirectory);
            }
            catch
            {
                profileDirectories = new string[0];
            }

            foreach (string directory in profileDirectories)
            {
                string profileDirectory = Path.GetFileName(directory);
                if (profileDirectory.Equals("Default", StringComparison.OrdinalIgnoreCase) ||
                    profileDirectory.StartsWith("Profile ", StringComparison.OrdinalIgnoreCase))
                {
                    if (!profiles.ContainsKey(profileDirectory))
                        profiles[profileDirectory] = ReadProfileName(directory, profileDirectory);
                }
            }

            if (profiles.Count <= 1)
                return;

            foreach (var profile in profiles)
            {
                string directory = Path.Combine(userDataDirectory, profile.Key);
                string profileIconPath = Path.Combine(directory, profileIconFile);
                string icon = File.Exists(profileIconPath)
                    ? icon2String(IconExtractor.fromFile(profileIconPath))
                    : baseBrowser.icon;

                browsers.Add(new Browser
                {
                    name = baseBrowser.name + " (" + profile.Value + ")",
                    exec = baseBrowser.exec,
                    icon = icon,
                    additionalArgs = "--profile-directory=" + profile.Key
                });
            }

            browsers.Remove(baseBrowser);
        }

        private static string ReadProfileName(string profileDirectory, string fallback)
        {
            string preferencesPath = Path.Combine(profileDirectory, "Preferences");
            try
            {
                if (File.Exists(preferencesPath))
                {
                    JObject preferences = JObject.Parse(File.ReadAllText(preferencesPath));
                    string name = (string)preferences.SelectToken("profile.name");
                    if (!string.IsNullOrWhiteSpace(name))
                        return name;
                }
            }
            catch { }
            return fallback;
        }

        private static List<Browser> find(RegistryKey hklm)
        {
            List<Browser> browsers = new List<Browser>();
            // startmenu internet key
            RegistryKey smi = hklm.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            if (smi != null)
                foreach (var browser in smi.GetSubKeyNames())
                {
                    try
                    {
                        var key = smi.OpenSubKey(browser);
                        var name = (string)key.GetValue(null);
                        var cmd = key.OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command");
                        var exec = (string)cmd.GetValue(null);

                        // by this point if registry is missing keys we are alreay out of here
                        // because of the try catch, but there are still things that could go wrong

                        //0. check if it can handle the http protocol
                        var capabilities = key.OpenSubKey("Capabilities");
                        // IE does not have the capabilities subkey...
                        // so assume that the app can handle http if it doesn't
                        // advertise it's capablities
                        if (capabilities != null)
                            if ((string)capabilities.OpenSubKey("URLAssociations").GetValue("http") == null)
                                continue;
                        //1. check if path is not empty
                        if (string.IsNullOrWhiteSpace(exec))
                            continue;

                        //1.1. remove possible "%1" from the end
                        exec = exec.Replace("\"%1\"", "");
                        //1.2. remove possible quotes around address
                        exec = exec.Trim("\"".ToCharArray());
                        //2. check if path is valid
                        if (!File.Exists(exec))
                            continue;
                        //3. check if name is valid
                        if (string.IsNullOrWhiteSpace(name))
                            name = Path.GetFileNameWithoutExtension(exec);

                        browsers.Add(new Browser()
                        {
                            name = name,
                            exec = exec,
                            icon = icon2String(IconExtractor.fromFile(exec))
                        });
                    }
                    catch (NullReferenceException)
                    {
                    } // incomplete registry record for browser, ignore it
                    catch (Exception ex)
                    {
                        // todo: log errors
                    }
                }
            return browsers;
        }
    }
}
