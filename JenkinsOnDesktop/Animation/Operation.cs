using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class Operation : AnimationTimeline
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(Command), typeof(Operation));

        public Command Command
        {
            get { return (Command)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }

        public override Type TargetPropertyType
        {
            get { return typeof(Command); }
        }

        public Operation()
        {
            this.Duration = new Duration(TimeSpan.FromMilliseconds(10));
            Storyboard.SetTarget(this, WindowOperator.GetInstance());
            Storyboard.SetTargetProperty(this, new PropertyPath("Command"));
        }

        public override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            AnimationClock animationClock)
        {
            return this.Command;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Operation();
        }

        protected override bool ShouldSerializeProperty(DependencyProperty dp)
        {
            if (dp == Storyboard.TargetPropertyProperty || dp == Timeline.DurationProperty)
            {
                return false;
            }
            return base.ShouldSerializeProperty(dp);
        }
    }
}
