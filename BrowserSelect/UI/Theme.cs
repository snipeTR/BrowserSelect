using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using BrowserSelect.Localization;
using BrowserSelect.Properties;
using Microsoft.Win32;

namespace BrowserSelect.UI
{
    /// <summary>
    /// Central place for the appearance of all BrowserSelect windows (Windows 10/11 look).
    ///
    /// Only the appearance is changed: fonts, colors, flat buttons, grid/list styling and DWM window
    /// attributes (dark title bar, Windows 11 rounded corners, optional Mica). Positions, sizes and
    /// behaviour of the controls stay as designed. Call <see cref="Apply(Form)"/> right after
    /// InitializeComponent() (before texts are measured) and again after controls are added at run time.
    /// Calling it again is safe; it is also used to switch the theme while a window is open.
    ///
    /// Every step is wrapped so that a failure (older Windows, missing font, ...) silently leaves the
    /// classic look instead of breaking the window.
    /// </summary>
    public static class Theme
    {
        // ---- settings -------------------------------------------------------------------------

        public const string ModeLight = "Light";
        public const string ModeDark = "Dark";
        public const string ModeSystem = "System";
        public static readonly string[] Modes = { ModeLight, ModeDark, ModeSystem };

        /// <summary>theme stored in the settings ("Light", "Dark" or "System"); unknown values = Light</summary>
        public static string Mode
        {
            get
            {
                try
                {
                    var mode = Settings.Default.Theme;
                    foreach (var m in Modes)
                        if (string.Equals(m, mode, StringComparison.OrdinalIgnoreCase))
                            return m;
                }
                catch (Exception) { }
                return ModeLight;
            }
        }

        /// <summary>Windows 11 Mica backdrop on the title bar (setting; ignored on Windows 10)</summary>
        public static bool UseMica
        {
            get
            {
                try { return Settings.Default.Mica; }
                catch (Exception) { return false; }
            }
        }

        /// <summary>true if the dark palette is used (Dark, or System while Windows apps use dark mode)</summary>
        public static bool IsDark
        {
            get
            {
                var mode = Mode;
                if (mode == ModeDark)
                    return true;
                if (mode == ModeSystem)
                    return SystemUsesDarkTheme();
                return false;
            }
        }

        /// <summary>Settings &gt; Personalization &gt; Colors &gt; "Choose your default app mode" is Dark</summary>
        public static bool SystemUsesDarkTheme()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    var value = key?.GetValue("AppsUseLightTheme");
                    return value is int && (int)value == 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ---- palette --------------------------------------------------------------------------

        public sealed class Palette
        {
            public Color Back, Text, SubtleText, Field, FieldText, Border, Separator;
            public Color ButtonBack, ButtonHover, ButtonPressed, ButtonBorder;
            public Color Header, Grid, Selection, SelectionText, Link, Accent, AccentText;
        }

        /// <summary>Windows 11 light colors</summary>
        public static readonly Palette Light = new Palette
        {
            Back = Color.FromArgb(243, 243, 243),
            Text = Color.FromArgb(27, 27, 27),
            SubtleText = Color.FromArgb(96, 96, 96),
            Field = Color.White,
            FieldText = Color.FromArgb(27, 27, 27),
            Border = Color.FromArgb(209, 209, 209),
            Separator = Color.FromArgb(222, 222, 222),
            ButtonBack = Color.FromArgb(253, 253, 253),
            ButtonHover = Color.FromArgb(232, 241, 251),
            ButtonPressed = Color.FromArgb(204, 228, 247),
            ButtonBorder = Color.FromArgb(204, 204, 204),
            Header = Color.FromArgb(246, 246, 246),
            Grid = Color.FromArgb(229, 229, 229),
            Selection = Color.FromArgb(204, 232, 255),
            SelectionText = Color.FromArgb(27, 27, 27),
            Link = Color.FromArgb(0, 95, 184),
            Accent = Color.FromArgb(0, 103, 192),
            AccentText = Color.White,
        };

        /// <summary>Windows 11 dark colors</summary>
        public static readonly Palette Dark = new Palette
        {
            Back = Color.FromArgb(32, 32, 32),
            Text = Color.FromArgb(240, 240, 240),
            SubtleText = Color.FromArgb(160, 160, 160),
            Field = Color.FromArgb(43, 43, 43),
            FieldText = Color.FromArgb(240, 240, 240),
            Border = Color.FromArgb(70, 70, 70),
            Separator = Color.FromArgb(61, 61, 61),
            ButtonBack = Color.FromArgb(45, 45, 45),
            ButtonHover = Color.FromArgb(58, 58, 58),
            ButtonPressed = Color.FromArgb(70, 70, 70),
            ButtonBorder = Color.FromArgb(80, 80, 80),
            Header = Color.FromArgb(50, 50, 50),
            Grid = Color.FromArgb(64, 64, 64),
            Selection = Color.FromArgb(38, 79, 120),
            SelectionText = Color.White,
            Link = Color.FromArgb(96, 205, 255),
            Accent = Color.FromArgb(76, 194, 255),
            AccentText = Color.Black,
        };

        public static Palette Current
        {
            get { return IsDark ? Dark : Light; }
        }

        // ---- fonts ----------------------------------------------------------------------------

        /// <summary>Latin UI font of Windows 10/11; used for every language without its own entry below</summary>
        public const string LatinFontName = "Segoe UI";
        public const float FontSize = 9f;
        private const float ClassicFontSize = 8.25f;
        /// <summary>Japanese and Chinese text is never drawn smaller than this</summary>
        public const float MinCjkFontSize = 9f;

        /// <summary>
        /// UI font candidates per UI language, tried from left to right; the first installed one is used,
        /// Segoe UI if none is. The keys are matched against the selected UI culture and its parents
        /// (zh-CN -> zh-Hans, zh-TW -> zh-Hant, ...). Only the font name is used: no font files are shipped,
        /// these fonts come with Windows and may not be redistributed.
        /// </summary>
        private static readonly KeyValuePair<string, string[]>[] FontCandidates =
        {
            new KeyValuePair<string, string[]>("ja", new[] { "Yu Gothic UI", "Meiryo UI" }),
            new KeyValuePair<string, string[]>("zh-Hans", new[] { "Microsoft YaHei UI", "Microsoft YaHei" }),
            new KeyValuePair<string, string[]>("zh-CN", new[] { "Microsoft YaHei UI", "Microsoft YaHei" }),
            new KeyValuePair<string, string[]>("zh-SG", new[] { "Microsoft YaHei UI", "Microsoft YaHei" }),
            new KeyValuePair<string, string[]>("zh-Hant", new[] { "Microsoft JhengHei UI", "Microsoft JhengHei" }),
            new KeyValuePair<string, string[]>("zh-TW", new[] { "Microsoft JhengHei UI", "Microsoft JhengHei" }),
            new KeyValuePair<string, string[]>("zh-HK", new[] { "Microsoft JhengHei UI", "Microsoft JhengHei" }),
            new KeyValuePair<string, string[]>("zh-MO", new[] { "Microsoft JhengHei UI", "Microsoft JhengHei" }),
        };

        private static HashSet<string> _installedFonts;
        private static string _fontCulture;
        private static string _fontName;
        private static bool _isCjk;

        /// <summary>font families installed on this computer (read once)</summary>
        private static bool IsInstalled(string family)
        {
            if (_installedFonts == null)
            {
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                try
                {
                    using (var fonts = new InstalledFontCollection())
                        foreach (var f in fonts.Families)
                            set.Add(f.Name);
                }
                catch (Exception) { }
                _installedFonts = set;
            }
            if (_installedFonts.Contains(family))
                return true;
            // a family the collection does not list (e.g. a face of a .ttc): GDI+ substitutes missing
            // fonts, so it is installed if the created font keeps the requested name
            try
            {
                using (var font = new Font(family, FontSize, FontStyle.Regular, GraphicsUnit.Point))
                    return string.Equals(font.Name, family, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The one place the UI font is chosen: the first installed candidate for the selected UI language
        /// (L10n / Strings.Culture, not the Windows language), otherwise Segoe UI. A language change takes
        /// effect on the next start, the result is still re-evaluated whenever the UI culture changes.
        /// </summary>
        private static void ResolveFont()
        {
            CultureInfo culture;
            try
            {
                culture = Strings.Culture ?? Thread.CurrentThread.CurrentUICulture;
            }
            catch (Exception)
            {
                culture = CultureInfo.InvariantCulture;
            }
            var key = culture.Name;
            if (_fontName != null && key == _fontCulture)
                return;
            var name = LatinFontName;
            var cjk = false;
            try
            {
                string[] candidates = null;
                for (var c = culture; c != null && !c.Equals(CultureInfo.InvariantCulture) && candidates == null; c = c.Parent)
                    foreach (var entry in FontCandidates)
                        if (string.Equals(entry.Key, c.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            candidates = entry.Value;
                            break;
                        }
                if (candidates != null)
                {
                    cjk = true;
                    name = candidates.FirstOrDefault(IsInstalled) ?? LatinFontName;
                }
            }
            catch (Exception) { }
            _fontName = name;
            _isCjk = cjk;
            _fontCulture = key;
        }

        /// <summary>UI font family for the selected UI language (Segoe UI, Yu Gothic UI, Microsoft YaHei UI, ...)</summary>
        public static string FontName
        {
            get
            {
                ResolveFont();
                return _fontName;
            }
        }

        /// <summary>true if the UI language is Japanese or Chinese (min. 9pt, no italics, no shrinking)</summary>
        public static bool IsCjk
        {
            get
            {
                ResolveFont();
                return _isCjk;
            }
        }

        /// <summary>
        /// UI font for code that sets a font itself (always use this instead of a font name). For Japanese
        /// and Chinese the size is at least 9pt and italic becomes bold (these fonts have no real italic).
        /// <paramref name="latinOnlyText"/>: the control only ever shows this ASCII text (digits, Latin
        /// letters), so a CJK UI language may keep the Latin font at the requested size.
        /// Returns null if no suitable font can be created.
        /// </summary>
        public static Font CreateUiFont(float size, FontStyle style, string latinOnlyText = null)
        {
            if (IsCjk && latinOnlyText != null && latinOnlyText.All(ch => ch < 128))
                return CreateFont(LatinFontName, size, style);
            return CreateFont(size, style);
        }

        /// <summary>true if a font family is installed (used for the Fluent icon fonts)</summary>
        internal static bool IsFontInstalled(string family)
        {
            return IsInstalled(family);
        }

        /// <summary>
        /// larger title font (Settings page headers): Segoe UI Semibold for Latin UI languages, the bold UI font
        /// for Japanese/Chinese (their UI fonts have no semibold face); null if no font can be created
        /// </summary>
        public static Font CreateTitleFont(float size)
        {
            if (!IsCjk && IsInstalled("Segoe UI Semibold"))
            {
                var semibold = CreateFont("Segoe UI Semibold", size, FontStyle.Regular);
                if (semibold != null)
                    return semibold;
            }
            return CreateFont(size, FontStyle.Bold);
        }

        private static Font _baseFont;
        private static string _baseFontName;

        /// <summary>UI font at 9pt (Segoe UI, or the Japanese/Chinese UI font); null if not installed</summary>
        public static Font BaseFont
        {
            get
            {
                if (_baseFont == null || _baseFontName != FontName)
                {
                    _baseFont = CreateFont(FontSize, FontStyle.Regular);
                    _baseFontName = FontName;
                }
                return _baseFont;
            }
        }

        private static Font CreateFont(float size, FontStyle style)
        {
            if (IsCjk)
            {
                size = Math.Max(size, MinCjkFontSize);
                if ((style & FontStyle.Italic) != 0)
                    style = (style & ~FontStyle.Italic) | FontStyle.Bold;
            }
            return CreateFont(FontName, size, style);
        }

        private static Font CreateFont(string family, float size, FontStyle style)
        {
            try
            {
                var font = new Font(family, size, style, GraphicsUnit.Point);
                if (string.Equals(font.Name, family, StringComparison.OrdinalIgnoreCase))
                    return font;
                font.Dispose();
            }
            catch (Exception) { }
            return null;
        }

        private static bool IsModernFont(Font font)
        {
            // Segoe UI also counts for Japanese/Chinese: set in code only for Latin-only text (CreateUiFont)
            return font != null && (string.Equals(font.Name, FontName, StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(font.Name, LatinFontName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// The UI font (see FontName) is set on the individual controls, never on the window itself: a font change on a
        /// window (or user control) with AutoScaleMode.Font would rescale the whole layout and make the
        /// windows bigger. This way every window keeps its exact size and control positions.
        /// Fixed-size buttons and labels are checked whenever their text changes (texts are translated):
        /// if the text does not fit in Segoe UI 9pt, Segoe UI 8.25pt is used, and if that does not fit
        /// either, the original font is kept, so nothing is clipped by the new font.
        /// Japanese/Chinese: never smaller than 9pt; a label that does not fit uses the free space around
        /// it instead (see GrowLabel), and the 9pt font is kept even if it does not fit (the original
        /// 8.25pt font would be smaller than 9pt).
        /// </summary>
        private static void ApplyFonts(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // children first: they get their own (explicit) font before a group box caption changes
                // the font they would otherwise inherit
                if (c.HasChildren && !(c is DataGridView))
                    ApplyFonts(c);
                if (!(c is ContainerControl) && !(c is IFluentOwnFont))
                    SetModernFont(c);
            }
        }

        private static void SetModernFont(Control c)
        {
            var info = Info(c);
            if (info.Original == null)
            {
                if (IsModernFont(c.Font))
                    return; // already the UI font (inherited from a themed parent or set in code)
                info.Original = c.Font;
                var classic = Math.Abs(c.Font.SizeInPoints - ClassicFontSize) < 0.1f;
                info.Preferred = CreateFont(classic ? FontSize : c.Font.SizeInPoints, c.Font.Style);
                // Japanese/Chinese: no smaller fallback (min. 9pt)
                info.Smaller = classic && !IsCjk ? CreateFont(ClassicFontSize, c.Font.Style) : null;
                if (info.Preferred == null)
                    return;
                if (NeedsFitting(c))
                    c.TextChanged += (s, e) => FitFont((Control)s);
            }
            if (info.Preferred == null)
                return;
            if (NeedsFitting(c))
                FitFont(c);
            else
                c.Font = info.Preferred;
        }

        /// <summary>fixed-size buttons and labels can clip a bigger font</summary>
        private static bool NeedsFitting(Control c)
        {
            if (c is VButton)
                return false; // vertical text, sized for it
            if (c is Button)
                return !((Button)c).AutoSize;
            if (c is Label)
                return !((Label)c).AutoSize;
            return false;
        }

        private static void FitFont(Control c)
        {
            try
            {
                var info = Info(c);
                if (info.Preferred == null)
                    return;
                // always assigned (no-op if unchanged) so the font is set on the control itself and not
                // inherited from a parent whose font changes later
                foreach (var font in new[] { info.Preferred, info.Smaller })
                {
                    if (font != null && Fits(c, font))
                    {
                        c.Font = font;
                        return;
                    }
                }
                // once the window has its final (DPI scaled) size: a label may use free space below it
                var cjk = IsCjk;
                if (info.Laidout && c is Label)
                {
                    // Japanese/Chinese: only the 9pt font may grow, the original font can be below 9pt
                    var candidates = cjk ? new[] { info.Preferred } : new[] { info.Preferred, info.Smaller, info.Original };
                    foreach (var font in candidates)
                    {
                        if (font != null && GrowLabel((Label)c, font))
                        {
                            c.Font = font;
                            return;
                        }
                    }
                }
                // Japanese/Chinese keep the 9pt UI font (the original font may be smaller than 9pt and
                // has no Japanese/Chinese glyphs of its own)
                c.Font = cjk ? info.Preferred : info.Original;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// makes a fixed-size label taller if its text needs more lines than it has room for and the
        /// space below it (up to the next control or the bottom of its parent) is free; if that is not
        /// enough, it also moves up into the free space above it
        /// </summary>
        private static bool GrowLabel(Label label, Font font)
        {
            var parent = label.Parent;
            if (parent == null || label.AutoEllipsis || label.Dock != DockStyle.None)
                return false;
            var needed = LabelTextHeight(label, font) + label.Padding.Vertical + (label.Height - label.ClientSize.Height);
            if (needed <= label.Height)
                return true;
            var limit = parent.ClientSize.Height - (parent is GroupBox ? Px(3) : 0);
            var top = parent is GroupBox ? Px(14) : 0;
            foreach (Control other in parent.Controls)
            {
                if (other == label || !other.Visible)
                    continue;
                var b = other.Bounds;
                if (b.Right <= label.Left || b.Left >= label.Right)
                    continue;
                if (b.Top >= label.Top + 1)
                    limit = Math.Min(limit, b.Top - 1);
                else if (b.Bottom <= label.Top)
                    top = Math.Max(top, b.Bottom + 1);
            }
            if (limit - top < needed)
                return false;
            // grow downwards, and move up a little if the space below is not enough
            var newTop = Math.Min(label.Top, limit - needed);
            label.SetBounds(label.Left, newTop, label.Width, needed);
            return true;
        }

        private static int LabelTextHeight(Label label, Font font)
        {
            var text = label.Text ?? "";
            var width = Math.Max(1, label.ClientSize.Width - label.Padding.Horizontal - 6);
            if (label.UseCompatibleTextRendering)
            {
                using (var g = label.CreateGraphics())
                    return (int)Math.Ceiling(g.MeasureString(text, font, width).Height);
            }
            return TextRenderer.MeasureText(text, font, new Size(width, int.MaxValue),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl).Height;
        }

        /// <summary>
        /// the window got its final size (WinForms scales it to the display scale before Load): keep it on
        /// the screen
        /// </summary>
        private static void Form_Load(object sender, EventArgs e)
        {
            FitToScreen((Form)sender);
        }

        private static void Refit(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.HasChildren && !(c is DataGridView))
                    Refit(c);
                ControlInfo info;
                if (Infos.TryGetValue(c, out info) && info.Preferred != null && NeedsFitting(c))
                {
                    info.Laidout = true;
                    FitFont(c);
                }
            }
        }

        /// <summary>
        /// a window bigger than the screen (e.g. Settings at 175 % on a 1920x1080 screen) gets scroll bars
        /// instead of controls hidden below the taskbar; the layout inside stays the same
        /// </summary>
        private static void FitToScreen(Form form)
        {
            try
            {
                var area = Screen.FromPoint(Cursor.Position).WorkingArea;
                var size = form.Size;
                if (size.Width > area.Width || size.Height > area.Height)
                {
                    var client = form.ClientSize;
                    var fitted = new Size(Math.Min(size.Width, area.Width), Math.Min(size.Height, area.Height));
                    form.MinimumSize = Size.Empty;
                    // a window with a resizable (anchored) layout just becomes smaller as long as it stays above its
                    // layout minimum (e.g. Settings at 200 % on a 1600 px wide screen); scroll bars only below that
                    var scroll = true;
                    object minimum;
                    if (LayoutMinimums.TryGetValue(form, out minimum))
                    {
                        var m = (Size)minimum;
                        var fittedClient = new Size(fitted.Width - (size.Width - client.Width), fitted.Height - (size.Height - client.Height));
                        scroll = fittedClient.Width < m.Width || fittedClient.Height < m.Height;
                    }
                    if (scroll)
                    {
                        form.AutoScrollMinSize = client;
                        form.AutoScroll = true;
                    }
                    form.Size = fitted;
                }
                KeepOnScreen(form);
            }
            catch (Exception) { }
        }

        private static readonly ConditionalWeakTable<Form, object> LayoutMinimums = new ConditionalWeakTable<Form, object>();

        /// <summary>
        /// smallest client size (in current pixels) at which the anchored layout of a window still works; a window
        /// bigger than the screen is made smaller down to this size before it gets scroll bars
        /// </summary>
        public static void SetLayoutMinimum(Form form, Size clientSize)
        {
            try
            {
                LayoutMinimums.Remove(form);
                LayoutMinimums.Add(form, clientSize);
            }
            catch (Exception) { }
        }

        private static void KeepOnScreen(Form form)
        {
            try
            {
                var area = Screen.FromControl(form).WorkingArea;
                var b = form.Bounds;
                var x = Math.Max(area.Left, Math.Min(b.Left, area.Right - b.Width));
                var y = Math.Max(area.Top, Math.Min(b.Top, area.Bottom - b.Height));
                if (x != b.Left || y != b.Top)
                    form.Location = new Point(x, y);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// the window is shown with its final (DPI scaled) layout: fit the fixed-size texts again for the
        /// real control sizes (labels may use free space around them, which needs the controls visible)
        /// </summary>
        private static void Form_Shown(object sender, EventArgs e)
        {
            var form = (Form)sender;
            try
            {
                Refit(form);
            }
            catch (Exception) { }
            KeepOnScreen(form);
        }

        private static bool Fits(Control c, Font font)
        {
            var text = c.Text ?? "";
            if (c is Button)
            {
                // flat buttons lay out their text inside the border with some padding; keep a safe margin
                var size = TextRenderer.MeasureText(text.Length > 0 ? text : "Ag", font);
                if (c.Height < Px(SmallButtonHeight))
                    return size.Width + 6 <= c.Width && size.Height + 2 <= c.Height;
                return size.Width + 16 <= c.Width && size.Height + 6 <= c.Height;
            }
            var label = (Label)c;
            var lineHeight = TextRenderer.MeasureText("Ag(", font).Height;
            if (lineHeight > label.Height)
                return false;
            if (label.AutoEllipsis || text.Length == 0)
                return true;
            // a few pixels narrower than the label: the label wraps words a little earlier than the measurement
            var width = Math.Max(1, label.ClientSize.Width - label.Padding.Horizontal - 6);
            int neededHeight;
            if (label.UseCompatibleTextRendering)
            {
                using (var g = label.CreateGraphics())
                    neededHeight = (int)Math.Ceiling(g.MeasureString(text, font, width).Height);
            }
            else
            {
                neededHeight = TextRenderer.MeasureText(text, font, new Size(width, int.MaxValue),
                    TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl).Height;
            }
            return neededHeight <= label.ClientSize.Height - label.Padding.Vertical;
        }

        // ---- colors ---------------------------------------------------------------------------

        private enum Role { Normal, Subtle, Separator, IconButton }

        /// <summary>what the theme remembers about a control (decided the first time it is themed)</summary>
        private sealed class ControlInfo
        {
            public bool RoleKnown;
            public Role Role;
            public Font Original, Preferred, Smaller;
            public bool PaintHooked;
            public bool Laidout;
        }

        private static readonly ConditionalWeakTable<Control, ControlInfo> Infos = new ConditionalWeakTable<Control, ControlInfo>();

        private static ControlInfo Info(Control c)
        {
            return Infos.GetValue(c, ctl => new ControlInfo());
        }

        private static Role GetRole(Control c)
        {
            var info = Info(c);
            if (!info.RoleKnown)
            {
                var role = Role.Normal;
                var label = c as Label;
                var button = c as Button;
                if (label != null && label.BorderStyle == BorderStyle.Fixed3D && label.Height <= 3)
                    role = Role.Separator;
                else if (c.ForeColor.IsKnownColor && c.ForeColor.ToKnownColor() == KnownColor.GrayText)
                    role = Role.Subtle;
                else if (button != null && button.FlatStyle == FlatStyle.Flat &&
                         button.FlatAppearance.BorderSize == 0 && button.BackgroundImage != null)
                    role = Role.IconButton;
                info.Role = role;
                info.RoleKnown = true;
            }
            return info.Role;
        }

        private static void ApplyColors(Control root, Palette p, bool dark)
        {
            if (root is Form)
            {
                root.BackColor = p.Back;
                root.ForeColor = p.Text;
            }
            foreach (Control c in root.Controls)
            {
                StyleControl(c, p, dark);
                if (c.HasChildren && !(c is DataGridView))
                    ApplyColors(c, p, dark);
            }
        }

        private static void StyleControl(Control c, Palette p, bool dark)
        {
            var fluent = c as IFluentControl;
            if (fluent != null)
            {
                // Fluent controls (UI\Fluent.cs) draw themselves in the theme colors
                fluent.ApplyTheme(p, dark);
                return;
            }
            var role = GetRole(c);
            if (c is Button)
            {
                StyleButton((Button)c, p, role);
            }
            else if (c is DataGridView)
            {
                StyleGrid((DataGridView)c, p, dark);
            }
            else if (c is TextBoxBase)
            {
                var tb = (TextBoxBase)c;
                var flush = tb is TextBox && ((TextBox)tb).BorderStyle == BorderStyle.None;
                var host = tb.Parent as FluentTextBoxHost;
                if (host != null)
                    tb.BackColor = host.FieldColor; // inside a rounded Fluent text field
                else if (flush && tb.Parent is FluentCard)
                    tb.BackColor = Fluent.CardBack; // read-only text in a card (help windows)
                else
                    tb.BackColor = flush || tb.ReadOnly ? p.Back : p.Field;
                tb.ForeColor = p.FieldText;
                SetDarkScrollbars(tb, dark);
            }
            else if (c is ListBox) // includes CheckedListBox
            {
                c.BackColor = p.Field;
                c.ForeColor = p.FieldText;
                SetDarkScrollbars(c, dark);
            }
            else if (c is ComboBox)
            {
                var cb = (ComboBox)c;
                // flat + colors is the only way to get a dark combo box in WinForms; light keeps the native look
                cb.FlatStyle = dark ? FlatStyle.Flat : FlatStyle.Standard;
                cb.BackColor = p.Field;
                cb.ForeColor = p.FieldText;
            }
            else if (c is LinkLabel)
            {
                var link = (LinkLabel)c;
                link.LinkColor = p.Link;
                link.ActiveLinkColor = p.Link;
                link.VisitedLinkColor = p.Link;
                link.ForeColor = p.Text;
            }
            else if (c is Label)
            {
                if (role == Role.Separator)
                {
                    ((Label)c).BorderStyle = BorderStyle.None;
                    c.BackColor = p.Separator;
                }
                else if (role == Role.Subtle)
                {
                    c.ForeColor = p.SubtleText;
                }
            }
            else if (c is GroupBox)
            {
                // an explicitly set ForeColor makes the themed group box draw its caption in that color
                c.ForeColor = p.Text;
                // the themed frame is drawn almost white; in the dark theme it is painted over (see PaintGroupBox)
                var info = Info(c);
                if (!info.PaintHooked)
                {
                    info.PaintHooked = true;
                    c.Paint += PaintGroupBox;
                }
            }
            else if (c is CheckBox || c is RadioButton)
            {
                c.ForeColor = p.Text;
            }
            else if (c is ScrollBar)
            {
                SetDarkScrollbars(c, dark);
            }
        }

        /// <summary>dark theme: repaints the frame and caption of a group box in the palette colors</summary>
        private static void PaintGroupBox(object sender, PaintEventArgs e)
        {
            try
            {
                if (!IsDark)
                    return;
                var box = (GroupBox)sender;
                var p = Dark;
                var g = e.Graphics;
                const TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
                var textSize = TextRenderer.MeasureText(g, box.Text ?? "", box.Font, Size.Empty, flags);
                var top = textSize.Height / 2;
                // erase the themed frame (incl. its rounded corners) with the background, then draw a thin one
                var frame = new Rectangle(0, top, box.Width - 1, box.Height - top - 1);
                using (var erase = new Pen(box.BackColor, 4))
                    g.DrawRectangle(erase, frame);
                using (var pen = new Pen(p.Border))
                    g.DrawRectangle(pen, frame);
                if (!string.IsNullOrEmpty(box.Text))
                {
                    var textRect = new Rectangle(7, 0, textSize.Width + 2, textSize.Height);
                    using (var back = new SolidBrush(box.BackColor))
                        g.FillRectangle(back, textRect);
                    TextRenderer.DrawText(g, box.Text, box.Font, new Point(8, 0),
                        box.Enabled ? p.Text : p.SubtleText, flags);
                }
            }
            catch (Exception) { }
        }

        private static void StyleButton(Button b, Palette p, Role role)
        {
            if (role == Role.IconButton)
            {
                // picture-only buttons (? / update) stay borderless, they only get a hover color
                b.FlatAppearance.MouseOverBackColor = p.ButtonHover;
                b.FlatAppearance.MouseDownBackColor = p.ButtonPressed;
                return;
            }
            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;
            if (b.Height < Px(SmallButtonHeight) && !(b is VButton))
            {
                // flat buttons keep a few pixels of padding around the text, which clips it in short
                // buttons (Refresh, Always); those draw their text themselves (see PaintSmallButton)
                var info = Info(b);
                if (!info.PaintHooked)
                {
                    info.PaintHooked = true;
                    b.Paint += PaintSmallButton;
                }
            }
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = p.ButtonBorder;
            b.FlatAppearance.MouseOverBackColor = p.ButtonHover;
            b.FlatAppearance.MouseDownBackColor = p.ButtonPressed;
            b.BackColor = p.ButtonBack;
            b.ForeColor = p.Text;
        }

        /// <summary>buttons lower than this get their text painted without the flat button's inner padding</summary>
        private const int SmallButtonHeight = 23;

        private static float _dpiScale;

        /// <summary>display scale the windows are drawn at (system DPI / 96; 1.5 at 150 %)</summary>
        internal static float DpiScale
        {
            get
            {
                if (_dpiScale <= 0)
                {
                    try
                    {
                        using (var g = Graphics.FromHwnd(IntPtr.Zero))
                            _dpiScale = g.DpiY / 96f;
                    }
                    catch (Exception) { _dpiScale = 1f; }
                }
                return _dpiScale;
            }
        }

        /// <summary>a size designed at 100 % in pixels at the current scale</summary>
        private static int Px(int value)
        {
            return (int)Math.Round(value * DpiScale);
        }

        private static void PaintSmallButton(object sender, PaintEventArgs e)
        {
            try
            {
                var b = (Button)sender;
                if (b.FlatStyle != FlatStyle.Flat || string.IsNullOrEmpty(b.Text))
                    return;
                var p = Current;
                var hot = b.Enabled && b.ClientRectangle.Contains(b.PointToClient(Control.MousePosition));
                var pressed = hot && (Control.MouseButtons & MouseButtons.Left) != 0;
                var back = pressed ? b.FlatAppearance.MouseDownBackColor
                    : hot ? b.FlatAppearance.MouseOverBackColor
                    : b.BackColor;
                var inner = Rectangle.Inflate(b.ClientRectangle, -1, -1);
                using (var brush = new SolidBrush(back))
                    e.Graphics.FillRectangle(brush, inner);
                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, inner,
                    b.Enabled ? b.ForeColor : p.SubtleText,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                    TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis | TextFormatFlags.HidePrefix);
            }
            catch (Exception) { }
        }

        /// <summary>accent color for a button while it is busy (e.g. "check now" in Settings); false restores the theme</summary>
        public static void SetBusy(Button b, bool busy)
        {
            try
            {
                var fluentButton = b as FluentButton;
                if (fluentButton != null)
                {
                    fluentButton.Busy = busy;
                    return;
                }
                var p = Current;
                if (busy)
                {
                    b.FlatStyle = FlatStyle.Flat;
                    b.UseVisualStyleBackColor = false;
                    b.BackColor = p.Accent;
                    b.ForeColor = p.AccentText;
                }
                else
                {
                    StyleButton(b, p, GetRole(b));
                }
            }
            catch (Exception) { }
        }

        private static void StyleGrid(DataGridView gv, Palette p, bool dark)
        {
            gv.BorderStyle = BorderStyle.None;
            gv.BackgroundColor = p.Field;
            gv.GridColor = p.Grid;
            gv.EnableHeadersVisualStyles = false;
            gv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            var header = gv.ColumnHeadersDefaultCellStyle;
            header.BackColor = p.Header;
            header.ForeColor = p.Text;
            header.SelectionBackColor = p.Header;
            header.SelectionForeColor = p.Text;

            var rowHeader = gv.RowHeadersDefaultCellStyle;
            rowHeader.BackColor = p.Header;
            rowHeader.ForeColor = p.Text;
            rowHeader.SelectionBackColor = p.Selection;
            rowHeader.SelectionForeColor = p.SelectionText;

            var cell = gv.DefaultCellStyle;
            cell.BackColor = p.Field;
            cell.ForeColor = p.FieldText;
            cell.SelectionBackColor = p.Selection;
            cell.SelectionForeColor = p.SelectionText;

            foreach (DataGridViewColumn column in gv.Columns)
            {
                var combo = column as DataGridViewComboBoxColumn;
                if (combo != null)
                    combo.FlatStyle = FlatStyle.Flat;
            }
            foreach (Control child in gv.Controls)
                if (child is ScrollBar)
                    SetDarkScrollbars(child, dark);
        }

        // ---- menus ----------------------------------------------------------------------------

        /// <summary>colors a context menu (e.g. "Open in Private Window") like the current theme</summary>
        public static void StyleMenu(ToolStrip menu)
        {
            try
            {
                var p = Current;
                menu.Renderer = new ToolStripProfessionalRenderer(new MenuColors(p)) { RoundedEdges = false };
                menu.BackColor = p.Field;
                menu.ForeColor = p.Text;
                var baseFont = BaseFont;
                if (baseFont != null)
                    menu.Font = baseFont;
                foreach (ToolStripItem item in menu.Items)
                    item.ForeColor = p.Text;
            }
            catch (Exception) { }
        }

        private sealed class MenuColors : ProfessionalColorTable
        {
            private readonly Palette _p;

            public MenuColors(Palette p)
            {
                _p = p;
                UseSystemColors = false;
            }

            public override Color ToolStripDropDownBackground { get { return _p.Field; } }
            public override Color ImageMarginGradientBegin { get { return _p.Field; } }
            public override Color ImageMarginGradientMiddle { get { return _p.Field; } }
            public override Color ImageMarginGradientEnd { get { return _p.Field; } }
            public override Color MenuBorder { get { return _p.Border; } }
            public override Color MenuItemBorder { get { return _p.Selection; } }
            public override Color MenuItemSelected { get { return _p.Selection; } }
            public override Color MenuItemSelectedGradientBegin { get { return _p.Selection; } }
            public override Color MenuItemSelectedGradientEnd { get { return _p.Selection; } }
            public override Color SeparatorDark { get { return _p.Separator; } }
            public override Color SeparatorLight { get { return _p.Separator; } }
        }

        // ---- entry point ----------------------------------------------------------------------

        /// <summary>applies fonts, colors and window attributes of the current theme to a window</summary>
        public static void Apply(Form form)
        {
            if (form == null)
                return;
            var p = Current;
            var dark = p == Dark;
            Fluent.RefreshAccent();
            try
            {
                ApplyFonts(form);
            }
            catch (Exception) { }
            try
            {
                ApplyColors(form, p, dark);
            }
            catch (Exception) { }

            form.HandleCreated -= Form_HandleCreated;
            form.HandleCreated += Form_HandleCreated;
            form.Load -= Form_Load;
            form.Load += Form_Load;
            form.Shown -= Form_Shown;
            form.Shown += Form_Shown;
            if (form.IsHandleCreated)
                ApplyWindowAttributes(form, dark, true);
            form.Invalidate(true);
            LayoutCheck.Hook(form);
        }

        private static void Form_HandleCreated(object sender, EventArgs e)
        {
            ApplyWindowAttributes((Form)sender, IsDark, false);
        }

        // ---- DWM (title bar, corners, Mica) ---------------------------------------------------

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;
        private const int DWMWCP_ROUND = 2;
        private const int DWMSBT_AUTO = 0;
        private const int DWMSBT_MAINWINDOW = 2; // Mica

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string appName, string idList);

        private const uint SWP_NOSIZE = 0x0001, SWP_NOMOVE = 0x0002, SWP_NOZORDER = 0x0004,
            SWP_NOACTIVATE = 0x0010, SWP_FRAMECHANGED = 0x0020;

        /// <summary>Windows build number (the app manifest declares Windows 10/11, so this is the real one)</summary>
        private static int WindowsBuild
        {
            get
            {
                var v = Environment.OSVersion.Version;
                return v.Major >= 10 ? v.Build : 0;
            }
        }

        private static void SetAttribute(IntPtr hwnd, int attribute, int value)
        {
            try
            {
                DwmSetWindowAttribute(hwnd, attribute, ref value, sizeof(int));
            }
            catch (Exception)
            {
                // dwmapi missing / attribute unknown: keep the default look
            }
        }

        private static void ApplyWindowAttributes(Form form, bool dark, bool refreshFrame)
        {
            try
            {
                var hwnd = form.Handle;
                var build = WindowsBuild;
                if (build >= 17763) // Windows 10 1809+: dark title bar
                    SetAttribute(hwnd, build >= 18985 ? DWMWA_USE_IMMERSIVE_DARK_MODE : DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1,
                        dark ? 1 : 0);
                if (build >= 22000) // Windows 11: rounded corners
                    SetAttribute(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, DWMWCP_ROUND);
                if (build >= 22621) // Windows 11 22H2+: Mica backdrop (visible on the title bar)
                    SetAttribute(hwnd, DWMWA_SYSTEMBACKDROP_TYPE, UseMica ? DWMSBT_MAINWINDOW : DWMSBT_AUTO);
                if (refreshFrame)
                    SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0,
                        SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
            }
            catch (Exception) { }
        }

        /// <summary>dark scroll bars for lists/text boxes (Windows 10 1809+; no effect elsewhere)</summary>
        private static void SetDarkScrollbars(Control c, bool dark)
        {
            if (WindowsBuild < 17763)
                return;
            EventHandler apply = null;
            apply = (s, e) =>
            {
                try
                {
                    SetWindowTheme(c.Handle, dark ? "DarkMode_Explorer" : null, null); // null = default theme
                }
                catch (Exception) { }
            };
            if (c.IsHandleCreated)
                apply(c, EventArgs.Empty);
            else if (dark)
            {
                EventHandler once = null;
                once = (s, e) =>
                {
                    c.HandleCreated -= once;
                    apply(s, e);
                };
                c.HandleCreated += once;
            }
        }
    }
}
