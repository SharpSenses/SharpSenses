using System;

namespace SharpSenses.Util {
    public static class MathEx {
        public static double CalcDistance(double x1, double y1, double x2, double y2) {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}