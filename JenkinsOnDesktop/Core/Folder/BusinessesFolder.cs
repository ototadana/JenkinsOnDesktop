using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using XPFriend.JenkinsOnDesktop.Properties;

namespace XPFriend.JenkinsOnDesktop.Core.Folder
{
    internal class BusinessesFolder
    {
        internal const string CheckJobStatus = "Check-job-status";
        internal const string TimeKeeping = "Time-keeping";

        private static string FullName { get { return Path.Combine(WorkspaceFolder.FullName, "Businesses"); } }

        internal static void Initialize(string businessName)
        {
            SetupFolder(CheckJobStatus, Resources.CJS_main, Resources.CJS_main_ja_JP);
            SetupFolder(TimeKeeping, Resources.TK_main, Resources.TK_main_ja_JP);
            SetupFolderIfNotExists(businessName);
        }

        internal static void SetupFolderIfNotExists(string name)
        {
            if (!Directory.Exists(GetFolder(name)))
            {
                SetupFolder(name, Resources.TK_main, Resources.TK_main_ja_JP);
            }
        }

        internal static string GetFolder(string name)
        {
            return Path.Combine(FullName, name);
        }

        private static void SetupFolder(string name, byte[] ps, byte[] psd)
        {
            WorkspaceFolder.ValidateAsFileName(name);
            string folder = GetFolder(name);
            WorkspaceFolder.SaveScriptIfNotExists(ps, folder, "main.ps1");
            WorkspaceFolder.SaveScriptIfNotExists(psd, Path.Combine(folder, "ja-JP"), "main.psd1");
        }

        internal static IEnumerable<string> GetFolderNames()
        {
            return WorkspaceFolder.GetSubFolder(FullName);
        }

        internal static void Save(string name, Business business)
        {
            SetupFolderIfNotExists(name);
            WorkspaceFolder.SaveObject(business, GetSettingFile(name), true);
        }

        internal static Business Load(string name)
        {
            string file = GetSettingFile(name);
            if (File.Exists(file))
            {
                return (Business)WorkspaceFolder.LoadObject(file);
            }
            else
            {
                return new Business();
            }
        }

        private static string GetSettingFile(string name)
        {
            return Path.Combine(GetFolder(name), "Config.xml");
        }

        internal static void Open(string name)
        {
            SetupFolderIfNotExists(name);
            WorkspaceFolder.Open(GetFolder(name));
        }
    }
}
