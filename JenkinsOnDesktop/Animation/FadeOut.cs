using System.Windows;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class FadeOut : Fade
    {
        public FadeOut() : base(0.0) { }

        protected override Freezable CreateInstanceCore()
        {
            return new FadeOut();
        }
    }
}
