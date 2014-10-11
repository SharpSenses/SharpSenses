using System;

namespace SharpSenses.Gestures {
    public class MovementBackward : MovementForward {
        public MovementBackward(Item item, double distance) : base(item, distance) { }

        protected override bool IsRightDirection(Point3D currentLocation) {
            return currentLocation.Z >= StartPosition.Z;
        }
    }
}