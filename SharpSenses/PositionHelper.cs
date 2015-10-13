using System;

namespace SharpSenses {
    public static class PositionHelper {
        public static Position CreatePosition(Point3D imagePosition, Point3D worldPosition) {
            return new Position {
                Image = new Point3D(imagePosition.X, imagePosition.Y),
                World = new Point3D(ToRoundedCentimeters(worldPosition.X),
                    ToRoundedCentimeters(worldPosition.Y),
                    ToRoundedCentimeters(worldPosition.Z))
            };
        }

        private static double ToRoundedCentimeters(double value) {
            return Math.Round(value * 100, 2);
        }
    }
}