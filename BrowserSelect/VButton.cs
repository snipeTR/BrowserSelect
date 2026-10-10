using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrowserSelect.UI;

namespace BrowserSelect {
    /// <summary>
    /// button at the right edge of the selection window (About, Settings). Windows 11 look (v1.5.8.0): a small
    /// rounded square button with a Segoe Fluent Icons / MDL2 Assets glyph (the text is the tooltip and the
    /// accessible name); without an icon font it is a rounded button with vertical text as before.
    /// </summary>
    class VButton : Button, IFluentControl {
        StringFormat Fmt = new StringFormat();
        private string faketext;
        private string _glyph;
        private Font _glyphFont;
        private bool _hover, _pressed;

        public VButton() {
            Fmt.Alignment = StringAlignment.Center;
            Fmt.LineAlignment = StringAlignment.Center;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
        }

        /// <summary>the (vertical) text of the button; Text itself stays empty</summary>
        public string Caption {
            get { return faketext; }
        }

        /// <summary>icon glyph; the button shows it instead of the vertical text if an icon font is installed</summary>
        public string Glyph {
            get { return _glyph; }
            set { _glyph = value; Invalidate(); }
        }

        /// <summary>true if the glyph is drawn (icon font installed)</summary>
        public bool ShowsGlyph {
            get { return !string.IsNullOrEmpty(_glyph) && Fluent.IconFontName != null; }
        }

        protected override void OnTextChanged(EventArgs e) {
            if(Text.Length>0)
            {
                faketext = Text;
                AccessibleName = faketext;
            }
            Text = "";
            base.OnTextChanged(e);
        }

        public void ApplyTheme(Theme.Palette p, bool dark) {
            ForeColor = p.Text;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected override void OnPaint(PaintEventArgs pevent) {
            var g = pevent.Graphics;
            try {
                var p = Theme.Current;
                var s = Fluent.Scale(this);
                g.Clear(Fluent.ParentBack(this));
                var fill = !Enabled ? Fluent.ButtonDisabledBack
                    : _pressed ? Fluent.ButtonPressed
                    : _hover ? Fluent.ButtonHover
                    : Fluent.ButtonBack;
                var radius = 4f * s;
                Fluent.FillRounded(g, ClientRectangle, radius, fill, Fluent.ButtonBorder);
                var color = !Enabled ? Fluent.DisabledText : _pressed ? p.SubtleText : p.Text;
                if (ShowsGlyph) {
                    if (_glyphFont == null)
                        _glyphFont = Fluent.CreateIconFont(11f);
                    TextRenderer.DrawText(g, _glyph, _glyphFont, ClientRectangle, color,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                        TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
                } else {
                    var state = g.Save();
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.TranslateTransform(Width, 0);
                    g.RotateTransform(90);
                    using (var brush = new SolidBrush(color))
                        g.DrawString(faketext, Font, brush, new Rectangle(0, 0, Height, Width), Fmt);
                    g.Restore(state);
                }
                if (Focused && ShowFocusCues)
                    Fluent.DrawFocus(g, ClientRectangle, radius, s);
            } catch (Exception) {
                base.OnPaint(pevent);
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (_glyphFont != null) {
                    _glyphFont.Dispose();
                    _glyphFont = null;
                }
                Fmt.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
