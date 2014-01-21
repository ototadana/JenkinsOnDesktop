
using System.IO;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using XPFriend.JenkinsOnDesktop.Core.ScriptEngine;

namespace XPFriend.JenkinsOnDesktop.Core
{
    internal class BusinessInformation
    {
        internal string Name { get; set; }
        internal string Synopsis { get; set; }
        internal string Description { get; set; }
        internal string Examples { get; set; }

        internal static BusinessInformation GetInstance(string name)
        {
            if (Directory.Exists(BusinessesFolder.GetFolder(name)))
            {
                return BusinessInformationScriptEngine.Load(name);
            }
            else
            {
                return new BusinessInformation() { Name = name };
            }
        }
    }
}
