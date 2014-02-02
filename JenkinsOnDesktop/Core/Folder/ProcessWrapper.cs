using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace XPFriend.JenkinsOnDesktop.Core.Folder
{
    internal class ProcessWrapper
    {
        internal virtual void Start(string fileName)
        {
            Process.Start(fileName);
        }
    }
}
