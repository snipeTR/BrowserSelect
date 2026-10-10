using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BrowserSelect.UI
{
    /// <summary>
    /// A control that draws itself in the current theme (Fluent / Windows 11 look). Theme.Apply calls
    /// <see cref="ApplyTheme"/> instead of its own styling for it.
    /// </summary>
    internal interface IFluentControl
    {
        void ApplyTheme(Theme.Palette p, bool dark);
    }

    /// <summary>a control that keeps its own font (Theme does not replace it with the 9pt UI font)</summary>
    internal interface IFluentOwnFont
    {
    }

    /// <summary>
    /// Windows 11 (Fluent) colors, accent color, icon font and drawing helpers for the owner-drawn controls
    /// below (FluentButton, FluentToggle, FluentNavItem, FluentCard, FluentFieldHost, FluentComboBox,
    /// FluentHeader). No third-party code: everything is drawn with GDI+/GDI. Sizes are given at 100 % and
    /// multiplied with <see cref="Scale"/> (the window's DPI / 96), so the controls are sharp at 100-200 %.
    /// </summary>
    public static class Fluent
    {
        private static bool Dark
        {
            get { return Theme.IsDark; }
        }

        // ---- colors (light / dark) -------------------------------------------------------------

        /// <summary>card (section) background: slightly lighter than the page in both themes</summary>
        public static Color CardBack { get { return Dark ? Color.FromArgb(39, 39, 39) : Color.FromArgb(251, 251, 251); } }
        public static Color CardBorder { get { return Dark ? Color.FromArgb(24, 24, 24) : Color.FromArgb(229, 229, 229); } }

        public static Color ButtonBack { get { return Dark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(254, 254, 254); } }
        public static Color ButtonHover { get { return Dark ? Color.FromArgb(58, 58, 58) : Color.FromArgb(246, 246, 246); } }
        public static Color ButtonPressed { get { return Dark ? Color.FromArgb(44, 44, 44) : Color.FromArgb(240, 240, 240); } }
        public static Color ButtonBorder { get { return Dark ? Color.FromArgb(66, 66, 66) : Color.FromArgb(211, 211, 211); } }
        public static Color ButtonDisabledBack { get { return Dark ? Color.FromArgb(44, 44, 44) : Color.FromArgb(249, 249, 249); } }
        public static Color DisabledText { get { return Dark ? Color.FromArgb(120, 120, 120) : Color.FromArgb(160, 160, 160); } }

        public static Color NavHover { get { return Dark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(234, 234, 234); } }
        public static Color NavSelected { get { return Dark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(230, 230, 230); } }
        public static Color NavPressed { get { return Dark ? Color.FromArgb(40, 40, 40) : Color.FromArgb(238, 238, 238); } }

        /// <summary>border of the "off" toggle switch and its knob</summary>
        public static Color StrongStroke { get { return Dark ? Color.FromArgb(160, 160, 160) : Color.FromArgb(118, 118, 118); } }
        public static Color FieldBorder { get { return Dark ? Color.FromArgb(70, 70, 70) : Color.FromArgb(209, 209, 209); } }
        public static Color FieldHover { get { return Dark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(249, 249, 249); } }

        // ---- accent color ------------------------------------------------------------------------

        private static bool _accentRead;
        private static Color _accentOnLight = Color.FromArgb(0, 103, 192);  // Windows default blue, AccentDark1
        private static Color _accentOnDark = Color.FromArgb(76, 194, 255);  // AccentLight2

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetColorizationColor(out uint color, [MarshalAs(UnmanagedType.Bool)] out bool opaque);

        /// <summary>reads the Windows accent color again (called by Theme.Apply, e.g. when the theme changes)</summary>
        public static void RefreshAccent()
        {
            _accentRead = false;
        }

        private static void ReadAccent()
        {
            if (_accentRead)
                return;
            _accentRead = true;
            try
            {
                // Settings > Personalization > Colors: the palette Windows itself derives from the accent color;
                // 8 RGBA entries: Light3, Light2, Light1, Base, Dark1, Dark2, Dark3, (unused). Windows 11 uses
                // Dark1 on light and Light2 on dark backgrounds
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent"))
                {
                    var bytes = key == null ? null : key.GetValue("AccentPalette") as byte[];
                    if (bytes != null && bytes.Length >= 32)
                    {
                        _accentOnDark = Color.FromArgb(bytes[4], bytes[5], bytes[6]);
                        _accentOnLight = Color.FromArgb(bytes[16], bytes[17], bytes[18]);
                        return;
                    }
                }
            }
            catch (Exception) { }

            Color? accent = null;
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM"))
                {
                    var value = key == null ? null : key.GetValue("AccentColor");
                    if (value is int)
                    {
                        var abgr = unchecked((uint)(int)value);
                        accent = Color.FromArgb((int)(abgr & 0xFF), (int)((abgr >> 8) & 0xFF), (int)((abgr >> 16) & 0xFF));
                    }
                }
            }
            catch (Exception) { }
            if (accent == null)
            {
                try
                {
                    uint argb;
                    bool opaque;
                    if (DwmGetColorizationColor(out argb, out opaque) == 0)
                        accent = Color.FromArgb((int)((argb >> 16) & 0xFF), (int)((argb >> 8) & 0xFF), (int)(argb & 0xFF));
                }
                catch (Exception) { }
            }
            if (accent != null)
            {
                // no palette: darker version on light, lighter version on dark backgrounds
                _accentOnLight = Mix(accent.Value, Color.Black, 0.15f);
                _accentOnDark = Mix(accent.Value, Color.White, 0.45f);
            }
        }

        /// <summary>accent color for the current theme (Windows accent color, default blue as fallback)</summary>
        public static Color Accent
        {
            get
            {
                ReadAccent();
                return Dark ? _accentOnDark : _accentOnLight;
            }
        }

        public static Color AccentHover { get { return Mix(Accent, Dark ? Color.Black : Color.White, 0.1f); } }
        public static Color AccentPressed { get { return Mix(Accent, Dark ? Color.Black : Color.White, 0.2f); } }
        public static Color AccentDisabled { get { return Dark ? Color.FromArgb(67, 67, 67) : Color.FromArgb(191, 191, 191); } }

        /// <summary>black or white, whichever is readable on the accent color</summary>
        public static Color OnAccent
        {
            get
            {
                var a = Accent;
                var luminance = (0.299 * a.R + 0.587 * a.G + 0.114 * a.B) / 255;
                return luminance > 0.6 ? Color.Black : Color.White;
            }
        }

        public static Color Mix(Color a, Color b, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Color.FromArgb(
                (int)Math.Round(a.R + (b.R - a.R) * amount),
                (int)Math.Round(a.G + (b.G - a.G) * amount),
                (int)Math.Round(a.B + (b.B - a.B) * amount));
        }

        // ---- icon font ---------------------------------------------------------------------------

        private static bool _iconChecked;
        private static string _iconFont;

        /// <summary>
        /// Segoe Fluent Icons (Windows 11), Segoe MDL2 Assets (Windows 10) or null (no icons). Both use the same
        /// code points for the glyphs used here.
        /// </summary>
        public static string IconFontName
        {
            get
            {
                if (!_iconChecked)
                {
                    _iconChecked = true;
                    foreach (var name in new[] { "Segoe Fluent Icons", "Segoe MDL2 Assets" })
                    {
                        if (Theme.IsFontInstalled(name))
                        {
                            _iconFont = name;
                            break;
                        }
                    }
                }
                return _iconFont;
            }
        }

        public static Font CreateIconFont(float size)
        {
            var name = IconFontName;
            if (name == null)
                return null;
            try
            {
                return new Font(name, size, FontStyle.Regular, GraphicsUnit.Point);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // glyphs (same code points in Segoe Fluent Icons and Segoe MDL2 Assets)
        public const string GlyphGlobe = "\uE774";
        public const string GlyphStar = "\uE734";
        public const string GlyphFilter = "\uE71C";
        public const string GlyphSettings = "\uE713";
        public const string GlyphSync = "\uE895";
        public const string GlyphChevronUp = "\uE70E";
        public const string GlyphChevronDown = "\uE70D";

        // ---- drawing helpers ---------------------------------------------------------------------

        /// <summary>
        /// DPI scale of a control (1.5 at 150 %). BrowserSelect is system-DPI aware, where Control.DeviceDpi of
        /// .NET Framework can stay at 96 although the windows are drawn at 150 or 200 %; the scale is therefore at
        /// least the system DPI the windows are drawn at (Theme.DpiScale).
        /// </summary>
        public static float Scale(Control c)
        {
            float system = 1f;
            try { system = Theme.DpiScale; }
            catch (Exception) { }
            try
            {
                var dpi = c.DeviceDpi;
                var own = dpi > 0 ? dpi / 96f : 1f;
                return Math.Max(own, system);
            }
            catch (Exception)
            {
                return system;
            }
        }

        public static GraphicsPath RoundRect(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            var d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d < 1f)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>fills a rounded rectangle and draws its 1 px border (crisp: drawn on the pixel centers)</summary>
        public static void FillRounded(Graphics g, Rectangle bounds, float radius, Color fill, Color? border)
        {
            var old = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new RectangleF(bounds.X + 0.5f, bounds.Y + 0.5f, bounds.Width - 1f, bounds.Height - 1f);
            if (r.Width > 0 && r.Height > 0)
            {
                using (var path = RoundRect(r, radius))
                {
                    using (var brush = new SolidBrush(fill))
                        g.FillPath(brush, path);
                    if (border != null)
                        using (var pen = new Pen(border.Value, 1f))
                            g.DrawPath(pen, path);
                }
            }
            g.SmoothingMode = old;
        }

        /// <summary>keyboard focus visual: 2 px rounded outline in the text color (like WinUI)</summary>
        public static void DrawFocus(Graphics g, Rectangle bounds, float radius, float scale)
        {
            var old = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var width = Math.Max(2f, (float)Math.Round(2f * scale));
            var r = new RectangleF(bounds.X + width / 2, bounds.Y + width / 2, bounds.Width - width, bounds.Height - width);
            if (r.Width > 0 && r.Height > 0)
                using (var path = RoundRect(r, radius))
                using (var pen = new Pen(Theme.Current.Text, width))
                    g.DrawPath(pen, path);
            g.SmoothingMode = old;
        }

        /// <summary>
        /// draws (wrapped) text vertically centered in a rectangle; TextRenderer only centers single lines
        /// </summary>
        public static void DrawTextCentered(Graphics g, string text, Font font, Rectangle r, Color color, bool hidePrefix)
        {
            if (string.IsNullOrEmpty(text) || r.Width <= 0 || r.Height <= 0)
                return;
            var flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl | TextFormatFlags.Left |
                        (hidePrefix ? TextFormatFlags.HidePrefix : TextFormatFlags.Default);
            var size = TextRenderer.MeasureText(g, text, font, new Size(r.Width, int.MaxValue), flags);
            var height = Math.Min(size.Height, r.Height);
            var rect = new Rectangle(r.X, r.Y + (r.Height - height) / 2, r.Width, height);
            TextRenderer.DrawText(g, text, font, rect, color, flags | TextFormatFlags.EndEllipsis);
        }

        /// <summary>height of a text wrapped to a width (for the layout checks)</summary>
        public static Size MeasureWrapped(Control c, string text, int width)
        {
            return TextRenderer.MeasureText(text ?? "", c.Font, new Size(Math.Max(1, width), int.MaxValue),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
        }

        public static Color ParentBack(Control c)
        {
            return c.Parent != null ? c.Parent.BackColor : Theme.Current.Back;
        }
    }

    /// <summary>
    /// Rounded Windows 11 button: standard (light fill, thin border) or <see cref="Accent"/> (accent color,
    /// for the primary action such as Apply), with hover, pressed, disabled and keyboard focus states.
    /// It is a normal Button (Click, PerformClick, DialogResult, AcceptButton/CancelButton all work).
    /// </summary>
    public class FluentButton : Button, IFluentControl
    {
        private bool _hover, _pressed, _accent, _busy;
        private string _glyph;
        private Font _glyphFont;

        public FluentButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
        }

        /// <summary>accent (primary) style</summary>
        [DefaultValue(false)]
        public bool Accent
        {
            get { return _accent; }
            set { _accent = value; Invalidate(); }
        }

        /// <summary>accent colored while a request runs (Theme.SetBusy)</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Busy
        {
            get { return _busy; }
            set { _busy = value; Invalidate(); }
        }

        /// <summary>icon glyph drawn instead of the text if Segoe Fluent Icons / MDL2 Assets is installed (the text stays for screen readers and as fallback)</summary>
        [DefaultValue(null)]
        public string Glyph
        {
            get { return _glyph; }
            set { _glyph = value; Invalidate(); }
        }

        public void ApplyTheme(Theme.Palette p, bool dark)
        {
            ForeColor = p.Text;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            _hover = _pressed = false;
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                var p = Theme.Current;
                var s = Fluent.Scale(this);
                g.Clear(Fluent.ParentBack(this));
                var accent = _accent || _busy;
                Color fill, border, text;
                if (!Enabled)
                {
                    fill = accent ? Fluent.AccentDisabled : Fluent.ButtonDisabledBack;
                    border = accent ? fill : Fluent.ButtonBorder;
                    text = accent ? (Theme.IsDark ? Color.FromArgb(160, 160, 160) : Color.White) : Fluent.DisabledText;
                }
                else if (accent)
                {
                    fill = _pressed ? Fluent.AccentPressed : _hover ? Fluent.AccentHover : Fluent.Accent;
                    border = fill;
                    text = Fluent.OnAccent;
                }
                else
                {
                    fill = _pressed ? Fluent.ButtonPressed : _hover ? Fluent.ButtonHover : Fluent.ButtonBack;
                    border = Fluent.ButtonBorder;
                    text = _pressed ? p.SubtleText : p.Text;
                }
                var radius = 4f * s;
                Fluent.FillRounded(g, ClientRectangle, radius, fill, border);

                var content = Rectangle.Inflate(ClientRectangle, -(int)Math.Round(4 * s), -1);
                var glyphFont = GlyphFont();
                if (glyphFont != null)
                {
                    TextRenderer.DrawText(g, _glyph, glyphFont, content, text,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                        TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
                }
                else
                {
                    var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;
                    if (!ShowKeyboardCues)
                        flags |= TextFormatFlags.HidePrefix;
                    TextRenderer.DrawText(g, Text, Font, content, text, flags);
                }
                if (Focused && ShowFocusCues)
                    Fluent.DrawFocus(g, ClientRectangle, radius, s);
            }
            catch (Exception)
            {
                base.OnPaint(e);
            }
        }

        private Font GlyphFont()
        {
            if (string.IsNullOrEmpty(_glyph))
                return null;
            if (_glyphFont == null)
                _glyphFont = Fluent.CreateIconFont(10f);
            return _glyphFont;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _glyphFont != null)
            {
                _glyphFont.Dispose();
                _glyphFont = null;
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Windows 11 toggle switch for on/off settings: the text on the left (wrapped if needed), the switch on
    /// the right. It is a CheckBox (Checked, CheckedChanged, Space key, screen readers work as before).
    /// </summary>
    public class FluentToggle : CheckBox, IFluentControl
    {
        private bool _hover, _pressed;

        public FluentToggle()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            AutoSize = false;
        }

        public void ApplyTheme(Theme.Palette p, bool dark)
        {
            ForeColor = p.Text;
            Invalidate();
        }

        private Rectangle TrackBounds
        {
            get
            {
                var s = Fluent.Scale(this);
                int w = (int)Math.Round(40 * s), h = (int)Math.Round(20 * s), margin = (int)Math.Round(4 * s);
                return new Rectangle(Width - w - margin, (Height - h) / 2, w, h);
            }
        }

        /// <summary>where the text is drawn (used by the layout checks)</summary>
        [Browsable(false)]
        public Rectangle TextBounds
        {
            get
            {
                var s = Fluent.Scale(this);
                var right = TrackBounds.Left - (int)Math.Round(12 * s);
                return new Rectangle(0, 0, Math.Max(1, right), Height);
            }
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnCheckedChanged(EventArgs e) { Invalidate(); base.OnCheckedChanged(e); }
        protected override void OnEnabledChanged(EventArgs e) { _hover = _pressed = false; Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                var p = Theme.Current;
                var s = Fluent.Scale(this);
                g.Clear(Fluent.ParentBack(this));

                Fluent.DrawTextCentered(g, Text, Font, TextBounds, Enabled ? p.Text : Fluent.DisabledText, !ShowKeyboardCues);

                var track = TrackBounds;
                var radius = track.Height / 2f;
                Color knob;
                if (Checked)
                {
                    var fill = !Enabled ? Fluent.AccentDisabled
                        : _pressed ? Fluent.AccentPressed
                        : _hover ? Fluent.AccentHover
                        : Fluent.Accent;
                    Fluent.FillRounded(g, track, radius, fill, fill);
                    knob = Enabled ? Fluent.OnAccent : (Theme.IsDark ? Color.FromArgb(160, 160, 160) : Color.White);
                }
                else
                {
                    var stroke = Enabled ? Fluent.StrongStroke : Fluent.DisabledText;
                    var fill = Enabled && (_hover || _pressed) ? Fluent.ButtonHover : Fluent.ParentBack(this);
                    Fluent.FillRounded(g, track, radius, fill, stroke);
                    knob = stroke;
                }
                // knob: 12 px, 14 px while the mouse is over it, wider while pressed (like Windows 11)
                var d = (Enabled && (_hover || _pressed) ? 14f : 12f) * s;
                var w = Enabled && _pressed ? d + 3f * s : d;
                var cy = track.Top + track.Height / 2f;
                var cx = Checked ? track.Right - track.Height / 2f - (w - d) / 2f : track.Left + track.Height / 2f + (w - d) / 2f;
                var old = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = Fluent.RoundRect(new RectangleF(cx - w / 2f, cy - d / 2f, w, d), d / 2f))
                using (var brush = new SolidBrush(knob))
                    g.FillPath(brush, path);
                g.SmoothingMode = old;

                if (Focused && ShowFocusCues)
                {
                    var m = (int)Math.Round(3 * s);
                    Fluent.DrawFocus(g, Rectangle.Inflate(track, m, m), radius + m, s);
                }
            }
            catch (Exception)
            {
                base.OnPaint(e);
            }
        }
    }

    /// <summary>
    /// item of the Settings navigation pane (left): icon + page name, rounded "pill" background when selected
    /// (with the accent color indicator bar) or under the mouse. It is a RadioButton: exactly one item is
    /// selected, the arrow keys move between the items.
    /// </summary>
    public class FluentNavItem : RadioButton, IFluentControl
    {
        private bool _hover, _pressed;
        private string _glyph;
        private Font _glyphFont;

        public FluentNavItem()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            AutoSize = false;
        }

        /// <summary>Segoe Fluent Icons / MDL2 Assets glyph (no icon if neither font is installed)</summary>
        [DefaultValue(null)]
        public string Glyph
        {
            get { return _glyph; }
            set { _glyph = value; Invalidate(); }
        }

        /// <summary>the page shown while this item is selected</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control Page { get; set; }

        public void ApplyTheme(Theme.Palette p, bool dark)
        {
            ForeColor = p.Text;
            Invalidate();
        }

        private bool HasIcon
        {
            get { return !string.IsNullOrEmpty(_glyph) && Fluent.IconFontName != null; }
        }

        /// <summary>where the text is drawn (used by the layout checks)</summary>
        [Browsable(false)]
        public Rectangle TextBounds
        {
            get
            {
                var s = Fluent.Scale(this);
                var left = (int)Math.Round((HasIcon ? 44 : 16) * s);
                return new Rectangle(left, 0, Math.Max(1, Width - left - (int)Math.Round(8 * s)), Height);
            }
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnCheckedChanged(EventArgs e) { Invalidate(); base.OnCheckedChanged(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                var p = Theme.Current;
                var s = Fluent.Scale(this);
                g.Clear(Fluent.ParentBack(this));
                var radius = 4f * s;
                Color? pill = _pressed ? Fluent.NavPressed
                    : Checked ? Fluent.NavSelected
                    : _hover ? (Color?)Fluent.NavHover
                    : null;
                if (pill != null && Enabled)
                    Fluent.FillRounded(g, ClientRectangle, radius, pill.Value, null);
                if (Checked)
                {
                    // accent indicator bar at the left edge of the selected item
                    float barH = 16f * s, barW = 3f * s;
                    var bar = new RectangleF(0, (Height - barH) / 2f, barW, barH);
                    var old = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = Fluent.RoundRect(bar, barW / 2f))
                    using (var brush = new SolidBrush(Fluent.Accent))
                        g.FillPath(brush, path);
                    g.SmoothingMode = old;
                }
                var color = Enabled ? p.Text : Fluent.DisabledText;
                if (HasIcon)
                {
                    if (_glyphFont == null)
                        _glyphFont = Fluent.CreateIconFont(12f);
                    if (_glyphFont != null)
                    {
                        var iconRect = new Rectangle((int)Math.Round(12 * s), 0, (int)Math.Round(22 * s), Height);
                        TextRenderer.DrawText(g, _glyph, _glyphFont, iconRect, color,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                            TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
                    }
                }
                Fluent.DrawTextCentered(g, Text, Font, TextBounds, color, !ShowKeyboardCues);
                if (Focused && ShowFocusCues)
                    Fluent.DrawFocus(g, ClientRectangle, radius, s);
            }
            catch (Exception)
            {
                base.OnPaint(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _glyphFont != null)
            {
                _glyphFont.Dispose();
                _glyphFont = null;
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// a section ("card"): rounded rectangle with a subtle border, its background slightly different from the
    /// page; replaces the group boxes visually. The page header names the section, so the card draws no caption
    /// (Text is kept for screen readers and the code that sets it).
    /// </summary>
    public class FluentCard : Panel, IFluentControl
    {
        public FluentCard()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; AccessibleName = value; }
        }

        public virtual void ApplyTheme(Theme.Palette p, bool dark)
        {
            BackColor = Fluent.CardBack;
            ForeColor = p.Text;
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                g.Clear(Fluent.ParentBack(this));
                Fluent.FillRounded(g, ClientRectangle, 6f * Fluent.Scale(this), BackColor, Fluent.CardBorder);
            }
            catch (Exception)
            {
                base.OnPaintBackground(e);
            }
        }
    }

    /// <summary>
    /// rounded field frame around a list or grid (its only child fills it, inset by <see cref="Inset"/> px at
    /// 100 %); gives CheckedListBox / DataGridView the Windows 11 rounded field look without changing them
    /// </summary>
    public class FluentFieldHost : Panel, IFluentControl
    {
        private int _inset = 2;

        public FluentFieldHost()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }

        [DefaultValue(2)]
        public int Inset
        {
            get { return _inset; }
            set { _inset = value; UpdatePadding(); }
        }

        private void UpdatePadding()
        {
            var px = Math.Max(1, (int)Math.Round(_inset * Fluent.Scale(this)));
            if (Padding.All != px)
                Padding = new Padding(px);
        }

        public void ApplyTheme(Theme.Palette p, bool dark)
        {
            BackColor = p.Field;
            UpdatePadding();
            Invalidate();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            UpdatePadding();
            base.OnHandleCreated(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                g.Clear(Fluent.ParentBack(this));
                Fluent.FillRounded(g, ClientRectangle, 4f * Fluent.Scale(this), BackColor, Fluent.FieldBorder);
            }
            catch (Exception)
            {
                base.OnPaintBackground(e);
            }
        }
    }

    /// <summary>
    /// drop-down list with a rounded Windows 11 frame and chevron, drawn over the native closed box (the list
    /// that opens, the items and every event stay the native ComboBox ones)
    /// </summary>
    public class FluentComboBox : ComboBox, IFluentControl
    {
        private const int WM_PAINT = 0x000F;
        private bool _hover;
        private Font _glyphFont;

        public FluentComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
        }

        public void ApplyTheme(Theme.Palette p, bool dark)
        {
            FlatStyle = FlatStyle.Flat;
            BackColor = p.Field;
            ForeColor = p.FieldText;
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT && DropDownStyle == ComboBoxStyle.DropDownList && IsHandleCreated)
            {
                try
                {
                    using (var g = Graphics.FromHwnd(Handle))
                        PaintOver(g);
                }
                catch (Exception) { }
            }
        }

        private void PaintOver(Graphics g)
        {
            var p = Theme.Current;
            var s = Fluent.Scale(this);
            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0)
                return;
            using (var back = new SolidBrush(Fluent.ParentBack(this)))
                g.FillRectangle(back, rect);
            var fill = Enabled && (_hover || DroppedDown) ? Fluent.FieldHover : p.Field;
            var border = Enabled && (Focused || DroppedDown) ? Fluent.Accent : Fluent.FieldBorder;
            Fluent.FillRounded(g, rect, 4f * s, fill, border);
            var chevronWidth = (int)Math.Round(28 * s);
            var textRect = new Rectangle((int)Math.Round(8 * s), 0, Math.Max(1, rect.Width - chevronWidth - (int)Math.Round(8 * s)), rect.Height);
            var color = Enabled ? p.FieldText : Fluent.DisabledText;
            TextRenderer.DrawText(g, Text, Font, textRect, color,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
            var chevronRect = new Rectangle(rect.Width - chevronWidth, 0, chevronWidth - (int)Math.Round(4 * s), rect.Height);
            if (_glyphFont == null)
                _glyphFont = Fluent.CreateIconFont(7f);
            var chevronColor = Enabled ? p.SubtleText : Fluent.DisabledText;
            if (_glyphFont != null)
            {
                TextRenderer.DrawText(g, Fluent.GlyphChevronDown, _glyphFont, chevronRect, chevronColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                    TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
            }
            else
            {
                var old = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float cx = chevronRect.Left + chevronRect.Width / 2f, cy = chevronRect.Top + chevronRect.Height / 2f, w = 4f * s;
                using (var pen = new Pen(chevronColor, Math.Max(1f, s)))
                    g.DrawLines(pen, new[] { new PointF(cx - w, cy - w / 2), new PointF(cx, cy + w / 2), new PointF(cx + w, cy - w / 2) });
                g.SmoothingMode = old;
            }
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnSelectedIndexChanged(EventArgs e) { base.OnSelectedIndexChanged(e); Invalidate(); }
        protected override void OnDropDownClosed(EventArgs e) { base.OnDropDownClosed(e); Invalidate(); }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Invalidate(); }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _glyphFont != null)
            {
                _glyphFont.Dispose();
                _glyphFont = null;
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>page title (Windows Settings style, larger semibold UI font; bold for Japanese/Chinese)</summary>
    public class FluentHeader : Label, IFluentControl, IFluentOwnFont
    {
        public FluentHeader()
        {
            AutoSize = true;
            UseMnemonic = false;
            var font = Theme.CreateTitleFont(15f);
            if (font != null)
                Font = font;
        }

        public void ApplyTheme(Theme.Palette p, bool dark)
        {
            ForeColor = p.Text;
        }
    }
}
