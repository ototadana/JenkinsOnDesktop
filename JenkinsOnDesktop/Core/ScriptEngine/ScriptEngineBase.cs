using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using XPFriend.JenkinsOnDesktop.Properties;

namespace XPFriend.JenkinsOnDesktop.Core.ScriptEngine
{
    internal abstract class ScriptEngineBase
    {
        private PowerShell powerShell = PowerShell.Create();

        protected PowerShell GetPowerShell()
        {
            powerShell.Commands.Clear();
            powerShell.Streams.ClearStreams();
            return powerShell;
        }

        protected void SetWorkingDirectory(PowerShell powerShell, string folder)
        {
            powerShell.Runspace.SessionStateProxy.Path.SetLocation(folder);
        }

        protected T GetObject<T>(PowerShell powerShell, Collection<PSObject> psobjects)
        {
            try
            {
                string error = GetErrorText(powerShell);
                if (error.Length > 0)
                {
                    throw new ApplicationException(error);
                }

                if(psobjects != null && psobjects.Count > 0)
                {
                    object o = psobjects.Select(pso => pso.BaseObject).First();
                    if (typeof(T).IsInstanceOfType(o))
                    {
                        return (T)o;
                    }
                }
            }
            finally
            {
                powerShell.Streams.ClearStreams();
            }

            throw new ApplicationException(
                string.Format(Resources.Error_TheScriptMustReturnObject, typeof(T).Name));
        }

        protected string GetErrorText(PowerShell powerShell)
        {
            if (powerShell.Streams.Error.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            foreach (var error in powerShell.Streams.Error)
            {
                Append(sb, error);
            }
            return sb.ToString();
        }

        protected void Append(StringBuilder sb, ErrorRecord error)
        {
            sb.Append(error).Append(Environment.NewLine);
            sb.Append(error.InvocationInfo.PositionMessage);
            sb.Append(Environment.NewLine);
        }
    }
}
