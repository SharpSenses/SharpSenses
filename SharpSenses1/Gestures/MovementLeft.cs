using System;
using SharpSenses.Poses;

namespace SharpSenses.Gestures {
    public class MovementLeft : Movement {
        public MovementLeft(Item item, double distance) : base(item, distance) { }
        protected override double GetProgress(Point3D currentLocation) {
            return Math.Abs(StartPosition.X - currentLocation.X);
        }

        protected override bool IsGoingRightDirection(Point3D currentLocation) {
            return currentLocation.X >= LastPosition.X &&
                   Math.Abs(StartPosition.Y - currentLocation.Y) < ToleranceForWrongDirection &&
                   Math.Abs(StartPosition.Z - currentLocation.Z) < ToleranceForWrongDirection;
        }
    }
}