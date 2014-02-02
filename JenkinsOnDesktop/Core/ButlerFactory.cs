using System;
using System.Windows;
using XPFriend.JenkinsOnDesktop.Animation;
using XPFriend.JenkinsOnDesktop.Properties;

namespace XPFriend.JenkinsOnDesktop.Core
{
    internal class ButlerFactory
    {
        internal const string CalmJenkins = "Calm-Jenkins";
        internal const string EmotionalJenkins = "Emotional-Jenkins";

        internal const string Normal = "Normal";
        internal const string Sad = "Sad";
        internal const string Angry = "Angry";
        internal const string Rageful = "Rageful";
        internal const string Expectant = "Expectant";
        internal const string Happy = "Happy";

        internal static Butler CreateEmotionalJenkins()
        {
            return CreateDefaultButler(ButlerFactory.EmotionalJenkins,
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
                TypicalAppearance = ButlerFactory.Sad
            };

            butler.Appearances[ButlerFactory.Normal] =
                new Appearance(null, null, Resources.Message_Report, null, Resources.jenkins);

            butler.Appearances[ButlerFactory.Sad] =
                new Appearance(null, null, Resources.Message_Report, "sad.png", Resources.sad);

            butler.Appearances[ButlerFactory.Angry] =
                new Appearance(null, null, Resources.Message_Report, "oni.png", Resources.oni);

            butler.Appearances[ButlerFactory.Rageful] =
                new Appearance(null, null, Resources.Message_Report, "onibi.png", Resources.onibi);

            butler.Appearances[ButlerFactory.Expectant] =
                new Appearance(null, null, Resources.Message_Report, "ninja.png", Resources.ninja, false);

            butler.Appearances[ButlerFactory.Happy] =
                new Appearance(null, null, Resources.Message_Report, null, Resources.jenkins);

            butler.Appearances[ButlerFactory.Rageful].Topmost = true;
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

            appearance.ExitAnimation.Add(new FadeOut()
            {
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
                Name = ButlerFactory.CalmJenkins,
                Nickname = Resources.Nickname_CalmJenkins,
                DisplayName = Resources.DisplayName_CalmJenkins,
                License = Resources.License_CalmJenkins,
                TypicalAppearance = ButlerFactory.Happy
            };

            butler.Appearances[ButlerFactory.Normal] = new Appearance(
                Resources.BalloonTip_Title, Resources.Message_Report, null, null, Resources.jenkins);

            butler.Appearances[ButlerFactory.Happy] = new Appearance(
                null, null, Resources.Message_Report, null, Resources.jenkins);

            return butler;
        }
    }
}
