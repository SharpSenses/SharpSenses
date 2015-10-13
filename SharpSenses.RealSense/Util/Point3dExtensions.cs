using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSenses.RealSense.Util {
    public static class Point3DExtensions {
        public static Point3D ToPoint3D(this PXCMPointF32 p) {
            return new Point3D(p.x, p.y);
        }

        public static Point3D ToPoint3D(this PXCMPoint3DF32 p) {
            return new Point3D(p.x, p.y, p.z);
        }
    }
}
