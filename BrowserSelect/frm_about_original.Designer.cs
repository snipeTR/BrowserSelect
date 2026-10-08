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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_header = new System.Windows.Forms.Label();
            this.lbl_intro = new System.Windows.Forms.Label();
            this.lbl_coded = new System.Windows.Forms.Label();
            this.lbl_contact = new System.Windows.Forms.Label();
            this.lnk_mail = new System.Windows.Forms.LinkLabel();
            this.lbl_github = new System.Windows.Forms.Label();
            this.lnk_github = new System.Windows.Forms.LinkLabel();
            this.lbl_separator = new System.Windows.Forms.Label();
            this.lbl_btc = new System.Windows.Forms.Label();
            this.pic_btc = new System.Windows.Forms.PictureBox();
            this.btn_copy = new System.Windows.Forms.Button();
            this.lnk_btc = new System.Windows.Forms.LinkLabel();
            this.btn_close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_header
            // 
            this.lbl_header.AutoSize = true;
            this.lbl_header.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_header.Location = new System.Drawing.Point(12, 12);
            this.lbl_header.Name = "lbl_header";
            this.lbl_header.Size = new System.Drawing.Size(134, 20);
            this.lbl_header.TabIndex = 0;
            this.lbl_header.Text = "Original project";
            // 
            // lbl_intro
            // 
            this.lbl_intro.Location = new System.Drawing.Point(12, 42);
            this.lbl_intro.Name = "lbl_intro";
            this.lbl_intro.Size = new System.Drawing.Size(456, 70);
            this.lbl_intro.TabIndex = 1;
            this.lbl_intro.Text = "This version of BrowserSelect is a fork of the original BrowserSelect project.";
            // 
            // lbl_coded
            // 
            this.lbl_coded.AutoSize = true;
            this.lbl_coded.Location = new System.Drawing.Point(12, 118);
            this.lbl_coded.Name = "lbl_coded";
            this.lbl_coded.Size = new System.Drawing.Size(88, 13);
            this.lbl_coded.TabIndex = 2;
            this.lbl_coded.Text = "Coded By: Bor691";
            // 
            // lbl_contact
            // 
            this.lbl_contact.AutoSize = true;
            this.lbl_contact.Location = new System.Drawing.Point(12, 138);
            this.lbl_contact.Name = "lbl_contact";
            this.lbl_contact.Size = new System.Drawing.Size(47, 13);
            this.lbl_contact.TabIndex = 3;
            this.lbl_contact.Text = "Contact:";
            // 
            // lnk_mail
            // 
            this.lnk_mail.AutoSize = true;
            this.lnk_mail.Location = new System.Drawing.Point(61, 138);
            this.lnk_mail.Name = "lnk_mail";
            this.lnk_mail.Size = new System.Drawing.Size(73, 13);
            this.lnk_mail.TabIndex = 4;
            this.lnk_mail.TabStop = true;
            this.lnk_mail.Text = "me@bor691.ir";
            this.lnk_mail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_mail_LinkClicked);
            // 
            // lbl_github
            // 
            this.lbl_github.AutoSize = true;
            this.lbl_github.Location = new System.Drawing.Point(12, 158);
            this.lbl_github.Name = "lbl_github";
            this.lbl_github.Size = new System.Drawing.Size(43, 13);
            this.lbl_github.TabIndex = 5;
            this.lbl_github.Text = "GitHub:";
            // 
            // lnk_github
            // 
            this.lnk_github.AutoSize = true;
            this.lnk_github.Location = new System.Drawing.Point(57, 158);
            this.lnk_github.Name = "lnk_github";
            this.lnk_github.Size = new System.Drawing.Size(211, 13);
            this.lnk_github.TabIndex = 6;
            this.lnk_github.TabStop = true;
            this.lnk_github.Text = "https://github.com/zumoshi/BrowserSelect";
            this.lnk_github.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_github_LinkClicked);
            // 
            // lbl_separator
            // 
            this.lbl_separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_separator.Location = new System.Drawing.Point(1, 184);
            this.lbl_separator.Name = "lbl_separator";
            this.lbl_separator.Size = new System.Drawing.Size(478, 2);
            this.lbl_separator.TabIndex = 7;
            // 
            // lbl_btc
            // 
            this.lbl_btc.AutoSize = true;
            this.lbl_btc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_btc.Location = new System.Drawing.Point(12, 196);
            this.lbl_btc.Name = "lbl_btc";
            this.lbl_btc.Size = new System.Drawing.Size(220, 13);
            this.lbl_btc.TabIndex = 8;
            this.lbl_btc.Text = "Bor691 (original author) - Bitcoin:";
            // 
            // pic_btc
            // 
            this.pic_btc.Image = global::BrowserSelect.Properties.Resources.bitcoin;
            this.pic_btc.Location = new System.Drawing.Point(12, 216);
            this.pic_btc.Name = "pic_btc";
            this.pic_btc.Size = new System.Drawing.Size(100, 100);
            this.pic_btc.TabIndex = 9;
            this.pic_btc.TabStop = false;
            // 
            // btn_copy
            // 
            this.btn_copy.Location = new System.Drawing.Point(118, 216);
            this.btn_copy.Name = "btn_copy";
            this.btn_copy.Size = new System.Drawing.Size(150, 23);
            this.btn_copy.TabIndex = 10;
            this.btn_copy.Text = "Copy Address";
            this.btn_copy.UseVisualStyleBackColor = true;
            this.btn_copy.Click += new System.EventHandler(this.btn_copy_Click);
            // 
            // lnk_btc
            // 
            this.lnk_btc.AutoSize = true;
            this.lnk_btc.Location = new System.Drawing.Point(12, 324);
            this.lnk_btc.Name = "lnk_btc";
            this.lnk_btc.Size = new System.Drawing.Size(221, 13);
            this.lnk_btc.TabIndex = 11;
            this.lnk_btc.TabStop = true;
            this.lnk_btc.Text = "1BA5Ndo24jtRgTEsvmGkrqRWTaJS4F3zNh";
            this.lnk_btc.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_btc_LinkClicked);
            // 
            // btn_close
            // 
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Location = new System.Drawing.Point(393, 345);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 12;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // frm_about_original
            // 
            this.AcceptButton = this.btn_close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(480, 380);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.lnk_btc);
            this.Controls.Add(this.btn_copy);
            this.Controls.Add(this.pic_btc);
            this.Controls.Add(this.lbl_btc);
            this.Controls.Add(this.lbl_separator);
            this.Controls.Add(this.lnk_github);
            this.Controls.Add(this.lbl_github);
            this.Controls.Add(this.lnk_mail);
            this.Controls.Add(this.lbl_contact);
            this.Controls.Add(this.lbl_coded);
            this.Controls.Add(this.lbl_intro);
            this.Controls.Add(this.lbl_header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_about_original";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BrowserSelect: Original project";
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_header;
        private System.Windows.Forms.Label lbl_intro;
        private System.Windows.Forms.Label lbl_coded;
        private System.Windows.Forms.Label lbl_contact;
        private System.Windows.Forms.LinkLabel lnk_mail;
        private System.Windows.Forms.Label lbl_github;
        private System.Windows.Forms.LinkLabel lnk_github;
        private System.Windows.Forms.Label lbl_separator;
        private System.Windows.Forms.Label lbl_btc;
        private System.Windows.Forms.PictureBox pic_btc;
        private System.Windows.Forms.Button btn_copy;
        private System.Windows.Forms.LinkLabel lnk_btc;
        private System.Windows.Forms.Button btn_close;
    }
}
