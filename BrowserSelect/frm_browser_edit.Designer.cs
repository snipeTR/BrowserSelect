namespace BrowserSelect
{
    partial class frm_browser_edit
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
            this.components = new System.ComponentModel.Container();
            this.card_fields = new BrowserSelect.UI.FluentCard();
            this.host_name = new BrowserSelect.UI.FluentTextBoxHost();
            this.host_exec = new BrowserSelect.UI.FluentTextBoxHost();
            this.host_args = new BrowserSelect.UI.FluentTextBoxHost();
            this.host_shortcut = new BrowserSelect.UI.FluentTextBoxHost();
            this.host_icon = new BrowserSelect.UI.FluentFieldHost();
            this.lbl_name = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.lbl_exec = new System.Windows.Forms.Label();
            this.txt_exec = new System.Windows.Forms.TextBox();
            this.btn_browse = new BrowserSelect.UI.FluentButton();
            this.lbl_args = new System.Windows.Forms.Label();
            this.txt_args = new System.Windows.Forms.TextBox();
            this.lbl_shortcut = new System.Windows.Forms.Label();
            this.txt_shortcut = new System.Windows.Forms.TextBox();
            this.lbl_shortcut_hint = new System.Windows.Forms.Label();
            this.lbl_icon = new System.Windows.Forms.Label();
            this.pic_icon = new System.Windows.Forms.PictureBox();
            this.btn_icon = new BrowserSelect.UI.FluentButton();
            this.btn_icon_reset = new BrowserSelect.UI.FluentButton();
            this.btn_ok = new BrowserSelect.UI.FluentButton();
            this.btn_cancel = new BrowserSelect.UI.FluentButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pic_icon)).BeginInit();
            this.card_fields.SuspendLayout();
            this.SuspendLayout();
            //
            // card_fields: every field of the dialog (Windows 11 look, v1.5.8.0); the rounded text fields
            // (host_*) contain the borderless text boxes; positions are set in code (LayoutWindow)
            //
            this.card_fields.Controls.Add(this.lbl_name);
            this.card_fields.Controls.Add(this.host_name);
            this.card_fields.Controls.Add(this.lbl_exec);
            this.card_fields.Controls.Add(this.host_exec);
            this.card_fields.Controls.Add(this.btn_browse);
            this.card_fields.Controls.Add(this.lbl_args);
            this.card_fields.Controls.Add(this.host_args);
            this.card_fields.Controls.Add(this.lbl_shortcut);
            this.card_fields.Controls.Add(this.host_shortcut);
            this.card_fields.Controls.Add(this.lbl_shortcut_hint);
            this.card_fields.Controls.Add(this.lbl_icon);
            this.card_fields.Controls.Add(this.host_icon);
            this.card_fields.Controls.Add(this.btn_icon);
            this.card_fields.Controls.Add(this.btn_icon_reset);
            this.card_fields.Location = new System.Drawing.Point(12, 12);
            this.card_fields.Name = "card_fields";
            this.card_fields.Size = new System.Drawing.Size(373, 175);
            this.card_fields.TabIndex = 0;
            this.host_name.Controls.Add(this.txt_name);
            this.host_name.Name = "host_name";
            this.host_name.TabIndex = 1;
            this.host_exec.Controls.Add(this.txt_exec);
            this.host_exec.Name = "host_exec";
            this.host_exec.TabIndex = 3;
            this.host_args.Controls.Add(this.txt_args);
            this.host_args.Name = "host_args";
            this.host_args.TabIndex = 6;
            this.host_shortcut.Controls.Add(this.txt_shortcut);
            this.host_shortcut.Name = "host_shortcut";
            this.host_shortcut.TabIndex = 8;
            this.host_icon.Controls.Add(this.pic_icon);
            this.host_icon.Inset = 6;
            this.host_icon.Name = "host_icon";
            this.host_icon.TabIndex = 11;
            //
            // lbl_name
            //
            this.lbl_name.AutoSize = true;
            this.lbl_name.Location = new System.Drawing.Point(12, 15);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(38, 13);
            this.lbl_name.TabIndex = 0;
            this.lbl_name.Text = "Name:";
            //
            // txt_name
            //
            this.txt_name.Location = new System.Drawing.Point(95, 12);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(290, 20);
            this.txt_name.TabIndex = 1;
            //
            // lbl_exec
            //
            this.lbl_exec.AutoSize = true;
            this.lbl_exec.Location = new System.Drawing.Point(12, 44);
            this.lbl_exec.Name = "lbl_exec";
            this.lbl_exec.Size = new System.Drawing.Size(63, 13);
            this.lbl_exec.TabIndex = 2;
            this.lbl_exec.Text = "Executable:";
            //
            // txt_exec
            //
            this.txt_exec.Location = new System.Drawing.Point(95, 41);
            this.txt_exec.Name = "txt_exec";
            this.txt_exec.Size = new System.Drawing.Size(209, 20);
            this.txt_exec.TabIndex = 3;
            //
            // btn_browse
            //
            this.btn_browse.Location = new System.Drawing.Point(310, 40);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(75, 23);
            this.btn_browse.TabIndex = 4;
            this.btn_browse.Text = "Browse...";
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            //
            // lbl_args
            //
            this.lbl_args.AutoSize = true;
            this.lbl_args.Location = new System.Drawing.Point(12, 73);
            this.lbl_args.Name = "lbl_args";
            this.lbl_args.Size = new System.Drawing.Size(60, 13);
            this.lbl_args.TabIndex = 5;
            this.lbl_args.Text = "Arguments:";
            //
            // txt_args
            //
            this.txt_args.Location = new System.Drawing.Point(95, 70);
            this.txt_args.Name = "txt_args";
            this.txt_args.Size = new System.Drawing.Size(290, 20);
            this.txt_args.TabIndex = 6;
            this.toolTip1.SetToolTip(this.txt_args, "Command line arguments always passed to this browser (e.g. --profile-directory=\"Profile 1\" or -P work)");
            //
            // lbl_shortcut
            //
            this.lbl_shortcut.AutoSize = true;
            this.lbl_shortcut.Location = new System.Drawing.Point(12, 102);
            this.lbl_shortcut.Name = "lbl_shortcut";
            this.lbl_shortcut.Size = new System.Drawing.Size(77, 13);
            this.lbl_shortcut.TabIndex = 7;
            this.lbl_shortcut.Text = "Shortcut keys:";
            //
            // txt_shortcut
            //
            this.txt_shortcut.Location = new System.Drawing.Point(95, 99);
            this.txt_shortcut.MaxLength = 5;
            this.txt_shortcut.Name = "txt_shortcut";
            this.txt_shortcut.Size = new System.Drawing.Size(60, 20);
            this.txt_shortcut.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txt_shortcut, "Keys that open this browser in the selection window (each character is a shortcut)");
            //
            // lbl_shortcut_hint
            //
            this.lbl_shortcut_hint.AutoSize = true;
            this.lbl_shortcut_hint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbl_shortcut_hint.Location = new System.Drawing.Point(161, 102);
            this.lbl_shortcut_hint.Name = "lbl_shortcut_hint";
            this.lbl_shortcut_hint.Size = new System.Drawing.Size(170, 13);
            this.lbl_shortcut_hint.TabIndex = 9;
            this.lbl_shortcut_hint.Text = "e.g. w (leave empty for automatic)";
            //
            // lbl_icon
            //
            this.lbl_icon.AutoSize = true;
            this.lbl_icon.Location = new System.Drawing.Point(12, 131);
            this.lbl_icon.Name = "lbl_icon";
            this.lbl_icon.Size = new System.Drawing.Size(31, 13);
            this.lbl_icon.TabIndex = 10;
            this.lbl_icon.Text = "Icon:";
            //
            // pic_icon
            //
            this.pic_icon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_icon.Location = new System.Drawing.Point(95, 128);
            this.pic_icon.Name = "pic_icon";
            this.pic_icon.Size = new System.Drawing.Size(52, 52);
            this.pic_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_icon.TabIndex = 11;
            this.pic_icon.TabStop = false;
            //
            // btn_icon
            //
            this.btn_icon.Location = new System.Drawing.Point(153, 128);
            this.btn_icon.Name = "btn_icon";
            this.btn_icon.Size = new System.Drawing.Size(75, 23);
            this.btn_icon.TabIndex = 12;
            this.btn_icon.Text = "Change...";
            this.btn_icon.Click += new System.EventHandler(this.btn_icon_Click);
            //
            // btn_icon_reset
            //
            this.btn_icon_reset.Location = new System.Drawing.Point(153, 157);
            this.btn_icon_reset.Name = "btn_icon_reset";
            this.btn_icon_reset.Size = new System.Drawing.Size(75, 23);
            this.btn_icon_reset.TabIndex = 13;
            this.btn_icon_reset.Text = "Default";
            this.btn_icon_reset.Click += new System.EventHandler(this.btn_icon_reset_Click);
            //
            // btn_ok
            //
            this.btn_ok.Location = new System.Drawing.Point(229, 195);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 14;
            this.btn_ok.Text = "OK";
            this.btn_ok.Accent = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            //
            // btn_cancel
            //
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(310, 195);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 15;
            this.btn_cancel.Text = "Cancel";
                        //
            // frm_browser_edit
            //
            this.AcceptButton = this.btn_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(397, 230);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.card_fields);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_browser_edit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Browser";
            this.Load += new System.EventHandler(this.frm_browser_edit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_icon)).EndInit();
            this.card_fields.ResumeLayout(false);
            this.card_fields.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label lbl_exec;
        private System.Windows.Forms.TextBox txt_exec;
        private BrowserSelect.UI.FluentButton btn_browse;
        private System.Windows.Forms.Label lbl_args;
        private System.Windows.Forms.TextBox txt_args;
        private System.Windows.Forms.Label lbl_shortcut;
        private System.Windows.Forms.TextBox txt_shortcut;
        private System.Windows.Forms.Label lbl_shortcut_hint;
        private System.Windows.Forms.Label lbl_icon;
        private System.Windows.Forms.PictureBox pic_icon;
        private BrowserSelect.UI.FluentButton btn_icon;
        private BrowserSelect.UI.FluentButton btn_icon_reset;
        private BrowserSelect.UI.FluentButton btn_ok;
        private BrowserSelect.UI.FluentButton btn_cancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private BrowserSelect.UI.FluentCard card_fields;
        private BrowserSelect.UI.FluentTextBoxHost host_name;
        private BrowserSelect.UI.FluentTextBoxHost host_exec;
        private BrowserSelect.UI.FluentTextBoxHost host_args;
        private BrowserSelect.UI.FluentTextBoxHost host_shortcut;
        private BrowserSelect.UI.FluentFieldHost host_icon;
    }
}
