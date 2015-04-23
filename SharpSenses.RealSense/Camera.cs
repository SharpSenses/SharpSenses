using SharpSenses.RealSense;

namespace SharpSenses {
    public abstract class Camera : BaseCamera {
        public static ICamera Create() {
            RealSenseAssembliesLoader.Load();
            return new RealSenseCamera();
        }
    }
}