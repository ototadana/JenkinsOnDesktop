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

        protected internal override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            double clockValue)
        {
            if (this.From == null)
            {
                this.From = MainWindow.Current.Opacity;
            }
            return base.GetCurrentValue(clockValue);
        }
    }
}
