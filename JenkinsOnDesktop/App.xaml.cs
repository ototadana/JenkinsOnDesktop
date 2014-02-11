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
        internal void Application_DispatcherUnhandledException(
            object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                HandleException(e.Exception);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    GetMessage(e.Exception),
                    "",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                App.Current.Shutdown();
            }
            finally
            {
                e.Handled = true;
            }
        }

        internal void HandleException(Exception exception)
        {
            string errorLogFile = GetErrorLogFile();
            WriteErrorLog(exception, errorLogFile);
            MainWindow window = XPFriend.JenkinsOnDesktop.MainWindow.Current;
            Workspace.Current.Butler.SetErrorMessage(GetMessage(exception), errorLogFile);
            window.Visibility = Visibility.Hidden;
            window.Left = 50;
            window.Top = 50;
            window.UpdateWindow();
            window.ShowMessage();
            window.Visibility = Visibility.Visible;
            window.Activate();
            window.Topmost = false;
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
