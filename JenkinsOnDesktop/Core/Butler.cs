using System;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using XPFriend.JenkinsOnDesktop.Core.ScriptEngine;
using XPFriend.JenkinsOnDesktop.Properties;
using XPFriend.JenkinsOnDesktop.Util;

namespace XPFriend.JenkinsOnDesktop.Core
{
    public class Butler
    {
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
        internal Storyboard ExitAnimation { get { return appearance.GetExitAnimationAsStoryboard(); } }
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

        internal static Butler GetInstance(string name)
        {
            return ButlersFolder.Load(name);
        }

        internal void ReadReport(Report report)
        {
            string feeling = ButlerScriptEngine.Feel(this.Name, report);
            UpdateAppearance(feeling, report);
        }

        internal void SetDefaultAppearance()
        {
            UpdateAppearance(ButlerFactory.Normal, null);
        }

        internal void UpdateAppearance(string feeling, Report report)
        {
            this.appearance = GetAppearance(feeling);
            this.hasNews = (report != null)? report.IsUpdated : false;
            this.sourceUrl = (report != null)? report.SourceUrl : null;
            this.messageText = GetMessageText(report);
            this.balloonTipText = GetBalloonTipText(report);
            this.balloonTipTitle = GetBalloonTipTitle(report);
        }

        internal Appearance GetAppearance(string feeling)
        {
            if (string.IsNullOrWhiteSpace(feeling))
            {
                feeling = ButlerFactory.Normal;
            }

            Appearance appearance;
            if (this.Appearances.TryGetValue(feeling, out appearance))
            {
                return appearance;
            }

            return this.Appearances[ButlerFactory.Normal];
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

        internal void SetErrorMessage(string errorMessage, string errorLogFile)
        {
            UpdateAppearance(ButlerFactory.Sad, null);
            this.sourceUrl = errorLogFile;
            this.messageText = errorMessage;
            this.appearance.Image = BitmapUtil.ToBitmapSource(Resources.sad);
            this.hasNews = true;
        }
    }
}
