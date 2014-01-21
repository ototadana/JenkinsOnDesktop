using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core
{
    public class Configuration
    {
        public string Butler { get; set; }
        public string Business { get; set; }
        public double DesktopMargin { get; set; }

        public Configuration()
        {
            this.Butler = XPFriend.JenkinsOnDesktop.Core.Butler.EmotionalJenkins;
            this.Business = BusinessesFolder.CheckJobStatus;
            this.DesktopMargin = 50;
        }

        internal static Configuration GetInstance()
        {
            Configuration configuration = WorkspaceFolder.LoadConfiguration();
            WorkspaceFolder.Initialize(configuration);
            return configuration;
        }

        internal void Save()
        {
            WorkspaceFolder.Save(this);
        }
    }
}
