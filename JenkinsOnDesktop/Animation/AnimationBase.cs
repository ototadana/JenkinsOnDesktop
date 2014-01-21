using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public abstract class WindowAnimationBase : AnimationTimeline
    {
        protected double? From { get; set; }
        protected double? To { get; set; }

        public override Type TargetPropertyType
        {
            get
            {
                base.ReadPreamble();
                return typeof(double);
            }
        }

        protected WindowAnimationBase()
            : base()
        {
            Storyboard.SetTarget(this, WindowOperator.GetInstance());
            this.Duration = new Duration(TimeSpan.FromSeconds(1));
        }

        public override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            AnimationClock animationClock)
        {
            return this.From + (this.To - this.From) * animationClock.CurrentProgress.Value;
        }

        protected override bool ShouldSerializeProperty(DependencyProperty dp)
        {
            if (dp == Storyboard.TargetPropertyProperty)
            {
                return false;
            }
            return base.ShouldSerializeProperty(dp);
        }
    }
}
