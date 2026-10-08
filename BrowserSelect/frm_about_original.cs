using System;
using System.Windows.Forms;
using BrowserSelect.Localization;

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
    }
}
