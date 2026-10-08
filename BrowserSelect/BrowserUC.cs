using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrowserSelect.Localization;

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
