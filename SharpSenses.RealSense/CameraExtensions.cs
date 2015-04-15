using SharpSenses.RealSense;

namespace SharpSenses {
    public static class CameraExtensions {

        public static ICamera Create() {
            return new RealSenseCamera();
        }
    }
}
