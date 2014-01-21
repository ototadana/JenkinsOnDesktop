using System.Collections;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using XPFriend.JenkinsOnDesktop.Core.ScriptEngine;

namespace XPFriend.JenkinsOnDesktop.Core
{
    public class Business
    {
        internal string Name { get; set; }
        public double TimerInterval { get; set; }
        public string Parameters { get; set; }

        public Business()
        {
            this.TimerInterval = 30;
        }

        internal static Business GetInstance(string name)
        {
            Business business = BusinessesFolder.Load(name);
            business.Name = name;
            return business;
        }

        internal void Save(string name)
        {
            BusinessesFolder.Save(name, this);
        }

        internal Hashtable CreateReport(Hashtable report)
        {
            return BusinessScriptEngine.Execute(Name, Parameters, report);
        }
    }
}
