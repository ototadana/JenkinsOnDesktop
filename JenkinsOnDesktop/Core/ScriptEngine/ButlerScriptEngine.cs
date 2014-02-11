using System.Collections.ObjectModel;
using System.Management.Automation;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core.ScriptEngine
{
    internal class ButlerScriptEngine : ScriptEngineBase
    {
        private static ButlerScriptEngine instance = new ButlerScriptEngine();

        internal static string Feel(string name, Report report)
        {
            return ButlerScriptEngine.instance.ExecuteScript(name, report);
        }

        internal string ExecuteScript(string name, Report report)
        {
            PowerShell powerShell = base.GetPowerShell();
            string folder = ButlersFolder.GetFolder(name);
            SetWorkingDirectory(powerShell, folder);
            powerShell.AddScript(@"foreach($i in $input) {$report = $i};. .\feel.ps1;Main");
            Collection<PSObject> results = Invoke(powerShell, new[] { report.Hashtable });
            return GetObject<string>(powerShell, results);
        }
    }
}
