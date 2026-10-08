using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrowserSelect.Localization;
using BrowserSelect.UI;

namespace BrowserSelect
{
    public partial class frm_help_rules : Form
    {
        public frm_help_rules()
        {
            InitializeComponent();
            // Windows 10/11 look (fonts, colors, title bar); before the texts so labels are measured with the final font
            Theme.Apply(this);
            ApplyTexts();
        }

        /// <summary>sets every visible text from Localization\Strings.resx (current UI language)</summary>
        private void ApplyTexts()
        {
            Text = Strings.HelpRules_Title;
            txt_help.Text = HelpText.ForTextBox(Strings.HelpRules_Text);
            btn_close.Text = Strings.Common_Close;
            ActiveControl = btn_close;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            HelpText.ScrollToTop(txt_help);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
