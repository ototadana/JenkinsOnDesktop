using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core
{
    internal class Workspace
    {
        private static Workspace current = new Workspace();
        private Report report = new Report();

        internal static Workspace Current { get { return current; } }
        internal Configuration Configuration { get; set; }
        internal Butler Butler { get; set; }
        internal Business Business { get; set; }
        internal Report Report { get { return this.report; } }
        internal bool HasConfigurationFile { get { return WorkspaceFolder.HasConfigurationFile; } }

        private Workspace()
        {
        }

        internal void Initialize()
        {
            this.report = new Report();
            this.Configuration = Configuration.GetInstance();
            this.Butler = Butler.GetInstance(this.Configuration.Butler, true);
            this.Business = Business.GetInstance(this.Configuration.Business);
        }

        internal void SaveConfigurations()
        {
            this.Business.Save(this.Configuration.Business);
            this.Configuration.Save();
        }

        internal void DoBusiness()
        {
            this.report.Hashtable = this.Business.CreateReport(this.report.Hashtable);
            this.Butler.ReadReport(this.report);
        }
    }
}
