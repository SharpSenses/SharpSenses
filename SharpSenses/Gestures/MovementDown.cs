using System;

namespace SharpSenses.Gestures {
    public class MovementDown : Movement {
        public MovementDown(Item item, double distance) : base(item, distance) { }
        protected override double GetProgress(Point3d currentLocation) {
            return Math.Abs(StartPosition.Y - currentLocation.Y);
        }

        protected override bool IsRightDirection(Point3d currentLocation) {
            return currentLocation.Y <= LastPosition.Y;
        }
    }
}