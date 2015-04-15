using System;
using System.Diagnostics;

namespace SharpSenses.Util {
    public static class MathEx {
        public static double CalcDistance(double x1, double y1, double x2, double y2) {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static double CalcDistance(Point3D p1, Point3D p2) {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}