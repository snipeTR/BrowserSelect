using System;
using System.Drawing;
using System.Windows.Forms;
using BrowserSelect.Localization;
using BrowserSelect.UI;

namespace BrowserSelect
{
    /// <summary>
    /// Credits of the original BrowserSelect project (Bor691 / zumoshi), opened with the
    /// "Original project info..." button of the About window.
    /// </summary>
    public partial class frm_about_original : Form
    {
        /// <summary>bitcoin address of the original author (Bor691)</summary>
        private const string BitcoinOriginal = "1BA5Ndo24jtRgTEsvmGkrqRWTaJS4F3zNh";
        private const string OriginalEmail = "me@bor691.ir";
        private const string OriginalRepository = "https://github.com/zumoshi/BrowserSelect";

        public frm_about_original()
        {
            InitializeComponent();
            // Windows 10/11 look (fonts, colors, title bar); before the texts so labels are measured with the final font
            Theme.Apply(this);
            ApplyTexts();
        }

        /// <summary>sets every visible text from Localization\Strings.resx (current UI language)</summary>
        private void ApplyTexts()
        {
            Text = Strings.AboutOriginal_Title;
            lbl_header.Text = Strings.AboutOriginal_Header;
            lbl_intro.Text = Strings.AboutOriginal_Intro;
            lbl_coded.Text = Strings.About_CodedBy;
            lbl_contact.Text = Strings.About_Contact;
            lnk_mail.Text = OriginalEmail;
            lbl_github.Text = Strings.About_GitHub;
            lnk_github.Text = OriginalRepository;
            lbl_btc.Text = Strings.About_DonateOriginal;
            btn_copy.Text = Strings.About_CopyAddress;
            lnk_btc.Text = BitcoinOriginal;
            btn_close.Text = Strings.Common_Close;

            // links follow their (translated) labels
            frm_About.PlaceAfter(lbl_contact, lnk_mail);
            frm_About.PlaceAfter(lbl_github, lnk_github);
        }

        private void lnk_mail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frm_About.OpenLink("mailto:" + OriginalEmail);
        }

        private void lnk_github_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frm_About.OpenLink(OriginalRepository);
        }

        private void lnk_btc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frm_About.OpenBitcoinUri(BitcoinOriginal, btn_copy);
        }

        private void btn_copy_Click(object sender, EventArgs e)
        {
            frm_About.CopyAddress(BitcoinOriginal, btn_copy);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frm_about_original_Load(object sender, EventArgs e)
        {
            LayoutWindow();
        }

        /// <summary>
        /// Windows 11 layout (v1.5.8.0), done in code for the final (DPI scaled) fonts and the translated texts:
        /// title, a card with the credits and links, a card with the donation, Close at the bottom right
        /// </summary>
        private void LayoutWindow()
        {
            try
            {
                SuspendLayout();
                var s = Fluent.Scale(this);
                Func<float, int> px = v => FluentLayout.Px(v, s);
                int margin = px(20), pad = px(16), gap = px(12), width = px(480), line = px(6);
                var inner = width - 2 * pad;

                lbl_header.Location = new Point(margin, px(14));
                var y = lbl_header.Bottom + px(14);

                var cy = FluentLayout.Wrap(lbl_intro, pad, pad, inner) + gap;
                cy = FluentLayout.Wrap(lbl_coded, pad, cy, inner) + line;
                lbl_contact.Location = new Point(pad, cy);
                frm_About.PlaceAfter(lbl_contact, lnk_mail);
                lnk_mail.Top = cy;
                cy = Math.Max(lbl_contact.Bottom, lnk_mail.Bottom) + line;
                lbl_github.Location = new Point(pad, cy);
                frm_About.PlaceAfter(lbl_github, lnk_github);
                lnk_github.Top = cy;
                cy = Math.Max(lbl_github.Bottom, lnk_github.Bottom);
                card_info.SetBounds(margin, y, width, cy + pad);
                y = card_info.Bottom + gap;

                pic_btc.SetBounds(pad, pad, px(100), px(100));
                var column = pic_btc.Right + pad;
                cy = FluentLayout.Wrap(lbl_btc, column, pad + px(2), width - pad - column) + line;
                lnk_btc.Location = new Point(column, cy);
                cy = lnk_btc.Bottom + gap;
                btn_copy.SetBounds(column, cy,
                    FluentLayout.ButtonWidth(btn_copy, px(140), s, Strings.About_CopyAddress, Strings.About_Copied), px(30));
                card_donate.SetBounds(margin, y, width, Math.Max(pic_btc.Bottom, btn_copy.Bottom) + pad);
                y = card_donate.Bottom + px(20);

                btn_close.Size = new Size(FluentLayout.ButtonWidth(btn_close, px(92), s), px(30));
                btn_close.Location = new Point(margin + width - btn_close.Width, y);
                ClientSize = new Size(width + 2 * margin, btn_close.Bottom + margin);
                ResumeLayout(true);
            }
            catch (Exception)
            {
                ResumeLayout(true);
            }
        }
    }
}
