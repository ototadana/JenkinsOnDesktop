using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class Locate : AnimationTimeline
    {
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(Locate));

        public Position Position
        {
            get { return (Position)base.GetValue(PositionProperty); }
            set { base.SetValue(PositionProperty, value); }
        }

        public override Type TargetPropertyType { get { return typeof(Position); } }

        public Locate()
        {
            this.Duration = new Duration(TimeSpan.FromMilliseconds(10));
            Storyboard.SetTarget(this, WindowOperator.GetInstance());
            Storyboard.SetTargetProperty(this, new PropertyPath("Position"));
        }

        public override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            AnimationClock animationClock)
        {
            return this.Position;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Locate();
        }
    }
}
