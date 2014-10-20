using System;

namespace SharpSenses.Poses {
    public class PoseSensor : IPoseSensor {
        public event Action<Hand> PeaceBegin;
        public event Action<Hand> PeaceEnd;

        public event Action PoseThumbsUpBegin;
        public event Action PoseThumbsUpEnd;
        public event Action PoseThumbsDownBegin;
        public event Action PoseThumbsDownEnd;
        public event Action BigFiveBegin;
        public event Action BigFiveEnd;

        public virtual void OnBigFiveEnd() {
            Action handler = BigFiveEnd;
            if (handler != null) handler();
        }

        public virtual void OnBigFiveBegin() {
            Action handler = BigFiveBegin;
            if (handler != null) handler();
        }

            
        public void OnPosePeaceBegin(Hand hand) {
            Action<Hand> handler = PeaceBegin;
            if (handler != null) handler(hand);
        }

        public void OnPosePeaceEnd(Hand hand) {
            Action<Hand> handler = PeaceEnd;
            if (handler != null) handler(hand);
        }

        public void OnPoseThumbsUpBegin() {
            Action handler = PoseThumbsUpBegin;
            if (handler != null) handler();
        }

        public void OnPoseThumbsDownBegin() {
            Action handler = PoseThumbsDownBegin;
            if (handler != null) handler();
        }

        public void OnPoseThumbsUpEnd() {
            Action handler = PoseThumbsUpEnd;
            if (handler != null) handler();
        }

        public void OnPoseThumbsDownEnd() {
            Action handler = PoseThumbsDownEnd;
            if (handler != null) handler();
        }
    }
}