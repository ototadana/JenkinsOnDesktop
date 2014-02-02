using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Xml;
using XPFriend.JenkinsOnDesktop.Properties;

namespace XPFriend.JenkinsOnDesktop.Core.Folder
{
    internal class WorkspaceFolder
    {
        private const string ConfigurationFile = "Config.xml";
        private static string applicationPath;

        internal static ProcessWrapper processWrapper = new ProcessWrapper();

        internal static string FullName
        {
            get { return WorkspaceFolder.applicationPath; }
        }

        internal static bool HasConfigurationFile
        {
            get 
            { 
                return File.Exists(Path.Combine(
                    WorkspaceFolder.FullName, WorkspaceFolder.ConfigurationFile)); 
            }
        }

        static WorkspaceFolder()
        {
            WorkspaceFolder.applicationPath = GetApplicationPath();
        }

        private static string GetApplicationPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationPath = Path.Combine(appData, "XPFriend\\JenkinsOnDesktop");
            if (!File.Exists(applicationPath))
            {
                Directory.CreateDirectory(applicationPath);
            }
            return applicationPath;
        }

        internal static void SetApplicationPath(string path)
        {
            WorkspaceFolder.applicationPath = path;
        }

        internal static void SaveScript(byte[] bytes, string folder, string file)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, file);
            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, bytes);
            }
        }

        internal static void Save(Configuration configuration)
        {
            SaveObject(configuration, Path.Combine(FullName, ConfigurationFile), true);
        }

        internal static void SaveObject(object targetObject, string file, bool overwrite)
        {
            if (!overwrite && File.Exists(file))
            {
                return;
            }

            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                XamlWriter.Save(targetObject, writer);
                fs.Close();
            }
        }

        internal static void Initialize(Configuration configuration)
        {
            ButlersFolder.Initialize(configuration.Butler);
            BusinessesFolder.Initialize(configuration.Business);
        }

        internal static Configuration LoadConfiguration()
        {
            if (!Directory.Exists(FullName))
            {
                Directory.CreateDirectory(FullName);
            }

            if (HasConfigurationFile)
            {
                return (Configuration)LoadObject(Path.Combine(FullName, ConfigurationFile));
            }
            else
            {
                return new Configuration();
            }
        }

        internal static object LoadObject(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                object targetObject = XamlReader.Load(fs);
                fs.Close();
                return targetObject;
            }
        }

        internal static IEnumerable<string> GetSubFolder(string folder)
        {
            foreach (string directory in Directory.GetDirectories(folder))
            {
                yield return Path.GetFileName(directory);
            }
        }

        internal static void ValidateAsFileName(string name)
        {
            char ch = Path.GetInvalidFileNameChars().FirstOrDefault(c => name.Contains(c));
            if (ch != '\0')
            {
                throw new ApplicationException(
                    string.Format(Resources.Error_InvalidCharacter, ch, name));
            }
        }

        internal static void Open(string path)
        {
            processWrapper.Start(path);
        }
    }
}
