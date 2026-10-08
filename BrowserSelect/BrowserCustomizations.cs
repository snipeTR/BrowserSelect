using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BrowserSelect.Properties;
using Newtonsoft.Json;

namespace BrowserSelect
{
    /// <summary>User customizations of a (detected or portable) browser, keyed by Browser.Identifier.</summary>
    public class BrowserOverride
    {
        /// <summary>custom icon as base64 PNG (null = use the browser's own icon)</summary>
        public string icon;
        /// <summary>custom keyboard shortcut characters (null/empty = automatic)</summary>
        public string shortcut;
        /// <summary>extra command line arguments always passed to a detected browser</summary>
        public string args;

        [JsonIgnore]
        public bool IsEmpty => string.IsNullOrEmpty(icon) && string.IsNullOrEmpty(shortcut) &&
                               string.IsNullOrEmpty(args);
    }

    /// <summary>
    /// Persists the browser list customizations (portable browsers, custom icons/shortcuts,
    /// manual order and usage counters) as JSON strings in the user settings, separately from the
    /// cached list of detected browsers so that "Refresh" never loses them.
    /// </summary>
    public static class BrowserCustomizations
    {
        public const string SortManual = "Manual";
        public const string SortAlphabetical = "Alphabetical";
        public const string SortMostUsed = "Most used";
        public static readonly string[] SortModes = { SortManual, SortAlphabetical, SortMostUsed };

        private static T Load<T>(string json) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(json))
                return new T();
            try
            {
                return JsonConvert.DeserializeObject<T>(json) ?? new T();
            }
            catch (JsonException)
            {
                return new T();
            }
        }

        // ---- portable / manually added browsers

        public static List<Browser> LoadCustomBrowsers()
        {
            var list = Load<List<Browser>>(Settings.Default.CustomBrowsers);
            list.RemoveAll(b => b == null || string.IsNullOrEmpty(b.exec));
            foreach (var b in list)
            {
                b.isCustom = true;
                if (b.additionalArgs == null)
                    b.additionalArgs = "";
            }
            return list;
        }

        public static void SaveCustomBrowsers(List<Browser> browsers)
        {
            Settings.Default.CustomBrowsers = JsonConvert.SerializeObject(browsers);
            Settings.Default.Save();
        }

        // ---- icon / shortcut / argument overrides

        public static Dictionary<string, BrowserOverride> LoadOverrides()
        {
            return new Dictionary<string, BrowserOverride>(
                Load<Dictionary<string, BrowserOverride>>(Settings.Default.BrowserOverrides),
                StringComparer.OrdinalIgnoreCase);
        }

        public static void SaveOverrides(Dictionary<string, BrowserOverride> overrides)
        {
            var clean = overrides.Where(x => x.Value != null && !x.Value.IsEmpty)
                .ToDictionary(x => x.Key, x => x.Value);
            Settings.Default.BrowserOverrides = clean.Count == 0 ? "" : JsonConvert.SerializeObject(clean);
            Settings.Default.Save();
        }

        public static BrowserOverride GetOverride(string identifier)
        {
            BrowserOverride o;
            return LoadOverrides().TryGetValue(identifier, out o) && o != null ? o : new BrowserOverride();
        }

        public static void SetOverride(string identifier, BrowserOverride value)
        {
            var overrides = LoadOverrides();
            if (value == null || value.IsEmpty)
                overrides.Remove(identifier);
            else
                overrides[identifier] = value;
            SaveOverrides(overrides);
        }

        // ---- order / usage

        public static List<string> LoadOrder()
        {
            return Load<List<string>>(Settings.Default.BrowserOrder);
        }

        public static void SaveOrder(IEnumerable<string> identifiers)
        {
            Settings.Default.BrowserOrder = JsonConvert.SerializeObject(identifiers.ToList());
            Settings.Default.Save();
        }

        public static Dictionary<string, int> LoadUsage()
        {
            return new Dictionary<string, int>(Load<Dictionary<string, int>>(Settings.Default.BrowserUsage),
                StringComparer.OrdinalIgnoreCase);
        }

        public static void IncrementUsage(Browser b)
        {
            try
            {
                var usage = LoadUsage();
                int count;
                usage.TryGetValue(b.Identifier, out count);
                usage[b.Identifier] = count + 1;
                Settings.Default.BrowserUsage = JsonConvert.SerializeObject(usage);
                Settings.Default.Save();
            }
            catch
            {
                // usage statistics must never prevent a link from opening
            }
        }

        /// <summary>moves all per-browser data from one identifier to another (e.g. arguments changed)</summary>
        public static void RenameIdentifier(string oldId, string newId)
        {
            if (string.Equals(oldId, newId, StringComparison.OrdinalIgnoreCase))
                return;
            var overrides = LoadOverrides();
            BrowserOverride o;
            if (overrides.TryGetValue(oldId, out o))
            {
                overrides.Remove(oldId);
                overrides[newId] = o;
                SaveOverrides(overrides);
            }
            var order = LoadOrder();
            int i = order.FindIndex(x => string.Equals(x, oldId, StringComparison.OrdinalIgnoreCase));
            if (i >= 0)
            {
                order[i] = newId;
                SaveOrder(order);
            }
            var usage = LoadUsage();
            int count;
            if (usage.TryGetValue(oldId, out count))
            {
                usage.Remove(oldId);
                usage[newId] = count;
                Settings.Default.BrowserUsage = JsonConvert.SerializeObject(usage);
            }
            if (Settings.Default.HideBrowsers.Contains(oldId))
            {
                Settings.Default.HideBrowsers.Remove(oldId);
                Settings.Default.HideBrowsers.Add(newId);
            }
            Settings.Default.Save();
        }

        /// <summary>applies icon/shortcut/argument overrides to a list of browsers (in place)</summary>
        public static void Apply(List<Browser> browsers)
        {
            var overrides = LoadOverrides();
            foreach (var b in browsers)
            {
                b.defaultIcon = b.icon;
                BrowserOverride o;
                if (!overrides.TryGetValue(b.Identifier, out o) || o == null)
                    continue;
                if (!string.IsNullOrEmpty(o.icon))
                    b.icon = o.icon;
                b.customShortcut = o.shortcut ?? "";
                b.extraArgs = o.args ?? "";
            }
        }

        /// <summary>sorts the browsers according to the selected sort mode</summary>
        public static List<Browser> Sort(List<Browser> browsers)
        {
            var order = LoadOrder();
            Func<Browser, int> manualIndex = b =>
            {
                int i = order.FindIndex(x => string.Equals(x, b.Identifier, StringComparison.OrdinalIgnoreCase));
                return i < 0 ? int.MaxValue : i;
            };
            // OrderBy is stable, so browsers missing from the saved order keep their detection order
            var manual = browsers.OrderBy(manualIndex).ToList();

            switch (Settings.Default.SortMode)
            {
                case SortAlphabetical:
                    return manual.OrderBy(b => b.name, StringComparer.CurrentCultureIgnoreCase).ToList();
                case SortMostUsed:
                    var usage = LoadUsage();
                    return manual.OrderByDescending(b =>
                    {
                        int c;
                        return usage.TryGetValue(b.Identifier, out c) ? c : 0;
                    }).ToList();
                default:
                    return manual;
            }
        }

        // ---- icons

        /// <summary>loads an icon from an .ico/.exe/.dll or image file and returns it as base64 PNG</summary>
        public static string IconFromFile(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (ext == ".ico" || ext == ".exe" || ext == ".dll")
            {
                var icon = IconExtractor.fromFile(path);
                if (icon == null)
                    throw new InvalidDataException("No icon found in " + path);
                using (icon)
                using (var bmp = icon.ToBitmap())
                    return ImageToString(bmp);
            }
            using (var stream = new MemoryStream(File.ReadAllBytes(path)))
            using (var image = Image.FromStream(stream))
                return ImageToString(image);
        }

        /// <summary>returns the default icon of an executable as base64 PNG (generic icon if none)</summary>
        public static string IconFromExecutable(string path)
        {
            try
            {
                var icon = IconExtractor.fromFile(path);
                if (icon != null)
                    return BrowserFinder.icon2String(icon);
            }
            catch
            {
                // fall through to the generic application icon
            }
            return BrowserFinder.icon2String(SystemIcons.Application);
        }

        /// <summary>scales an image to fit 128x128 (keeping aspect ratio) and encodes it as base64 PNG</summary>
        public static string ImageToString(Image image)
        {
            const int size = 128;
            using (var bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    float scale = Math.Min((float)size / image.Width, (float)size / image.Height);
                    float w = image.Width * scale, h = image.Height * scale;
                    g.DrawImage(image, (size - w) / 2, (size - h) / 2, w, h);
                }
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
}
