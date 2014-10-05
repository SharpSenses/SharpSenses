using SharpPerceptual;

namespace SharpSenses {
    public class Hand : FlexiblePart {
        public Finger Thumb { get; private set; }
        public Finger Index { get; private set; }
        public Finger Middle { get; private set; }
        public Finger Ring { get; private set; }
        public Finger Pinky { get; private set; }
        public Side Side { get; set; }

        public Hand(Side side) {
            Thumb = new Finger();
            Index = new Finger();
            Middle = new Finger();
            Ring = new Finger();
            Pinky = new Finger();
            Side = side;
        }

        public override string GetInfo() {
            string fingers = Thumb.IsVisible ? "1" : "0";
            fingers+= Index.IsVisible ? "1" : "0";
            fingers+= Middle.IsVisible ? "1" : "0";
            fingers+= Ring.IsVisible ? "1" : "0";
            fingers+= Pinky.IsVisible ? "1" : "0";
            return Side + " F:" + fingers + " " + base.GetInfo();
        }
    }

    public enum Side {
        Left, Right
    }
}