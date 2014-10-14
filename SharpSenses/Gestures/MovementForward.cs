using System;

namespace SharpSenses.Gestures {
    public class MovementForward : Movement {
        public MovementForward(Item item, double distance) : base(item, distance) {}

        protected override Point3d RemoveNoise(Point3d position) {
            position.Z = Math.Round(position.Z, 2);
            return position;
        }

        protected override bool IsRightDirection(Point3d currentLocation) {
            return currentLocation.Z <= LastPosition.Z;
        }

        protected override double GetProgress(Point3d currentLocation) {
            return Math.Abs(StartPosition.Z - currentLocation.Z);
        }
    }
}