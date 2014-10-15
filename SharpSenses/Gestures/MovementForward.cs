using System;

namespace SharpSenses.Gestures {
    public class MovementForward : Movement {
        public MovementForward(Item item, double distance) : base(item, distance) {}

        protected override bool IsRightDirection(Point3D currentLocation) {
            return currentLocation.Z <= LastPosition.Z;
        }

        protected override double GetProgress(Point3D currentLocation) {
            return Math.Abs(StartPosition.Z - currentLocation.Z);
        }
    }
}