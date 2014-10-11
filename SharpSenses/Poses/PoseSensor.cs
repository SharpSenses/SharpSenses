using System;

namespace SharpSenses.Poses {
    public class PoseSensor : IPoseSensor {
        public event Action PosePeaceEnd;
        public event Action PosePeaceBegin;
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

            
        public void OnPosePeaceBegin() {
            Action handler = PosePeaceBegin;
            if (handler != null) handler();
        }

        public void OnPoseThumbsUpBegin() {
            Action handler = PoseThumbsUpBegin;
            if (handler != null) handler();
        }

        public void OnPoseThumbsDownBegin() {
            Action handler = PoseThumbsDownBegin;
            if (handler != null) handler();
        }

        public void OnPosePeaceEnd() {
            Action handler = PosePeaceEnd;
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