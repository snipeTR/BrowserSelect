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
        /// Layout (v1.5.7.0, Windows 11 look): navigation pane on the left (pnl_nav: one FluentNavItem per
        /// page, Language/Theme/Mica at the bottom), one page panel per section (pg_*) with a header and
        /// cards (FluentCard, former group boxes), Close/Apply and "Link opened from" at the bottom right.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_settings));
            this.pnl_nav = new System.Windows.Forms.Panel();
            this.nav_browsers = new BrowserSelect.UI.FluentNavItem();
            this.nav_default = new BrowserSelect.UI.FluentNavItem();
            this.nav_rules = new BrowserSelect.UI.FluentNavItem();
            this.nav_options = new BrowserSelect.UI.FluentNavItem();
            this.nav_update = new BrowserSelect.UI.FluentNavItem();
            this.lbl_language = new System.Windows.Forms.Label();
            this.cmb_language = new BrowserSelect.UI.FluentComboBox();
            this.lbl_theme = new System.Windows.Forms.Label();
            this.cmb_theme = new BrowserSelect.UI.FluentComboBox();
            this.chk_mica = new BrowserSelect.UI.FluentToggle();
            this.pg_browsers = new System.Windows.Forms.Panel();
            this.lbl_page_browsers = new BrowserSelect.UI.FluentHeader();
            this.groupBox1 = new BrowserSelect.UI.FluentCard();
            this.host_browsers = new BrowserSelect.UI.FluentFieldHost();
            this.browser_filter = new System.Windows.Forms.CheckedListBox();
            this.btn_browser_add = new BrowserSelect.UI.FluentButton();
            this.btn_browser_edit = new BrowserSelect.UI.FluentButton();
            this.btn_browser_remove = new BrowserSelect.UI.FluentButton();
            this.btn_refresh = new BrowserSelect.UI.FluentButton();
            this.lbl_sort = new System.Windows.Forms.Label();
            this.cmb_sort = new BrowserSelect.UI.FluentComboBox();
            this.btn_browser_up = new BrowserSelect.UI.FluentButton();
            this.btn_browser_down = new BrowserSelect.UI.FluentButton();
            this.pg_default = new System.Windows.Forms.Panel();
            this.lbl_page_default = new BrowserSelect.UI.FluentHeader();
            this.groupBox2 = new BrowserSelect.UI.FluentCard();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_setdefault = new BrowserSelect.UI.FluentButton();
            this.btn_filetypes = new BrowserSelect.UI.FluentButton();
            this.pg_rules = new System.Windows.Forms.Panel();
            this.lbl_page_rules = new BrowserSelect.UI.FluentHeader();
            this.groupBox3 = new BrowserSelect.UI.FluentCard();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.host_rules = new BrowserSelect.UI.FluentFieldHost();
            this.gv_filters = new System.Windows.Forms.DataGridView();
            this.button1 = new BrowserSelect.UI.FluentButton();
            this.btn_move_up = new BrowserSelect.UI.FluentButton();
            this.btn_move_down = new BrowserSelect.UI.FluentButton();
            this.btn_delete = new BrowserSelect.UI.FluentButton();
            this.pg_options = new System.Windows.Forms.Panel();
            this.lbl_page_options = new BrowserSelect.UI.FluentHeader();
            this.card_running = new BrowserSelect.UI.FluentCard();
            this.chk_running_only = new BrowserSelect.UI.FluentToggle();
            this.card_alt = new BrowserSelect.UI.FluentCard();
            this.chk_alt_ignore = new BrowserSelect.UI.FluentToggle();
            this.groupBox5 = new BrowserSelect.UI.FluentCard();
            this.chk_avoid_fullscreen = new BrowserSelect.UI.FluentToggle();
            this.lbl_fullscreen_fallback = new System.Windows.Forms.Label();
            this.cmb_fullscreen_fallback = new BrowserSelect.UI.FluentComboBox();
            this.card_transfer = new BrowserSelect.UI.FluentCard();
            this.btn_export = new BrowserSelect.UI.FluentButton();
            this.btn_import = new BrowserSelect.UI.FluentButton();
            this.pg_update = new System.Windows.Forms.Panel();
            this.lbl_page_update = new BrowserSelect.UI.FluentHeader();
            this.groupBox4 = new BrowserSelect.UI.FluentCard();
            this.chk_check_update = new BrowserSelect.UI.FluentToggle();
            this.btn_check_update = new BrowserSelect.UI.FluentButton();
            this.card_feedback = new BrowserSelect.UI.FluentCard();
            this.label2 = new System.Windows.Forms.Label();
            this.pnl_footer = new System.Windows.Forms.Panel();
            this.lbl_source = new System.Windows.Forms.Label();
            this.btn_cancel = new BrowserSelect.UI.FluentButton();
            this.btn_apply = new BrowserSelect.UI.FluentButton();
            this.matchType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.pattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browser = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isPrivate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.arguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnl_nav.SuspendLayout();
            this.pnl_footer.SuspendLayout();
            this.pg_browsers.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.host_browsers.SuspendLayout();
            this.pg_default.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pg_rules.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.host_rules.SuspendLayout();
            this.pg_options.SuspendLayout();
            this.card_running.SuspendLayout();
            this.card_alt.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.card_transfer.SuspendLayout();
            this.pg_update.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.card_feedback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gv_filters)).BeginInit();
            this.SuspendLayout();
            //
            // pnl_nav
            //
            this.pnl_nav.Controls.Add(this.nav_browsers);
            this.pnl_nav.Controls.Add(this.nav_default);
            this.pnl_nav.Controls.Add(this.nav_rules);
            this.pnl_nav.Controls.Add(this.nav_options);
            this.pnl_nav.Controls.Add(this.nav_update);
            this.pnl_nav.Controls.Add(this.lbl_language);
            this.pnl_nav.Controls.Add(this.cmb_language);
            this.pnl_nav.Controls.Add(this.lbl_theme);
            this.pnl_nav.Controls.Add(this.cmb_theme);
            this.pnl_nav.Controls.Add(this.chk_mica);
            this.pnl_nav.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_nav.Location = new System.Drawing.Point(0, 0);
            this.pnl_nav.Name = "pnl_nav";
            this.pnl_nav.Size = new System.Drawing.Size(248, 496);
            this.pnl_nav.TabIndex = 0;
            //
            // nav_browsers
            //
            this.nav_browsers.Glyph = BrowserSelect.UI.Fluent.GlyphGlobe;
            this.nav_browsers.Checked = true;
            this.nav_browsers.Location = new System.Drawing.Point(8, 8);
            this.nav_browsers.Name = "nav_browsers";
            this.nav_browsers.Size = new System.Drawing.Size(224, 36);
            this.nav_browsers.TabIndex = 0;
            this.nav_browsers.Text = "Browsers";
            //
            // nav_default
            //
            this.nav_default.Glyph = BrowserSelect.UI.Fluent.GlyphStar;
            this.nav_default.Location = new System.Drawing.Point(8, 46);
            this.nav_default.Name = "nav_default";
            this.nav_default.Size = new System.Drawing.Size(224, 36);
            this.nav_default.TabIndex = 1;
            this.nav_default.Text = "Default Browser";
            //
            // nav_rules
            //
            this.nav_rules.Glyph = BrowserSelect.UI.Fluent.GlyphFilter;
            this.nav_rules.Location = new System.Drawing.Point(8, 84);
            this.nav_rules.Name = "nav_rules";
            this.nav_rules.Size = new System.Drawing.Size(224, 36);
            this.nav_rules.TabIndex = 2;
            this.nav_rules.Text = "Auto Select Filters";
            //
            // nav_options
            //
            this.nav_options.Glyph = BrowserSelect.UI.Fluent.GlyphSettings;
            this.nav_options.Location = new System.Drawing.Point(8, 122);
            this.nav_options.Name = "nav_options";
            this.nav_options.Size = new System.Drawing.Size(224, 36);
            this.nav_options.TabIndex = 3;
            this.nav_options.Text = "Options";
            //
            // nav_update
            //
            this.nav_update.Glyph = BrowserSelect.UI.Fluent.GlyphSync;
            this.nav_update.Location = new System.Drawing.Point(8, 160);
            this.nav_update.Name = "nav_update";
            this.nav_update.Size = new System.Drawing.Size(224, 36);
            this.nav_update.TabIndex = 4;
            this.nav_update.Text = "Update checker";
            //
            // lbl_language
            //
            this.lbl_language.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.lbl_language.AutoSize = true;
            this.lbl_language.Location = new System.Drawing.Point(12, 360);
            this.lbl_language.Name = "lbl_language";
            this.lbl_language.Size = new System.Drawing.Size(58, 13);
            this.lbl_language.TabIndex = 5;
            this.lbl_language.Text = "Language:";
            //
            // cmb_language
            //
            this.cmb_language.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.cmb_language.Location = new System.Drawing.Point(12, 378);
            this.cmb_language.Name = "cmb_language";
            this.cmb_language.Size = new System.Drawing.Size(216, 21);
            this.cmb_language.TabIndex = 6;
            this.toolTip1.SetToolTip(this.cmb_language, "User interface language. Every language added to BrowserSelect is listed here.");
            this.cmb_language.SelectedIndexChanged += new System.EventHandler(this.cmb_language_SelectedIndexChanged);
            //
            // lbl_theme
            //
            this.lbl_theme.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.lbl_theme.AutoSize = true;
            this.lbl_theme.Location = new System.Drawing.Point(12, 410);
            this.lbl_theme.Name = "lbl_theme";
            this.lbl_theme.Size = new System.Drawing.Size(43, 13);
            this.lbl_theme.TabIndex = 7;
            this.lbl_theme.Text = "Theme:";
            //
            // cmb_theme
            //
            this.cmb_theme.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.cmb_theme.Location = new System.Drawing.Point(12, 428);
            this.cmb_theme.Name = "cmb_theme";
            this.cmb_theme.Size = new System.Drawing.Size(216, 21);
            this.cmb_theme.TabIndex = 8;
            this.cmb_theme.SelectedIndexChanged += new System.EventHandler(this.cmb_theme_SelectedIndexChanged);
            //
            // chk_mica
            //
            this.chk_mica.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.chk_mica.Location = new System.Drawing.Point(12, 456);
            this.chk_mica.Name = "chk_mica";
            this.chk_mica.Size = new System.Drawing.Size(216, 32);
            this.chk_mica.TabIndex = 9;
            this.chk_mica.Text = "Mica";
            this.chk_mica.CheckedChanged += new System.EventHandler(this.chk_mica_CheckedChanged);
            //
            // pg_browsers
            //
            this.pg_browsers.Controls.Add(this.lbl_page_browsers);
            this.pg_browsers.Controls.Add(this.groupBox1);
            this.pg_browsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_browsers.Location = new System.Drawing.Point(248, 0);
            this.pg_browsers.Name = "pg_browsers";
            this.pg_browsers.Size = new System.Drawing.Size(652, 446);
            this.pg_browsers.TabIndex = 1;
            //
            // lbl_page_browsers
            //
            this.lbl_page_browsers.Location = new System.Drawing.Point(6, 14);
            this.lbl_page_browsers.Name = "lbl_page_browsers";
            this.lbl_page_browsers.TabIndex = 0;
            this.lbl_page_browsers.Text = "Browsers";
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.host_browsers);
            this.groupBox1.Controls.Add(this.btn_browser_add);
            this.groupBox1.Controls.Add(this.btn_browser_edit);
            this.groupBox1.Controls.Add(this.btn_browser_remove);
            this.groupBox1.Controls.Add(this.btn_refresh);
            this.groupBox1.Controls.Add(this.lbl_sort);
            this.groupBox1.Controls.Add(this.cmb_sort);
            this.groupBox1.Controls.Add(this.btn_browser_up);
            this.groupBox1.Controls.Add(this.btn_browser_down);
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.groupBox1.Location = new System.Drawing.Point(4, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(636, 378);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.Text = "Browsers";
            //
            // host_browsers
            //
            this.host_browsers.Controls.Add(this.browser_filter);
            this.host_browsers.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.host_browsers.Inset = 4;
            this.host_browsers.Location = new System.Drawing.Point(12, 12);
            this.host_browsers.Name = "host_browsers";
            this.host_browsers.Size = new System.Drawing.Size(448, 354);
            this.host_browsers.TabIndex = 0;
            //
            // browser_filter
            //
            this.browser_filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser_filter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.browser_filter.FormattingEnabled = true;
            this.browser_filter.IntegralHeight = false;
            this.browser_filter.Name = "browser_filter";
            this.browser_filter.TabIndex = 0;
            this.browser_filter.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.browser_filter_ItemCheck);
            this.browser_filter.SelectedIndexChanged += new System.EventHandler(this.browser_filter_SelectedIndexChanged);
            this.browser_filter.DoubleClick += new System.EventHandler(this.btn_browser_edit_Click);
            //
            // btn_browser_add
            //
            this.btn_browser_add.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btn_browser_add.Location = new System.Drawing.Point(472, 12);
            this.btn_browser_add.Name = "btn_browser_add";
            this.btn_browser_add.Size = new System.Drawing.Size(152, 30);
            this.btn_browser_add.TabIndex = 1;
            this.btn_browser_add.Text = "Add...";
            this.toolTip1.SetToolTip(this.btn_browser_add, "Add a portable browser (or any program) by selecting its executable");
            this.btn_browser_add.Click += new System.EventHandler(this.btn_browser_add_Click);
            //
            // btn_browser_edit
            //
            this.btn_browser_edit.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btn_browser_edit.Location = new System.Drawing.Point(472, 48);
            this.btn_browser_edit.Name = "btn_browser_edit";
            this.btn_browser_edit.Size = new System.Drawing.Size(152, 30);
            this.btn_browser_edit.TabIndex = 2;
            this.btn_browser_edit.Text = "Edit...";
            this.toolTip1.SetToolTip(this.btn_browser_edit, "Change the icon, shortcut keys and arguments of the selected browser");
            this.btn_browser_edit.Click += new System.EventHandler(this.btn_browser_edit_Click);
            //
            // btn_browser_remove
            //
            this.btn_browser_remove.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btn_browser_remove.Location = new System.Drawing.Point(472, 84);
            this.btn_browser_remove.Name = "btn_browser_remove";
            this.btn_browser_remove.Size = new System.Drawing.Size(152, 30);
            this.btn_browser_remove.TabIndex = 3;
            this.btn_browser_remove.Text = "Remove";
            this.toolTip1.SetToolTip(this.btn_browser_remove, "Remove a manually added browser (uncheck a browser to hide it)");
            this.btn_browser_remove.Click += new System.EventHandler(this.btn_browser_remove_Click);
            //
            // btn_refresh
            //
            this.btn_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btn_refresh.Location = new System.Drawing.Point(472, 128);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(152, 30);
            this.btn_refresh.TabIndex = 4;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            //
            // lbl_sort
            //
            this.lbl_sort.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.lbl_sort.AutoSize = true;
            this.lbl_sort.Location = new System.Drawing.Point(472, 176);
            this.lbl_sort.Name = "lbl_sort";
            this.lbl_sort.Size = new System.Drawing.Size(29, 13);
            this.lbl_sort.TabIndex = 5;
            this.lbl_sort.Text = "Sort:";
            //
            // cmb_sort
            //
            this.cmb_sort.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.cmb_sort.Location = new System.Drawing.Point(472, 194);
            this.cmb_sort.Name = "cmb_sort";
            this.cmb_sort.Size = new System.Drawing.Size(152, 21);
            this.cmb_sort.TabIndex = 6;
            this.toolTip1.SetToolTip(this.cmb_sort, "Order of the browsers in the selection window");
            this.cmb_sort.SelectedIndexChanged += new System.EventHandler(this.cmb_sort_SelectedIndexChanged);
            //
            // btn_browser_up
            //
            this.btn_browser_up.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btn_browser_up.Glyph = BrowserSelect.UI.Fluent.GlyphChevronUp;
            this.btn_browser_up.Location = new System.Drawing.Point(472, 228);
            this.btn_browser_up.Name = "btn_browser_up";
            this.btn_browser_up.Size = new System.Drawing.Size(73, 30);
            this.btn_browser_up.TabIndex = 7;
            this.btn_browser_up.Text = "\u25B2";
            this.toolTip1.SetToolTip(this.btn_browser_up, "Move the selected browser up (Manual sort)");
            this.btn_browser_up.Click += new System.EventHandler(this.btn_browser_up_Click);
            //
            // btn_browser_down
            //
            this.btn_browser_down.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btn_browser_down.Glyph = BrowserSelect.UI.Fluent.GlyphChevronDown;
            this.btn_browser_down.Location = new System.Drawing.Point(551, 228);
            this.btn_browser_down.Name = "btn_browser_down";
            this.btn_browser_down.Size = new System.Drawing.Size(73, 30);
            this.btn_browser_down.TabIndex = 8;
            this.btn_browser_down.Text = "\u25BC";
            this.toolTip1.SetToolTip(this.btn_browser_down, "Move the selected browser down (Manual sort)");
            this.btn_browser_down.Click += new System.EventHandler(this.btn_browser_down_Click);
            //
            // pg_default
            //
            this.pg_default.Controls.Add(this.lbl_page_default);
            this.pg_default.Controls.Add(this.groupBox2);
            this.pg_default.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_default.Location = new System.Drawing.Point(248, 0);
            this.pg_default.Name = "pg_default";
            this.pg_default.Size = new System.Drawing.Size(652, 446);
            this.pg_default.TabIndex = 2;
            this.pg_default.Visible = false;
            //
            // lbl_page_default
            //
            this.lbl_page_default.Location = new System.Drawing.Point(6, 14);
            this.lbl_page_default.Name = "lbl_page_default";
            this.lbl_page_default.TabIndex = 0;
            this.lbl_page_default.Text = "Default Browser";
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btn_setdefault);
            this.groupBox2.Controls.Add(this.btn_filetypes);
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.groupBox2.Location = new System.Drawing.Point(4, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(636, 112);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.Text = "Default Browser";
            //
            // label1
            //
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(604, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "BrowserSelect must be set as default browser for it to function correctly. It can also open .html and .url files.";
            //
            // btn_setdefault
            //
            this.btn_setdefault.Accent = true;
            this.btn_setdefault.Location = new System.Drawing.Point(16, 66);
            this.btn_setdefault.Name = "btn_setdefault";
            this.btn_setdefault.Size = new System.Drawing.Size(220, 30);
            this.btn_setdefault.TabIndex = 1;
            this.btn_setdefault.Text = "Set as Default Browser";
            this.btn_setdefault.Click += new System.EventHandler(this.btn_setdefault_Click);
            //
            // btn_filetypes
            //
            this.btn_filetypes.Location = new System.Drawing.Point(244, 66);
            this.btn_filetypes.Name = "btn_filetypes";
            this.btn_filetypes.Size = new System.Drawing.Size(160, 30);
            this.btn_filetypes.TabIndex = 2;
            this.btn_filetypes.Text = "File types...";
            this.toolTip1.SetToolTip(this.btn_filetypes, "Register BrowserSelect for .htm/.html/.shtml/.xhtml/.url files (adds it to \"Open with\" and Default apps)");
            this.btn_filetypes.Click += new System.EventHandler(this.btn_filetypes_Click);
            //
            // pg_rules
            //
            this.pg_rules.Controls.Add(this.lbl_page_rules);
            this.pg_rules.Controls.Add(this.groupBox3);
            this.pg_rules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_rules.Location = new System.Drawing.Point(248, 0);
            this.pg_rules.Name = "pg_rules";
            this.pg_rules.Size = new System.Drawing.Size(652, 446);
            this.pg_rules.TabIndex = 3;
            this.pg_rules.Visible = false;
            //
            // lbl_page_rules
            //
            this.lbl_page_rules.Location = new System.Drawing.Point(6, 14);
            this.lbl_page_rules.Name = "lbl_page_rules";
            this.lbl_page_rules.TabIndex = 0;
            this.lbl_page_rules.Text = "Auto Select Filters";
            //
            // groupBox3
            //
            this.groupBox3.Controls.Add(this.linkLabel1);
            this.groupBox3.Controls.Add(this.host_rules);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.btn_move_up);
            this.groupBox3.Controls.Add(this.btn_move_down);
            this.groupBox3.Controls.Add(this.btn_delete);
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.groupBox3.Location = new System.Drawing.Point(4, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(636, 378);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.Text = "Auto Select Filters";
            //
            // linkLabel1
            //
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(212, 235);
            this.linkLabel1.Text = resources.GetString("linkLabel1.Text");
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(612, 48);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            //
            // host_rules
            //
            this.host_rules.Controls.Add(this.gv_filters);
            this.host_rules.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.host_rules.Inset = 2;
            this.host_rules.Location = new System.Drawing.Point(12, 62);
            this.host_rules.Name = "host_rules";
            this.host_rules.Size = new System.Drawing.Size(612, 266);
            this.host_rules.TabIndex = 1;
            //
            // gv_filters
            //
            this.gv_filters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gv_filters.AllowUserToAddRows = false;
            this.gv_filters.AllowUserToDeleteRows = false;
            this.gv_filters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_filters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.matchType,
            this.pattern,
            this.browser,
            this.isPrivate,
            this.arguments});
            this.gv_filters.Name = "gv_filters";
            this.gv_filters.TabIndex = 0;
            this.gv_filters.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gv_filters_CellBeginEditRule);
            this.gv_filters.CurrentCellDirtyStateChanged += new System.EventHandler(this.gv_filters_CurrentCellDirtyStateChanged);
            this.gv_filters.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gv_filters_DataError);
            //
            // button1
            //
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.button1.Location = new System.Drawing.Point(12, 336);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "Help";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // btn_move_up
            //
            this.btn_move_up.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btn_move_up.Location = new System.Drawing.Point(102, 336);
            this.btn_move_up.Name = "btn_move_up";
            this.btn_move_up.Size = new System.Drawing.Size(100, 30);
            this.btn_move_up.TabIndex = 3;
            this.btn_move_up.Text = "Move Up";
            this.btn_move_up.Click += new System.EventHandler(this.btn_move_up_Click);
            //
            // btn_move_down
            //
            this.btn_move_down.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btn_move_down.Location = new System.Drawing.Point(208, 336);
            this.btn_move_down.Name = "btn_move_down";
            this.btn_move_down.Size = new System.Drawing.Size(100, 30);
            this.btn_move_down.TabIndex = 4;
            this.btn_move_down.Text = "Move Down";
            this.btn_move_down.Click += new System.EventHandler(this.btn_move_down_Click);
            //
            // btn_delete
            //
            this.btn_delete.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btn_delete.Location = new System.Drawing.Point(314, 336);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(84, 30);
            this.btn_delete.TabIndex = 5;
            this.btn_delete.Text = "Delete";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            //
            // pg_options
            //
            this.pg_options.Controls.Add(this.lbl_page_options);
            this.pg_options.Controls.Add(this.card_running);
            this.pg_options.Controls.Add(this.card_alt);
            this.pg_options.Controls.Add(this.groupBox5);
            this.pg_options.Controls.Add(this.card_transfer);
            this.pg_options.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_options.Location = new System.Drawing.Point(248, 0);
            this.pg_options.Name = "pg_options";
            this.pg_options.Size = new System.Drawing.Size(652, 446);
            this.pg_options.TabIndex = 4;
            this.pg_options.Visible = false;
            //
            // lbl_page_options
            //
            this.lbl_page_options.Location = new System.Drawing.Point(6, 14);
            this.lbl_page_options.Name = "lbl_page_options";
            this.lbl_page_options.TabIndex = 0;
            this.lbl_page_options.Text = "Options";
            //
            // card_running
            //
            this.card_running.Controls.Add(this.chk_running_only);
            this.card_running.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.card_running.Location = new System.Drawing.Point(4, 60);
            this.card_running.Name = "card_running";
            this.card_running.Size = new System.Drawing.Size(636, 48);
            this.card_running.TabIndex = 1;
            //
            // chk_running_only
            //
            this.chk_running_only.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.chk_running_only.Location = new System.Drawing.Point(16, 4);
            this.chk_running_only.Name = "chk_running_only";
            this.chk_running_only.Size = new System.Drawing.Size(608, 40);
            this.chk_running_only.TabIndex = 0;
            this.chk_running_only.Text = "Show running browsers only";
            this.toolTip1.SetToolTip(this.chk_running_only, "Only list browsers that are currently running (all browsers are shown if none is running)");
            this.chk_running_only.CheckedChanged += new System.EventHandler(this.chk_running_only_CheckedChanged);
            //
            // card_alt
            //
            this.card_alt.Controls.Add(this.chk_alt_ignore);
            this.card_alt.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.card_alt.Location = new System.Drawing.Point(4, 112);
            this.card_alt.Name = "card_alt";
            this.card_alt.Size = new System.Drawing.Size(636, 48);
            this.card_alt.TabIndex = 2;
            //
            // chk_alt_ignore
            //
            this.chk_alt_ignore.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.chk_alt_ignore.Location = new System.Drawing.Point(16, 4);
            this.chk_alt_ignore.Name = "chk_alt_ignore";
            this.chk_alt_ignore.Size = new System.Drawing.Size(608, 40);
            this.chk_alt_ignore.TabIndex = 0;
            this.chk_alt_ignore.Text = "Hold Alt on a link to skip rules";
            this.toolTip1.SetToolTip(this.chk_alt_ignore, "If Alt is held down while a link is clicked, the Auto Select rules are ignored and the browser list is shown.");
            this.chk_alt_ignore.CheckedChanged += new System.EventHandler(this.chk_alt_ignore_CheckedChanged);
            //
            // groupBox5
            //
            this.groupBox5.Controls.Add(this.chk_avoid_fullscreen);
            this.groupBox5.Controls.Add(this.lbl_fullscreen_fallback);
            this.groupBox5.Controls.Add(this.cmb_fullscreen_fallback);
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.groupBox5.Location = new System.Drawing.Point(4, 164);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(636, 88);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.Text = "Options";
            //
            // chk_avoid_fullscreen
            //
            this.chk_avoid_fullscreen.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.chk_avoid_fullscreen.Location = new System.Drawing.Point(16, 4);
            this.chk_avoid_fullscreen.Name = "chk_avoid_fullscreen";
            this.chk_avoid_fullscreen.Size = new System.Drawing.Size(608, 40);
            this.chk_avoid_fullscreen.TabIndex = 0;
            this.chk_avoid_fullscreen.Text = "Avoid full-screen windows";
            this.chk_avoid_fullscreen.CheckedChanged += new System.EventHandler(this.chk_avoid_fullscreen_CheckedChanged);
            //
            // lbl_fullscreen_fallback
            //
            this.lbl_fullscreen_fallback.AutoSize = true;
            this.lbl_fullscreen_fallback.Location = new System.Drawing.Point(32, 56);
            this.lbl_fullscreen_fallback.Name = "lbl_fullscreen_fallback";
            this.lbl_fullscreen_fallback.Size = new System.Drawing.Size(150, 13);
            this.lbl_fullscreen_fallback.TabIndex = 1;
            this.lbl_fullscreen_fallback.Text = "If all windows are full screen:";
            //
            // cmb_fullscreen_fallback
            //
            this.cmb_fullscreen_fallback.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.cmb_fullscreen_fallback.Location = new System.Drawing.Point(364, 52);
            this.cmb_fullscreen_fallback.Name = "cmb_fullscreen_fallback";
            this.cmb_fullscreen_fallback.Size = new System.Drawing.Size(256, 21);
            this.cmb_fullscreen_fallback.TabIndex = 2;
            this.cmb_fullscreen_fallback.SelectedIndexChanged += new System.EventHandler(this.cmb_fullscreen_fallback_SelectedIndexChanged);
            //
            // card_transfer
            //
            this.card_transfer.Controls.Add(this.btn_export);
            this.card_transfer.Controls.Add(this.btn_import);
            this.card_transfer.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.card_transfer.Location = new System.Drawing.Point(4, 256);
            this.card_transfer.Name = "card_transfer";
            this.card_transfer.Size = new System.Drawing.Size(636, 56);
            this.card_transfer.TabIndex = 4;
            //
            // btn_export
            //
            this.btn_export.Location = new System.Drawing.Point(16, 13);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(160, 30);
            this.btn_export.TabIndex = 0;
            this.btn_export.Text = "Export...";
            this.toolTip1.SetToolTip(this.btn_export, "Save rules and settings to a file");
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            //
            // btn_import
            //
            this.btn_import.Location = new System.Drawing.Point(184, 13);
            this.btn_import.Name = "btn_import";
            this.btn_import.Size = new System.Drawing.Size(160, 30);
            this.btn_import.TabIndex = 1;
            this.btn_import.Text = "Import...";
            this.toolTip1.SetToolTip(this.btn_import, "Load rules and settings from a file (replaces the current ones)");
            this.btn_import.Click += new System.EventHandler(this.btn_import_Click);
            //
            // pg_update
            //
            this.pg_update.Controls.Add(this.lbl_page_update);
            this.pg_update.Controls.Add(this.groupBox4);
            this.pg_update.Controls.Add(this.card_feedback);
            this.pg_update.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_update.Location = new System.Drawing.Point(248, 0);
            this.pg_update.Name = "pg_update";
            this.pg_update.Size = new System.Drawing.Size(652, 446);
            this.pg_update.TabIndex = 5;
            this.pg_update.Visible = false;
            //
            // lbl_page_update
            //
            this.lbl_page_update.Location = new System.Drawing.Point(6, 14);
            this.lbl_page_update.Name = "lbl_page_update";
            this.lbl_page_update.TabIndex = 0;
            this.lbl_page_update.Text = "Update checker";
            //
            // groupBox4
            //
            this.groupBox4.Controls.Add(this.chk_check_update);
            this.groupBox4.Controls.Add(this.btn_check_update);
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.groupBox4.Location = new System.Drawing.Point(4, 60);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(636, 52);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.Text = "Update checker";
            //
            // chk_check_update
            //
            this.chk_check_update.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.chk_check_update.Location = new System.Drawing.Point(16, 6);
            this.chk_check_update.Name = "chk_check_update";
            this.chk_check_update.Size = new System.Drawing.Size(440, 40);
            this.chk_check_update.TabIndex = 0;
            this.chk_check_update.Text = "enable";
            this.chk_check_update.CheckedChanged += new System.EventHandler(this.chk_check_update_CheckedChanged);
            //
            // btn_check_update
            //
            this.btn_check_update.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btn_check_update.Location = new System.Drawing.Point(468, 11);
            this.btn_check_update.Name = "btn_check_update";
            this.btn_check_update.Size = new System.Drawing.Size(152, 30);
            this.btn_check_update.TabIndex = 1;
            this.btn_check_update.Text = "check now";
            this.btn_check_update.Click += new System.EventHandler(this.btn_check_update_Click);
            //
            // card_feedback
            //
            this.card_feedback.Controls.Add(this.label2);
            this.card_feedback.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.card_feedback.Location = new System.Drawing.Point(4, 116);
            this.card_feedback.Name = "card_feedback";
            this.card_feedback.Size = new System.Drawing.Size(636, 60);
            this.card_feedback.TabIndex = 2;
            //
            // label2
            //
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.label2.Location = new System.Drawing.Point(16, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(604, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "if you have feature requests,bug reports or suggestions please submit an issue on the project\'s Github.";
            //
            // pnl_footer
            //
            this.pnl_footer.Controls.Add(this.lbl_source);
            this.pnl_footer.Controls.Add(this.btn_cancel);
            this.pnl_footer.Controls.Add(this.btn_apply);
            this.pnl_footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_footer.Location = new System.Drawing.Point(248, 446);
            this.pnl_footer.Name = "pnl_footer";
            this.pnl_footer.Size = new System.Drawing.Size(652, 50);
            this.pnl_footer.TabIndex = 6;
            //
            // lbl_source
            //
            this.lbl_source.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.lbl_source.AutoEllipsis = true;
            this.lbl_source.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbl_source.Location = new System.Drawing.Point(8, 16);
            this.lbl_source.Name = "lbl_source";
            this.lbl_source.Size = new System.Drawing.Size(424, 15);
            this.lbl_source.TabIndex = 0;
            //
            // btn_cancel
            //
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(452, 8);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(92, 30);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "Close";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            //
            // btn_apply
            //
            this.btn_apply.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btn_apply.Enabled = false;
            this.btn_apply.Accent = true;
            this.btn_apply.Location = new System.Drawing.Point(550, 8);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(92, 30);
            this.btn_apply.TabIndex = 2;
            this.btn_apply.Text = "Apply";
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
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
            // frm_settings
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(900, 496);
            // docking order: the last added control docks first (navigation pane: full height on the left,
            // then the footer below the pages, then the page that fills the rest)
            this.Controls.Add(this.pg_browsers);
            this.Controls.Add(this.pg_default);
            this.Controls.Add(this.pg_rules);
            this.Controls.Add(this.pg_options);
            this.Controls.Add(this.pg_update);
            this.Controls.Add(this.pnl_footer);
            this.Controls.Add(this.pnl_nav);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(820, 535);
            this.Name = "frm_settings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_settings_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_settings_FormClosed);
            this.Load += new System.EventHandler(this.frm_settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gv_filters)).EndInit();
            this.card_feedback.ResumeLayout(false);
            this.card_feedback.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.pg_update.ResumeLayout(false);
            this.pg_update.PerformLayout();
            this.card_transfer.ResumeLayout(false);
            this.card_transfer.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.card_alt.ResumeLayout(false);
            this.card_alt.PerformLayout();
            this.card_running.ResumeLayout(false);
            this.card_running.PerformLayout();
            this.pg_options.ResumeLayout(false);
            this.pg_options.PerformLayout();
            this.host_rules.ResumeLayout(false);
            this.host_rules.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.pg_rules.ResumeLayout(false);
            this.pg_rules.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pg_default.ResumeLayout(false);
            this.pg_default.PerformLayout();
            this.host_browsers.ResumeLayout(false);
            this.host_browsers.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pg_browsers.ResumeLayout(false);
            this.pg_browsers.PerformLayout();
            this.pnl_footer.ResumeLayout(false);
            this.pnl_nav.ResumeLayout(false);
            this.pnl_nav.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnl_nav;
        private BrowserSelect.UI.FluentNavItem nav_browsers;
        private BrowserSelect.UI.FluentNavItem nav_default;
        private BrowserSelect.UI.FluentNavItem nav_rules;
        private BrowserSelect.UI.FluentNavItem nav_options;
        private BrowserSelect.UI.FluentNavItem nav_update;
        private System.Windows.Forms.Label lbl_language;
        private BrowserSelect.UI.FluentComboBox cmb_language;
        private System.Windows.Forms.Label lbl_theme;
        private BrowserSelect.UI.FluentComboBox cmb_theme;
        private BrowserSelect.UI.FluentToggle chk_mica;
        private System.Windows.Forms.Panel pg_browsers;
        private BrowserSelect.UI.FluentHeader lbl_page_browsers;
        private BrowserSelect.UI.FluentCard groupBox1;
        private BrowserSelect.UI.FluentFieldHost host_browsers;
        private System.Windows.Forms.CheckedListBox browser_filter;
        private BrowserSelect.UI.FluentButton btn_browser_add;
        private BrowserSelect.UI.FluentButton btn_browser_edit;
        private BrowserSelect.UI.FluentButton btn_browser_remove;
        private BrowserSelect.UI.FluentButton btn_refresh;
        private System.Windows.Forms.Label lbl_sort;
        private BrowserSelect.UI.FluentComboBox cmb_sort;
        private BrowserSelect.UI.FluentButton btn_browser_up;
        private BrowserSelect.UI.FluentButton btn_browser_down;
        private System.Windows.Forms.Panel pg_default;
        private BrowserSelect.UI.FluentHeader lbl_page_default;
        private BrowserSelect.UI.FluentCard groupBox2;
        private System.Windows.Forms.Label label1;
        private BrowserSelect.UI.FluentButton btn_setdefault;
        private BrowserSelect.UI.FluentButton btn_filetypes;
        private System.Windows.Forms.Panel pg_rules;
        private BrowserSelect.UI.FluentHeader lbl_page_rules;
        private BrowserSelect.UI.FluentCard groupBox3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private BrowserSelect.UI.FluentFieldHost host_rules;
        private System.Windows.Forms.DataGridView gv_filters;
        private BrowserSelect.UI.FluentButton button1;
        private BrowserSelect.UI.FluentButton btn_move_up;
        private BrowserSelect.UI.FluentButton btn_move_down;
        private BrowserSelect.UI.FluentButton btn_delete;
        private System.Windows.Forms.Panel pg_options;
        private BrowserSelect.UI.FluentHeader lbl_page_options;
        private BrowserSelect.UI.FluentCard card_running;
        private BrowserSelect.UI.FluentToggle chk_running_only;
        private BrowserSelect.UI.FluentCard card_alt;
        private BrowserSelect.UI.FluentToggle chk_alt_ignore;
        private BrowserSelect.UI.FluentCard groupBox5;
        private BrowserSelect.UI.FluentToggle chk_avoid_fullscreen;
        private System.Windows.Forms.Label lbl_fullscreen_fallback;
        private BrowserSelect.UI.FluentComboBox cmb_fullscreen_fallback;
        private BrowserSelect.UI.FluentCard card_transfer;
        private BrowserSelect.UI.FluentButton btn_export;
        private BrowserSelect.UI.FluentButton btn_import;
        private System.Windows.Forms.Panel pg_update;
        private BrowserSelect.UI.FluentHeader lbl_page_update;
        private BrowserSelect.UI.FluentCard groupBox4;
        private BrowserSelect.UI.FluentToggle chk_check_update;
        private BrowserSelect.UI.FluentButton btn_check_update;
        private BrowserSelect.UI.FluentCard card_feedback;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnl_footer;
        private System.Windows.Forms.Label lbl_source;
        private BrowserSelect.UI.FluentButton btn_cancel;
        private BrowserSelect.UI.FluentButton btn_apply;
        private System.Windows.Forms.DataGridViewComboBoxColumn matchType;
        private System.Windows.Forms.DataGridViewTextBoxColumn pattern;
        private System.Windows.Forms.DataGridViewComboBoxColumn browser;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPrivate;
        private System.Windows.Forms.DataGridViewTextBoxColumn arguments;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
