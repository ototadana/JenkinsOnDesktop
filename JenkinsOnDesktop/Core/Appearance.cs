using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using XPFriend.JenkinsOnDesktop.Animation;

namespace XPFriend.JenkinsOnDesktop.Core
{
    public class Appearance
    {
        public string IconFile { get; set; }
        public string ImageFile { get; set; }
        public int BalloonTipTimeout { get; set; }
        public bool Topmost { get; set; }
        public MessageStyle MessageStyle { get; set; }
        public Timelines EnterAnimation { get; set; }
        public Timelines ExitAnimation { get; set; }

        [DefaultValue(null)]
        public string MessageText { get; set; }

        [DefaultValue(null)]
        public string BalloonTipText { get; set; }

        [DefaultValue(null)]
        public string BalloonTipTitle { get; set; }

        internal Icon Icon { get; set; }
        internal BitmapSource Image { get; set; }
        internal Bitmap Bitmap { get; set; }
        internal System.Windows.Forms.ToolTipIcon ToolTipIcon { get; set; }

        public Appearance()
        {
            this.BalloonTipTimeout = 10;
            this.MessageStyle = new MessageStyle();
            this.IconFile = "jenkins_icon.ico";
            this.ImageFile = "jenkins.png";
            this.EnterAnimation = new Timelines();
            this.ExitAnimation = new Timelines();
        }

        private static Storyboard CreateStoryBoard(Timelines timelines)
        {
            Storyboard storyBoard = new Storyboard();
            storyBoard.FillBehavior = FillBehavior.HoldEnd;
            foreach (Timeline timeline in timelines)
            {
                storyBoard.Children.Add(timeline);
            }
            return storyBoard;
        }

        internal Storyboard GetEnterAnimationAsStoryboard()
        {
            if (this.EnterAnimation.Count == 0)
            {
                this.EnterAnimation.Add(new Operation()
                {
                    Command = Command.UpdateWindow
                });

                this.EnterAnimation.Add(new SlideIn()
                {
                    Direction = Direction.Right,
                    Position = Position.LeftBottom,
                    BeginTime = TimeSpan.FromSeconds(0.5)
                });

                this.EnterAnimation.Add(new Operation()
                {
                    Command = Command.ShowMessage,
                    BeginTime = TimeSpan.FromSeconds(1.5)
                });
            }
            return CreateStoryBoard(EnterAnimation);
        }

        internal Storyboard GetExitAnimationAsStoryBoard()
        {
            if (this.ExitAnimation.Count == 0)
            {
                this.ExitAnimation.Add(new Operation()
                {
                    Command = Command.HideMessage
                });

                this.ExitAnimation.Add(new SlideOut()
                {
                    Direction = Direction.Left,
                    BeginTime = TimeSpan.FromSeconds(0.3)
                });
            }
            return CreateStoryBoard(ExitAnimation);
        }
    }
}
