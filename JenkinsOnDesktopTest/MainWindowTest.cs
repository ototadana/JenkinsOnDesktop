using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Globalization;
using System.Threading;
using XPFriend.JenkinsOnDesktop.Core;
using System.Windows.Controls;

namespace XPFriend.JenkinsOnDesktop
{
    [TestClass]
    public class MainWindowTest
    {
        private CultureInfo defaultUICulture;

        [TestInitialize]
        public void Setup()
        {
            MainWindow.Current = null;
            defaultUICulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            TestUtil.UpdateWorkspaceFolder(GetType().Name);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Thread.CurrentThread.CurrentUICulture = defaultUICulture;
            if (MainWindow.Current != null)
            {
                MainWindow.Current.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestScreenWidthAndScreenHeight()
        {
            // setup
            MainWindow window = new MainWindow();

            try
            {
                // expect
                Assert.AreEqual(SystemParameters.VirtualScreenWidth, window.ScreenWidth);
                Assert.AreEqual(SystemParameters.VirtualScreenHeight, window.ScreenHeight);
            }
            finally
            {
                // cleanup
                window.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestCurrent()
        {
            // setup
            MainWindow window = new MainWindow();
            {
                // expect
                Assert.IsNull(MainWindow.Current);
            }

            {
                // when
                MainWindow.Current = window;

                // then
                Assert.AreSame(MainWindow.Current, window);
            }

            {
                // when
                MainWindow.Current = null;
                Assert.IsNull(MainWindow.Current);
                App app = AppTest.GetApp();
                app.MainWindow = window;

                // then
                Assert.AreSame(MainWindow.Current, window);
            }
        }

        [TestMethod]
        public void TestConstructor()
        {
            // when
            MainWindow window = new MainWindow();

            try
            {
                // then
                WorkspaceTest.AssertInitialized(Workspace.Current);
                Assert.IsFalse(window.ShowInTaskbar);
                NotifyIcon notifyIcon = GetField<NotifyIcon>(window, "notifyIcon");
                Assert.IsTrue(notifyIcon.Visible);
                Assert.AreEqual(2, notifyIcon.ContextMenuStrip.Items.Count);
                Assert.AreEqual("CallTheButler", notifyIcon.ContextMenuStrip.Items[0].Name);
                Assert.AreEqual("Call Jenkins", notifyIcon.ContextMenuStrip.Items[0].Text);
                Assert.AreEqual(true, notifyIcon.ContextMenuStrip.Items[0].DoubleClickEnabled);
                Assert.AreEqual("Quit", notifyIcon.ContextMenuStrip.Items[1].Text);

                Assert.IsTrue(GetField<DispatcherTimer>(window, "timer").IsEnabled);
                Assert.AreEqual(TimeSpan.FromSeconds(30), GetField<DispatcherTimer>(window, "timer").Interval);
                Assert.AreSame(Workspace.Current.Butler.Icon, notifyIcon.Icon);
                Assert.AreEqual(Workspace.Current.Butler.DisplayName, window.Title);
            }
            finally
            {
                // cleanup
                window.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestQuit()
        {
            // setup
            MainWindow window = new MainWindow();
            App app = AppTest.GetApp();
            app.MainWindow = window;
            NotifyIcon notifyIcon = GetField<NotifyIcon>(window, "notifyIcon");
            Assert.IsTrue(notifyIcon.Visible);

            // when
            window.Quit();

            // then
            Assert.IsFalse(notifyIcon.Visible);
        }

        [TestMethod]
        public void TestUpdateWindow()
        {
            // setup
            MainWindow window = new MainWindow();
            Butler butler = Workspace.Current.Butler;
            Border message = GetField<Border>(window, "message");

            try
            {
                {
                    // when
                    window.UpdateWindow();
                    Assert.IsFalse(butler.HasMessage);

                    // then
                    Assert.AreEqual(butler.Image, GetField<Image>(window, "butlerImage").Source);
                    Assert.AreEqual(Visibility.Collapsed, GetField<UIElement>(window, "configuration").Visibility);
                    Assert.AreEqual(Visibility.Hidden, message.Visibility);
                    Assert.IsTrue(window.Topmost);
                }

                {
                    // when
                    Report report = ButlerTest.CreateReport();
                    butler.UpdateAppearance(ButlerFactory.Sad, report);
                    window.UpdateWindow();
                    Assert.IsTrue(butler.HasMessage);
                    TextBlock messageText = GetField<TextBlock>(window, "messageText");

                    // then
                    Assert.AreEqual(butler.MessageText, messageText.Text);
                    Assert.AreEqual(butler.MessageStyle.FontFamily, messageText.FontFamily);
                    Assert.AreEqual(butler.MessageStyle.FontSize, messageText.FontSize);
                    Assert.AreEqual(butler.SourceUrl, messageText.ToolTip);
                    Assert.AreEqual(butler.MessageStyle.Foreground, messageText.Foreground);
                    Assert.AreEqual(1.0, message.Opacity);
                    Assert.AreEqual(butler.MessageStyle.Width, message.Width);
                    Assert.AreEqual(butler.MessageStyle.Height, message.Height);
                    Assert.AreEqual(butler.MessageStyle.Padding, message.Padding);
                    Assert.AreEqual(System.Windows.Input.Cursors.Hand, message.Cursor);
                    Assert.AreEqual(butler.MessageStyle.Background, message.Background);
                    Assert.AreEqual(butler.MessageStyle.BorderBrush, message.BorderBrush);
                    Assert.AreEqual(butler.MessageStyle.BorderThickness, message.BorderThickness);
                    Assert.AreEqual(butler.MessageStyle.CornerRadius, message.CornerRadius);
                }

                {
                    // when
                    butler.MessageStyle.Position = MessagePosition.Left;
                    window.UpdateWindow();

                    // then
                    Assert.AreEqual(0, Grid.GetColumn(message));
                    Assert.AreEqual(1, Grid.GetRow(message));
                }

                {
                    // when
                    butler.MessageStyle.Position = MessagePosition.Top;
                    window.UpdateWindow();

                    // then
                    Assert.AreEqual(1, Grid.GetColumn(message));
                    Assert.AreEqual(0, Grid.GetRow(message));
                }

                {
                    // when
                    butler.MessageStyle.Position = MessagePosition.Bottom;
                    window.UpdateWindow();

                    // then
                    Assert.AreEqual(1, Grid.GetColumn(message));
                    Assert.AreEqual(2, Grid.GetRow(message));
                }

                {
                    // when
                    butler.MessageStyle.Position = MessagePosition.Right;
                    window.UpdateWindow();

                    // then
                    Assert.AreEqual(2, Grid.GetColumn(message));
                    Assert.AreEqual(1, Grid.GetRow(message));
                }
            }
            finally
            {
                // cleanup
                window.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestShowConfiguration()
        {
            // setup
            MainWindow window = new MainWindow();
            Butler butler = Workspace.Current.Butler;
            System.Windows.Controls.ComboBox butlerName = GetField<System.Windows.Controls.ComboBox>(window, "butlerName");
            System.Windows.Controls.ComboBox businessName = GetField<System.Windows.Controls.ComboBox>(window, "businessName");

            try
            {
                // when
                window.ShowConfiguration(butler);

                // then
                Assert.AreEqual(2, butlerName.Items.Count);
                Assert.AreEqual("Emotional-Jenkins", butlerName.Text);
                Assert.AreEqual("Calm-Jenkins", butlerName.Items[0]);
                Assert.AreEqual("Emotional-Jenkins", butlerName.Items[1]);

                Assert.AreEqual(2, businessName.Items.Count);
                Assert.AreEqual("Check-job-status", businessName.Text);
                Assert.AreEqual("Check-job-status", businessName.Items[0]);
                Assert.AreEqual("Time-keeping", businessName.Items[1]);

                Assert.AreEqual(butler.DisplayName, GetField<System.Windows.Controls.TextBox>(window, "butlerDisplayName").Text);
                Assert.AreEqual(butler.License, GetField<System.Windows.Controls.TextBox>(window, "butlerLicense").Text);
                Assert.AreEqual(butler.TypicalAppearanceImage, GetField<Image>(window, "butlerImageSample").Source);

                Assert.IsTrue(GetField<TabItem>(window, "configurationButlerTab").IsSelected);
            }
            finally
            {
                // cleanup
                window.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestUpdateBusinessInformation()
        {
            // setup
            MainWindow window = new MainWindow();
            Butler butler = Workspace.Current.Butler;
            string name = "Check-job-status";
            BusinessInformation businessInformation = BusinessInformation.GetInstance(name);
            Business businessConfiguration = Business.GetInstance(name);

            try
            {
                // when
                window.ShowConfiguration(butler);
                window.UpdateBusinessInformation(name);

                // then
                Assert.AreEqual(businessInformation.Synopsis, GetField<System.Windows.Controls.TextBox>(window, "businessSynopsis").Text);
                Assert.AreEqual(businessInformation.Description, GetField<System.Windows.Controls.TextBox>(window, "businessDescription").Text);
                Assert.AreEqual(businessInformation.Examples, GetField<System.Windows.Controls.TextBox>(window, "businessExamples").Text);
                Assert.AreEqual("", GetField<System.Windows.Controls.TextBox>(window, "parameters").Text);
                Assert.AreEqual(businessConfiguration.TimerInterval, GetField<Slider>(window, "timerInterval").Value);
            }
            finally
            {
                // cleanup
                window.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestIsOnDesktop()
        {
            // setup
            MainWindow window = new MainWindow();

            try
            {
                {
                    // when
                    SetupTestIsOnDesktop(window, 0.1, 0.0, 0.0);

                    // then
                    Assert.IsTrue(window.IsOnDesktop(true));
                    Assert.IsFalse(window.IsOnDesktop(false));
                }
                {
                    // when
                    SetupTestIsOnDesktop(window, 0.0, 0.0, 0.0);

                    // then
                    Assert.IsFalse(window.IsOnDesktop(true));
                }
                {
                    // when
                    SetupTestIsOnDesktop(window, 0.1, -0.1, 0.0);

                    // then
                    Assert.IsFalse(window.IsOnDesktop(true));
                }
                {
                    // when
                    SetupTestIsOnDesktop(window, 0.1, int.MaxValue, 0.0);

                    // then
                    Assert.IsFalse(window.IsOnDesktop(true));
                }
                {
                    // when
                    SetupTestIsOnDesktop(window, 0.1, 0.0, -0.1);

                    // then
                    Assert.IsFalse(window.IsOnDesktop(true));
                }
                {
                    // when
                    SetupTestIsOnDesktop(window, 0.1, 0.0, int.MaxValue);

                    // then
                    Assert.IsFalse(window.IsOnDesktop(true));
                }
                {
                    // when
                    SetupTestIsOnDesktop(window, 0.1, 0.0, 0.0);

                    // then
                    Assert.IsTrue(window.IsOnDesktop(true));
                }
            }
            finally
            {
                // cleanup
                window.HideNotifyIcon();
            }
        }

        private static void SetupTestIsOnDesktop(MainWindow window, double opacity, double left, double top)
        {
            window.Opacity = opacity;
            window.Left = left;
            window.Top = top;
        }

        private T GetField<T>(MainWindow window, string name)
        {
            return (T)GetField(typeof(MainWindow), name).GetValue(window);
        }

        private FieldInfo GetField(Type type, string name)
        {
            return type.GetField(name,
                BindingFlags.GetField | BindingFlags.SetField | 
                BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
