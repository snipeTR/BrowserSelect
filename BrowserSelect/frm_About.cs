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
    public partial class frm_About : Form
    {
        /// <summary>bitcoin address of the maintainer of this fork (snipeTR)</summary>
        private const string BitcoinFork = "bc1q3jqugh66ctwzqr7tqjafunlpaaejqgt265rwjq";

        public frm_About()
        {
            InitializeComponent();
            // big title (Segoe UI Semibold; bold for Japanese/Chinese), set before the theme measures the texts
            var title = Theme.CreateTitleFont(20f);
            if (title != null)
                label1.Font = title;
            // Windows 10/11 look (fonts, colors, title bar); before the texts so labels are measured with the final font
            Theme.Apply(this);
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
            LayoutWindow();
        }

        /// <summary>
        /// Windows 11 layout (v1.5.8.0), done in code for the final (DPI scaled) fonts and the translated texts:
        /// header with icon, name, version and maintainer; cards for the description, the links and the donation;
        /// Close at the bottom right. The window height follows the content.
        /// </summary>
        private void LayoutWindow()
        {
            try
            {
                SuspendLayout();
                var s = Fluent.Scale(this);
                Func<float, int> px = v => FluentLayout.Px(v, s);
                int margin = px(20), pad = px(16), gap = px(12), width = px(560);
                var buttonHeight = px(30);

                // header
                pictureBox1.SetBounds(margin, margin, px(64), px(64));
                var textLeft = pictureBox1.Right + px(16);
                label1.Location = new Point(textLeft, margin - px(2));
                lab_ver.Location = new Point(label1.Right + px(4), label1.Bottom - lab_ver.Height - px(3));
                FluentLayout.Wrap(lbl_fork, textLeft, label1.Bottom + px(2), margin + width - textLeft);
                var y = Math.Max(pictureBox1.Bottom, lbl_fork.Bottom) + px(20);

                // description
                var inner = width - 2 * pad;
                FluentLayout.Wrap(label3, pad, pad, inner);
                card_info.SetBounds(margin, y, width, label3.Bottom + pad);
                y = card_info.Bottom + gap;

                // links: GitHub of this fork, "Original project info..." on the same row if it fits
                lbl_fork_github.Location = new Point(pad, pad);
                PlaceAfter(lbl_fork_github, lnk_fork);
                lnk_fork.Top = pad;
                btn_original.Size = new Size(FluentLayout.ButtonWidth(btn_original, px(160), s), buttonHeight);
                int linksBottom;
                if (lnk_fork.Right + gap + btn_original.Width <= width - pad)
                {
                    btn_original.Location = new Point(width - pad - btn_original.Width, pad);
                    var row = Math.Max(btn_original.Height, lnk_fork.Height);
                    lbl_fork_github.Top = lnk_fork.Top = pad + (row - lnk_fork.Height) / 2;
                    linksBottom = pad + row;
                }
                else
                {
                    btn_original.Location = new Point(pad, Math.Max(lnk_fork.Bottom, lbl_fork_github.Bottom) + gap);
                    linksBottom = btn_original.Bottom;
                }
                card_links.SetBounds(margin, y, width, linksBottom + pad);
                y = card_links.Bottom + gap;

                // donation: text, QR code on the left, name / address / Copy on the right
                var top = FluentLayout.Wrap(label5, pad, pad, inner) + gap;
                pic_btc_fork.SetBounds(pad, top, px(100), px(100));
                var column = pic_btc_fork.Right + pad;
                var columnWidth = width - pad - column;
                var cy = FluentLayout.Wrap(lbl_btc_fork, column, top, columnWidth) + px(6);
                lnk_btc_fork.Location = new Point(column, cy);
                cy = lnk_btc_fork.Bottom + gap;
                btn_btc_fork_copy.SetBounds(column, cy,
                    FluentLayout.ButtonWidth(btn_btc_fork_copy, px(140), s, Strings.About_CopyAddress, Strings.About_Copied), buttonHeight);
                card_donate.SetBounds(margin, y, width, Math.Max(pic_btc_fork.Bottom, btn_btc_fork_copy.Bottom) + pad);
                y = card_donate.Bottom + px(20);

                btn_close.Size = new Size(FluentLayout.ButtonWidth(btn_close, px(92), s), buttonHeight);
                btn_close.Location = new Point(margin + width - btn_close.Width, y);
                ClientSize = new Size(width + 2 * margin, btn_close.Bottom + margin);
                ResumeLayout(true);
            }
            catch (Exception)
            {
                ResumeLayout(true);
            }
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
