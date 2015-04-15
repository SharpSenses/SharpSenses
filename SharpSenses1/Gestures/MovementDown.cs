using System;

namespace SharpSenses.Gestures {
    public class MovementDown : Movement {
        public MovementDown(Item item, double distance) : base(item, distance) { }
        protected override double GetProgress(Point3D currentLocation) {
            return Math.Abs(StartPosition.Y - currentLocation.Y);
        }

        protected override bool IsGoingRightDirection(Point3D currentLocation) {
            return currentLocation.Y <= LastPosition.Y &&
                   Math.Abs(StartPosition.Z - currentLocation.Z) < ToleranceForWrongDirection &&
                   Math.Abs(StartPosition.X - currentLocation.X) < ToleranceForWrongDirection;
        }
    }
}