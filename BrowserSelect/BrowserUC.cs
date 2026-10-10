using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrowserSelect.Localization;
using BrowserSelect.UI;

namespace BrowserSelect {
    /// <summary>
    /// one browser of the selection window: a rounded Windows 11 card (icon, name, shortcut keys, "Always"
    /// button) that shows hover/pressed states with the Windows accent color. Clicking anywhere on the card
    /// opens the link (see Form1.browser_click).
    /// </summary>
    public partial class BrowserUC : UserControl, IFluentControl {
        public Browser browser;
        private bool _hover, _pressed;

        public BrowserUC(Browser b,int index) {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            button1.Cursor = Cursors.Default;
            HookHover(this);

            this.browser = b;
            button1.Text = Strings.Main_Always;

            name.Text = b.name;
            // number keys 1-9 select by position; letters come from the name or the custom shortcut
            var keys = new List<string>();
            if (index < 9)
                keys.Add(Convert.ToString(index + 1));
            keys.AddRange(b.shortcuts.Select(c => c.ToString()));
            shortcuts.Text = "( " + String.Join(",", keys) + " )";
            shortcuts.ForeColor = Color.FromKnownColor(KnownColor.GrayText);
            // Windows 10/11 look: the shortcut keys row is secondary text in the UI font at 8pt; the row is
            // 1px taller so the new font is not clipped (it still ends exactly where the Always button starts).
            // Japanese/Chinese UI: Latin-only keys keep Segoe UI 8pt (the row has no room for a 9pt CJK
            // font); keys with other characters get the CJK UI font at 9pt (see Theme.CreateUiFont)
            try
            {
                var small = Theme.CreateUiFont(8f, FontStyle.Regular, shortcuts.Text);
                if (small != null)
                {
                    shortcuts.Font = small;
                    shortcuts.Height = Math.Max(shortcuts.Height, button1.Top - shortcuts.Top);
                }
            }
            catch (Exception) { }
            icon.Image = b.string2Icon();//.ToBitmap();
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            // screen readers: the card is the button of this browser
            AccessibleName = b.name;
            AccessibleRole = AccessibleRole.PushButton;
            BackColor = CardFill;
        }

        // ---- card look (hover / pressed) --------------------------------------------------------------

        /// <summary>background of the card in its current state; the children inherit it</summary>
        private Color CardFill
        {
            get
            {
                var dark = Theme.IsDark;
                if (_pressed)
                    return Fluent.Mix(Fluent.CardBack, Fluent.Accent, dark ? 0.18f : 0.12f);
                if (_hover)
                    return Fluent.Mix(Fluent.CardBack, Fluent.Accent, dark ? 0.10f : 0.06f);
                return Fluent.CardBack;
            }
        }

        public void ApplyTheme(Theme.Palette p, bool dark)
        {
            BackColor = CardFill;
            Invalidate(true);
        }

        /// <summary>mouse over the card or any of its children (labels, icon, Always button)</summary>
        private void HookHover(Control c)
        {
            c.MouseEnter += (s, e) => SetState(true, _pressed);
            c.MouseLeave += (s, e) => SetState(IsMouseOver(), false);
            c.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    SetState(true, true);
            };
            c.MouseUp += (s, e) => SetState(IsMouseOver(), false);
            foreach (Control child in c.Controls)
                HookHover(child);
        }

        private bool IsMouseOver()
        {
            try
            {
                return !IsDisposed && ClientRectangle.Contains(PointToClient(Cursor.Position));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetState(bool hover, bool pressed)
        {
            if (hover == _hover && pressed == _pressed)
                return;
            _hover = hover;
            _pressed = pressed;
            BackColor = CardFill; // repaints the card and the children (they inherit the color)
            Invalidate(true);
        }

        /// <summary>the card: rounded rectangle (8 px like the Windows 11 window corners) inset by 3 px</summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                var s = Fluent.Scale(this);
                g.Clear(Parent != null ? Parent.BackColor : Theme.Current.Back);
                var inset = (int)Math.Round(3 * s);
                var card = Rectangle.Inflate(ClientRectangle, -inset, -inset);
                var border = _hover || _pressed ? Fluent.Accent : Fluent.CardBorder;
                Fluent.FillRounded(g, card, 8f * s, CardFill, border);
            }
            catch (Exception)
            {
                base.OnPaintBackground(e);
            }
        }
        public new event EventHandler Click {
            add {
                base.Click += value;
                foreach (Control control in Controls) {
                    control.Click += value;
                }
            }
            remove {
                base.Click -= value;
                foreach (Control control in Controls) {
                    control.Click -= value;
                }
            }
        }

        public bool Always { get; set; } = false;

        private void button1_Click(object sender, EventArgs e)
        {
            Always = true;
        }
    }
}
