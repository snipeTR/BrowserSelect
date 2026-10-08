using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        }

        private BindingList<AutoMatchRule> rules = new BindingList<AutoMatchRule>();
        private BindingSource rulesBindingSource;
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

            //populate Rules in the gridview
            foreach (var rule in Settings.Default.AutoBrowser)
                rules.Add(rule);

            //populate list of browsers (browser filter + Rule List ComboBox)
            PopulateBrowsers(BrowserFinder.find());
            rulesBindingSource = new BindingSource();
            rulesBindingSource.DataSource = rules;
            gv_filters.DataSource = rulesBindingSource;

            chk_check_update.Checked = Settings.Default.check_update != "nope";
            chk_alt_ignore.Checked = Settings.Default.AltIgnoresRules;

            // show which application opened the current link, to help writing "Source App" rules
            if (!string.IsNullOrEmpty(Program.SourceApp))
            {
                lbl_source.Text = "Link opened from: " + System.IO.Path.GetFileName(Program.SourceApp);
                toolTip1.SetToolTip(lbl_source, Program.SourceApp);
            }
        }

        private void PopulateBrowsers(List<Browser> browsers)
        {
            var c = ((DataGridViewComboBoxColumn)gv_filters.Columns["browser"]);
            c.Items.Clear();
            browser_filter.Items.Clear();
            foreach (Browser b in browsers)
            {
                browser_filter.Items.Add(b, !Settings.Default.HideBrowsers.Contains(b.Identifier));
                c.Items.Add(b.ToString());
            }
            // add browser select and the "do nothing" option to the list
            c.Items.Add(AutoMatchRule.DisplayBrowserSelect);
            c.Items.Add(AutoMatchRule.IgnoreUrl);
            // keep rules pointing at a browser that is no longer installed displayable/editable
            foreach (var rule in rules)
                if (!string.IsNullOrEmpty(rule.Browser) && !c.Items.Contains(rule.Browser))
                    c.Items.Add(rule.Browser);
        }

        private void btn_setdefault_Click(object sender, EventArgs e)
        {
            // Preserve the current default before asking Windows to handle the association UI.
            DefaultBrowserRegistration.CaptureExistingDefault();
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
                        "Windows could not open Default apps.\n\n" + fallbackException.Message +
                        "\n\nAssociation UI error: " + ex.Message,
                        "Unable to change default browser", MessageBoxButtons.OK,
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
            //Save changes to the BrowserFilter List
            if (e.NewValue == CheckState.Checked)
            {
                Settings.Default.HideBrowsers.Remove(((Browser)browser_filter.Items[e.Index]).Identifier);
            }
            else
            {
                Settings.Default.HideBrowsers.Add(((Browser)browser_filter.Items[e.Index]).Identifier);
            }
            Settings.Default.Save();
        }

        private void frm_settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            // alert user of unsaved changes
            if (btn_apply.Enabled)
            {
                var window = MessageBox.Show("You have unsaved changes, are you sure you want to close without saving ?",
                    "Unsaved Changes", MessageBoxButtons.YesNo);
                if (window == DialogResult.No) e.Cancel = true;
                else e.Cancel = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // open RuleList help
            Process.Start("https://github.com/zumoshi/BrowserSelect/blob/master/help/filters.md");
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            //save rules
            gv_filters.EndEdit();

            //clear rules (instead of checking for changes we just overwrite the whole ruleset)
            Settings.Default.AutoBrowser.Clear();
            foreach (var rule in rules)
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
                        MessageBox.Show("Invalid Rule: " + err);
                    }
                }

            }
            //save rules
            Settings.Default.Save();
            //Enabled property of apply button is used as a flag for unsaved changes
            btn_apply.Enabled = false;
            btn_cancel.Text = "Close";
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

        private void gv_filters_CellBeginEdit(object sender, EventArgs e)
        {
            //set the unsaved changes flag to true
            btn_apply.Enabled = true;
            btn_cancel.Text = "Cancel";
        }

        private void gv_filters_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!gv_filters.IsCurrentCellDirty)
                return;
            btn_apply.Enabled = true;
            btn_cancel.Text = "Cancel";
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
            gv_filters.EndEdit();
            if (rulesBindingSource != null)
                rulesBindingSource.EndEdit();

            int rowIndex = gv_filters.CurrentCell == null
                ? -1
                : gv_filters.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= rules.Count)
                return;

            int columnIndex = gv_filters.CurrentCell.ColumnIndex;
            rules.RemoveAt(rowIndex);

            if (rules.Count > 0)
            {
                int newIndex = Math.Min(rowIndex, rules.Count - 1);
                if (columnIndex >= gv_filters.Columns.Count)
                    columnIndex = 0;
                gv_filters.ClearSelection();
                gv_filters.CurrentCell = gv_filters.Rows[newIndex].Cells[columnIndex];
                gv_filters.Rows[newIndex].Selected = true;
            }

            btn_apply.Enabled = true;
            btn_cancel.Text = "Cancel";
        }

        private void MoveSelectedRule(int offset)
        {
            gv_filters.EndEdit();
            if (rulesBindingSource != null)
                rulesBindingSource.EndEdit();

            int rowIndex = gv_filters.CurrentCell == null
                ? -1
                : gv_filters.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= rules.Count)
                return;

            int targetIndex = rowIndex + offset;
            if (targetIndex < 0 || targetIndex >= rules.Count)
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

            btn_apply.Enabled = true;
            btn_cancel.Text = "Cancel";
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
            btn.BackColor = Color.Blue;
            btn.Enabled = false;
            // run inside a Task to prevent freezing the UI
            Task.Factory.StartNew(() => uc.check()).ContinueWith(x =>
            {
                try
                {
                    if (uc.Checked)
                    {
                        if (uc.Updated)
                            MessageBox.Show(String.Format(
                                "New Update Available!\nCurrent Version: {1}\nLast Version: {0}" +
                                "\nto Update download and install the new version from project's github.",
                                uc.LVer, uc.CVer));
                        else
                            MessageBox.Show("You are running the lastest version.");
                    }
                    else
                        MessageBox.Show("Unable to check for updates.\nPlease make sure you are connected to internet.");
                    btn.UseVisualStyleBackColor = true;
                    btn.Enabled = true;
                }
                catch (Exception) { }
                return x;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void chk_check_update_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.check_update = (((CheckBox)sender).Checked) ? "0" : "nope";
            Settings.Default.Save();
        }

        private void chk_alt_ignore_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AltIgnoresRules = chk_alt_ignore.Checked;
            Settings.Default.Save();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            PopulateBrowsers(BrowserFinder.find(true));
            this.mainForm.updateBrowsers();
        }

        private void gv_filters_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // to prevent System.ArgumentException: DataGridViewComboBoxCell value is not valid MessageBoxes
        }
    }
}
