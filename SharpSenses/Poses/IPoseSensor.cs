using System;

namespace SharpSenses.Poses {
    public interface IPoseSensor {
        event Action<Hand> PeaceBegin;
        event Action<Hand> PeaceEnd;
    }
}