using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
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

        public const string FontName = "Segoe UI";
        public const float FontSize = 9f;
        private const float ClassicFontSize = 8.25f;

        private static Font _baseFont;

        /// <summary>Segoe UI 9pt (null if the font is not installed)</summary>
        public static Font BaseFont
        {
            get
            {
                if (_baseFont == null)
                    _baseFont = CreateFont(FontSize, FontStyle.Regular);
                return _baseFont;
            }
        }

        private static Font CreateFont(float size, FontStyle style)
        {
            try
            {
                var font = new Font(FontName, size, style, GraphicsUnit.Point);
                if (string.Equals(font.Name, FontName, StringComparison.OrdinalIgnoreCase))
                    return font;
                font.Dispose();
            }
            catch (Exception) { }
            return null;
        }

        private static bool IsModernFont(Font font)
        {
            return font != null && string.Equals(font.Name, FontName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Windows whose layout scales with the font (AutoScaleMode.Font) get Segoe UI 9pt on the form
        /// itself: WinForms then scales the whole layout proportionally, so nothing is clipped and the
        /// relative positions stay the same. Other windows (the browser picker uses AutoScaleMode.Dpi)
        /// keep their exact size: only the individual controls get the new font, and a fixed-size label
        /// keeps its old font if the new one would not fit.
        /// </summary>
        private static void ApplyFonts(Form form)
        {
            var baseFont = BaseFont;
            if (baseFont == null)
                return;
            if (form.AutoScaleMode == AutoScaleMode.Font)
            {
                if (!IsModernFont(form.Font))
                    form.Font = baseFont;
                ReplaceExplicitFonts(form);
            }
            else
            {
                ApplyLeafFonts(form);
            }
        }

        /// <summary>controls with an explicitly set classic font (titles, bold labels, ...) get Segoe UI in the same size/style</summary>
        private static void ReplaceExplicitFonts(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (!IsModernFont(c.Font))
                {
                    var size = Math.Abs(c.Font.SizeInPoints - ClassicFontSize) < 0.1f ? FontSize : c.Font.SizeInPoints;
                    var font = CreateFont(size, c.Font.Style);
                    if (font != null)
                        c.Font = font;
                }
                if (c.HasChildren)
                    ReplaceExplicitFonts(c);
            }
        }

        private static void ApplyLeafFonts(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.HasChildren || c is ContainerControl)
                {
                    ApplyLeafFonts(c);
                    continue;
                }
                if (IsModernFont(c.Font))
                    continue;
                var size = Math.Abs(c.Font.SizeInPoints - ClassicFontSize) < 0.1f ? FontSize : c.Font.SizeInPoints;
                var font = CreateFont(size, c.Font.Style);
                if (font == null)
                    continue;
                var label = c as Label;
                if (label != null && !label.AutoSize &&
                    TextRenderer.MeasureText("Ag(", font).Height > label.Height)
                {
                    font.Dispose();
                    continue; // would be clipped: keep the classic font
                }
                c.Font = font;
            }
        }

        // ---- colors ---------------------------------------------------------------------------

        private enum Role { Normal, Subtle, Separator, IconButton }

        private sealed class RoleBox
        {
            public Role Role;
        }

        // role of a control is decided once (the first time it is themed) from its designer properties
        private static readonly ConditionalWeakTable<Control, RoleBox> Roles = new ConditionalWeakTable<Control, RoleBox>();

        private static Role GetRole(Control c)
        {
            return Roles.GetValue(c, ctl =>
            {
                var role = Role.Normal;
                var label = ctl as Label;
                var button = ctl as Button;
                if (label != null && label.BorderStyle == BorderStyle.Fixed3D && label.Height <= 3)
                    role = Role.Separator;
                else if (ctl.ForeColor.IsKnownColor && ctl.ForeColor.ToKnownColor() == KnownColor.GrayText)
                    role = Role.Subtle;
                else if (button != null && button.FlatStyle == FlatStyle.Flat &&
                         button.FlatAppearance.BorderSize == 0 && button.BackgroundImage != null)
                    role = Role.IconButton;
                return new RoleBox { Role = role };
            }).Role;
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
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = p.ButtonBorder;
            b.FlatAppearance.MouseOverBackColor = p.ButtonHover;
            b.FlatAppearance.MouseDownBackColor = p.ButtonPressed;
            b.BackColor = p.ButtonBack;
            b.ForeColor = p.Text;
        }

        /// <summary>accent color for a button while it is busy (e.g. "check now" in Settings); false restores the theme</summary>
        public static void SetBusy(Button b, bool busy)
        {
            try
            {
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
            if (form.IsHandleCreated)
                ApplyWindowAttributes(form, dark, true);
            form.Invalidate(true);
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
