using System.Collections;
using System.ComponentModel;

namespace XPFriend.JenkinsOnDesktop.Animation
{
    public class Timelines : ArrayList
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int Capacity
        {
            get { return base.Capacity; }
            set { base.Capacity = value; }
        }
    }
}
