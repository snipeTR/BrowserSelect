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
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_About));
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_fork = new System.Windows.Forms.Label();
            this.lbl_fork_github = new System.Windows.Forms.Label();
            this.lnk_fork = new System.Windows.Forms.LinkLabel();
            this.btn_original = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_btc_fork = new System.Windows.Forms.Label();
            this.pic_btc_fork = new System.Windows.Forms.PictureBox();
            this.btn_btc_fork_copy = new System.Windows.Forms.Button();
            this.lnk_btc_fork = new System.Windows.Forms.LinkLabel();
            this.btn_close = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lab_ver = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc_fork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(146, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "BrowserSelect";
            // 
            // lbl_fork
            // 
            this.lbl_fork.AutoSize = true;
            this.lbl_fork.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_fork.Location = new System.Drawing.Point(148, 75);
            this.lbl_fork.Name = "lbl_fork";
            this.lbl_fork.Size = new System.Drawing.Size(250, 16);
            this.lbl_fork.TabIndex = 7;
            this.lbl_fork.Text = "This fork is maintained by: snipeTR";
            // 
            // lbl_fork_github
            // 
            this.lbl_fork_github.AutoSize = true;
            this.lbl_fork_github.Location = new System.Drawing.Point(148, 100);
            this.lbl_fork_github.Name = "lbl_fork_github";
            this.lbl_fork_github.Size = new System.Drawing.Size(43, 13);
            this.lbl_fork_github.TabIndex = 8;
            this.lbl_fork_github.Text = "GitHub:";
            // 
            // lnk_fork
            // 
            this.lnk_fork.AutoSize = true;
            this.lnk_fork.Location = new System.Drawing.Point(193, 100);
            this.lnk_fork.Name = "lnk_fork";
            this.lnk_fork.Size = new System.Drawing.Size(206, 13);
            this.lnk_fork.TabIndex = 9;
            this.lnk_fork.TabStop = true;
            this.lnk_fork.Text = "https://github.com/snipeTR/BrowserSelect";
            this.lnk_fork.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // btn_original
            // 
            this.btn_original.AutoSize = true;
            this.btn_original.Location = new System.Drawing.Point(150, 125);
            this.btn_original.MinimumSize = new System.Drawing.Size(180, 25);
            this.btn_original.Name = "btn_original";
            this.btn_original.Size = new System.Drawing.Size(180, 25);
            this.btn_original.TabIndex = 10;
            this.btn_original.Text = "Original project info...";
            this.btn_original.UseVisualStyleBackColor = true;
            this.btn_original.Click += new System.EventHandler(this.btn_original_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(616, 116);
            this.label3.TabIndex = 10;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(1, 308);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(638, 2);
            this.label4.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(616, 30);
            this.label5.TabIndex = 12;
            this.label5.Text = "If you find this program useful and would like to thank the developers you may do" +
    "nate using bitcoin.";
            // 
            // lbl_btc_fork
            // 
            this.lbl_btc_fork.AutoSize = true;
            this.lbl_btc_fork.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_btc_fork.Location = new System.Drawing.Point(12, 352);
            this.lbl_btc_fork.Name = "lbl_btc_fork";
            this.lbl_btc_fork.Size = new System.Drawing.Size(186, 13);
            this.lbl_btc_fork.TabIndex = 13;
            this.lbl_btc_fork.Text = "snipeTR (this fork) - Bitcoin:";
            // 
            // pic_btc_fork
            // 
            this.pic_btc_fork.Image = global::BrowserSelect.Properties.Resources.bitcoin_snipetr;
            this.pic_btc_fork.Location = new System.Drawing.Point(12, 372);
            this.pic_btc_fork.Name = "pic_btc_fork";
            this.pic_btc_fork.Size = new System.Drawing.Size(100, 100);
            this.pic_btc_fork.TabIndex = 14;
            this.pic_btc_fork.TabStop = false;
            // 
            // btn_btc_fork_copy
            // 
            this.btn_btc_fork_copy.Location = new System.Drawing.Point(118, 372);
            this.btn_btc_fork_copy.Name = "btn_btc_fork_copy";
            this.btn_btc_fork_copy.Size = new System.Drawing.Size(150, 23);
            this.btn_btc_fork_copy.TabIndex = 15;
            this.btn_btc_fork_copy.Text = "Copy Address";
            this.btn_btc_fork_copy.UseVisualStyleBackColor = true;
            this.btn_btc_fork_copy.Click += new System.EventHandler(this.btn_btc_fork_copy_Click);
            // 
            // lnk_btc_fork
            // 
            this.lnk_btc_fork.AutoSize = true;
            this.lnk_btc_fork.Location = new System.Drawing.Point(12, 480);
            this.lnk_btc_fork.Name = "lnk_btc_fork";
            this.lnk_btc_fork.Size = new System.Drawing.Size(250, 13);
            this.lnk_btc_fork.TabIndex = 16;
            this.lnk_btc_fork.TabStop = true;
            this.lnk_btc_fork.Text = "bc1q3jqugh66ctwzqr7tqjafunlpaaejqgt265rwjq";
            this.lnk_btc_fork.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_btc_fork_LinkClicked);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(553, 506);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 21;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lab_ver
            // 
            this.lab_ver.AutoSize = true;
            this.lab_ver.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_ver.Location = new System.Drawing.Point(302, 33);
            this.lab_ver.Name = "lab_ver";
            this.lab_ver.Size = new System.Drawing.Size(69, 26);
            this.lab_ver.TabIndex = 22;
            this.lab_ver.Text = "v%.%";
            // 
            // frm_About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 541);
            this.Controls.Add(this.lab_ver);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.lnk_btc_fork);
            this.Controls.Add(this.btn_btc_fork_copy);
            this.Controls.Add(this.pic_btc_fork);
            this.Controls.Add(this.lbl_btc_fork);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_original);
            this.Controls.Add(this.lnk_fork);
            this.Controls.Add(this.lbl_fork_github);
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
            ((System.ComponentModel.ISupportInitialize)(this.pic_btc_fork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_fork;
        private System.Windows.Forms.Label lbl_fork_github;
        private System.Windows.Forms.LinkLabel lnk_fork;
        private System.Windows.Forms.Button btn_original;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_btc_fork;
        private System.Windows.Forms.PictureBox pic_btc_fork;
        private System.Windows.Forms.Button btn_btc_fork_copy;
        private System.Windows.Forms.LinkLabel lnk_btc_fork;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label lab_ver;
    }
}
