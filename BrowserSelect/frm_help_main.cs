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
    public partial class frm_help_main : Form
    {
        public frm_help_main()
        {
            InitializeComponent();
            // Windows 10/11 look (fonts, colors, title bar); before the texts so labels are measured with the final font
            Theme.Apply(this);
            ApplyTexts();
        }

        /// <summary>sets every visible text from Localization\Strings.resx (current UI language)</summary>
        private void ApplyTexts()
        {
            Text = Strings.HelpMain_Title;
            txt_help.Text = HelpText.ForTextBox(Strings.HelpMain_Text);
            btn_close.Text = Strings.Common_Close;
            ActiveControl = btn_close;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // after the window got its final (DPI scaled) size
            HelpText.PlaceCloseButton(this, btn_close);
            HelpText.ScrollToTop(txt_help);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            // the Close button follows the bottom right corner itself instead of relying on its anchor
            if (IsHandleCreated && Visible)
                HelpText.PlaceCloseButton(this, btn_close);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    /// <summary>helpers shared by the two help windows</summary>
    internal static class HelpText
    {
        /// <summary>resx values use \n line breaks; a multiline TextBox needs \r\n</summary>
        public static string ForTextBox(string text)
        {
            return (text ?? "").Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        /// <summary>
        /// puts the Close button in the free strip below the text (bottom padding of the window). This
        /// window is designed at 150% and scaled down; the anchored button could otherwise end up over
        /// the text.
        /// </summary>
        public static void PlaceCloseButton(Form form, Button button)
        {
            var strip = form.Padding.Bottom;
            var client = form.ClientSize;
            var location = new Point(client.Width - form.Padding.Right - button.Width,
                client.Height - strip + Math.Max(0, (strip - button.Height) / 2));
            if (button.Location != location)
                button.Location = location;
        }

        /// <summary>no selected text and the beginning of the help visible</summary>
        public static void ScrollToTop(TextBox box)
        {
            box.SelectionStart = 0;
            box.SelectionLength = 0;
            box.ScrollToCaret();
        }
    }
}
