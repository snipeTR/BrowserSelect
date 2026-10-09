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
    public partial class BrowserUC : UserControl {
        public Browser browser;
        public BrowserUC(Browser b,int index) {
            InitializeComponent();

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
