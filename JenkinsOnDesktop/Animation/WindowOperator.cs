using System;
using System.Windows;
using System.Windows.Media.Animation;
using XPFriend.JenkinsOnDesktop.Core;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class WindowOperator : DependencyObject, IAnimatable
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(Command), typeof(WindowOperator));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(WindowOperator));

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double?), typeof(WindowOperator));

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double?), typeof(WindowOperator));

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double?), typeof(WindowOperator));

        private static WindowOperator windowOperator = new WindowOperator();

        private static double? margin;

        private static double Margin
        {
            get
            {
                if (WindowOperator.margin == null)
                {
                    WindowOperator.margin = Workspace.Current.Configuration.DesktopMargin;
                }
                return (double)WindowOperator.margin;
            }
        }


        private WindowOperator() { }

        public static WindowOperator GetInstance()
        {
            return WindowOperator.windowOperator;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            MainWindow window = MainWindow.Current;
            if (e.Property == CommandProperty)
            {
                DoCommand(window, (Command)e.NewValue);
            }
            else if (e.Property == PositionProperty)
            {
                Locate(window, (Position)e.NewValue);
            }
            else if (e.Property == LeftProperty)
            {
                MainWindow.Current.Left = (double)e.NewValue;
            }
            else if (e.Property == TopProperty)
            {
                MainWindow.Current.Top = (double)e.NewValue;
            }
            else if (e.Property == OpacityProperty)
            {
                MainWindow.Current.Opacity = (double)e.NewValue;
            }
        }

        private static void Locate(MainWindow window, Position position)
        {
            if (position == Position.Current)
            {
                return;
            }

            window.Left = GetLeft(window, position);
            window.Top = GetTop(window, position);
        }

        internal static double GetTop(MainWindow window, Position position)
        {
            if (position == Position.LeftTop ||
                position == Position.CenterTop ||
                position == Position.RightTop)
            {
                return GetTop(window);
            }
            else if (position == Position.LeftMiddle ||
               position == Position.CenterMiddle ||
               position == Position.RightMiddle)
            {
                return GetMiddle(window);
            }
            else
            {
                return GetBottom(window);
            }
        }

        internal static double GetLeft(MainWindow window, Position position)
        {
            if (position == Position.LeftTop ||
                position == Position.LeftMiddle ||
                position == Position.LeftBottom)
            {
                return GetLeft(window);
            }
            else if (position == Position.CenterTop ||
              position == Position.CenterMiddle ||
              position == Position.CenterBottom)
            {
                return GetCenter(window);
            }
            else
            {
                return GetRight(window);
            }
        }

        internal static double GetLeft(MainWindow window)
        {
            return Margin;
        }

        internal static double GetRight(MainWindow window)
        {
            return GetRight(window, Margin);
        }

        private static double GetRight(MainWindow window, double margin)
        {
            double width = window.ScreenWidth - margin;
            return ConvertNaNToZero(width - MainWindow.Current.ActualWidth);
        }

        internal static double GetCenter(MainWindow window)
        {
            return GetRight(window, 0.0) / 2;
        }

        internal static double GetTop(MainWindow window)
        {
            return Margin;
        }

        internal static double GetBottom(MainWindow window)
        {
            return GetBottom(window, Margin);
        }

        private static double GetBottom(MainWindow window, double margin)
        {
            double height = window.ScreenHeight - margin;
            return ConvertNaNToZero(height - MainWindow.Current.ActualHeight);
        }

        internal static double GetMiddle(MainWindow window)
        {
            return GetBottom(window, 0.0) / 2;
        }

        internal static double ConvertNaNToZero(double value)
        {
            if (Double.IsNaN(value) || Double.IsInfinity(value))
            {
                return Margin;
            }
            else
            {
                return value;
            }
        }

        private static void DoCommand(MainWindow window, Command command)
        {
            if (command == Command.UpdateWindow)
            {
                window.UpdateWindow();
            }
            else if (command == Command.Show)
            {
                window.Opacity = 1.0;
            }
            else if (command == Command.Hide)
            {
                window.Opacity = 0.0;
            }
            else if (command == Command.ShowMessage)
            {
                window.ShowMessage();
            }
            else if (command == Command.HideMessage)
            {
                window.HideMessage();
            }
            else if (command == Command.Maximize)
            {
                window.WindowState = WindowState.Maximized;
            }
            else if (command == Command.Minimize)
            {
                window.WindowState = WindowState.Minimized;
            }
            else if (command == Command.Normalize)
            {
                window.WindowState = WindowState.Normal;
            }
        }

        public void ApplyAnimationClock(
            DependencyProperty dp, 
            AnimationClock clock, 
            HandoffBehavior handoffBehavior)
        {
            throw new NotImplementedException();
        }

        public void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock)
        {
            throw new NotImplementedException();
        }

        public void BeginAnimation(
            DependencyProperty dp, 
            AnimationTimeline animation, 
            HandoffBehavior handoffBehavior)
        {
            throw new NotImplementedException();
        }

        public void BeginAnimation(DependencyProperty dp, AnimationTimeline animation)
        {
            throw new NotImplementedException();
        }

        public object GetAnimationBaseValue(DependencyProperty dp)
        {
            throw new NotImplementedException();
        }

        public bool HasAnimatedProperties
        {
            get { throw new NotImplementedException(); }
        }
    }
}
