using System.Windows;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class Align : MoveAnimationBase
    {
        public Align()
            : base()
        {
            this.AccelerationRatio = 0.8;
        }

        protected override double? GetToPropertyValue()
        {
            Direction direction = this.Direction;
            MainWindow window = MainWindow.Current;
            if (window == null)
            {
                return 0.0;
            }

            if (direction == Direction.Left)
            {
                return WindowOperator.GetLeft(window);
            }
            else if (direction == Direction.Center)
            {
                return WindowOperator.GetCenter(window);
            }
            else if (direction == Direction.Right)
            {
                return WindowOperator.GetRight(window);
            }
            else if (direction == Direction.Top)
            {
                return WindowOperator.GetTop(window);
            }
            else if (direction == Direction.Middle)
            {
                return WindowOperator.GetMiddle(window);
            }
            else // if (direction == Direction.Bottom)
            {
                return WindowOperator.GetBottom(window);
            }
        }

        protected override double? GetFromPropertyValue()
        {
            Direction direction = this.Direction;
            Window window = MainWindow.Current;
            if (direction == Direction.Left ||
                direction == Direction.Center ||
                direction == Direction.Right)
            {
                return window.Left;
            }
            else
            {
                return window.Top;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Align();
        }
    }
}
