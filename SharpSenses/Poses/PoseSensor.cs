using System;

namespace SharpSenses.Poses {
    public class PoseSensor : IPoseSensor {
        public event EventHandler<HandPoseEventArgs> PeaceBegin;
        public event EventHandler<HandPoseEventArgs> PeaceEnd;
            
        public void OnPosePeaceBegin(Hand hand) {
            var handler = PeaceBegin;
            if (handler != null) handler(this, new HandPoseEventArgs(hand));
        }

        public void OnPosePeaceEnd(Hand hand) {
            var handler = PeaceEnd;
            if (handler != null) handler(this, new HandPoseEventArgs(hand));
        }
    }
}