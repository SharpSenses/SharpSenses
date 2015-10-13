using System.Collections.Generic;

namespace SharpSenses.RealSense.Capabilities {
    public interface ICapability {
        IEnumerable<Capability> Dependencies { get; }
        void Configure(RealSenseCamera camera);
        void Loop();
    }
}