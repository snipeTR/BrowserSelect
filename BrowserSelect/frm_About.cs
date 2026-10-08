using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrowserSelect.Localization;

namespace BrowserSelect
{
    public partial class frm_About : Form
    {
        /// <summary>bitcoin address of the maintainer of this fork (snipeTR)</summary>
        private const string BitcoinFork = "bc1q3jqugh66ctwzqr7tqjafunlpaaejqgt265rwjq";

        public frm_About()
        {
            InitializeComponent();
            ApplyTexts();
        }

        /// <summary>sets every visible text from Localization\Strings.resx (current UI language)</summary>
        private void ApplyTexts()
        {
            Text = Strings.About_Title;
            lbl_fork.Text = Strings.About_ForkBy;
            lbl_fork_github.Text = Strings.About_GitHub;
            label3.Text = Strings.About_Description;
            label5.Text = Strings.About_DonateText;
            lbl_btc_fork.Text = Strings.About_DonateFork;
            btn_btc_fork_copy.Text = Strings.About_CopyAddress;
            btn_original.Text = Strings.About_OriginalButton;
            btn_close.Text = Strings.Common_Close;
            lnk_btc_fork.Text = BitcoinFork;

            // links follow their (translated) labels
            PlaceAfter(lbl_fork_github, lnk_fork);
        }

        /// <summary>credits of the original project (Bor691 / zumoshi) are shown in their own window</summary>
        private void btn_original_Click(object sender, EventArgs e)
        {
            using (var frm = new frm_about_original())
                frm.ShowDialog(this);
        }

        internal static void PlaceAfter(Label label, Control link)
        {
            link.Left = label.Left + label.PreferredWidth + 2;
        }

        private void frm_About_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = IconExtractor.fromFile(Application.ExecutablePath).ToBitmap();
            var v = Application.ProductVersion;
            lab_ver.Text = "v" + v.Remove(v.Length - 2);
            lab_ver.Left = label1.Left + label1.PreferredWidth;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink(((LinkLabel)sender).Text);
        }

        /// <summary>opens a web/mail link; ignores the error if there is no application for it</summary>
        internal static void OpenLink(string target)
        {
            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch (Exception)
            {
                // no default browser / mail client
            }
        }

        private void frm_About_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void lnk_btc_fork_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenBitcoinUri(BitcoinFork, btn_btc_fork_copy);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_btc_fork_copy_Click(object sender, EventArgs e)
        {
            CopyAddress(BitcoinFork, btn_btc_fork_copy);
        }

        /// <summary>opens the bitcoin: link in the installed wallet; copies the address if there is none</summary>
        internal static void OpenBitcoinUri(string address, Button copyButton)
        {
            try
            {
                System.Diagnostics.Process.Start("bitcoin:" + address);
            }
            catch (Exception)
            {
                // no application handles bitcoin: links
                CopyAddress(address, copyButton);
            }
        }

        /// <summary>copies the address to the clipboard and shows "Copied!" on the button for a moment</summary>
        internal static void CopyAddress(string address, Button button)
        {
            try
            {
                Clipboard.SetText(address);
            }
            catch (Exception)
            {
                // clipboard temporarily locked by another application
                return;
            }
            button.Text = Strings.About_Copied;
            var timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                timer.Dispose();
                if (!button.IsDisposed)
                    button.Text = Strings.About_CopyAddress;
            };
            timer.Start();
        }
    }
}
