using System;
using System.Collections.Generic;

namespace SharpSenses.RealSense.Capabilities {
    public interface ICapability : IDisposable {
        IEnumerable<Capability> Dependencies { get; }
        void Configure(RealSenseCamera camera);
        void Loop(LoopObjects loopObjects);
    }
}