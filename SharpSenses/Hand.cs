using System;
using System.Collections.Generic;

namespace SharpSenses {
    public class Hand : FlexiblePart {
        public Finger Thumb { get; private set; }
        public Finger Index { get; private set; }
        public Finger Middle { get; private set; }
        public Finger Ring { get; private set; }
        public Finger Pinky { get; private set; }
        public Side Side { get; set; }

        public event Action<Finger> FingerOpened;
        public event Action<Finger> FingerClosed;

        public Hand(Side side) {
            Thumb = new Finger(FingerKind.Thumb);
            Index = new Finger(FingerKind.Index);
            Middle = new Finger(FingerKind.Middle);
            Ring = new Finger(FingerKind.Ring);
            Pinky = new Finger(FingerKind.Pinky);
            Side = side;
            var fingers = GetAllFingers();
            fingers.ForEach(f => f.Opened += (s, a) => OnFingerOpened(f));
            fingers.ForEach(f => f.Closed += (s, a) => OnFingerClosed(f));
        }


        public List<Finger> GetAllFingers() {
            return new List<Finger> {
                Thumb,
                Index,
                Middle,
                Ring,
                Pinky
            };
        }

        public override string GetInfo() {
            string fingers = Thumb.IsVisible ? "1" : "0";
            fingers+= Index.IsVisible ? "1" : "0";
            fingers+= Middle.IsVisible ? "1" : "0";
            fingers+= Ring.IsVisible ? "1" : "0";
            fingers+= Pinky.IsVisible ? "1" : "0";
            return Side + " F:" + fingers + " " + base.GetInfo();
        }

        protected virtual void OnFingerOpened(Finger finger) {
            var handler = FingerOpened;
            if (handler != null) handler(finger);
        }

        protected virtual void OnFingerClosed(Finger finger) {
            var handler = FingerClosed;
            if (handler != null) handler(finger);
        }
    }

    public enum Side {
        Left, Right
    }
}