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

        internal Appearance(
            string title, string balloonTipText, string messageText,
            string imageFile, Bitmap bitmap) :
            this(title, balloonTipText, messageText, imageFile, bitmap, true)
        {
        }

        internal Appearance(
            string title, string balloonTipText, string messageText,
            string imageFile, Bitmap bitmap, bool useDefaultAnimation) : this()
        {
            this.BalloonTipTitle = title;
            this.BalloonTipText = balloonTipText;
            this.MessageText = messageText;
            this.Bitmap = bitmap;

            if (imageFile != null)
            {
                this.ImageFile = imageFile;
            }

            if (useDefaultAnimation)
            {
                this.GetEnterAnimationAsStoryboard();
                this.GetExitAnimationAsStoryboard();
            }

            this.MessageStyle.SetDefaultStyle();
        }

        private static Storyboard CreateStoryboard(Timelines timelines)
        {
            Storyboard storyboard = new Storyboard();
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            foreach (Timeline timeline in timelines)
            {
                storyboard.Children.Add(timeline);
            }
            return storyboard;
        }

        internal Storyboard GetEnterAnimationAsStoryboard()
        {
            if (this.EnterAnimation.Count == 0)
            {
                SetupDefaultEnterAnimation(this.EnterAnimation);
            }
            return CreateStoryboard(this.EnterAnimation);
        }

        private static void SetupDefaultEnterAnimation(Timelines enterAnimation)
        {
            enterAnimation.Add(new Operation()
            {
                Command = Command.UpdateWindow
            });

            enterAnimation.Add(new SlideIn()
            {
                Direction = Direction.Right,
                Position = Position.LeftBottom,
                BeginTime = TimeSpan.FromSeconds(0.1)
            });

            enterAnimation.Add(new Operation()
            {
                Command = Command.ShowMessage,
                BeginTime = TimeSpan.FromSeconds(1.2)
            });
        }

        internal Storyboard GetExitAnimationAsStoryboard()
        {
            if (this.ExitAnimation.Count == 0)
            {
                SetupDefaultExitAnimation(this.ExitAnimation);
            }
            return CreateStoryboard(this.ExitAnimation);
        }

        private static void SetupDefaultExitAnimation(Timelines exitAnimation)
        {
            exitAnimation.Add(new Operation()
            {
                Command = Command.HideMessage
            });

            exitAnimation.Add(new SlideOut()
            {
                Direction = Direction.Left,
                BeginTime = TimeSpan.FromSeconds(0.3)
            });
        }
    }
}
