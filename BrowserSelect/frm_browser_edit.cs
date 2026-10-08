using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrowserSelect
{
    /// <summary>
    /// Dialog used to add a portable browser (any executable chosen with a browse button) or to
    /// customize an existing browser's icon, keyboard shortcut and extra arguments.
    /// </summary>
    public partial class frm_browser_edit : Form
    {
        private readonly Browser _browser;
        private readonly List<Browser> _others;
        private string _defaultIcon;
        private string _customIcon;

        /// <summary>true if a new (portable) browser is being added</summary>
        public bool IsNew { get; }
        /// <summary>true if name, executable and arguments of this browser are editable</summary>
        public bool IsCustom { get; }

        public string BrowserName => txt_name.Text.Trim();
        public string Executable => txt_exec.Text.Trim().Trim('"');
        public string Arguments => txt_args.Text.Trim();
        public string Shortcut => txt_shortcut.Text.Trim();
        /// <summary>custom icon as base64 PNG, null to use the default icon</summary>
        public string CustomIcon => _customIcon;
        /// <summary>default icon of the selected executable (base64 PNG)</summary>
        public string DefaultIcon => _defaultIcon;

        /// <param name="browser">browser to edit, or null to add a new portable browser</param>
        /// <param name="allBrowsers">all known browsers, used to keep names unique</param>
        public frm_browser_edit(Browser browser, IEnumerable<Browser> allBrowsers)
        {
            InitializeComponent();
            _browser = browser;
            _others = allBrowsers.Where(b => !ReferenceEquals(b, browser)).ToList();
            IsNew = browser == null;
            IsCustom = IsNew || browser.isCustom;

            if (IsNew)
            {
                Text = "Add Browser";
            }
            else
            {
                Text = "Edit Browser - " + browser.name;
                txt_name.Text = browser.name;
                txt_exec.Text = browser.exec;
                txt_args.Text = IsCustom ? browser.additionalArgs : browser.extraArgs;
                txt_shortcut.Text = browser.customShortcut;
                _defaultIcon = browser.defaultIcon ?? browser.icon;
                _customIcon = BrowserCustomizations.GetOverride(browser.Identifier).icon;
            }

            // detected browsers keep their name/executable (rules and the registry refer to them)
            txt_name.ReadOnly = !IsCustom;
            txt_exec.ReadOnly = !IsCustom;
            btn_browse.Enabled = IsCustom;
            if (!IsCustom)
                toolTip1.SetToolTip(txt_args, "Extra command line arguments always passed to this browser");
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            var icon = _customIcon ?? _defaultIcon;
            var old = pic_icon.Image;
            pic_icon.Image = string.IsNullOrEmpty(icon) ? null : new Browser { icon = icon }.string2Icon();
            old?.Dispose();
            btn_icon_reset.Enabled = _customIcon != null;
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Title = "Select the browser executable",
                Filter = "Programs (*.exe)|*.exe|All files (*.*)|*.*",
                CheckFileExists = true
            })
            {
                if (File.Exists(Executable))
                    dialog.InitialDirectory = Path.GetDirectoryName(Executable);
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                txt_exec.Text = dialog.FileName;
                if (string.IsNullOrWhiteSpace(txt_name.Text))
                    txt_name.Text = SuggestName(dialog.FileName);
                _defaultIcon = BrowserCustomizations.IconFromExecutable(dialog.FileName);
                UpdatePreview();
            }
        }

        private static string SuggestName(string path)
        {
            try
            {
                var info = FileVersionInfo.GetVersionInfo(path);
                if (!string.IsNullOrWhiteSpace(info.ProductName))
                    return info.ProductName.Trim();
                if (!string.IsNullOrWhiteSpace(info.FileDescription))
                    return info.FileDescription.Trim();
            }
            catch
            {
                // fall back to the file name
            }
            return Path.GetFileNameWithoutExtension(path);
        }

        private void btn_icon_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Title = "Select an icon",
                Filter = "Icons and images|*.ico;*.exe;*.dll;*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*",
                CheckFileExists = true
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                try
                {
                    _customIcon = BrowserCustomizations.IconFromFile(dialog.FileName);
                    UpdatePreview();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Unable to load an icon from this file.\n\n" + ex.Message,
                        "Custom icon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btn_icon_reset_Click(object sender, EventArgs e)
        {
            _customIcon = null;
            UpdatePreview();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            string error = Validate_();
            if (error != null)
            {
                MessageBox.Show(this, error, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (IsCustom && (_defaultIcon == null || (_browser != null &&
                    !string.Equals(_browser.exec, Executable, StringComparison.OrdinalIgnoreCase))))
                _defaultIcon = BrowserCustomizations.IconFromExecutable(Executable);
            DialogResult = DialogResult.OK;
            Close();
        }

        private string Validate_()
        {
            if (!IsCustom)
                return null;
            if (BrowserName.Length == 0)
                return "Please enter a name for the browser.";
            if (BrowserName == AutoMatchRule.DisplayBrowserSelect || BrowserName == AutoMatchRule.IgnoreUrl)
                return "This name is reserved, please choose another one.";
            if (_others.Any(b => string.Equals(b.name, BrowserName, StringComparison.OrdinalIgnoreCase)))
                return "A browser named '" + BrowserName + "' already exists, please choose another name.";
            if (Executable.Length == 0 || !File.Exists(Executable))
                return "Please select an existing executable (use Browse...).";
            var identifier = $"{Executable} {Arguments}";
            if (_others.Any(b => string.Equals(b.Identifier, identifier, StringComparison.OrdinalIgnoreCase)))
                return "This browser (same executable and arguments) is already in the list.";
            return null;
        }
    }
}
