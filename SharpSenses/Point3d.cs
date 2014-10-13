using System;

namespace SharpSenses {
    public struct Point3d {
        private double _x;
        private double _y;
        private double _z;

        public double X {
            get { return _x; }
            set { _x = value; }
        }

        public double Y {
            get { return _y; }
            set { _y = value; }
        }

        public double Z {
            get { return _z; }
            set { _z = value; }
        }

        public Point3d(double x = 0, double y = 0, double z = 0) {
            _x = x;
            _y = y;
            _z = z;
        }

        public bool Equals(Point3d other) {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public Point3d Clone() {
            return new Point3d(_x, _y, _z);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point3d && Equals((Point3d) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode*397) ^ Y.GetHashCode();
                hashCode = (hashCode*397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public static Point3d Zero {
            get { return new Point3d(); }
        }

        public static Point3d operator +(Point3d a, Point3d b) {
            return new Point3d {
                X = a.X + b.X,
                Y = a.Y + b.Y,
                Z = a.Z + b.Z
            };
        }

        public static Point3d operator -(Point3d a, Point3d b) {
            return new Point3d {
                X = a.X - b.X,
                Y = a.Y - b.Y,
                Z = a.Z - b.Z
            };
        }

        public static Point3d operator *(Point3d a, int times) {
            return new Point3d {
                X = a.X*times,
                Y = a.Y*times,
                Z = a.Z*times
            };
        }

        public override string ToString() {
            return String.Format("Point3d x:{0} y:{1} z:{2}", X,Y,Z);
        }

        //public Point MapToScreen(int screenWidth, int screenHeight) {
        //    screenWidth = Convert.ToInt32(screenWidth*1.75);
        //    screenHeight = Convert.ToInt32(screenHeight * 1.5);
        //    var left = (int)(screenWidth - (X / Camera.ResolutionWidth) * screenWidth);
        //    var top = (int)((Y / Camera.ResolutionHeight) * screenHeight);
        //    left = left - screenWidth/5;
        //    top = top - screenHeight/4;
        //    return new Point(left, top);
        //}
    }
}