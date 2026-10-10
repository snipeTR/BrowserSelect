using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrowserSelect.Localization;
using BrowserSelect.UI;
using BrowserSelect.Properties;
using Microsoft.Win32;

namespace BrowserSelect
{
    public partial class frm_settings : Form
    {

        public Form1 mainForm;

        public frm_settings(Form mainForm)
        {
            this.mainForm = (Form1)mainForm;
            InitializeComponent();
            // Windows 10/11 look (fonts, colors, title bar); before the texts so labels are measured with the final font
            Theme.Apply(this);
            ApplyTexts();
            SetupNavigation();
        }

        #region Navigation pane (Windows 11 Settings look)

        /// <summary>
        /// the navigation pane on the left selects the page shown on the right; every page is one of the former
        /// group boxes (Browsers, Default Browser, Auto Select Filters, Options, Update checker), so their names
        /// (and the "Settings &gt; ..." paths in the help) stay the same. The arrow keys move between the items.
        /// </summary>
        private void SetupNavigation()
        {
            nav_browsers.Page = pg_browsers;
            nav_default.Page = pg_default;
            nav_rules.Page = pg_rules;
            nav_options.Page = pg_options;
            nav_update.Page = pg_update;
            foreach (var item in NavItems)
                item.CheckedChanged += NavItem_CheckedChanged;
            ShowPage(NavItems.FirstOrDefault(i => i.Checked) ?? nav_browsers);
        }

        private FluentNavItem[] NavItems
        {
            get { return new[] { nav_browsers, nav_default, nav_rules, nav_options, nav_update }; }
        }

        private void NavItem_CheckedChanged(object sender, EventArgs e)
        {
            var item = (FluentNavItem)sender;
            if (item.Checked)
                ShowPage(item);
        }

        /// <summary>shows the page of a navigation item (and selects the item)</summary>
        private void ShowPage(FluentNavItem item)
        {
            if (!item.Checked)
                item.Checked = true; // calls ShowPage again via CheckedChanged
            foreach (var other in NavItems)
                if (other.Page != null && other.Page != item.Page)
                    other.Page.Visible = false;
            if (item.Page != null && !item.Page.Visible)
                item.Page.Visible = true;
            // UI screenshot workflow: a layout report (clipping etc.) for every page
            if (LayoutCheck.Enabled && IsHandleCreated)
            {
                var timer = new Timer { Interval = 700 };
                timer.Tick += (s, a) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    if (!IsDisposed)
                        LayoutCheck.Write(this);
                };
                timer.Start();
            }
        }

        #endregion

        /// <summary>
        /// sets every visible text from Localization\Strings.resx (current UI language);
        /// the English texts in the designer are only the design-time defaults
        /// </summary>
        private void ApplyTexts()
        {
            Text = Strings.Settings_Title;

            // navigation items, page headers and cards use the (translated) names of the former group boxes
            nav_browsers.Text = lbl_page_browsers.Text = groupBox1.Text = Strings.Settings_BrowsersGroup;
            nav_default.Text = lbl_page_default.Text = groupBox2.Text = Strings.Settings_DefaultGroup;
            nav_rules.Text = lbl_page_rules.Text = groupBox3.Text = Strings.Settings_RulesGroup;
            nav_options.Text = lbl_page_options.Text = groupBox5.Text = Strings.Settings_OptionsGroup;
            nav_update.Text = lbl_page_update.Text = groupBox4.Text = Strings.Settings_UpdateGroup;

            btn_browser_add.Text = Strings.Settings_BrowserAdd;
            toolTip1.SetToolTip(btn_browser_add, Strings.Settings_BrowserAddTooltip);
            btn_browser_edit.Text = Strings.Settings_BrowserEdit;
            toolTip1.SetToolTip(btn_browser_edit, Strings.Settings_BrowserEditTooltip);
            btn_browser_remove.Text = Strings.Settings_BrowserRemove;
            toolTip1.SetToolTip(btn_browser_remove, Strings.Settings_BrowserRemoveTooltip);
            lbl_sort.Text = Strings.Settings_SortLabel;
            toolTip1.SetToolTip(cmb_sort, Strings.Settings_SortTooltip);
            toolTip1.SetToolTip(btn_browser_up, Strings.Settings_BrowserUpTooltip);
            toolTip1.SetToolTip(btn_browser_down, Strings.Settings_BrowserDownTooltip);
            btn_refresh.Text = Strings.Settings_Refresh;

            label1.Text = Strings.Settings_DefaultInfo;
            btn_setdefault.Text = Strings.Settings_SetDefault;
            btn_filetypes.Text = Strings.Settings_FileTypes;
            toolTip1.SetToolTip(btn_filetypes, Strings.Settings_FileTypesTooltip);

            chk_running_only.Text = Strings.Settings_RunningOnly;
            toolTip1.SetToolTip(chk_running_only, Strings.Settings_RunningOnlyTooltip);
            chk_alt_ignore.Text = Strings.Settings_AltIgnore;
            toolTip1.SetToolTip(chk_alt_ignore, Strings.Settings_AltIgnoreTooltip);
            chk_avoid_fullscreen.Text = Strings.Settings_AvoidFullscreen;
            toolTip1.SetToolTip(chk_avoid_fullscreen, Strings.Settings_AvoidFullscreenTooltip);
            lbl_fullscreen_fallback.Text = Strings.Settings_FullscreenFallback;
            toolTip1.SetToolTip(lbl_fullscreen_fallback, Strings.Settings_FullscreenFallbackTooltip);
            toolTip1.SetToolTip(cmb_fullscreen_fallback, Strings.Settings_FullscreenFallbackTooltip);
            btn_export.Text = Strings.Settings_Export;
            toolTip1.SetToolTip(btn_export, Strings.Settings_ExportTooltip);
            btn_import.Text = Strings.Settings_Import;
            toolTip1.SetToolTip(btn_import, Strings.Settings_ImportTooltip);

            chk_check_update.Text = Strings.Settings_UpdateEnable;
            btn_check_update.Text = Strings.Settings_UpdateCheckNow;
            label2.Text = Strings.Settings_FeedbackInfo;

            var info = Strings.Settings_RulesInfo ?? "";
            var link = Strings.Settings_RulesInfoLink ?? "";
            linkLabel1.Text = info;
            var linkStart = link.Length > 0 ? info.IndexOf(link, StringComparison.Ordinal) : -1;
            linkLabel1.LinkArea = linkStart >= 0
                ? new LinkArea(linkStart, link.Length)
                : new LinkArea(0, info.Length);
            matchType.HeaderText = Strings.Settings_ColMatch;
            pattern.HeaderText = Strings.Settings_ColPattern;
            browser.HeaderText = Strings.Settings_ColBrowser;
            isPrivate.HeaderText = Strings.Settings_ColPrivate;
            arguments.HeaderText = Strings.Settings_ColArguments;
            button1.Text = Strings.Settings_Help;
            btn_move_up.Text = Strings.Settings_MoveUp;
            btn_move_down.Text = Strings.Settings_MoveDown;
            btn_delete.Text = Strings.Settings_Delete;
            btn_apply.Text = Strings.Settings_Apply;
            btn_cancel.Text = Strings.Common_Close;
        }

        // the rule grid shows the rules followed by one "add" row (AddRuleRow, painted as a "+" in the row header).
        // The add row is a placeholder object at the end of the list, never a rule: it is not saved, validated,
        // exported, deleted or moved, and its cells cannot be edited (see the "Rule grid: add row" region).
        private BindingList<AutoMatchRule> rules = new BindingList<AutoMatchRule>();
        private BindingSource rulesBindingSource;
        private readonly AutoMatchRule AddRuleRow = new AutoMatchRule { Pattern = "", Browser = "" };
        private void frm_settings_Load(object sender, EventArgs e)
        {
            //check if browser select is the default browser or not
            //to disable/enable "set Browser select as default" button
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
            {
                var default_browser = key?.GetValue("ProgId");

                //disable the set default if already default
                if (default_browser != null && (string)default_browser == "bselectURL")
                    btn_setdefault.Enabled = false;
            }

            // columns are defined in the designer; don't let the grid add one per rule property
            gv_filters.AutoGenerateColumns = false;
            matchType.Items.AddRange(AutoMatchRule.MatchTypes);

            //populate Rules in the gridview (+ the "add" row at the end)
            foreach (var rule in Settings.Default.AutoBrowser)
                rules.Add(rule);
            rules.Add(AddRuleRow);

            //populate list of browsers (browser filter + Rule List ComboBox)
            PopulateBrowsers(BrowserFinder.find());

            _populating = true;
            cmb_sort.Items.AddRange(BrowserCustomizations.SortModes);
            cmb_sort.SelectedItem = BrowserCustomizations.SortModes.Contains(Settings.Default.SortMode)
                ? Settings.Default.SortMode
                : BrowserCustomizations.SortManual;
            chk_running_only.Checked = Settings.Default.ShowRunningOnly;
            _populating = false;
            UpdateBrowserButtons();
            rulesBindingSource = new BindingSource();
            rulesBindingSource.DataSource = rules;
            // new rules are only added with the "+" row; no automatic empty "new row" (it shifted the row
            // indexes against the rule list, so Delete on it removed the rule above)
            rulesBindingSource.AllowNew = false;
            gv_filters.AllowUserToAddRows = false;
            gv_filters.AllowUserToDeleteRows = false;
            gv_filters.DataSource = rulesBindingSource;
            SetupAddRow();

            _populating = true;
            chk_check_update.Checked = Settings.Default.check_update != "nope";
            chk_alt_ignore.Checked = Settings.Default.AltIgnoresRules;
            _populating = false;
            PopulateFullscreenOptions();

            PopulateLanguages();
            PopulateThemes();

            // show which application opened the current link, to help writing "Source App" rules
            if (!string.IsNullOrEmpty(Program.SourceApp))
            {
                lbl_source.Text = L10n.T("Settings_LinkOpenedFrom", System.IO.Path.GetFileName(Program.SourceApp));
                toolTip1.SetToolTip(lbl_source, Program.SourceApp);
            }
        }

        // set while the controls are filled programmatically, to ignore their change events
        private bool _populating;
        // browsers in the order they are displayed in browser_filter
        private List<Browser> _browsers = new List<Browser>();

        private void PopulateBrowsers(List<Browser> browsers, string selectIdentifier = null)
        {
            var c = ((DataGridViewComboBoxColumn)gv_filters.Columns["browser"]);
            _populating = true;
            try
            {
                _browsers = browsers;
                c.Items.Clear();
                browser_filter.Items.Clear();
                foreach (Browser b in browsers)
                {
                    browser_filter.Items.Add(b, !Settings.Default.HideBrowsers.Contains(b.Identifier));
                    if (!c.Items.Contains(b.ToString()))
                        c.Items.Add(b.ToString());
                }
                if (selectIdentifier != null)
                    browser_filter.SelectedIndex = browsers.FindIndex(b => b.Identifier == selectIdentifier);
            }
            finally
            {
                _populating = false;
            }
            UpdateBrowserButtons();
            // add browser select and the "do nothing" option to the list
            c.Items.Add(AutoMatchRule.DisplayBrowserSelect);
            c.Items.Add(AutoMatchRule.IgnoreUrl);
            // keep rules pointing at a browser that is no longer installed displayable/editable
            foreach (var rule in RealRules)
                if (!string.IsNullOrEmpty(rule.Browser) && !c.Items.Contains(rule.Browser))
                    c.Items.Add(rule.Browser);
        }

        private void btn_setdefault_Click(object sender, EventArgs e)
        {
            // Preserve the current default before asking Windows to handle the association UI.
            DefaultBrowserRegistration.CaptureExistingDefault();
            OpenAssociationUI();
        }

        private void btn_filetypes_Click(object sender, EventArgs e)
        {
            try
            {
                DefaultBrowserRegistration.EnsureApplicationRegistered();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, L10n.T("Settings_FileTypesError", ex.Message), Strings.Settings_FileTypesTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show(this,
                L10n.T("Settings_FileTypesDone", string.Join(", ", DefaultBrowserRegistration.FileExtensions)),
                Strings.Settings_FileTypesTitle, MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            OpenAssociationUI();
        }

        /// <summary>opens the Windows UI where the user picks BrowserSelect for protocols and file types</summary>
        private void OpenAssociationUI()
        {
            IApplicationAssociationRegistrationUI associationUi = null;
            try
            {
                DefaultBrowserRegistration.EnsureApplicationRegistered();
                associationUi = (IApplicationAssociationRegistrationUI)
                    new ApplicationAssociationRegistrationUI();
                Marshal.ThrowExceptionForHR(
                    associationUi.LaunchAdvancedAssociationUI("BrowserSelect"));
            }
            catch (Exception ex)
            {
                // The documented association UI is not available on every Windows build.
                // Fall back to the Windows Default apps page instead of writing UserChoice.
                try
                {
                    Process.Start(new ProcessStartInfo("ms-settings:defaultapps")
                    {
                        UseShellExecute = true
                    });
                }
                catch (Exception fallbackException)
                {
                    MessageBox.Show(
                        L10n.T("Settings_DefaultAppsError", fallbackException.Message, ex.Message),
                        Strings.Settings_DefaultAppsErrorTitle, MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            finally
            {
                if (associationUi != null && Marshal.IsComObject(associationUi))
                    Marshal.ReleaseComObject(associationUi);
            }
        }

        [ComImport]
        [Guid("1F76A169-F994-40AC-8FC8-0959E8874710")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IApplicationAssociationRegistrationUI
        {
            [PreserveSig]
            int LaunchAdvancedAssociationUI(
                [MarshalAs(UnmanagedType.LPWStr)] string applicationRegistryName);
        }

        [ComImport]
        [Guid("1968106D-F3B5-44CF-890E-116FCB9ECEF1")]
        [ClassInterface(ClassInterfaceType.None)]
        private class ApplicationAssociationRegistrationUI
        {
        }

        private void browser_filter_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_populating)
                return;
            //Save changes to the BrowserFilter List
            var identifier = ((Browser)browser_filter.Items[e.Index]).Identifier;
            if (e.NewValue == CheckState.Checked)
            {
                Settings.Default.HideBrowsers.Remove(identifier);
            }
            else if (!Settings.Default.HideBrowsers.Contains(identifier))
            {
                Settings.Default.HideBrowsers.Add(identifier);
            }
            Settings.Default.Save();
        }

        private Browser SelectedBrowser => browser_filter.SelectedItem as Browser;

        private void browser_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBrowserButtons();
        }

        private void UpdateBrowserButtons()
        {
            var selected = SelectedBrowser;
            int index = browser_filter.SelectedIndex;
            bool manual = (cmb_sort.SelectedItem as string ?? BrowserCustomizations.SortManual) ==
                          BrowserCustomizations.SortManual;
            btn_browser_edit.Enabled = selected != null;
            btn_browser_remove.Enabled = selected != null && selected.isCustom;
            btn_browser_up.Enabled = manual && selected != null && index > 0;
            btn_browser_down.Enabled = manual && selected != null && index < browser_filter.Items.Count - 1;
        }

        /// <summary>reloads the browser list (keeping the selection) and updates the main window</summary>
        private void RefreshBrowsers(string selectIdentifier)
        {
            PopulateBrowsers(BrowserFinder.find(), selectIdentifier);
            mainForm?.updateBrowsers();
        }

        private void btn_browser_add_Click(object sender, EventArgs e)
        {
            using (var dialog = new frm_browser_edit(null, _browsers))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                var browser = new Browser
                {
                    name = dialog.BrowserName,
                    exec = dialog.Executable,
                    additionalArgs = dialog.Arguments,
                    icon = dialog.DefaultIcon
                };
                var custom = BrowserCustomizations.LoadCustomBrowsers();
                custom.Add(browser);
                BrowserCustomizations.SaveCustomBrowsers(custom);
                BrowserCustomizations.SetOverride(browser.Identifier,
                    new BrowserOverride { icon = dialog.CustomIcon, shortcut = dialog.Shortcut });
                RefreshBrowsers(browser.Identifier);
            }
        }

        private void btn_browser_edit_Click(object sender, EventArgs e)
        {
            var browser = SelectedBrowser;
            if (browser == null)
                return;
            using (var dialog = new frm_browser_edit(browser, _browsers))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                string identifier = browser.Identifier;
                if (browser.isCustom)
                {
                    var custom = BrowserCustomizations.LoadCustomBrowsers();
                    var stored = custom.FirstOrDefault(x =>
                        string.Equals(x.Identifier, identifier, StringComparison.OrdinalIgnoreCase));
                    if (stored != null)
                    {
                        string oldName = stored.name;
                        stored.name = dialog.BrowserName;
                        stored.exec = dialog.Executable;
                        stored.additionalArgs = dialog.Arguments;
                        stored.icon = dialog.DefaultIcon ?? stored.icon;
                        BrowserCustomizations.SaveCustomBrowsers(custom);
                        BrowserCustomizations.RenameIdentifier(identifier, stored.Identifier);
                        if (oldName != stored.name)
                            RenameBrowserInRules(oldName, stored.name);
                        identifier = stored.Identifier;
                    }
                    BrowserCustomizations.SetOverride(identifier,
                        new BrowserOverride { icon = dialog.CustomIcon, shortcut = dialog.Shortcut });
                }
                else
                {
                    BrowserCustomizations.SetOverride(identifier, new BrowserOverride
                    {
                        icon = dialog.CustomIcon,
                        shortcut = dialog.Shortcut,
                        args = dialog.Arguments
                    });
                }
                RefreshBrowsers(identifier);
            }
        }

        /// <summary>rules refer to browsers by name; keep them working when a browser is renamed</summary>
        private void RenameBrowserInRules(string oldName, string newName)
        {
            foreach (var rule in RealRules)
                if (rule.Browser == oldName)
                    rule.Browser = newName;
            rulesBindingSource?.ResetBindings(false);

            for (int i = 0; i < Settings.Default.AutoBrowser.Count; i++)
            {
                AutoMatchRule rule = Settings.Default.AutoBrowser[i];
                if (rule.Browser == oldName)
                {
                    rule.Browser = newName;
                    Settings.Default.AutoBrowser[i] = rule.ToString();
                }
            }
            Settings.Default.Save();
        }

        private void btn_browser_remove_Click(object sender, EventArgs e)
        {
            var browser = SelectedBrowser;
            if (browser == null || !browser.isCustom)
                return;
            if (MessageBox.Show(this, L10n.T("Settings_RemoveBrowserConfirm", browser.name), Strings.Settings_RemoveBrowserTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            var custom = BrowserCustomizations.LoadCustomBrowsers();
            custom.RemoveAll(x => string.Equals(x.Identifier, browser.Identifier, StringComparison.OrdinalIgnoreCase));
            BrowserCustomizations.SaveCustomBrowsers(custom);
            BrowserCustomizations.SetOverride(browser.Identifier, null);
            Settings.Default.HideBrowsers.Remove(browser.Identifier);
            Settings.Default.Save();
            RefreshBrowsers(null);
        }

        private void btn_browser_up_Click(object sender, EventArgs e)
        {
            MoveBrowser(-1);
        }

        private void btn_browser_down_Click(object sender, EventArgs e)
        {
            MoveBrowser(1);
        }

        private void MoveBrowser(int offset)
        {
            int index = browser_filter.SelectedIndex;
            int target = index + offset;
            if (index < 0 || target < 0 || target >= _browsers.Count)
                return;
            var order = _browsers.Select(b => b.Identifier).ToList();
            var moved = order[index];
            order[index] = order[target];
            order[target] = moved;
            BrowserCustomizations.SaveOrder(order);
            RefreshBrowsers(moved);
        }

        private void cmb_sort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_populating || cmb_sort.SelectedItem == null)
                return;
            Settings.Default.SortMode = (string)cmb_sort.SelectedItem;
            Settings.Default.Save();
            RefreshBrowsers(SelectedBrowser?.Identifier);
        }

        /// <summary>
        /// fills the language drop-down (bottom left) with English plus every installed translation
        /// (see Localization\L10n.cs) and selects the one stored in the settings
        /// </summary>
        private void PopulateLanguages()
        {
            lbl_language.Text = Strings.Language_Label;
            toolTip1.SetToolTip(cmb_language, Strings.Language_Tooltip);

            _populating = true;
            cmb_language.Items.Clear();
            var languages = L10n.Available();
            foreach (var language in languages)
                cmb_language.Items.Add(language);
            var saved = string.IsNullOrWhiteSpace(Settings.Default.Language) ? L10n.Current : Settings.Default.Language;
            var selected = languages.FirstOrDefault(l => l.Code.Equals(saved, StringComparison.OrdinalIgnoreCase))
                           ?? languages[0];
            cmb_language.SelectedItem = selected;
            _populating = false;
        }

        private void cmb_language_SelectedIndexChanged(object sender, EventArgs e)
        {
            var language = cmb_language.SelectedItem as LanguageOption;
            if (_populating || language == null)
                return;
            if (language.Code.Equals(Settings.Default.Language, StringComparison.OrdinalIgnoreCase))
                return;
            Settings.Default.Language = language.Code;
            Settings.Default.Save();
            // forms already open keep their texts; the new language is used from the next start
            if (!language.Code.Equals(L10n.Current, StringComparison.OrdinalIgnoreCase))
                MessageBox.Show(Strings.Language_RestartRequired, Strings.Language_RestartTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>an entry of the theme drop-down (bottom left)</summary>
        private class ThemeChoice
        {
            public string Mode;
            public string Text;

            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        /// fills the theme drop-down (Light / Dark / Follow Windows) and the Mica check box (bottom left)
        /// from the settings; see UI\Theme.cs
        /// </summary>
        private void PopulateThemes()
        {
            lbl_theme.Text = Strings.Theme_Label;
            toolTip1.SetToolTip(cmb_theme, Strings.Theme_Tooltip);
            chk_mica.Text = Strings.Theme_Mica;
            toolTip1.SetToolTip(chk_mica, Strings.Theme_MicaTooltip);

            _populating = true;
            cmb_theme.Items.Clear();
            var choices = new[]
            {
                new ThemeChoice { Mode = Theme.ModeLight, Text = Strings.Theme_Light },
                new ThemeChoice { Mode = Theme.ModeDark, Text = Strings.Theme_Dark },
                new ThemeChoice { Mode = Theme.ModeSystem, Text = Strings.Theme_System },
            };
            cmb_theme.Items.AddRange(choices);
            cmb_theme.SelectedItem = choices.FirstOrDefault(c => c.Mode == Theme.Mode) ?? choices[0];
            chk_mica.Checked = Theme.UseMica;
            _populating = false;
        }

        private void cmb_theme_SelectedIndexChanged(object sender, EventArgs e)
        {
            var choice = cmb_theme.SelectedItem as ThemeChoice;
            if (_populating || choice == null || choice.Mode == Theme.Mode)
                return;
            Settings.Default.Theme = choice.Mode;
            Settings.Default.Save();
            ApplyThemeNow();
        }

        private void chk_mica_CheckedChanged(object sender, EventArgs e)
        {
            if (_populating)
                return;
            Settings.Default.Mica = chk_mica.Checked;
            Settings.Default.Save();
            ApplyThemeNow();
        }

        /// <summary>the new theme is applied at once to this window and the browser list behind it</summary>
        private void ApplyThemeNow()
        {
            Theme.Apply(this);
            if (mainForm != null)
                Theme.Apply(mainForm);
        }

        /// <summary>an entry of the "If all windows are full screen" drop-down (Settings &gt; Options)</summary>
        private class FallbackChoice
        {
            public string Value;
            public string Text;

            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        /// "Avoid full-screen windows" check box and the "If all windows are full screen" drop-down
        /// (see BrowserWindowFocus.cs)
        /// </summary>
        private void PopulateFullscreenOptions()
        {
            _populating = true;
            var choices = new[]
            {
                new FallbackChoice { Value = BrowserWindowFocus.FallbackPrimary, Text = Strings.Settings_FallbackPrimary },
                new FallbackChoice { Value = BrowserWindowFocus.FallbackLastUsed, Text = Strings.Settings_FallbackLastUsed },
            };
            cmb_fullscreen_fallback.Items.Clear();
            cmb_fullscreen_fallback.Items.AddRange(choices);
            cmb_fullscreen_fallback.SelectedItem = choices.FirstOrDefault(c =>
                    c.Value.Equals(Settings.Default.FullscreenFallback, StringComparison.OrdinalIgnoreCase))
                ?? choices[0];
            chk_avoid_fullscreen.Checked = Settings.Default.AvoidFullscreen;
            UpdateFullscreenControls();
            _populating = false;
        }

        private void UpdateFullscreenControls()
        {
            lbl_fullscreen_fallback.Enabled = chk_avoid_fullscreen.Checked;
            cmb_fullscreen_fallback.Enabled = chk_avoid_fullscreen.Checked;
        }

        private void chk_avoid_fullscreen_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFullscreenControls();
            if (_populating)
                return;
            Settings.Default.AvoidFullscreen = chk_avoid_fullscreen.Checked;
            Settings.Default.Save();
        }

        private void cmb_fullscreen_fallback_SelectedIndexChanged(object sender, EventArgs e)
        {
            var choice = cmb_fullscreen_fallback.SelectedItem as FallbackChoice;
            if (_populating || choice == null)
                return;
            Settings.Default.FullscreenFallback = choice.Value;
            Settings.Default.Save();
        }

        private void chk_running_only_CheckedChanged(object sender, EventArgs e)
        {
            if (_populating)
                return;
            Settings.Default.ShowRunningOnly = chk_running_only.Checked;
            Settings.Default.Save();
            mainForm?.updateBrowsers();
        }

        private void frm_settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            // alert user of unsaved changes
            if (btn_apply.Enabled)
            {
                var window = MessageBox.Show(Strings.Settings_UnsavedChanges,
                    Strings.Settings_UnsavedChangesTitle, MessageBoxButtons.YesNo);
                if (window == DialogResult.No) e.Cancel = true;
                else e.Cancel = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // open the rule list help (help/filters.md of this fork, translated page if there is one)
            var file = Strings.Settings_RulesHelpFile;
            if (string.IsNullOrWhiteSpace(file) || file.IndexOfAny(new[] { '/', '\\', ':' }) >= 0)
                file = "filters.md";
            try
            {
                Process.Start(UpdateChecker.HelpUrl + file.Trim());
            }
            catch (Exception)
            {
                // no default browser / association: nothing else to do
            }
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            //save rules
            gv_filters.EndEdit();

            //clear rules (instead of checking for changes we just overwrite the whole ruleset)
            Settings.Default.AutoBrowser.Clear();
            foreach (var rule in RealRules)
            {
                //check if rule has both pattern and browser defined
                if (rule.valid())
                    //add it to rule list
                    Settings.Default.AutoBrowser.Add(rule.ToString());
                else
                {
                    //ignore rule if both pattern and browser is empty otherwise inform user of missing part
                    var err = rule.error();
                    if (err.Length > 0)
                    {
                        MessageBox.Show(L10n.T("Settings_InvalidRule", err));
                    }
                }

            }
            //save rules
            Settings.Default.Save();
            //Enabled property of apply button is used as a flag for unsaved changes
            btn_apply.Enabled = false;
            btn_cancel.Text = Strings.Common_Close;
        }

        private frm_help_rules _frmHelp;
        private void button1_Click(object sender, EventArgs e)
        {
            if (_frmHelp != null)
            {   //help window is open
                _frmHelp.Focus();
            }
            else
            {   //its not open...
                _frmHelp = new frm_help_rules();
                _frmHelp.FormClosed += (o, ev) => _frmHelp = null;
                _frmHelp.Show();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //close the window without saving or warning about unsaved changes
            btn_apply.Enabled = false;
            Close();
        }

        private void gv_filters_CellBeginEditRule(object sender, DataGridViewCellCancelEventArgs e)
        {
            // the "+" row is not a rule: its cells are never edited
            if (IsAddRow(e.RowIndex))
            {
                e.Cancel = true;
                return;
            }
            MarkUnsaved();
        }

        /// <summary>sets the unsaved changes flag (Apply enabled, Close becomes Cancel)</summary>
        private void MarkUnsaved()
        {
            btn_apply.Enabled = true;
            btn_cancel.Text = Strings.Common_Cancel;
        }

        private void gv_filters_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!gv_filters.IsCurrentCellDirty)
                return;
            btn_apply.Enabled = true;
            btn_cancel.Text = Strings.Common_Cancel;
            // commit checkbox/combobox edits right away so they are not lost when clicking Apply
            if (gv_filters.CurrentCell is DataGridViewCheckBoxCell || gv_filters.CurrentCell is DataGridViewComboBoxCell)
                gv_filters.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void btn_move_up_Click(object sender, EventArgs e)
        {
            MoveSelectedRule(-1);
        }

        private void btn_move_down_Click(object sender, EventArgs e)
        {
            MoveSelectedRule(1);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            DeleteSelectedRules();
        }

        /// <summary>
        /// deletes exactly the selected rules (selected rows, or the rows of the selected cells). The rules are
        /// looked up by object, not by row index; the "+" row is never deleted, with nothing else selected
        /// nothing happens
        /// </summary>
        private void DeleteSelectedRules()
        {
            if (rulesBindingSource == null)
                return;
            gv_filters.EndEdit();
            rulesBindingSource.EndEdit();

            var rows = new HashSet<int>();
            foreach (DataGridViewRow row in gv_filters.SelectedRows)
                rows.Add(row.Index);
            foreach (DataGridViewCell cell in gv_filters.SelectedCells)
                rows.Add(cell.RowIndex);
            var selected = rows.Where(i => i >= 0 && i < RuleCount)
                .OrderBy(i => i)
                .Select(i => rules[i])
                .ToList();
            if (selected.Count == 0)
                return;

            int firstIndex = rules.IndexOf(selected[0]);
            int columnIndex = gv_filters.CurrentCell != null && gv_filters.CurrentCell.ColumnIndex >= 0
                ? gv_filters.CurrentCell.ColumnIndex
                : 0;
            foreach (var rule in selected)
                rules.Remove(rule);

            // select the rule now at the same position (or the last rule; the "+" row if there is none left)
            int newIndex = Math.Max(0, Math.Min(firstIndex, RuleCount - 1));
            if (newIndex < gv_filters.Rows.Count && columnIndex < gv_filters.Columns.Count)
            {
                gv_filters.ClearSelection();
                gv_filters.CurrentCell = gv_filters.Rows[newIndex].Cells[columnIndex];
                if (!IsAddRow(newIndex))
                    gv_filters.Rows[newIndex].Selected = true;
            }

            MarkUnsaved();
        }

        private void MoveSelectedRule(int offset)
        {
            gv_filters.EndEdit();
            if (rulesBindingSource != null)
                rulesBindingSource.EndEdit();

            int rowIndex = gv_filters.CurrentCell == null
                ? -1
                : gv_filters.CurrentCell.RowIndex;
            // the "+" row is never moved and nothing is moved below it
            if (rowIndex < 0 || rowIndex >= RuleCount)
                return;

            int targetIndex = rowIndex + offset;
            if (targetIndex < 0 || targetIndex >= RuleCount)
                return;

            int columnIndex = gv_filters.CurrentCell.ColumnIndex;
            AutoMatchRule selectedRule = rules[rowIndex];

            rules.RaiseListChangedEvents = false;
            rules[rowIndex] = rules[targetIndex];
            rules[targetIndex] = selectedRule;
            rules.RaiseListChangedEvents = true;
            rulesBindingSource.ResetBindings(false);

            gv_filters.ClearSelection();
            if (columnIndex >= gv_filters.Columns.Count)
                columnIndex = 0;
            gv_filters.CurrentCell = gv_filters.Rows[targetIndex].Cells[columnIndex];
            gv_filters.Rows[targetIndex].Selected = true;

            MarkUnsaved();
        }

        private void frm_settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            //close the help window (if it was open)
            _frmHelp?.Close();
        }

        private void btn_check_update_Click(object sender, EventArgs e)
        {
            var btn = ((Button)sender);
            var uc = new UpdateChecker();
            // color the button to indicate request, disable it to prevent multiple instances
            Theme.SetBusy(btn, true);
            btn.Enabled = false;
            // run inside a Task to prevent freezing the UI
            Task.Factory.StartNew(() => uc.check()).ContinueWith(x =>
            {
                try
                {
                    if (uc.Checked)
                    {
                        if (uc.Updated)
                            UpdateInstaller.Offer(this, uc.LVer, uc.CVer);
                        else
                            MessageBox.Show(Strings.Settings_UpToDate);
                    }
                    else
                        MessageBox.Show(Strings.Settings_UpdateFailed);
                    Theme.SetBusy(btn, false);
                    btn.Enabled = true;
                }
                catch (Exception) { }
                return x;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void chk_check_update_CheckedChanged(object sender, EventArgs e)
        {
            if (_populating)
                return;
            Settings.Default.check_update = (((CheckBox)sender).Checked) ? "0" : "nope";
            Settings.Default.Save();
        }

        private void chk_alt_ignore_CheckedChanged(object sender, EventArgs e)
        {
            if (_populating)
                return;
            Settings.Default.AltIgnoresRules = chk_alt_ignore.Checked;
            Settings.Default.Save();
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            if (btn_apply.Enabled)
            {
                var answer = MessageBox.Show(this,
                    Strings.Settings_ExportUnsaved, Strings.Settings_ExportTitle, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (answer == DialogResult.Cancel)
                    return;
                if (answer == DialogResult.Yes)
                    btn_apply_Click(sender, e);
            }
            using (var dialog = new SaveFileDialog
            {
                Title = Strings.Settings_ExportDialogTitle,
                Filter = Strings.Settings_SettingsFileFilter,
                FileName = "BrowserSelect-settings.json"
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                try
                {
                    SettingsTransfer.Export(dialog.FileName);
                    MessageBox.Show(this, L10n.T("Settings_ExportDone", dialog.FileName), Strings.Settings_ExportTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, L10n.T("Settings_ExportFailed", ex.Message), Strings.Settings_ExportTitle, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Title = Strings.Settings_ImportDialogTitle,
                Filter = Strings.Settings_SettingsFileFilter,
                CheckFileExists = true
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                if (MessageBox.Show(this,
                        Strings.Settings_ImportConfirm, Strings.Settings_ImportTitle, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
                try
                {
                    int count = SettingsTransfer.Import(dialog.FileName);
                    ReloadFromSettings();
                    MessageBox.Show(this, L10n.T("Settings_ImportDone", count), Strings.Settings_ImportTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, L10n.T("Settings_ImportFailed", ex.Message), Strings.Settings_ImportTitle, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>reloads every control from the saved settings (used after an import)</summary>
        private void ReloadFromSettings()
        {
            gv_filters.CancelEdit();
            rules.Clear();
            foreach (var rule in Settings.Default.AutoBrowser)
                rules.Add(rule);
            rules.Add(AddRuleRow);
            rulesBindingSource.ResetBindings(false);
            btn_apply.Enabled = false;
            btn_cancel.Text = Strings.Common_Close;

            _populating = true;
            cmb_sort.SelectedItem = BrowserCustomizations.SortModes.Contains(Settings.Default.SortMode)
                ? Settings.Default.SortMode
                : BrowserCustomizations.SortManual;
            chk_running_only.Checked = Settings.Default.ShowRunningOnly;
            chk_alt_ignore.Checked = Settings.Default.AltIgnoresRules;
            chk_check_update.Checked = Settings.Default.check_update != "nope";
            _populating = false;
            PopulateFullscreenOptions();

            RefreshBrowsers(null);
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            PopulateBrowsers(BrowserFinder.find(true), SelectedBrowser?.Identifier);
            this.mainForm.updateBrowsers();
        }

        #region Rule grid: add row ("+")

        /// <summary>number of real rules (the list ends with the "+" row)</summary>
        private int RuleCount
        {
            get { return rules.Count > 0 && ReferenceEquals(rules[rules.Count - 1], AddRuleRow) ? rules.Count - 1 : rules.Count; }
        }

        /// <summary>every rule of the grid, without the "+" row</summary>
        private IEnumerable<AutoMatchRule> RealRules
        {
            get { return rules.Where(r => !ReferenceEquals(r, AddRuleRow)); }
        }

        private bool IsAddRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= gv_filters.Rows.Count)
                return false;
            return ReferenceEquals(gv_filters.Rows[rowIndex].DataBoundItem, AddRuleRow);
        }

        private bool _addRowHover;

        private void SetupAddRow()
        {
            gv_filters.ShowCellToolTips = true;
            gv_filters.CellPainting += gv_filters_CellPainting;
            gv_filters.CellMouseClick += gv_filters_CellMouseClick;
            gv_filters.CellMouseEnter += gv_filters_CellMouseEnter;
            gv_filters.CellMouseLeave += gv_filters_CellMouseLeave;
            gv_filters.CellToolTipTextNeeded += gv_filters_CellToolTipTextNeeded;
            gv_filters.KeyDown += gv_filters_KeyDown;
            gv_filters.DataBindingComplete += (s, e) => ProtectAddRow();
            gv_filters.DataBindingComplete += (s, e) => FitColumnHeaders();
            gv_filters.VisibleChanged += (s, e) => FitColumnHeaders();
            gv_filters.FontChanged += (s, e) => FitColumnHeaders();
            gv_filters.RowsAdded += (s, e) => ProtectAddRow();
            ProtectAddRow();
        }

        /// <summary>
        /// every column is at least as wide as its (translated) header text, e.g. German "Abgleich" or Japanese
        /// "ブラウザー" (the grid sits on a page that is hidden at first, so this is repeated when it is shown)
        /// </summary>
        private void FitColumnHeaders()
        {
            try
            {
                var font = gv_filters.ColumnHeadersDefaultCellStyle.Font ?? gv_filters.Font;
                var padding = (int)Math.Ceiling(14 * Fluent.Scale(gv_filters));
                foreach (DataGridViewColumn column in gv_filters.Columns)
                {
                    if (!column.Visible || string.IsNullOrEmpty(column.HeaderText))
                        continue;
                    var needed = TextRenderer.MeasureText(column.HeaderText, font).Width + padding;
                    if (column.MinimumWidth < needed)
                        column.MinimumWidth = needed;
                    if (column.Width < needed)
                        column.Width = needed;
                }
            }
            catch (Exception) { }
        }

        /// <summary>the "+" row is read-only (also after a re-binding, e.g. Move Up/Down or Import)</summary>
        private void ProtectAddRow()
        {
            try
            {
                for (int i = 0; i < gv_filters.Rows.Count; i++)
                {
                    bool add = IsAddRow(i);
                    if (gv_filters.Rows[i].ReadOnly != add)
                        gv_filters.Rows[i].ReadOnly = add;
                }
            }
            catch (Exception) { }
        }

        /// <summary>inserts an empty rule (Match = Domain) above the "+" row and starts editing its Pattern</summary>
        private void AddRule()
        {
            if (rulesBindingSource == null)
                return;
            gv_filters.EndEdit();
            rulesBindingSource.EndEdit();
            int index = RuleCount;
            rules.Insert(index, new AutoMatchRule());
            ProtectAddRow();
            MarkUnsaved();
            // after the click has been processed by the grid
            BeginInvoke((Action)(() =>
            {
                try
                {
                    if (index >= gv_filters.Rows.Count || IsAddRow(index))
                        return;
                    gv_filters.ClearSelection();
                    gv_filters.CurrentCell = gv_filters.Rows[index].Cells[pattern.Index];
                    gv_filters.Rows[index].Selected = true;
                    gv_filters.Focus();
                    gv_filters.BeginEdit(true);
                }
                catch (Exception) { }
            }));
        }

        private void gv_filters_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && IsAddRow(e.RowIndex))
                AddRule();
        }

        private void gv_filters_KeyDown(object sender, KeyEventArgs e)
        {
            if (gv_filters.IsCurrentCellInEditMode)
                return;
            if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.None)
            {
                // same as the Delete button: exactly the selected rules, never the "+" row
                DeleteSelectedRules();
                e.Handled = true;
            }
            else if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Insert) && e.Modifiers == Keys.None &&
                     gv_filters.CurrentCell != null && IsAddRow(gv_filters.CurrentCell.RowIndex))
            {
                AddRule();
                e.Handled = true;
            }
        }

        private void gv_filters_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (IsAddRow(e.RowIndex))
                e.ToolTipText = Strings.RuleAddRow;
        }

        private void gv_filters_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsAddRow(e.RowIndex))
                return;
            gv_filters.Cursor = Cursors.Hand;
            _addRowHover = true;
            gv_filters.InvalidateCell(-1, e.RowIndex);
        }

        private void gv_filters_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsAddRow(e.RowIndex))
                return;
            gv_filters.Cursor = Cursors.Default;
            _addRowHover = false;
            gv_filters.InvalidateCell(-1, e.RowIndex);
        }

        /// <summary>
        /// the "+" row: blank cells (background and grid lines only) and a "+" in the row header, drawn with the
        /// grid's current (light/dark theme) colors and scaled with the cell size
        /// </summary>
        private void gv_filters_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || !IsAddRow(e.RowIndex))
                return;
            var style = e.CellStyle;
            var back = style.BackColor;
            var fore = style.ForeColor;
            if (e.ColumnIndex < 0 && _addRowHover)
            {
                back = style.SelectionBackColor;
                fore = style.SelectionForeColor;
            }
            using (var brush = new SolidBrush(back))
                e.Graphics.FillRectangle(brush, e.CellBounds);
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);

            if (e.ColumnIndex < 0)
            {
                var r = e.CellBounds;
                float scale = Math.Max(1f, e.Graphics.DpiX / 96f);
                int size = (int)Math.Round(Math.Min(Math.Min(r.Width, r.Height) * 0.55f, 11f * scale));
                if (size < 5)
                    size = 5;
                float thickness = Math.Max(1f, (float)Math.Round(1.5f * scale));
                int cx = r.Left + r.Width / 2;
                int cy = r.Top + r.Height / 2;
                using (var pen = new Pen(fore, thickness))
                {
                    var old = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    e.Graphics.DrawLine(pen, cx - size / 2, cy, cx - size / 2 + size, cy);
                    e.Graphics.DrawLine(pen, cx, cy - size / 2, cx, cy - size / 2 + size);
                    e.Graphics.SmoothingMode = old;
                }
            }
            e.Handled = true;
        }

        #endregion

        private void gv_filters_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // to prevent System.ArgumentException: DataGridViewComboBoxCell value is not valid MessageBoxes
        }
    }
}
