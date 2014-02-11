using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop.Core.ScriptEngine
{
    internal class BusinessInformationScriptEngine : ScriptEngineBase
    {
        private const string Script =
          @"$ErrorActionPreference = ""SilentlyContinue""
            . .\main.ps1
            $info = @{}
            $help = get-help Main
            $info.Synopsis = $help.Synopsis
            $info.Description = $help.Description[0].Text
            $info.Parameters = &{$help.syntax | Out-String -Width 9999 | foreach {$_.Trim() -split ""Main""} | select -index 1}
            $info.Examples = @()
            foreach($example in $help.examples.example) {
                $text = &{$example | Out-String -Width 9999 | foreach {$_.Trim() -replace "".*Main "","""" -replace ""[`r`n]+"",""`r`n""}}
                $info.Examples += $text
            }
            $info";

        private static BusinessInformationScriptEngine instance = new BusinessInformationScriptEngine();

        internal static BusinessInformation Load(string name)
        {
            BusinessInformation business = new BusinessInformation() { Name = name };
            Hashtable info = BusinessInformationScriptEngine.instance.ExecuteScript(name);
            business.Synopsis = GetAsString(info["Synopsis"]);
            business.Description = GetAsString(info["Description"]);
            business.Examples = GetExamples(GetAsString(info["Parameters"]), (Array)info["Examples"]);
            return business;
        }

        internal Hashtable ExecuteScript(string name)
        {
            string folder = BusinessesFolder.GetFolder(name);
            PowerShell powerShell = base.GetPowerShell();
            powerShell.Runspace.SessionStateProxy.Path.SetLocation(folder);
            powerShell.AddScript(Script);
            Collection<PSObject> results = Invoke(powerShell, null);
            return results.Select(pso => (Hashtable)pso.BaseObject).First();
        }

        private static string GetExamples(string parameters, Array examples)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(parameters).
                Append(Environment.NewLine).Append(Environment.NewLine).Append(Environment.NewLine);

            foreach (object example in examples)
            {
                string s = GetAsString(example);
                sb.Append(s.Trim()).Append(Environment.NewLine).Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private static string GetAsString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else if (obj is PSObject)
            {
                return GetAsString(((PSObject)obj).BaseObject);
            }
            else
            {
                return obj.ToString().Trim();
            }
        }
    }
}
