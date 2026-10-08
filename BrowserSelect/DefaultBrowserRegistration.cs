using System;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Forms;
using BrowserSelect.Properties;

namespace BrowserSelect
{
    /// <summary>Reads the browser choice before BrowserSelect is registered as the handler.</summary>
    internal static class DefaultBrowserRegistration
    {
        private const string CapabilitiesPath =
            @"Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities";

        public static void EnsureApplicationRegistered()
        {
            string executable = Application.ExecutablePath;
            using (RegistryKey client = Registry.CurrentUser.CreateSubKey(
                @"Software\Clients\StartMenuInternet\BROWSERSELECT.EXE"))
            {
                client.SetValue(null, "BrowserSelect");
            }

            using (RegistryKey capabilities = Registry.CurrentUser.CreateSubKey(CapabilitiesPath))
            {
                capabilities.SetValue("ApplicationName", "BrowserSelect");
                capabilities.SetValue("ApplicationDescription", "Choose a browser dynamically.");
                capabilities.SetValue("ApplicationIcon", executable + ",0");
            }

            using (RegistryKey startMenu = Registry.CurrentUser.CreateSubKey(
                CapabilitiesPath + @"\StartMenu"))
            {
                startMenu.SetValue("StartMenuInternet", "BROWSERSELECT.EXE");
            }

            using (RegistryKey associations = Registry.CurrentUser.CreateSubKey(
                CapabilitiesPath + @"\URLAssociations"))
            {
                associations.SetValue("http", "bselectURL");
                associations.SetValue("https", "bselectURL");
            }

            using (RegistryKey registeredApps = Registry.CurrentUser.CreateSubKey(
                @"Software\RegisteredApplications"))
            {
                registeredApps.SetValue("BrowserSelect", CapabilitiesPath);
            }

            using (RegistryKey handler = Registry.CurrentUser.CreateSubKey(
                @"Software\Classes\bselectURL"))
            {
                handler.SetValue(null, "BrowserSelect URL");
                handler.SetValue("URL Protocol", "");
            }
            using (RegistryKey command = Registry.CurrentUser.CreateSubKey(
                @"Software\Classes\bselectURL\shell\open\command"))
            {
                command.SetValue(null, "\"" + executable + "\" \"%1\"");
            }
        }

        private const string AssociationPath =
            @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\";

        public static void CaptureExistingDefault()
        {
            if (!string.IsNullOrEmpty(Settings.Default.DefaultBrowser))
                return;

            string progId = ReadProgId("http") ?? ReadProgId("https");
            string name = ResolveBrowserName(progId);
            if (string.IsNullOrEmpty(name))
                return;

            Settings.Default.DefaultBrowser = name;
            Settings.Default.Save();
        }

        private static string ReadProgId(string scheme)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                AssociationPath + scheme + @"\UserChoice"))
            {
                return key == null ? null : key.GetValue("ProgId") as string;
            }
        }

        private static string ResolveBrowserName(string progId)
        {
            if (string.IsNullOrEmpty(progId))
                return null;

            // BrowserSelect discovers registered browsers through this same capabilities
            // registration, which gives us a stable mapping from ProgId to display name.
            using (RegistryKey clients = Registry.CurrentUser.OpenSubKey(
                @"Software\Clients\StartMenuInternet"))
            {
                if (clients != null)
                {
                    foreach (string clientName in clients.GetSubKeyNames())
                    {
                        using (RegistryKey associations = clients.OpenSubKey(
                            clientName + @"\Capabilities\URLAssociations"))
                        {
                            string http = associations == null ? null :
                                associations.GetValue("http") as string;
                            string https = associations == null ? null :
                                associations.GetValue("https") as string;
                            if (string.Equals(progId, http, StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(progId, https, StringComparison.OrdinalIgnoreCase))
                            {
                                using (RegistryKey client = clients.OpenSubKey(clientName))
                                {
                                    string registeredName = client == null ? null :
                                        client.GetValue(null) as string;
                                    Browser browser = BrowserFinder.find().FirstOrDefault(
                                        b => string.Equals(b.name, registeredName,
                                            StringComparison.OrdinalIgnoreCase));
                                    if (browser != null)
                                        return browser.name;
                                    if (!string.IsNullOrEmpty(registeredName))
                                        return registeredName;
                                }
                            }
                        }
                    }
                }
            }

            // Fallback for ProgIds registered only under Classes (including custom browser
            // registrations that don't appear in StartMenuInternet).
            using (RegistryKey application = Registry.ClassesRoot.OpenSubKey(
                progId + @"\Application"))
            {
                string appName = application == null ? null :
                    application.GetValue("ApplicationName") as string;
                if (!string.IsNullOrEmpty(appName))
                    return appName;
            }
            return progId;
        }
    }
}
