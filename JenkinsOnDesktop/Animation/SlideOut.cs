using System.Windows;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class SlideOut : MoveAnimationBase
    {
        public SlideOut() : base()
        {
            this.AccelerationRatio = 0.8;
        }

        protected override double? GetToPropertyValue()
        {
            Direction direction = this.Direction;

            MainWindow window = MainWindow.Current;
            if (direction == Direction.Left)
            {
                return -window.ScreenWidth;
            }
            else if (direction == Direction.Right)
            {
                return window.ScreenWidth * 2;
            }
            else if (direction == Direction.Top)
            {
                return -window.ScreenHeight;
            }
            else // if (direction == Direction.Bottom)
            {
                return window.ScreenHeight * 2;
            }
        }

        protected override double? GetFromPropertyValue()
        {
            Window window = MainWindow.Current;
            Direction direction = this.Direction;
            if (direction == Direction.Left || 
                direction == Direction.Right || 
                direction == Direction.Center)
            {
                return window.Left;
            }
            else // if (direction == Direction.Top || 
                 //     direction == Direction.Bottom || 
                 //     direction == Direction.Middle)
            {
                return window.Top;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new SlideOut();
        }
    }
}
