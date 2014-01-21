using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class SlideIn : MoveAnimationBase
    {
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(SlideIn));

        private bool located;

        public Position Position
        {
            get { return (Position)base.GetValue(PositionProperty); }
            set { base.SetValue(PositionProperty, value); }
        }

        public SlideIn() : base() 
        {
            DecelerationRatio = 0.8;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new SlideIn();
        }

        protected override double? GetFromPropertyValue()
        {
            MainWindow window = MainWindow.Current;
            Direction direction = this.Direction;
            if (direction == Direction.Left)
            {
                return window.ScreenWidth * 2;
            }
            else if(direction == Direction.Right || direction == Direction.Center)
            {
                return -window.ScreenWidth;
            }
            else if(direction == Direction.Top)
            {
                return window.ScreenHeight * 2;
            }
            else // if (direction == Direction.Bottom || direction == Direction.Middle)
            {
                return -window.ScreenHeight;
            }
        }

        protected override double? GetToPropertyValue()
        {
            MainWindow window = MainWindow.Current;
            Direction direction = this.Direction;
            if (direction == Direction.Left || 
                direction == Direction.Center ||
                direction == Direction.Right)
            {
                return WindowOperator.GetLeft(window, this.Position);
            }
            else
            {
                return WindowOperator.GetTop(window, this.Position);
            }
        }

        public override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            AnimationClock animationClock)
        {
            if (!this.located)
            {
                Locate();
                this.located = true;
            }
            return base.GetCurrentValue(defaultOriginValue, defaultDestinationValue, animationClock);
        }

        private void Locate()
        {
            MainWindow window = MainWindow.Current;
            Direction direction = this.Direction;
            if (direction == Direction.Left || 
                direction == Direction.Center ||
                direction == Direction.Right)
            {
                window.Top = WindowOperator.GetTop(window, this.Position);
            }
            else
            {
                window.Left = WindowOperator.GetLeft(window, this.Position);
            }
        }
    }
}
