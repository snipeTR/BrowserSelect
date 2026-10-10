using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrowserSelect.Localization;
using BrowserSelect.UI;

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
            // rounded Windows 11 text fields around the (borderless) text boxes
            host_name.Box = txt_name;
            host_exec.Box = txt_exec;
            host_args.Box = txt_args;
            host_shortcut.Box = txt_shortcut;
            // Windows 10/11 look (fonts, colors, title bar); before the texts so labels are measured with the final font
            Theme.Apply(this);
            ApplyTexts();
            _browser = browser;
            _others = allBrowsers.Where(b => !ReferenceEquals(b, browser)).ToList();
            IsNew = browser == null;
            IsCustom = IsNew || browser.isCustom;

            if (IsNew)
            {
                Text = Strings.BrowserEdit_AddTitle;
            }
            else
            {
                Text = L10n.T("BrowserEdit_EditTitle", browser.name);
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
                toolTip1.SetToolTip(txt_args, Strings.BrowserEdit_ExtraArgumentsTooltip);
            UpdatePreview();
        }

        /// <summary>sets every visible text from Localization\Strings.resx (current UI language)</summary>
        private void ApplyTexts()
        {
            lbl_name.Text = Strings.BrowserEdit_Name;
            lbl_exec.Text = Strings.BrowserEdit_Executable;
            btn_browse.Text = Strings.BrowserEdit_Browse;
            lbl_args.Text = Strings.BrowserEdit_Arguments;
            toolTip1.SetToolTip(txt_args, Strings.BrowserEdit_ArgumentsTooltip);
            lbl_shortcut.Text = Strings.BrowserEdit_Shortcut;
            toolTip1.SetToolTip(txt_shortcut, Strings.BrowserEdit_ShortcutTooltip);
            lbl_shortcut_hint.Text = Strings.BrowserEdit_ShortcutHint;
            lbl_icon.Text = Strings.BrowserEdit_Icon;
            btn_icon.Text = Strings.BrowserEdit_ChangeIcon;
            btn_icon_reset.Text = Strings.BrowserEdit_DefaultIcon;
            btn_ok.Text = Strings.Common_OK;
            btn_cancel.Text = Strings.Common_Cancel;
        }

        private void frm_browser_edit_Load(object sender, EventArgs e)
        {
            LayoutWindow();
        }

        /// <summary>
        /// Windows 11 layout (v1.5.8.0), done in code for the final (DPI scaled) fonts and the translated labels:
        /// one card with a row per field (label column as wide as the longest label), OK (accent) and Cancel
        /// at the bottom right
        /// </summary>
        private void LayoutWindow()
        {
            try
            {
                SuspendLayout();
                var s = Fluent.Scale(this);
                Func<float, int> px = v => FluentLayout.Px(v, s);
                int margin = px(16), pad = px(16), rowGap = px(8), fieldWidth = px(340);
                var buttonHeight = px(30);
                var labels = new Label[] { lbl_name, lbl_exec, lbl_args, lbl_shortcut, lbl_icon };
                var labelWidth = 0;
                foreach (var l in labels)
                    labelWidth = Math.Max(labelWidth, l.PreferredWidth);
                var fieldX = pad + labelWidth + px(12);
                var rowHeight = FluentTextBoxHost.PreferredHeightFor(txt_name, s);
                Action<Label, int, int> placeLabel = (l, top, height) => l.Location = new Point(pad, top + (height - l.Height) / 2);

                var y = pad;
                host_name.SetBounds(fieldX, y, fieldWidth, rowHeight);
                placeLabel(lbl_name, y, rowHeight);
                y += rowHeight + rowGap;

                btn_browse.Size = new Size(FluentLayout.ButtonWidth(btn_browse, px(88), s), rowHeight);
                host_exec.SetBounds(fieldX, y, fieldWidth - btn_browse.Width - px(8), rowHeight);
                btn_browse.Location = new Point(fieldX + fieldWidth - btn_browse.Width, y);
                placeLabel(lbl_exec, y, rowHeight);
                y += rowHeight + rowGap;

                host_args.SetBounds(fieldX, y, fieldWidth, rowHeight);
                placeLabel(lbl_args, y, rowHeight);
                y += rowHeight + rowGap;

                host_shortcut.SetBounds(fieldX, y, px(72), rowHeight);
                var hintX = host_shortcut.Right + px(12);
                FluentLayout.Wrap(lbl_shortcut_hint, hintX, y, fieldX + fieldWidth - hintX);
                var shortcutRow = Math.Max(rowHeight, lbl_shortcut_hint.Height);
                host_shortcut.Top = y + (shortcutRow - rowHeight) / 2;
                lbl_shortcut_hint.Top = y + (shortcutRow - lbl_shortcut_hint.Height) / 2;
                placeLabel(lbl_shortcut, y, shortcutRow);
                y += shortcutRow + rowGap;

                // icon preview in a rounded frame, Change... and Default next to it
                var iconButtons = FluentLayout.ButtonWidth(btn_icon, px(96), s, btn_icon.Text, btn_icon_reset.Text);
                var iconSize = 2 * buttonHeight + px(6);
                host_icon.SetBounds(fieldX, y, iconSize, iconSize);
                btn_icon.SetBounds(host_icon.Right + px(12), y, iconButtons, buttonHeight);
                btn_icon_reset.SetBounds(btn_icon.Left, y + buttonHeight + px(6), iconButtons, buttonHeight);
                placeLabel(lbl_icon, y, buttonHeight);
                y = host_icon.Bottom + pad;

                var cardWidth = fieldX + fieldWidth + pad;
                card_fields.SetBounds(margin, margin, cardWidth, y);

                var buttonWidth = FluentLayout.ButtonWidth(btn_ok, px(92), s, btn_ok.Text, btn_cancel.Text);
                var by = card_fields.Bottom + px(16);
                btn_cancel.SetBounds(card_fields.Right - buttonWidth, by, buttonWidth, buttonHeight);
                btn_ok.SetBounds(btn_cancel.Left - px(8) - buttonWidth, by, buttonWidth, buttonHeight);
                ClientSize = new Size(card_fields.Right + margin, btn_ok.Bottom + margin);
                ResumeLayout(true);
            }
            catch (Exception)
            {
                ResumeLayout(true);
            }
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
                Title = Strings.BrowserEdit_SelectExecutable,
                Filter = Strings.BrowserEdit_ProgramsFilter,
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
                Title = Strings.BrowserEdit_SelectIcon,
                Filter = Strings.BrowserEdit_IconFilter,
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
                    MessageBox.Show(this, L10n.T("BrowserEdit_IconError", ex.Message),
                        Strings.BrowserEdit_IconErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                return Strings.BrowserEdit_NameRequired;
            if (BrowserName == AutoMatchRule.DisplayBrowserSelect || BrowserName == AutoMatchRule.IgnoreUrl)
                return Strings.BrowserEdit_NameReserved;
            if (_others.Any(b => string.Equals(b.name, BrowserName, StringComparison.OrdinalIgnoreCase)))
                return L10n.T("BrowserEdit_NameExists", BrowserName);
            if (Executable.Length == 0 || !File.Exists(Executable))
                return Strings.BrowserEdit_ExecutableRequired;
            var identifier = $"{Executable} {Arguments}";
            if (_others.Any(b => string.Equals(b.Identifier, identifier, StringComparison.OrdinalIgnoreCase)))
                return Strings.BrowserEdit_AlreadyListed;
            return null;
        }
    }
}
