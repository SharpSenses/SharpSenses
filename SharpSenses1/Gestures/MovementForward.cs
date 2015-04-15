using System;

namespace SharpSenses.Gestures {
    public class MovementForward : Movement {
        public MovementForward(Item item, double distance) : base(item, distance) {}

        protected override double GetProgress(Point3D currentLocation) {
            return Math.Abs(StartPosition.Z - currentLocation.Z);
        }

        protected override bool IsGoingRightDirection(Point3D currentLocation) {
            return currentLocation.Z <= LastPosition.Z &&
                   Math.Abs(StartPosition.Y - currentLocation.Y) < ToleranceForWrongDirection &&
                   Math.Abs(StartPosition.X - currentLocation.X) < ToleranceForWrongDirection;
        }
    }
}