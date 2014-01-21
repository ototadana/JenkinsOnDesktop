using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using XPFriend.JenkinsOnDesktop.Animation;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using XPFriend.JenkinsOnDesktop.Core.ScriptEngine;
using XPFriend.JenkinsOnDesktop.Properties;
using XPFriend.JenkinsOnDesktop.Util;

namespace XPFriend.JenkinsOnDesktop.Core
{
    public class Butler
    {
        private const string CalmJenkins = "Calm-Jenkins";
        internal const string EmotionalJenkins = "Emotional-Jenkins";

        private const string Normal = "Normal";
        private const string Angry = "Angry";
        private const string Sad = "Sad";
        private const string Rageful = "Rageful";
        private const string Expectant = "Expectant";
        private const string Happy = "Happy";

        private Appearance appearance;
        private bool hasNews;
        private string sourceUrl;
        private string messageText;
        private string balloonTipText;
        private string balloonTipTitle;

        public string Name { get; set; }
        public string Nickname { get; set; }
        public string DisplayName { get; set; }
        public string License { get; set; }
        public string TypicalAppearance { get; set; }
        public Appearances Appearances { get; set; }

        internal bool HasNews { get { return hasNews; } }
        internal string SourceUrl { get { return sourceUrl; } }
        internal string MessageText { get { return messageText; } }
        internal string BalloonTipText { get { return balloonTipText; } }
        internal string BalloonTipTitle { get { return balloonTipTitle; } }
        internal Icon Icon { get { return appearance.Icon; } }
        internal BitmapSource Image { get { return appearance.Image; } }
        internal System.Windows.Forms.ToolTipIcon ToolTipIcon { get { return appearance.ToolTipIcon; } }
        internal int BalloonTipTimeout { get { return appearance.BalloonTipTimeout; } }
        internal bool Topmost { get { return appearance.Topmost; } }
        internal MessageStyle MessageStyle { get { return appearance.MessageStyle; } }
        internal Storyboard EnterAnimation { get { return appearance.GetEnterAnimationAsStoryboard(); } }
        internal Storyboard ExitAnimation { get { return appearance.GetExitAnimationAsStoryBoard(); } }
        internal bool HasMessage { get { return !string.IsNullOrWhiteSpace(messageText); } }

        internal BitmapSource TypicalAppearanceImage 
        { 
            get 
            {
                try
                {
                    return this.Appearances[TypicalAppearance].Image;
                }
                catch (Exception)
                {
                    if (this.Appearances.Count == 0)
                    {
                        return null;
                    }
                    return this.Appearances.Values.First().Image;
                }
            } 
        }

        public Butler()
        {
            this.Appearances = new Appearances();
        }

        internal static Butler GetInstance(string name, bool createButlersFolderIfNotExists)
        {
            return ButlersFolder.Load(name, createButlersFolderIfNotExists);
        }

        internal void ReadReport(Report report)
        {
            string feeling = ButlerScriptEngine.Feel(this.Name, report);
            UpdateAppearance(feeling, report);
        }

        internal void SetDefaultAppearance()
        {
            UpdateAppearance(Butler.Normal, null);
        }

        private void UpdateAppearance(string feeling, Report report)
        {
            this.appearance = GetAppearance(feeling);
            this.hasNews = (report != null)? report.IsUpdated : false;
            this.sourceUrl = (report != null)? report.SourceUrl : null;
            this.messageText = GetMessageText(report);
            this.balloonTipText = GetBalloonTipText(report);
            this.balloonTipTitle = GetBalloonTipTitle(report);
        }

        internal Appearance GetDefaultAppearance()
        {
            return GetAppearance(Normal);
        }

        private Appearance GetAppearance(string feeling)
        {
            Appearance appearance;
            if(Appearances.TryGetValue(feeling, out appearance)) 
            {
                return appearance;
            }
            return this.Appearances[Butler.Normal];
        }

        private string GetMessageText(Report report)
        {
            return Format(this.appearance.MessageText, report);
        }

        private string GetBalloonTipTitle(Report report)
        {
            return Format(this.appearance.BalloonTipTitle, report);
        }

        private string GetBalloonTipText(Report report)
        {
            return Format(this.appearance.BalloonTipText, report);
        }

        private string Format(string format, Report report)
        {
            if (report == null)
            {
                return null;
            }
            return report.Format(format);
        }

        internal static Butler CreateEmotionalJenkins()
        {
            return CreateDefaultButler(Butler.EmotionalJenkins, 
                Resources.Nickname_EmotionalJenkins, Resources.DisplayName_EmotionalJenkins);
        }

        internal static Butler CreateDefaultButler(string name, string nickname, string displayName)
        {
            Butler butler = new Butler()
            {
                Name = name, 
                Nickname = nickname,
                DisplayName = displayName,
                License = Resources.License_EmotionalJenkins,
                TypicalAppearance = Butler.Sad
            };

            butler.Appearances[Normal] = 
                CreateAppearance(null, null, Resources.Message_Report, null, Resources.jenkins);

            butler.Appearances[Sad] = 
                CreateAppearance(null, null, Resources.Message_Report, "sad.png", Resources.sad);

            butler.Appearances[Angry] = 
                CreateAppearance(null, null, Resources.Message_Report, "oni.png", Resources.oni);

            butler.Appearances[Rageful] = 
                CreateAppearance(null, null, Resources.Message_Report, "onibi.png", Resources.onibi);

            butler.Appearances[Expectant] = 
                CreateAppearance(null, null, Resources.Message_Report, "ninja.png", Resources.ninja, false);

            butler.Appearances[Happy] = 
                CreateAppearance(null, null, Resources.Message_Report, null, Resources.jenkins);

            butler.Appearances[Rageful].Topmost = true;
            SetNinjaAnimation(butler.Appearances[Expectant]);
            return butler;
        }

        private static void SetNinjaAnimation(Appearance appearance)
        {
            appearance.EnterAnimation.Add(new Operation() 
            { 
                Command = Command.UpdateWindow 
            });

            appearance.EnterAnimation.Add(new SlideIn() 
            { 
                Direction = Direction.Left, 
                Position = Position.LeftBottom, 
                BeginTime = TimeSpan.FromSeconds(0.5) 
            });

            appearance.EnterAnimation.Add(new Operation() 
            { 
                Command = Command.ShowMessage, 
                BeginTime = TimeSpan.FromSeconds(1.5) 
            });

            appearance.ExitAnimation.Add(new Operation() 
            { 
                Command = Command.HideMessage 
            });

            appearance.ExitAnimation.Add(new FadeOut() { 
                BeginTime = TimeSpan.FromSeconds(0.25), 
                Duration = new Duration(TimeSpan.FromSeconds(0.3)) 
            });

            appearance.ExitAnimation.Add(new SlideOut() 
            { 
                Direction = Direction.Left, 
                BeginTime = TimeSpan.FromSeconds(0.4), 
                Duration = new Duration(TimeSpan.FromSeconds(0.1)) 
            });

            appearance.ExitAnimation.Add(new Operation() 
            { 
                Command = Command.Show, 
                BeginTime = TimeSpan.FromSeconds(1) 
            });
        }

        internal static Butler CreateCalmJenkins()
        {
            Butler butler = new Butler()
            {
                Name = CalmJenkins,
                Nickname = Resources.Nickname_CalmJenkins,
                DisplayName = Resources.DisplayName_CalmJenkins,
                License = Resources.License_CalmJenkins,
                TypicalAppearance = Happy
            };

            butler.Appearances[Normal] = CreateAppearance(
                Resources.BalloonTip_Title, Resources.Message_Report, null, null, Resources.jenkins);

            butler.Appearances[Happy] = CreateAppearance(
                null, null, Resources.Message_Report, null, Resources.jenkins);

            return butler;
        }

        private static Appearance CreateAppearance(
            string title, 
            string balloonTipText, 
            string messageText, 
            string imageFile, 
            Bitmap bitmap)
        {
            return CreateAppearance(title, balloonTipText, messageText, imageFile, bitmap, true);
        }

        private static Appearance CreateAppearance(
            string title, 
            string balloonTipText, 
            string messageText, 
            string imageFile, 
            Bitmap bitmap, 
            bool useDefaultAnimation)
        {
            Appearance appearance = new Appearance() 
            { 
                BalloonTipTitle = title, 
                BalloonTipText = balloonTipText, 
                MessageText = messageText,
                Bitmap = bitmap
            };

            if (imageFile != null)
            {
                appearance.ImageFile = imageFile;
            }

            if (useDefaultAnimation)
            {
                appearance.GetEnterAnimationAsStoryboard();
                appearance.GetExitAnimationAsStoryBoard();
            }

            InitializeMessageStyle(appearance.MessageStyle);
            return appearance;
        }

        private static void InitializeMessageStyle(MessageStyle style)
        {
            style.Width = 200;
            style.Height = 200;
            style.BorderBrush = new SolidColorBrush(Colors.Black);
            style.BorderThickness = new Thickness(6);
            style.CornerRadius = new CornerRadius(8);
            style.Background = new SolidColorBrush(Colors.White);
        }

        internal void SetErrorMessage(string errorMessage, string errorLogFile)
        {
            UpdateAppearance(Sad, null);
            this.sourceUrl = errorLogFile;
            this.messageText = errorMessage;
            this.appearance.Image = BitmapUtil.ToBitmapSource(Resources.sad);
            this.hasNews = true;
        }
    }
}
