using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XPFriend.JenkinsOnDesktop.Properties;
using XPFriend.JenkinsOnDesktop.Util;

namespace XPFriend.JenkinsOnDesktop.Core.Folder
{
    internal class ButlersFolder
    {
        private static string FullName { get { return Path.Combine(WorkspaceFolder.FullName, "Butlers"); } }

        internal static void Initialize(string name)
        {
            SetupFolder(ButlerFactory.CreateCalmJenkins(), Resources.CalmJ_feel);
            SetupFolder(ButlerFactory.CreateEmotionalJenkins(), Resources.EmotionalJ_feel);
            SetupFolderIfNotExists(name);
        }

        private static void SetupFolder(Butler butler, byte[] feelScript)
        {
            string name = butler.Name;
            WorkspaceFolder.ValidateAsFileName(name);
            string butlerFolder = GetFolder(name);
            CreateFolder(butlerFolder);
            SaveResources(butlerFolder, butler, feelScript);
            WorkspaceFolder.SaveObject(butler, GetSettingFile(name), false);
        }

        private static void CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        internal static string GetFolder(string name)
        {
            return Path.Combine(FullName, name);
        }

        private static string GetSettingFile(string name)
        {
            return Path.Combine(GetFolder(name), "Butler.xml");
        }

        private static void SaveResources(string butlerFolder, Butler butler, byte[] feelScript)
        {
            foreach (Appearance appearance in butler.Appearances.Values)
            {
                SaveIcon(appearance, butlerFolder);
                SaveImage(appearance, butlerFolder);
            }
            WorkspaceFolder.SaveScriptIfNotExists(feelScript, butlerFolder, "feel.ps1");
        }

        private static void SaveIcon(Appearance appearance, string butlerFolder)
        {
            if (appearance != null)
            {
                string path = Path.Combine(butlerFolder, appearance.IconFile);
                if (!File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        Resources.jenkins_icon.Save(fs);
                        fs.Close();
                    }
                }
            }
        }

        private static void SaveImage(Appearance appearance, string butlerFolder)
        {
            if (appearance != null)
            {
                string path = Path.Combine(butlerFolder, appearance.ImageFile);
                if (!File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        appearance.Bitmap.Save(fs, appearance.Bitmap.RawFormat);
                        fs.Close();
                    }
                }
            }
        }

        internal static Butler Load(string name)
        {
            Butler butler;
            if (Directory.Exists(GetFolder(name)))
            {
                string butlerSettingFile = GetSettingFile(name);
                butler = LoadButler(name, butlerSettingFile);
            }
            else
            {
                butler = ButlerFactory.CreateDefaultButler(name, name, name);
            }

            InitializeAppearances(butler, GetFolder(name));
            return butler;
        }

        private static void InitializeAppearances(Butler butler, string butlerFolder)
        {
            foreach (Appearance appearance in butler.Appearances.Values)
            {
                LoadImages(appearance, butlerFolder, Resources.jenkins);
            }
            butler.SetDefaultAppearance();
        }

        private static Butler LoadButler(string name, string butlerSettingFile)
        {
            return (Butler)WorkspaceFolder.LoadObject(butlerSettingFile);
        }

        internal static void SetupFolderIfNotExists(string name) 
        {
            if(!Directory.Exists(GetFolder(name)))
            {
                SetupFolder(ButlerFactory.CreateDefaultButler(name, name, name), Resources.EmotionalJ_feel);
            }
        }

        private static void LoadImages(Appearance appearance, string butlerFolder, Bitmap defaultImage)
        {
            if (appearance == null)
            {
                return;
            }

            try
            {
                appearance.Icon = new Icon(Path.Combine(butlerFolder, appearance.IconFile));
            }
            catch (Exception)
            {
                appearance.Icon = Resources.jenkins_icon;
            }

            try
            {
                string file = Path.Combine(butlerFolder, appearance.ImageFile);
                appearance.Image = BitmapUtil.ToBitmapSource(LoadImage(file));
            }
            catch (Exception)
            {
                appearance.Image = BitmapUtil.ToBitmapSource(defaultImage);
            }

            if (appearance.MessageStyle.BackgroundFile != null)
            {
                LoadImage(appearance.MessageStyle, butlerFolder);
            }
        }

        private static Bitmap LoadImage(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                Bitmap image = (Bitmap)Bitmap.FromStream(fs);
                fs.Close();
                return image;
            }
        }

        private static void LoadImage(MessageStyle style, string butlerFolder)
        {
            string file = Path.Combine(butlerFolder, style.BackgroundFile);
            BitmapSource image = BitmapUtil.ToBitmapSource(LoadImage(file));
            style.Background = new ImageBrush(image);

            if (style.Width == 0)
            {
                style.Width = image.Width;
            }

            if (style.Height == 0)
            {
                style.Height = image.Height;
            }
        }

        internal static IEnumerable<string> GetFolderNames()
        {
            return WorkspaceFolder.GetSubFolder(FullName);
        }

        internal static void Open(string name)
        {
            SetupFolderIfNotExists(name);
            WorkspaceFolder.Open(GetFolder(name));
        }
    }
}
