using System;

namespace SharpSenses.Gestures {
    public class MovementRight : Movement {
        public MovementRight(Item item, double distance) : base(item, distance) {}
        protected override double GetProgress(Point3d currentLocation) {
            return Math.Abs(StartPosition.X - currentLocation.X);
        }

        protected override bool IsRightDirection(Point3d currentLocation) {
            return currentLocation.X >= LastPosition.X;
        }
    }
}