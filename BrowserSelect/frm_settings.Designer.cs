namespace BrowserSelect {
    partial class frm_settings {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_settings));
            this.btn_setdefault = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.browser_filter = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbl_source = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_move_up = new System.Windows.Forms.Button();
            this.btn_move_down = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.gv_filters = new System.Windows.Forms.DataGridView();
            this.matchType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.pattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browser = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isPrivate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.arguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chk_check_update = new System.Windows.Forms.CheckBox();
            this.btn_check_update = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chk_alt_ignore = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btn_browser_add = new System.Windows.Forms.Button();
            this.btn_browser_edit = new System.Windows.Forms.Button();
            this.btn_browser_remove = new System.Windows.Forms.Button();
            this.lbl_sort = new System.Windows.Forms.Label();
            this.cmb_sort = new System.Windows.Forms.ComboBox();
            this.btn_browser_up = new System.Windows.Forms.Button();
            this.btn_browser_down = new System.Windows.Forms.Button();
            this.chk_running_only = new System.Windows.Forms.CheckBox();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_import = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gv_filters)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            //
            // btn_setdefault
            //
            this.btn_setdefault.Location = new System.Drawing.Point(6, 58);
            this.btn_setdefault.Name = "btn_setdefault";
            this.btn_setdefault.Size = new System.Drawing.Size(208, 26);
            this.btn_setdefault.TabIndex = 0;
            this.btn_setdefault.Text = "Set as Default Browser";
            this.btn_setdefault.UseVisualStyleBackColor = true;
            this.btn_setdefault.Click += new System.EventHandler(this.btn_setdefault_Click);
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "BrowserSelect must be set as default browser for it to function correctly. this b" +
    "utton will set it as the default browser.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 499);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "if you have feature requests,bug reports or suggestions please submit an issue on" +
    " the project\'s Github.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_browser_down);
            this.groupBox1.Controls.Add(this.btn_browser_up);
            this.groupBox1.Controls.Add(this.cmb_sort);
            this.groupBox1.Controls.Add(this.lbl_sort);
            this.groupBox1.Controls.Add(this.btn_browser_remove);
            this.groupBox1.Controls.Add(this.btn_browser_edit);
            this.groupBox1.Controls.Add(this.btn_browser_add);
            this.groupBox1.Controls.Add(this.browser_filter);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 232);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Browsers";
            // 
            // browser_filter
            // 
            this.browser_filter.FormattingEnabled = true;
            this.browser_filter.Location = new System.Drawing.Point(6, 19);
            this.browser_filter.Name = "browser_filter";
            this.browser_filter.Size = new System.Drawing.Size(208, 139);
            this.browser_filter.TabIndex = 0;
            this.browser_filter.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.browser_filter_ItemCheck);
            this.browser_filter.SelectedIndexChanged += new System.EventHandler(this.browser_filter_SelectedIndexChanged);
            this.browser_filter.DoubleClick += new System.EventHandler(this.btn_browser_edit_Click);
            //
            // btn_browser_add
            //
            this.btn_browser_add.Location = new System.Drawing.Point(6, 170);
            this.btn_browser_add.Name = "btn_browser_add";
            this.btn_browser_add.Size = new System.Drawing.Size(66, 23);
            this.btn_browser_add.TabIndex = 1;
            this.btn_browser_add.Text = "Add...";
            this.toolTip1.SetToolTip(this.btn_browser_add, "Add a portable browser (or any program) by selecting its executable");
            this.btn_browser_add.UseVisualStyleBackColor = true;
            this.btn_browser_add.Click += new System.EventHandler(this.btn_browser_add_Click);
            //
            // btn_browser_edit
            //
            this.btn_browser_edit.Location = new System.Drawing.Point(77, 170);
            this.btn_browser_edit.Name = "btn_browser_edit";
            this.btn_browser_edit.Size = new System.Drawing.Size(66, 23);
            this.btn_browser_edit.TabIndex = 2;
            this.btn_browser_edit.Text = "Edit...";
            this.toolTip1.SetToolTip(this.btn_browser_edit, "Change the icon, shortcut keys and arguments of the selected browser");
            this.btn_browser_edit.UseVisualStyleBackColor = true;
            this.btn_browser_edit.Click += new System.EventHandler(this.btn_browser_edit_Click);
            //
            // btn_browser_remove
            //
            this.btn_browser_remove.Location = new System.Drawing.Point(148, 170);
            this.btn_browser_remove.Name = "btn_browser_remove";
            this.btn_browser_remove.Size = new System.Drawing.Size(66, 23);
            this.btn_browser_remove.TabIndex = 3;
            this.btn_browser_remove.Text = "Remove";
            this.toolTip1.SetToolTip(this.btn_browser_remove, "Remove a manually added browser (uncheck a browser to hide it)");
            this.btn_browser_remove.UseVisualStyleBackColor = true;
            this.btn_browser_remove.Click += new System.EventHandler(this.btn_browser_remove_Click);
            //
            // lbl_sort
            //
            this.lbl_sort.AutoSize = true;
            this.lbl_sort.Location = new System.Drawing.Point(6, 205);
            this.lbl_sort.Name = "lbl_sort";
            this.lbl_sort.Size = new System.Drawing.Size(29, 13);
            this.lbl_sort.TabIndex = 4;
            this.lbl_sort.Text = "Sort:";
            //
            // cmb_sort
            //
            this.cmb_sort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sort.FormattingEnabled = true;
            this.cmb_sort.Location = new System.Drawing.Point(38, 201);
            this.cmb_sort.Name = "cmb_sort";
            this.cmb_sort.Size = new System.Drawing.Size(100, 21);
            this.cmb_sort.TabIndex = 5;
            this.toolTip1.SetToolTip(this.cmb_sort, "Order of the browsers in the selection window");
            this.cmb_sort.SelectedIndexChanged += new System.EventHandler(this.cmb_sort_SelectedIndexChanged);
            //
            // btn_browser_up
            //
            this.btn_browser_up.Location = new System.Drawing.Point(144, 200);
            this.btn_browser_up.Name = "btn_browser_up";
            this.btn_browser_up.Size = new System.Drawing.Size(33, 23);
            this.btn_browser_up.TabIndex = 6;
            this.btn_browser_up.Text = "\u25B2";
            this.toolTip1.SetToolTip(this.btn_browser_up, "Move the selected browser up (Manual sort)");
            this.btn_browser_up.UseVisualStyleBackColor = true;
            this.btn_browser_up.Click += new System.EventHandler(this.btn_browser_up_Click);
            //
            // btn_browser_down
            //
            this.btn_browser_down.Location = new System.Drawing.Point(181, 200);
            this.btn_browser_down.Name = "btn_browser_down";
            this.btn_browser_down.Size = new System.Drawing.Size(33, 23);
            this.btn_browser_down.TabIndex = 7;
            this.btn_browser_down.Text = "\u25BC";
            this.toolTip1.SetToolTip(this.btn_browser_down, "Move the selected browser down (Manual sort)");
            this.btn_browser_down.UseVisualStyleBackColor = true;
            this.btn_browser_down.Click += new System.EventHandler(this.btn_browser_down_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_setdefault);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 250);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 90);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Default Browser";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lbl_source);
            this.groupBox3.Controls.Add(this.btn_cancel);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.linkLabel1);
            this.groupBox3.Controls.Add(this.btn_apply);
            this.groupBox3.Controls.Add(this.btn_delete);
            this.groupBox3.Controls.Add(this.btn_move_down);
            this.groupBox3.Controls.Add(this.btn_move_up);
            this.groupBox3.Controls.Add(this.gv_filters);
            this.groupBox3.Location = new System.Drawing.Point(238, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(650, 516);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Auto Select Filters";
            //
            // lbl_source
            //
            this.lbl_source.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_source.AutoEllipsis = true;
            this.lbl_source.Location = new System.Drawing.Point(340, 492);
            this.lbl_source.Name = "lbl_source";
            this.lbl_source.Size = new System.Drawing.Size(142, 13);
            this.lbl_source.TabIndex = 12;
            this.lbl_source.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(488, 487);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "Close";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(6, 487);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Help";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(212, 235);
            this.linkLabel1.Location = new System.Drawing.Point(3, 16);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(644, 40);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = resources.GetString("linkLabel1.Text");
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btn_apply
            // 
            this.btn_apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_apply.Enabled = false;
            this.btn_apply.Location = new System.Drawing.Point(569, 487);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(75, 23);
            this.btn_apply.TabIndex = 5;
            this.btn_apply.Text = "Apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            //
            // btn_move_up
            //
            this.btn_move_up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_move_up.Location = new System.Drawing.Point(87, 487);
            this.btn_move_up.Name = "btn_move_up";
            this.btn_move_up.Size = new System.Drawing.Size(75, 23);
            this.btn_move_up.TabIndex = 9;
            this.btn_move_up.Text = "Move Up";
            this.btn_move_up.UseVisualStyleBackColor = true;
            this.btn_move_up.Click += new System.EventHandler(this.btn_move_up_Click);
            //
            // btn_move_down
            //
            this.btn_move_down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_move_down.Location = new System.Drawing.Point(168, 487);
            this.btn_move_down.Name = "btn_move_down";
            this.btn_move_down.Size = new System.Drawing.Size(85, 23);
            this.btn_move_down.TabIndex = 10;
            this.btn_move_down.Text = "Move Down";
            this.btn_move_down.UseVisualStyleBackColor = true;
            this.btn_move_down.Click += new System.EventHandler(this.btn_move_down_Click);
            //
            // btn_delete
            //
            this.btn_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_delete.Location = new System.Drawing.Point(259, 487);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(75, 23);
            this.btn_delete.TabIndex = 11;
            this.btn_delete.Text = "Delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // gv_filters
            // 
            this.gv_filters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gv_filters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_filters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.matchType,
            this.pattern,
            this.browser,
            this.isPrivate,
            this.arguments});
            this.gv_filters.Location = new System.Drawing.Point(6, 59);
            this.gv_filters.Name = "gv_filters";
            this.gv_filters.Size = new System.Drawing.Size(638, 422);
            this.gv_filters.TabIndex = 1;
            this.gv_filters.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gv_filters_CellBeginEdit);
            this.gv_filters.CurrentCellDirtyStateChanged += new System.EventHandler(this.gv_filters_CurrentCellDirtyStateChanged);
            this.gv_filters.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gv_filters_DataError);
            this.gv_filters.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gv_filters_CellBeginEdit);
            this.gv_filters.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gv_filters_CellBeginEdit);
            //
            // matchType
            //
            this.matchType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.matchType.DataPropertyName = "MatchType";
            this.matchType.HeaderText = "Match";
            this.matchType.Name = "matchType";
            this.matchType.ToolTipText = "What part of the link the pattern is compared to";
            this.matchType.Width = 85;
            // 
            // pattern
            // 
            this.pattern.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pattern.DataPropertyName = "Pattern";
            this.pattern.HeaderText = "Pattern";
            this.pattern.Name = "pattern";
            this.pattern.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // browser
            // 
            this.browser.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.browser.DataPropertyName = "Browser";
            this.browser.HeaderText = "Browser";
            this.browser.Name = "browser";
            this.browser.Width = 51;
            //
            // isPrivate
            //
            this.isPrivate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.isPrivate.DataPropertyName = "IsPrivate";
            this.isPrivate.HeaderText = "Private";
            this.isPrivate.Name = "isPrivate";
            this.isPrivate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.isPrivate.ToolTipText = "Open the link in a private/incognito window";
            this.isPrivate.Width = 46;
            //
            // arguments
            //
            this.arguments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.arguments.DataPropertyName = "Arguments";
            this.arguments.HeaderText = "Arguments";
            this.arguments.Name = "arguments";
            this.arguments.ToolTipText = "Custom command line flags passed to the browser (e.g. --incognito --disable-web-security)";
            this.arguments.Width = 130;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chk_check_update);
            this.groupBox4.Controls.Add(this.btn_check_update);
            this.groupBox4.Location = new System.Drawing.Point(12, 452);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(220, 41);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Update checker";
            // 
            // chk_check_update
            // 
            this.chk_check_update.AutoSize = true;
            this.chk_check_update.Location = new System.Drawing.Point(12, 16);
            this.chk_check_update.Name = "chk_check_update";
            this.chk_check_update.Size = new System.Drawing.Size(58, 17);
            this.chk_check_update.TabIndex = 1;
            this.chk_check_update.Text = "enable";
            this.chk_check_update.UseVisualStyleBackColor = true;
            this.chk_check_update.CheckedChanged += new System.EventHandler(this.chk_check_update_CheckedChanged);
            // 
            // btn_check_update
            // 
            this.btn_check_update.Location = new System.Drawing.Point(139, 12);
            this.btn_check_update.Name = "btn_check_update";
            this.btn_check_update.Size = new System.Drawing.Size(75, 23);
            this.btn_check_update.TabIndex = 0;
            this.btn_check_update.Text = "check now";
            this.btn_check_update.UseVisualStyleBackColor = true;
            this.btn_check_update.Click += new System.EventHandler(this.btn_check_update_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(170, 7);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(59, 19);
            this.btn_refresh.TabIndex = 2;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            //
            // groupBox5
            //
            this.groupBox5.Controls.Add(this.btn_import);
            this.groupBox5.Controls.Add(this.btn_export);
            this.groupBox5.Controls.Add(this.chk_running_only);
            this.groupBox5.Controls.Add(this.chk_alt_ignore);
            this.groupBox5.Location = new System.Drawing.Point(12, 346);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(220, 100);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Options";
            //
            // btn_export
            //
            this.btn_export.Location = new System.Drawing.Point(6, 68);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(101, 23);
            this.btn_export.TabIndex = 2;
            this.btn_export.Text = "Export...";
            this.toolTip1.SetToolTip(this.btn_export, "Save rules and settings to a file");
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            //
            // btn_import
            //
            this.btn_import.Location = new System.Drawing.Point(113, 68);
            this.btn_import.Name = "btn_import";
            this.btn_import.Size = new System.Drawing.Size(101, 23);
            this.btn_import.TabIndex = 3;
            this.btn_import.Text = "Import...";
            this.toolTip1.SetToolTip(this.btn_import, "Load rules and settings from a file (replaces the current ones)");
            this.btn_import.UseVisualStyleBackColor = true;
            this.btn_import.Click += new System.EventHandler(this.btn_import_Click);
            //
            // chk_running_only
            //
            this.chk_running_only.AutoSize = true;
            this.chk_running_only.Location = new System.Drawing.Point(9, 19);
            this.chk_running_only.Name = "chk_running_only";
            this.chk_running_only.Size = new System.Drawing.Size(158, 17);
            this.chk_running_only.TabIndex = 0;
            this.chk_running_only.Text = "Show running browsers only";
            this.toolTip1.SetToolTip(this.chk_running_only, "Only list browsers that are currently running (all browsers are shown if none is running)");
            this.chk_running_only.UseVisualStyleBackColor = true;
            this.chk_running_only.CheckedChanged += new System.EventHandler(this.chk_running_only_CheckedChanged);
            //
            // chk_alt_ignore
            //
            this.chk_alt_ignore.AutoSize = true;
            this.chk_alt_ignore.Location = new System.Drawing.Point(9, 42);
            this.chk_alt_ignore.Name = "chk_alt_ignore";
            this.chk_alt_ignore.Size = new System.Drawing.Size(176, 17);
            this.chk_alt_ignore.TabIndex = 1;
            this.chk_alt_ignore.Text = "Hold Alt on a link to skip rules";
            this.toolTip1.SetToolTip(this.chk_alt_ignore, "If Alt is held down while a link is clicked, the Auto Select rules are ignored and the browser list is shown.");
            this.chk_alt_ignore.UseVisualStyleBackColor = true;
            this.chk_alt_ignore.CheckedChanged += new System.EventHandler(this.chk_alt_ignore_CheckedChanged);
            // 
            // frm_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(900, 540);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(700, 579);
            this.Name = "frm_settings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_settings_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_settings_FormClosed);
            this.Load += new System.EventHandler(this.frm_settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gv_filters)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_setdefault;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox browser_filter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView gv_filters;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_move_up;
        private System.Windows.Forms.Button btn_move_down;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Label lbl_source;
        private System.Windows.Forms.DataGridViewComboBoxColumn matchType;
        private System.Windows.Forms.DataGridViewTextBoxColumn pattern;
        private System.Windows.Forms.DataGridViewComboBoxColumn browser;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPrivate;
        private System.Windows.Forms.DataGridViewTextBoxColumn arguments;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chk_check_update;
        private System.Windows.Forms.Button btn_check_update;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chk_alt_ignore;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btn_browser_add;
        private System.Windows.Forms.Button btn_browser_edit;
        private System.Windows.Forms.Button btn_browser_remove;
        private System.Windows.Forms.Label lbl_sort;
        private System.Windows.Forms.ComboBox cmb_sort;
        private System.Windows.Forms.Button btn_browser_up;
        private System.Windows.Forms.Button btn_browser_down;
        private System.Windows.Forms.CheckBox chk_running_only;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Button btn_import;
    }
}
