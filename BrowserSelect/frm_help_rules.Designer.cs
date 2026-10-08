namespace BrowserSelect
{
    partial class frm_help_rules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_help_rules));
            this.txt_help = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_help
            // 
            this.txt_help.BackColor = System.Drawing.SystemColors.Control;
            this.txt_help.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_help.Location = new System.Drawing.Point(8, 8);
            this.txt_help.Multiline = true;
            this.txt_help.Name = "txt_help";
            this.txt_help.ReadOnly = true;
            this.txt_help.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_help.Size = new System.Drawing.Size(664, 434);
            this.txt_help.TabIndex = 0;
            this.txt_help.TabStop = false;
            this.txt_help.Text = resources.GetString("label1.Text");
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Location = new System.Drawing.Point(593, 445);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 1;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // frm_help_rules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(680, 480);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.txt_help);
            this.Padding = new System.Windows.Forms.Padding(8, 8, 8, 38);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frm_help_rules";
            this.Text = "BrowserSelect - Help";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_help;
        private System.Windows.Forms.Button btn_close;
    }
}