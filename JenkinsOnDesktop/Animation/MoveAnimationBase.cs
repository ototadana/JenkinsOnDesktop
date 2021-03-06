﻿using System.Windows;
using System.Windows.Media.Animation;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public abstract class MoveAnimationBase : WindowAnimationBase
    {
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Direction), typeof(MoveAnimationBase));

        public Direction Direction
        {
            get { return (Direction)base.GetValue(DirectionProperty); }
            set { base.SetValue(DirectionProperty, value); }
        }

        protected MoveAnimationBase() : base()
        {
            UpdateProperties();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.NewValue is Direction)
            {
                UpdateProperties();
            }
        }

        protected virtual void UpdateProperties()
        {
            if (this.Direction == Direction.Left || 
                this.Direction == Direction.Right)
            {
                Storyboard.SetTargetProperty(this, new PropertyPath("Left"));
            }
            else
            {
                Storyboard.SetTargetProperty(this, new PropertyPath("Top"));
            }
        }

        protected internal override object GetCurrentValue(
            object defaultOriginValue, 
            object defaultDestinationValue, 
            double clockValue)
        {
            if (this.From == null)
            {
                this.To = GetToPropertyValue();
                this.From = GetFromPropertyValue();
            }
            return base.GetCurrentValue(clockValue);
        }

        protected abstract double? GetToPropertyValue();

        protected abstract double? GetFromPropertyValue();
    }
}
