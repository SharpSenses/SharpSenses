using System;

namespace SharpSenses.Poses {
    public interface IPoseSensor {
        event EventHandler<HandPoseEventArgs> PeaceBegin;
        event EventHandler<HandPoseEventArgs> PeaceEnd;
    }
}