using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core.ScriptEngine
{
    internal class BusinessScriptEngine : ScriptEngineBase
    {
        private static BusinessScriptEngine instance = new BusinessScriptEngine();

        internal static Hashtable Execute(string businessName, string parameters, Hashtable report)
        {
            return BusinessScriptEngine.instance.ExecuteScript(businessName, parameters, report);
        }

        private Hashtable ExecuteScript(string businessName, string parameters, Hashtable report)
        {
            PowerShell powerShell = base.GetPowerShell();
            SetWorkingDirectory(powerShell, BusinessesFolder.GetFolder(businessName));
            powerShell.AddScript(@"foreach($i in $input) {$report = $i};. .\main.ps1;Main " + parameters);
            try
            {
                Collection<PSObject> results = powerShell.Invoke(new[] { report });
                return GetObject<Hashtable>(powerShell, results);
            }
            catch (RuntimeException e)
            {
                StringBuilder sb = new StringBuilder(GetErrorText(powerShell));
                Append(sb, e.ErrorRecord);
                throw new ApplicationException(sb.ToString());
            }
        }
    }
}
