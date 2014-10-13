using System;
using System.Drawing;

namespace SharpSenses.Gestures {
    public enum Direction {
        None,
        Forward,
        Backward,
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionHelper {
        public static Direction GetDirection(Point3d before, Point3d after) {
            var dif = after - before;
            var mz = Math.Abs(dif.Z);
            var mx = Math.Abs(dif.X);
            var my = Math.Abs(dif.Y);
            if (mz > mx && mz > my) {
                return dif.Z > 0 ? Direction.Backward : Direction.Forward;
            }
            if (mx > my) {
                return dif.X > 0 ? Direction.Left : Direction.Right;
            }
            return dif.Y > 0 ? Direction.Up : Direction.Down;
        }

        public static Direction GetDirection(Point before, Point after) {
            return GetDirection(new Point3d(before.X, before.Y), new Point3d(after.X, after.Y));
        }
    }
}