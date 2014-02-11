using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class FadeIn : Fade
    {
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(FadeIn));

        private bool located;

        public Position Position
        {
            get { return (Position)base.GetValue(PositionProperty); }
            set { base.SetValue(PositionProperty, value); }
        }

        public FadeIn() : base(1.0) { }

        protected override Freezable CreateInstanceCore()
        {
            return new FadeIn();
        }

        protected internal override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            double clockValue)
        {
            if (!this.located)
            {
                WindowOperator.GetInstance().SetValue(WindowOperator.PositionProperty, this.Position);
                this.located = true;
            }
            return base.GetCurrentValue(defaultOriginValue, defaultDestinationValue, clockValue);
        }
    }
}
