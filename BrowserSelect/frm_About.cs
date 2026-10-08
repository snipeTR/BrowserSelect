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
        /// <summary>bitcoin address of the original author (Bor691)</summary>
        private const string BitcoinOriginal = "1BA5Ndo24jtRgTEsvmGkrqRWTaJS4F3zNh";
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
            lbl_coded.Text = Strings.About_CodedBy;
            lbl_contact.Text = Strings.About_Contact;
            lbl_github.Text = Strings.About_GitHub;
            lbl_fork.Text = Strings.About_ForkBy;
            lbl_fork_github.Text = Strings.About_GitHub;
            label3.Text = Strings.About_Description;
            label5.Text = Strings.About_DonateText;
            lbl_btc_fork.Text = Strings.About_DonateFork;
            label6.Text = Strings.About_DonateOriginal;
            btn_btc_fork_copy.Text = Strings.About_CopyAddress;
            btn_bitcoin_copy.Text = Strings.About_CopyAddress;
            btn_close.Text = Strings.Common_Close;
            lnk_btc_fork.Text = BitcoinFork;
            linkLabel3.Text = BitcoinOriginal;

            // links follow their (translated) labels
            PlaceAfter(lbl_contact, linkLabel1);
            PlaceAfter(lbl_github, linkLabel2);
            PlaceAfter(lbl_fork_github, lnk_fork);
        }

        private static void PlaceAfter(Label label, Control link)
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("Mailto:me@bor691.ir");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void frm_About_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenBitcoinUri(BitcoinOriginal);
        }

        private void lnk_btc_fork_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenBitcoinUri(BitcoinFork);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_bitcoin_copy_Click(object sender, EventArgs e)
        {
            CopyAddress(BitcoinOriginal, btn_bitcoin_copy);
        }

        private void btn_btc_fork_copy_Click(object sender, EventArgs e)
        {
            CopyAddress(BitcoinFork, btn_btc_fork_copy);
        }

        /// <summary>opens the bitcoin: link in the installed wallet; copies the address if there is none</summary>
        private void OpenBitcoinUri(string address)
        {
            try
            {
                System.Diagnostics.Process.Start("bitcoin:" + address);
            }
            catch (Exception)
            {
                // no application handles bitcoin: links
                CopyAddress(address, address == BitcoinFork ? btn_btc_fork_copy : btn_bitcoin_copy);
            }
        }

        /// <summary>copies the address to the clipboard and shows "Copied!" on the button for a moment</summary>
        private void CopyAddress(string address, Button button)
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
