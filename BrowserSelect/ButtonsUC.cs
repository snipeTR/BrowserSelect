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

namespace BrowserSelect
{
    /// <summary>
    /// the column of buttons at the right edge of the selection window: About and Settings at the top (the ?
    /// help / update button of Form1 is placed at the bottom of this column). Windows 11 look (v1.5.8.0): small
    /// rounded icon buttons with tooltips; vertical text buttons if no icon font is installed.
    /// </summary>
    public partial class ButtonsUC : UserControl
    {

        public Form callingForm;
        private readonly ToolTip _tips = new ToolTip();

        public ButtonsUC(Form callingForm)
        {
            this.callingForm = callingForm;
            InitializeComponent();
            add_button(Strings.Main_About, Fluent.GlyphInfo, show_about, 0);
            add_button(Strings.Main_Settings, Fluent.GlyphSettings, show_setting, 1);

            // http://www.telerik.com/blogs/winforms-scaling-at-large-dpi-settings-is-it-even-possible-
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new Padding(0);
            Disposed += (s, e) => _tips.Dispose();
        }

        private void show_setting(object sender, EventArgs e)
        {
            new frm_settings(this.callingForm).ShowDialog();
            // pick up changes made in settings (hidden browsers, order, icons, shortcuts, ...)
            (this.callingForm as Form1)?.updateBrowsers();
        }

        private void show_about(object sender, EventArgs e)
        {
            new frm_About().ShowDialog();
        }

        private List<VButton> vbtn = new List<VButton>();
        private void add_button(string text, string glyph, EventHandler evt, int index)
        {
            // code for vertical buttons on the right, they are custom controls
            // without support for form designer, so we initiate them in code
            var btn = new VButton();
            btn.Text = text;
            btn.Glyph = glyph;
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btn.Width = 20;
            btn.Height = 75;
            btn.Top = index * 80;
            btn.Left = 5;
            btn.Cursor = Cursors.Hand;
            Controls.Add(btn);
            btn.Click += evt;
            if (btn.ShowsGlyph)
                _tips.SetToolTip(btn, text);

            vbtn.Add(btn);
        }

        /// <summary>true if the buttons are icon buttons (Segoe Fluent Icons / MDL2 Assets installed)</summary>
        public bool IconButtons
        {
            get { return vbtn.Count > 0 && vbtn[0].ShowsGlyph; }
        }

        /// <summary>
        /// size of an icon button (square) in pixels at the current scale
        /// </summary>
        public static int ButtonSize(float scale)
        {
            return (int)Math.Round(30 * scale);
        }

        /// <summary>
        /// lays the column out for browser cards of the given height (pixels); <paramref name="inset"/> is the
        /// space between the edge of a card control and the drawn card, so the buttons line up with the cards
        /// </summary>
        public void LayoutColumn(int height, int inset, float scale)
        {
            if (!IconButtons)
                return; // vertical text buttons keep the classic layout
            var size = ButtonSize(scale);
            var gap = (int)Math.Round(6 * scale);
            for (int i = 0; i < vbtn.Count; i++)
                vbtn[i].SetBounds(inset, inset + i * (size + gap), size, size);
            Size = new Size(size + 2 * inset, height);
        }
    }
}
