using System;

namespace SharpSenses.Gestures {
    public class MovementUp : Movement {
        public MovementUp(Item item, double distance) : base(item, distance) {}
        protected override double GetProgress(Point3D currentLocation) {
            return Math.Abs(StartPosition.Y - currentLocation.Y);            
        }

        protected override bool IsRightDirection(Point3D currentLocation) {
            return currentLocation.Y >= LastPosition.Y;            
        }
    }
}