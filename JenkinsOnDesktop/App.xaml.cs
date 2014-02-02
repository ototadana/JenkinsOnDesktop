using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using XPFriend.JenkinsOnDesktop.Core;
using XPFriend.JenkinsOnDesktop.Core.Folder;

namespace XPFriend.JenkinsOnDesktop
{
    public partial class App : Application
    {
        private static App instance;

        internal static App Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = App.Current as App;
                }
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        internal void Application_DispatcherUnhandledException(
            object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorLogFile = GetErrorLogFile();
                WriteErrorLog(e.Exception, errorLogFile);
                MainWindow window = XPFriend.JenkinsOnDesktop.MainWindow.Current;
                Workspace workspace = Workspace.Current;
                if (workspace.Butler == null)
                {
                    workspace.Butler = ButlerFactory.CreateEmotionalJenkins();
                    window.ContextMenu = new ContextMenu();
                    window.ContextMenu.Items.Add(CreateQuitMenuItem(window));
                }
                workspace.Butler.SetErrorMessage(GetMessage(e.Exception), errorLogFile);
                window.Visibility = Visibility.Hidden;
                window.Left = 50;
                window.Top = 50;
                window.UpdateWindow();
                window.ShowMessage();
                window.Visibility = Visibility.Visible;
                window.Activate();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    GetMessage(e.Exception),
                    "",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                App.Instance.Shutdown();
            }
            finally
            {
                e.Handled = true;
            }
        }

        private MenuItem CreateQuitMenuItem(MainWindow window)
        {
            MenuItem quitMenuItem = new MenuItem()
            {
                Header = XPFriend.JenkinsOnDesktop.Properties.Resources.NotifyIcon_Quit
            };
            quitMenuItem.Click += (sn, ev) => window.Quit();
            return quitMenuItem;
        }

        private string GetErrorLogFile()
        {
            string errorLogFolder = Path.Combine(WorkspaceFolder.FullName, "Logs");
            if (!Directory.Exists(errorLogFolder))
            {
                Directory.CreateDirectory(errorLogFolder);
            }
            return Path.Combine(
                errorLogFolder,
                "error" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + ".txt");
        }

        private static string GetMessage(Exception e)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(e.Message);

            Exception innerException = null;
            while ((innerException = e.InnerException) != null && innerException != e)
            {
                stack.Push(innerException.Message);
                e = innerException;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(stack.Pop());
            while (stack.Count > 0)
            {
                sb.Append(Environment.NewLine).Append("  <-- ");
                sb.Append(stack.Pop());
            }
            return sb.ToString();
        }

        private static void WriteErrorLog(Exception e, string errorLogFile)
        {
            try
            {
                File.WriteAllText(errorLogFile,
                    errorLogFile + Environment.NewLine +
                    "---" + Environment.NewLine +
                    e.ToString());
            }
            catch (Exception)
            {
            }
        }
    }
}
