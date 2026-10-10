namespace BrowserSelect {
    partial class frm_About {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// <summary>
        /// Windows 11 look (v1.5.8.0): header (icon, name, version, maintainer) and three cards (description,
        /// links, donation). The final positions and sizes are set in code (frm_About.LayoutWindow) because
        /// the texts are translated.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_About));
            this.label1 = new BrowserSelect.UI.FluentHeader();
            this.lbl_fork = new System.Windows.Forms.Label();
            this.card_info = new BrowserSelect.UI.FluentCard();
            this.card_links = new BrowserSelect.UI.FluentCard();
            this.card_donate = new BrowserSelect.UI.FluentCard();
            this.lbl_fork_github = new System.Windows.Forms.Label();
            this.lnk_fork = new System.Windows.Forms.LinkLabel();
            this.btn_original = new BrowserSelect.UI.FluentButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_btc_fork = new System.Windows.Forms.Label();
            this.pic_btc_fork = new System.Windows.Forms.PictureBox();
            this.btn_btc_fork_copy = new BrowserSelect.UI.FluentButton();
            this.lnk_btc_fork = new System.Windows.Forms.LinkLabel();
            this.btn_close = new BrowserSelect.UI.FluentButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lab_ver = new System.Windows.Forms.Label();
            this.card_info.SuspendLayout();
            this.card_links.SuspendLayout();
            this.card_donate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc_fork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(20, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 20);
            this.label1.Name = "label1";
            this.label1.TabIndex = 1;
            this.label1.Text = "BrowserSelect";
            // 
            // lab_ver
            // 
            this.lab_ver.AutoSize = true;
            this.lab_ver.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_ver.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lab_ver.Location = new System.Drawing.Point(280, 24);
            this.lab_ver.Name = "lab_ver";
            this.lab_ver.TabIndex = 22;
            this.lab_ver.Text = "v%.%";
            this.lab_ver.UseMnemonic = false;
            // 
            // lbl_fork
            // 
            this.lbl_fork.AutoSize = true;
            this.lbl_fork.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_fork.Location = new System.Drawing.Point(100, 60);
            this.lbl_fork.Name = "lbl_fork";
            this.lbl_fork.TabIndex = 7;
            this.lbl_fork.Text = "This fork is maintained by: snipeTR";
            this.lbl_fork.UseMnemonic = false;
            // 
            // card_info
            // 
            this.card_info.Controls.Add(this.label3);
            this.card_info.Location = new System.Drawing.Point(20, 100);
            this.card_info.Name = "card_info";
            this.card_info.Size = new System.Drawing.Size(560, 130);
            this.card_info.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(528, 98);
            this.label3.TabIndex = 0;
            this.label3.Text = resources.GetString("label3.Text");
            this.label3.UseMnemonic = false;
            // 
            // card_links
            // 
            this.card_links.Controls.Add(this.lbl_fork_github);
            this.card_links.Controls.Add(this.lnk_fork);
            this.card_links.Controls.Add(this.btn_original);
            this.card_links.Location = new System.Drawing.Point(20, 240);
            this.card_links.Name = "card_links";
            this.card_links.Size = new System.Drawing.Size(560, 62);
            this.card_links.TabIndex = 3;
            // 
            // lbl_fork_github
            // 
            this.lbl_fork_github.AutoSize = true;
            this.lbl_fork_github.Location = new System.Drawing.Point(16, 22);
            this.lbl_fork_github.Name = "lbl_fork_github";
            this.lbl_fork_github.TabIndex = 0;
            this.lbl_fork_github.Text = "GitHub:";
            // 
            // lnk_fork
            // 
            this.lnk_fork.AutoSize = true;
            this.lnk_fork.Location = new System.Drawing.Point(64, 22);
            this.lnk_fork.Name = "lnk_fork";
            this.lnk_fork.TabIndex = 1;
            this.lnk_fork.TabStop = true;
            this.lnk_fork.Text = "https://github.com/snipeTR/BrowserSelect";
            this.lnk_fork.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // btn_original
            // 
            this.btn_original.Location = new System.Drawing.Point(360, 16);
            this.btn_original.Name = "btn_original";
            this.btn_original.Size = new System.Drawing.Size(184, 30);
            this.btn_original.TabIndex = 2;
            this.btn_original.Text = "Original project info...";
            this.btn_original.Click += new System.EventHandler(this.btn_original_Click);
            // 
            // card_donate
            // 
            this.card_donate.Controls.Add(this.label5);
            this.card_donate.Controls.Add(this.lbl_btc_fork);
            this.card_donate.Controls.Add(this.pic_btc_fork);
            this.card_donate.Controls.Add(this.lnk_btc_fork);
            this.card_donate.Controls.Add(this.btn_btc_fork_copy);
            this.card_donate.Location = new System.Drawing.Point(20, 312);
            this.card_donate.Name = "card_donate";
            this.card_donate.Size = new System.Drawing.Size(560, 180);
            this.card_donate.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(528, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "If you find this program useful and would like to thank the developers you may do" +
    "nate using bitcoin.";
            this.label5.UseMnemonic = false;
            // 
            // pic_btc_fork
            // 
            this.pic_btc_fork.Image = global::BrowserSelect.Properties.Resources.bitcoin_snipetr;
            this.pic_btc_fork.Location = new System.Drawing.Point(16, 56);
            this.pic_btc_fork.Name = "pic_btc_fork";
            this.pic_btc_fork.Size = new System.Drawing.Size(100, 100);
            this.pic_btc_fork.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_btc_fork.TabIndex = 1;
            this.pic_btc_fork.TabStop = false;
            // 
            // lbl_btc_fork
            // 
            this.lbl_btc_fork.AutoSize = true;
            this.lbl_btc_fork.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_btc_fork.Location = new System.Drawing.Point(132, 60);
            this.lbl_btc_fork.Name = "lbl_btc_fork";
            this.lbl_btc_fork.TabIndex = 2;
            this.lbl_btc_fork.Text = "snipeTR (this fork) - Bitcoin:";
            this.lbl_btc_fork.UseMnemonic = false;
            // 
            // lnk_btc_fork
            // 
            this.lnk_btc_fork.AutoSize = true;
            this.lnk_btc_fork.Location = new System.Drawing.Point(132, 84);
            this.lnk_btc_fork.Name = "lnk_btc_fork";
            this.lnk_btc_fork.TabIndex = 3;
            this.lnk_btc_fork.TabStop = true;
            this.lnk_btc_fork.Text = "bc1q3jqugh66ctwzqr7tqjafunlpaaejqgt265rwjq";
            this.lnk_btc_fork.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_btc_fork_LinkClicked);
            // 
            // btn_btc_fork_copy
            // 
            this.btn_btc_fork_copy.Location = new System.Drawing.Point(132, 110);
            this.btn_btc_fork_copy.Name = "btn_btc_fork_copy";
            this.btn_btc_fork_copy.Size = new System.Drawing.Size(150, 30);
            this.btn_btc_fork_copy.TabIndex = 4;
            this.btn_btc_fork_copy.Text = "Copy Address";
            this.btn_btc_fork_copy.Click += new System.EventHandler(this.btn_btc_fork_copy_Click);
            // 
            // label4 (separator of the classic layout; the cards separate the sections now)
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(1, 308);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(2, 2);
            this.label4.TabIndex = 11;
            this.label4.Visible = false;
            // 
            // btn_close
            // 
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Location = new System.Drawing.Point(488, 508);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(92, 30);
            this.btn_close.TabIndex = 21;
            this.btn_close.Text = "Close";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // frm_About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(600, 558);
            this.Controls.Add(this.lab_ver);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.card_donate);
            this.Controls.Add(this.card_links);
            this.Controls.Add(this.card_info);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl_fork);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frm_About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Browser Select: About";
            this.Load += new System.EventHandler(this.frm_About_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_About_KeyDown);
            this.card_info.ResumeLayout(false);
            this.card_links.ResumeLayout(false);
            this.card_links.PerformLayout();
            this.card_donate.ResumeLayout(false);
            this.card_donate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc_fork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private BrowserSelect.UI.FluentHeader label1;
        private BrowserSelect.UI.FluentCard card_info;
        private BrowserSelect.UI.FluentCard card_links;
        private BrowserSelect.UI.FluentCard card_donate;
        private System.Windows.Forms.Label lbl_fork;
        private System.Windows.Forms.Label lbl_fork_github;
        private System.Windows.Forms.LinkLabel lnk_fork;
        private BrowserSelect.UI.FluentButton btn_original;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_btc_fork;
        private System.Windows.Forms.PictureBox pic_btc_fork;
        private BrowserSelect.UI.FluentButton btn_btc_fork_copy;
        private System.Windows.Forms.LinkLabel lnk_btc_fork;
        private BrowserSelect.UI.FluentButton btn_close;
        private System.Windows.Forms.Label lab_ver;
    }
}
