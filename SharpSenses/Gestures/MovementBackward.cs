using System;

namespace SharpSenses.Gestures {
    public class MovementBackward : MovementForward {
        public MovementBackward(double distance, TimeSpan window) : base(distance, window) { }

        protected override bool IsRightDirection(Point3D currentLocation) {
            return currentLocation.Z >= StartPosition.Z;
        }
    }
}