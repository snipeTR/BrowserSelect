using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace BrowserSelect.Localization
{
    /// <summary>
    /// Localization infrastructure.
    ///
    /// All user interface texts come from one file: Localization\Strings.resx (English, the default and
    /// fallback language). A translation is added as Localization\Strings.&lt;culture&gt;.resx (for example
    /// Strings.tr.resx) containing the same keys; MSBuild compiles it into a satellite assembly
    /// (&lt;culture&gt;\BrowserSelect.resources.dll next to BrowserSelect.exe). Every language found that way
    /// is listed automatically in the language drop-down of the settings window.
    ///
    /// Only the UI culture (CurrentUICulture) is changed. CurrentCulture (number/date parsing) is left
    /// alone on purpose so that settings, timestamps and rules keep working exactly as before.
    /// </summary>
    public static class L10n
    {
        /// <summary>language of Strings.resx; used when nothing else is selected or available</summary>
        public const string DefaultLanguage = "en";

        /// <summary>
        /// Text for <paramref name="key"/> in the current UI language. Falls back to English (Strings.resx)
        /// when the key is not translated, and to the key itself if it does not exist at all, so a missing
        /// entry can never crash the application.
        /// </summary>
        public static string T(string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;
            try
            {
                return Strings.ResourceManager.GetString(key, Strings.Culture) ?? key;
            }
            catch (Exception)
            {
                return key;
            }
        }

        /// <summary><see cref="T(string)"/> followed by string.Format with the given arguments.</summary>
        public static string T(string key, params object[] args)
        {
            var text = T(key);
            try
            {
                return string.Format(text, args);
            }
            catch (FormatException)
            {
                return text;
            }
        }

        /// <summary>
        /// Applies the language stored in the settings (an IETF tag such as "en"). Unknown, empty or
        /// not installed languages fall back to English.
        /// </summary>
        public static void Apply(string language)
        {
            var culture = Resolve(language);
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Strings.Culture = culture;
        }

        /// <summary>language tag currently used for the user interface</summary>
        public static string Current
        {
            get
            {
                var culture = Strings.Culture ?? Thread.CurrentThread.CurrentUICulture;
                return Available().Select(l => l.Code)
                    .FirstOrDefault(c => c.Equals(culture.Name, StringComparison.OrdinalIgnoreCase))
                    ?? DefaultLanguage;
            }
        }

        /// <summary>
        /// Languages that can be selected: English (built in) plus every culture that has a satellite
        /// resource assembly (&lt;culture&gt;\BrowserSelect.resources.dll) next to the executable.
        /// </summary>
        public static List<LanguageOption> Available()
        {
            var list = new List<LanguageOption> { new LanguageOption(CultureInfo.GetCultureInfo(DefaultLanguage)) };
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var satellite = typeof(L10n).Assembly.GetName().Name + ".resources.dll";
                foreach (var dir in Directory.GetDirectories(baseDir))
                {
                    if (!File.Exists(Path.Combine(dir, satellite)))
                        continue;
                    CultureInfo culture;
                    try
                    {
                        culture = CultureInfo.GetCultureInfo(Path.GetFileName(dir));
                    }
                    catch (CultureNotFoundException)
                    {
                        continue;
                    }
                    if (culture.Equals(CultureInfo.InvariantCulture) ||
                        list.Any(l => l.Code.Equals(culture.Name, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    list.Add(new LanguageOption(culture));
                }
            }
            catch (Exception)
            {
                // unreadable install folder: English is always available
            }
            return list;
        }

        private static CultureInfo Resolve(string language)
        {
            if (!string.IsNullOrWhiteSpace(language))
            {
                var match = Available().FirstOrDefault(l =>
                    l.Code.Equals(language.Trim(), StringComparison.OrdinalIgnoreCase));
                if (match != null)
                    return match.Culture;
            }
            return CultureInfo.GetCultureInfo(DefaultLanguage);
        }
    }

    /// <summary>an entry of the language drop-down</summary>
    public class LanguageOption
    {
        public LanguageOption(CultureInfo culture)
        {
            Culture = culture;
        }

        public CultureInfo Culture { get; private set; }

        /// <summary>IETF language tag stored in the settings, e.g. "en"</summary>
        public string Code
        {
            get { return Culture.Name; }
        }

        public override string ToString()
        {
            // e.g. "English (EN)", "Türkçe (TR)"
            var name = Culture.NativeName;
            if (name.Length > 0)
                name = char.ToUpper(name[0], Culture) + name.Substring(1);
            return name + " (" + Code.ToUpperInvariant() + ")";
        }
    }
}
