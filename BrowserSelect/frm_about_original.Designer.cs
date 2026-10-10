namespace BrowserSelect
{
    partial class frm_about_original
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Windows 11 look (v1.5.8.0): title, a card with the credits and links and a card with the donation
        /// (QR code, address, Copy). The final positions are set in code (frm_about_original.LayoutWindow).
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_header = new BrowserSelect.UI.FluentHeader();
            this.card_info = new BrowserSelect.UI.FluentCard();
            this.card_donate = new BrowserSelect.UI.FluentCard();
            this.lbl_intro = new System.Windows.Forms.Label();
            this.lbl_coded = new System.Windows.Forms.Label();
            this.lbl_contact = new System.Windows.Forms.Label();
            this.lnk_mail = new System.Windows.Forms.LinkLabel();
            this.lbl_github = new System.Windows.Forms.Label();
            this.lnk_github = new System.Windows.Forms.LinkLabel();
            this.lbl_separator = new System.Windows.Forms.Label();
            this.lbl_btc = new System.Windows.Forms.Label();
            this.pic_btc = new System.Windows.Forms.PictureBox();
            this.btn_copy = new BrowserSelect.UI.FluentButton();
            this.lnk_btc = new System.Windows.Forms.LinkLabel();
            this.btn_close = new BrowserSelect.UI.FluentButton();
            this.card_info.SuspendLayout();
            this.card_donate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_header
            // 
            this.lbl_header.AutoSize = true;
            this.lbl_header.Location = new System.Drawing.Point(20, 16);
            this.lbl_header.Name = "lbl_header";
            this.lbl_header.TabIndex = 0;
            this.lbl_header.Text = "Original project";
            // 
            // card_info
            // 
            this.card_info.Controls.Add(this.lbl_intro);
            this.card_info.Controls.Add(this.lbl_coded);
            this.card_info.Controls.Add(this.lbl_contact);
            this.card_info.Controls.Add(this.lnk_mail);
            this.card_info.Controls.Add(this.lbl_github);
            this.card_info.Controls.Add(this.lnk_github);
            this.card_info.Location = new System.Drawing.Point(20, 56);
            this.card_info.Name = "card_info";
            this.card_info.Size = new System.Drawing.Size(460, 150);
            this.card_info.TabIndex = 1;
            // 
            // lbl_intro
            // 
            this.lbl_intro.AutoSize = true;
            this.lbl_intro.MaximumSize = new System.Drawing.Size(428, 0);
            this.lbl_intro.Location = new System.Drawing.Point(16, 16);
            this.lbl_intro.Name = "lbl_intro";
            this.lbl_intro.Size = new System.Drawing.Size(428, 50);
            this.lbl_intro.TabIndex = 0;
            this.lbl_intro.Text = "This version of BrowserSelect is a fork of the original BrowserSelect project.";
            this.lbl_intro.UseMnemonic = false;
            // 
            // lbl_coded
            // 
            this.lbl_coded.AutoSize = true;
            this.lbl_coded.Location = new System.Drawing.Point(16, 76);
            this.lbl_coded.Name = "lbl_coded";
            this.lbl_coded.TabIndex = 1;
            this.lbl_coded.Text = "Coded By: Bor691";
            this.lbl_coded.UseMnemonic = false;
            // 
            // lbl_contact
            // 
            this.lbl_contact.AutoSize = true;
            this.lbl_contact.Location = new System.Drawing.Point(16, 98);
            this.lbl_contact.Name = "lbl_contact";
            this.lbl_contact.TabIndex = 2;
            this.lbl_contact.Text = "Contact:";
            // 
            // lnk_mail
            // 
            this.lnk_mail.AutoSize = true;
            this.lnk_mail.Location = new System.Drawing.Point(70, 98);
            this.lnk_mail.Name = "lnk_mail";
            this.lnk_mail.TabIndex = 3;
            this.lnk_mail.TabStop = true;
            this.lnk_mail.Text = "me@bor691.ir";
            this.lnk_mail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_mail_LinkClicked);
            // 
            // lbl_github
            // 
            this.lbl_github.AutoSize = true;
            this.lbl_github.Location = new System.Drawing.Point(16, 120);
            this.lbl_github.Name = "lbl_github";
            this.lbl_github.TabIndex = 4;
            this.lbl_github.Text = "GitHub:";
            // 
            // lnk_github
            // 
            this.lnk_github.AutoSize = true;
            this.lnk_github.Location = new System.Drawing.Point(70, 120);
            this.lnk_github.Name = "lnk_github";
            this.lnk_github.TabIndex = 5;
            this.lnk_github.TabStop = true;
            this.lnk_github.Text = "https://github.com/zumoshi/BrowserSelect";
            this.lnk_github.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_github_LinkClicked);
            // 
            // lbl_separator (separator of the classic layout; the cards separate the sections now)
            // 
            this.lbl_separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_separator.Location = new System.Drawing.Point(1, 1);
            this.lbl_separator.Name = "lbl_separator";
            this.lbl_separator.Size = new System.Drawing.Size(2, 2);
            this.lbl_separator.TabIndex = 7;
            this.lbl_separator.Visible = false;
            // 
            // card_donate
            // 
            this.card_donate.Controls.Add(this.lbl_btc);
            this.card_donate.Controls.Add(this.pic_btc);
            this.card_donate.Controls.Add(this.lnk_btc);
            this.card_donate.Controls.Add(this.btn_copy);
            this.card_donate.Location = new System.Drawing.Point(20, 218);
            this.card_donate.Name = "card_donate";
            this.card_donate.Size = new System.Drawing.Size(460, 132);
            this.card_donate.TabIndex = 2;
            // 
            // pic_btc
            // 
            this.pic_btc.Image = global::BrowserSelect.Properties.Resources.bitcoin;
            this.pic_btc.Location = new System.Drawing.Point(16, 16);
            this.pic_btc.Name = "pic_btc";
            this.pic_btc.Size = new System.Drawing.Size(100, 100);
            this.pic_btc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_btc.TabIndex = 0;
            this.pic_btc.TabStop = false;
            // 
            // lbl_btc
            // 
            this.lbl_btc.AutoSize = true;
            this.lbl_btc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_btc.Location = new System.Drawing.Point(132, 20);
            this.lbl_btc.Name = "lbl_btc";
            this.lbl_btc.TabIndex = 1;
            this.lbl_btc.Text = "Bor691 (original author) - Bitcoin:";
            this.lbl_btc.UseMnemonic = false;
            // 
            // lnk_btc
            // 
            this.lnk_btc.AutoSize = true;
            this.lnk_btc.Location = new System.Drawing.Point(132, 44);
            this.lnk_btc.Name = "lnk_btc";
            this.lnk_btc.TabIndex = 2;
            this.lnk_btc.TabStop = true;
            this.lnk_btc.Text = "1BA5Ndo24jtRgTEsvmGkrqRWTaJS4F3zNh";
            this.lnk_btc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_btc_LinkClicked);
            // 
            // btn_copy
            // 
            this.btn_copy.Location = new System.Drawing.Point(132, 70);
            this.btn_copy.Name = "btn_copy";
            this.btn_copy.Size = new System.Drawing.Size(150, 30);
            this.btn_copy.TabIndex = 3;
            this.btn_copy.Text = "Copy Address";
            this.btn_copy.Click += new System.EventHandler(this.btn_copy_Click);
            // 
            // btn_close
            // 
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Location = new System.Drawing.Point(388, 366);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(92, 30);
            this.btn_close.TabIndex = 12;
            this.btn_close.Text = "Close";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // frm_about_original
            // 
            this.AcceptButton = this.btn_close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(500, 416);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.card_donate);
            this.Controls.Add(this.card_info);
            this.Controls.Add(this.lbl_separator);
            this.Controls.Add(this.lbl_header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_about_original";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BrowserSelect: Original project";
            this.Load += new System.EventHandler(this.frm_about_original_Load);
            this.card_info.ResumeLayout(false);
            this.card_info.PerformLayout();
            this.card_donate.ResumeLayout(false);
            this.card_donate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrowserSelect.UI.FluentHeader lbl_header;
        private BrowserSelect.UI.FluentCard card_info;
        private BrowserSelect.UI.FluentCard card_donate;
        private System.Windows.Forms.Label lbl_intro;
        private System.Windows.Forms.Label lbl_coded;
        private System.Windows.Forms.Label lbl_contact;
        private System.Windows.Forms.LinkLabel lnk_mail;
        private System.Windows.Forms.Label lbl_github;
        private System.Windows.Forms.LinkLabel lnk_github;
        private System.Windows.Forms.Label lbl_separator;
        private System.Windows.Forms.Label lbl_btc;
        private System.Windows.Forms.PictureBox pic_btc;
        private BrowserSelect.UI.FluentButton btn_copy;
        private System.Windows.Forms.LinkLabel lnk_btc;
        private BrowserSelect.UI.FluentButton btn_close;
    }
}
