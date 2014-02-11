using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using XPFriend.JenkinsOnDesktop.Core;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    [TestClass]
    public class WindowOperatorTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            if (MainWindow.Current != null)
            {
                MainWindow.Current.HideNotifyIcon();
            }
        }

        [TestMethod]
        public void TestGetInstance()
        {
            // expect
            Assert.AreSame(WindowOperator.GetInstance(), WindowOperator.GetInstance());
        }

        [TestMethod]
        public void TestApplyAnimationClock()
        {
            try
            {
                // when
                WindowOperator.GetInstance().ApplyAnimationClock(null, null, HandoffBehavior.Compose);
                Assert.Fail();
            }
            catch (NotImplementedException e)
            {
                // then
                Console.WriteLine(e);
            }

            try
            {
                // when
                WindowOperator.GetInstance().ApplyAnimationClock(null, null);
                Assert.Fail();
            }
            catch (NotImplementedException e)
            {
                // then
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestBeginAnimation()
        {
            try
            {
                // when
                WindowOperator.GetInstance().BeginAnimation(null, null, HandoffBehavior.Compose);
                Assert.Fail();
            }
            catch (NotImplementedException e)
            {
                // then
                Console.WriteLine(e);
            }

            try
            {
                // when
                WindowOperator.GetInstance().BeginAnimation(null, null);
                Assert.Fail();
            }
            catch (NotImplementedException e)
            {
                // then
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestGetAnimationBaseValue()
        {
            try
            {
                // when
                WindowOperator.GetInstance().GetAnimationBaseValue(null);
                Assert.Fail();
            }
            catch (NotImplementedException e)
            {
                // then
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestHasAnimatedProperties()
        {
            try
            {
                // when
                bool b = WindowOperator.GetInstance().HasAnimatedProperties;
                Assert.Fail();
            }
            catch (NotImplementedException e)
            {
                // then
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestConvertNaNToZero()
        {
            // setup
            Workspace.Current.Configuration = new Configuration();
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(margin, WindowOperator.ConvertNaNToZero(double.NaN));
            Assert.AreEqual(margin, WindowOperator.ConvertNaNToZero(double.PositiveInfinity));
            Assert.AreEqual(margin, WindowOperator.ConvertNaNToZero(double.NegativeInfinity));
            Assert.AreEqual(1.0, WindowOperator.ConvertNaNToZero(1.0));
        }

        [TestMethod]
        public void TestGetLeft()
        {
            // setup
            Workspace.Current.Configuration = new Configuration();
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(margin, WindowOperator.GetLeft(null));
        }

        [TestMethod]
        public void TestGetTop()
        {
            // setup
            Workspace.Current.Configuration = new Configuration();
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(margin, WindowOperator.GetTop(null));
        }

        [TestMethod]
        public void TestGetRight()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(window.ScreenWidth - margin, WindowOperator.GetRight(window));
        }

        [TestMethod]
        public void TestGetBottom()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(window.ScreenHeight - margin, WindowOperator.GetBottom(window));
        }

        [TestMethod]
        public void TestGetCenter()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(window.ScreenWidth / 2, WindowOperator.GetCenter(window));
        }

        [TestMethod]
        public void TestGetMiddle()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(window.ScreenHeight / 2, WindowOperator.GetMiddle(window));
        }

        [TestMethod]
        public void TestGetLeftWithPosition()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(margin, WindowOperator.GetLeft(window, Position.LeftTop));
            Assert.AreEqual(margin, WindowOperator.GetLeft(window, Position.LeftMiddle));
            Assert.AreEqual(margin, WindowOperator.GetLeft(window, Position.LeftBottom));
            Assert.AreEqual(window.ScreenWidth / 2, WindowOperator.GetLeft(window, Position.CenterTop));
            Assert.AreEqual(window.ScreenWidth / 2, WindowOperator.GetLeft(window, Position.CenterMiddle));
            Assert.AreEqual(window.ScreenWidth / 2, WindowOperator.GetLeft(window, Position.CenterBottom));
            Assert.AreEqual(window.ScreenWidth - margin, WindowOperator.GetLeft(window, Position.RightTop));
            Assert.AreEqual(window.ScreenWidth - margin, WindowOperator.GetLeft(window, Position.RightMiddle));
            Assert.AreEqual(window.ScreenWidth - margin, WindowOperator.GetLeft(window, Position.RightBottom));
        }

        [TestMethod]
        public void TestGetTopWithPosition()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            // expect
            Assert.AreEqual(margin, WindowOperator.GetTop(window, Position.LeftTop));
            Assert.AreEqual(margin, WindowOperator.GetTop(window, Position.CenterTop));
            Assert.AreEqual(margin, WindowOperator.GetTop(window, Position.RightTop));
            Assert.AreEqual(window.ScreenHeight / 2, WindowOperator.GetTop(window, Position.LeftMiddle));
            Assert.AreEqual(window.ScreenHeight / 2, WindowOperator.GetTop(window, Position.CenterMiddle));
            Assert.AreEqual(window.ScreenHeight / 2, WindowOperator.GetTop(window, Position.RightMiddle));
            Assert.AreEqual(window.ScreenHeight - margin, WindowOperator.GetTop(window, Position.LeftBottom));
            Assert.AreEqual(window.ScreenHeight - margin, WindowOperator.GetTop(window, Position.CenterBottom));
            Assert.AreEqual(window.ScreenHeight - margin, WindowOperator.GetTop(window, Position.RightBottom));
        }

        [TestMethod]
        public void TestLeft()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;

            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.LeftProperty, 10.0);

                // then
                Assert.AreEqual(10.0, window.Left);
            }
            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.LeftProperty, null);

                // then
                Assert.AreEqual(10.0, window.Left);
            }
        }

        [TestMethod]
        public void TestTop()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;

            // when
            WindowOperator.GetInstance().SetValue(WindowOperator.TopProperty, 10.0);

            // then
            Assert.AreEqual(10.0, window.Top);
        }

        [TestMethod]
        public void TestPosition()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            window.Left = 10.0;
            window.Top = 11.0;
            double margin = Workspace.Current.Configuration.DesktopMargin;

            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.PositionProperty, Position.Current);

                // then
                Assert.AreEqual(10.0, window.Left);
                Assert.AreEqual(11.0, window.Top);
            }
            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.PositionProperty, Position.LeftBottom);

                // then
                Assert.AreEqual(margin, window.Left);
                Assert.AreEqual(window.ScreenHeight - margin, window.Top);
            }
        }

        [TestMethod]
        public void TestOpacity()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            window.Opacity = 1.0;

            // when
            WindowOperator.GetInstance().SetValue(WindowOperator.OpacityProperty, 0.5);

            // then
            Assert.AreEqual(0.5, window.Opacity);
        }

        [TestMethod]
        public void TestCommand()
        {
            // setup
            MainWindow window = new MainWindow();
            MainWindow.Current = window;
            window.Opacity = 1.0;
            window.WindowState = WindowState.Normal;
            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.CommandProperty, Command.Hide);

                // then
                Assert.AreEqual(0.0, window.Opacity);
            }
            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.CommandProperty, Command.Show);

                // then
                Assert.AreEqual(1.0, window.Opacity);
            }
            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.CommandProperty, Command.Maximize);

                // then
                Assert.AreEqual(WindowState.Maximized, window.WindowState);
            }
            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.CommandProperty, Command.Minimize);

                // then
                Assert.AreEqual(WindowState.Minimized, window.WindowState);
            }
            {
                // when
                WindowOperator.GetInstance().SetValue(WindowOperator.CommandProperty, Command.Normalize);

                // then
                Assert.AreEqual(WindowState.Normal, window.WindowState);
            }
        }
    }
}
