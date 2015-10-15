using SharpSenses.RealSense;
using SharpSenses.RealSense.Capabilities;

namespace SharpSenses {
    public abstract class Camera : BaseCamera {

        public static ICamera Create(params Capability[] capabilities) {
            RealSenseAssembliesLoader.Load();
            var camera = new RealSenseCamera();
            if (capabilities != null) {
                foreach (var capability in capabilities) {
                    camera.AddCapability(capability);
                }
            }
            return camera;
        }

        public static ICamera Create() {
            return Create(CapabilityHelper.All());
        }
    }
}