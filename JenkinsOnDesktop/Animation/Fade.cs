using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public abstract class Fade : WindowAnimationBase
    {
        protected Fade(double? to) : base()
        {
            Storyboard.SetTargetProperty(this, new PropertyPath("Opacity"));
            this.To = to;
        }

        public override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            AnimationClock animationClock)
        {
            if (this.From == null)
            {
                this.From = MainWindow.Current.Opacity;
            }
            return base.GetCurrentValue(defaultOriginValue, defaultDestinationValue, animationClock);
        }
    }
}
