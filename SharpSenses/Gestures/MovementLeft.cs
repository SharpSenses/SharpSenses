using System;

namespace SharpSenses.Gestures {
    public class MovementLeft : Movement {
        public MovementLeft(Item item, double distance) : base(item, distance) {}
        protected override double GetProgress(Point3d currentLocation) {
            return Math.Abs(StartPosition.X - currentLocation.X);
        }

        protected override bool IsRightDirection(Point3d currentLocation) {
            return currentLocation.X <= LastPosition.X;            
        }
    }
}