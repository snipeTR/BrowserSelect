using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrowserSelect.Localization;
using Newtonsoft.Json.Linq;

namespace BrowserSelect
{
    /// <summary>
    /// "Update available" flow: shows the update message, asks whether the new version should be downloaded
    /// and started, downloads the x64 installer of the latest published release into %TEMP% and runs it.
    /// If BrowserSelect is still running, the installer itself asks for permission to close it.
    /// </summary>
    static class UpdateInstaller
    {
        /// <summary>GitHub API: latest published (non-draft, non-prerelease) release</summary>
        public const string LatestReleaseApi = "https://api.github.com/repos/" + UpdateChecker.Repository + "/releases/latest";

        /// <summary>
        /// shows the "update available" message (OK) and then asks whether to download and run the new version.
        /// Must be called on the UI thread.
        /// </summary>
        public static void Offer(IWin32Window owner, string latestVersion, string currentVersion)
        {
            MessageBox.Show(owner, L10n.T("Common_UpdateAvailable", latestVersion, currentVersion, UpdateChecker.ReleasesUrl),
                "BrowserSelect", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var answer = MessageBox.Show(owner, L10n.T("Update_AskDownload", latestVersion), "BrowserSelect",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (answer != DialogResult.Yes)
                return;

            string failure;
            using (var frm = new frm_update_download())
            {
                frm.ShowDialog(owner);
                failure = frm.FailureReason;
            }
            if (failure != null)
                ShowFailure(owner, failure);
        }

        /// <summary>
        /// asks the GitHub API for the latest published release and returns the installer asset
        /// (BrowserSelect-&lt;version&gt;-x64-Setup.exe; any .exe as fallback), or null if the release has none.
        /// </summary>
        internal static async Task<InstallerAsset> FindInstallerAsync()
        {
            using (var wc = CreateWebClient())
            {
                wc.Headers[HttpRequestHeader.Accept] = "application/vnd.github+json";
                var json = JObject.Parse(await wc.DownloadStringTaskAsync(LatestReleaseApi));
                var assets = (json["assets"] as JArray ?? new JArray()).OfType<JObject>()
                    .Select(a => new InstallerAsset
                    {
                        Name = (string)a["name"] ?? "",
                        Url = (string)a["browser_download_url"] ?? "",
                        Size = (long?)a["size"] ?? 0,
                        Tag = (string)json["tag_name"] ?? ""
                    })
                    .Where(a => a.Url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) &&
                                a.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return assets.FirstOrDefault(a => a.Name.EndsWith("-x64-Setup.exe", StringComparison.OrdinalIgnoreCase))
                       ?? assets.FirstOrDefault();
            }
        }

        internal static WebClient CreateWebClient()
        {
            // GitHub only accepts TLS 1.2+ (same setting as UpdateChecker, #43)
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            var wc = new WebClient();
            // the GitHub API rejects requests without a User-Agent
            wc.Headers[HttpRequestHeader.UserAgent] = "BrowserSelect/" + Application.ProductVersion;
            return wc;
        }

        /// <summary>
        /// target path in %TEMP%. A file left over from an earlier download is replaced; if it is locked
        /// (e.g. that installer is still open) a unique name is used instead.
        /// </summary>
        internal static string TempPath(string assetName)
        {
            var name = Path.GetFileName(assetName);
            if (string.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                name = "BrowserSelect-x64-Setup.exe";
            var path = Path.Combine(Path.GetTempPath(), name);
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                return path;
            }
            catch (Exception)
            {
                return Path.Combine(Path.GetTempPath(),
                    Path.GetFileNameWithoutExtension(name) + "-" + DateTime.Now.Ticks + Path.GetExtension(name));
            }
        }

        /// <summary>
        /// shows a localized error and offers to open the releases page in the browser instead
        /// </summary>
        internal static void ShowFailure(IWin32Window owner, string reason)
        {
            var answer = MessageBox.Show(owner, L10n.T("Update_Failed", reason, UpdateChecker.LatestReleaseUrl),
                "BrowserSelect", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (answer != DialogResult.Yes)
                return;
            try
            {
                Process.Start(UpdateChecker.LatestReleaseUrl);
            }
            catch (Exception) { }
        }
    }

    class InstallerAsset
    {
        public string Name;
        public string Url;
        public long Size;
        public string Tag;
    }

    /// <summary>
    /// small progress window: finds the installer, downloads it (non-blocking, cancelable) and starts it
    /// </summary>
    class frm_update_download : Form
    {
        private readonly Label lbl_status;
        private readonly ProgressBar progress;
        private readonly Button btn_cancel;
        private WebClient _client;
        private string _file;
        private bool _cancelled;

        /// <summary>set when the update could not be downloaded/started; shown by the caller after the window closed</summary>
        public string FailureReason { get; private set; }

        public frm_update_download()
        {
            Text = L10n.T("Update_DownloadTitle");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 120);
            try
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (Exception)
            {
                ShowIcon = false;
            }

            lbl_status = new Label
            {
                Location = new Point(12, 12),
                Size = new Size(396, 36),
                AutoEllipsis = true,
                Text = L10n.T("Update_Finding")
            };
            progress = new ProgressBar
            {
                Location = new Point(12, 52),
                Size = new Size(396, 20),
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };
            btn_cancel = new Button
            {
                Location = new Point(308, 84),
                Size = new Size(100, 26),
                Text = L10n.T("Common_Cancel"),
                DialogResult = DialogResult.None
            };
            btn_cancel.Click += (s, e) => Cancel();
            CancelButton = btn_cancel;

            Controls.Add(lbl_status);
            Controls.Add(progress);
            Controls.Add(btn_cancel);

            Shown += async (s, e) => await Run();
            FormClosing += (s, e) =>
            {
                // closing the window (X) while downloading = cancel
                if (_client != null)
                {
                    e.Cancel = true;
                    Cancel();
                }
            };
        }

        private void Cancel()
        {
            _cancelled = true;
            if (_client != null)
                _client.CancelAsync();
            else
                Finish(null);
        }

        private async Task Run()
        {
            UseWaitCursor = true;
            InstallerAsset asset;
            try
            {
                asset = await UpdateInstaller.FindInstallerAsync();
            }
            catch (Exception ex)
            {
                Finish(_cancelled ? null : L10n.T("Update_ErrorNetwork", ex.Message));
                return;
            }
            if (_cancelled || IsDisposed)
            {
                Finish(null);
                return;
            }
            if (asset == null)
            {
                Fail(L10n.T("Update_ErrorNoAsset"));
                return;
            }

            _file = UpdateInstaller.TempPath(asset.Name);
            lbl_status.Text = L10n.T("Update_Downloading", asset.Name);
            progress.Style = ProgressBarStyle.Continuous;
            progress.Value = 0;

            Exception error = null;
            bool cancelled = false;
            _client = UpdateInstaller.CreateWebClient();
            _client.DownloadProgressChanged += (s, e) =>
            {
                var total = e.TotalBytesToReceive > 0 ? e.TotalBytesToReceive : asset.Size;
                if (total > 0)
                    progress.Value = (int)Math.Max(0, Math.Min(100, e.BytesReceived * 100 / total));
                lbl_status.Text = L10n.T("Update_Downloading", asset.Name) +
                                  "\n" + (e.BytesReceived / 1024) + " / " + (total / 1024) + " KB";
            };
            try
            {
                await _client.DownloadFileTaskAsync(new Uri(asset.Url), _file);
            }
            catch (Exception ex)
            {
                var we = ex as WebException;
                cancelled = _cancelled || (we != null && we.Status == WebExceptionStatus.RequestCanceled);
                error = ex;
            }
            finally
            {
                _client.Dispose();
                _client = null;
            }

            if (error != null || cancelled)
            {
                TryDelete(_file);
                Finish(cancelled ? null : L10n.T("Update_ErrorNetwork", error.Message));
                return;
            }

            var info = new FileInfo(_file);
            if (!info.Exists || info.Length == 0 || (asset.Size > 0 && info.Length != asset.Size))
            {
                TryDelete(_file);
                Fail(L10n.T("Update_ErrorIncomplete"));
                return;
            }

            lbl_status.Text = L10n.T("Update_Starting");
            try
            {
                Process.Start(new ProcessStartInfo(_file) { UseShellExecute = true });
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                // ERROR_CANCELLED: the user declined to run it (e.g. a SmartScreen / security prompt)
                Finish(null);
                return;
            }
            catch (Exception ex)
            {
                Fail(L10n.T("Update_ErrorStart", ex.Message));
                return;
            }
            Finish(null);
        }

        private void Fail(string reason)
        {
            Finish(reason);
        }

        /// <summary>closes the window; a non-null reason is reported by UpdateInstaller.Offer afterwards</summary>
        private void Finish(string failureReason)
        {
            if (failureReason != null)
                FailureReason = failureReason;
            if (IsDisposed)
                return;
            UseWaitCursor = false;
            Close();
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception) { }
        }
    }
}
