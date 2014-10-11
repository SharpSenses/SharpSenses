using System;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses {
    public interface ICamera : IDisposable {
        Hand LeftHand { get; }
        Hand RightHand { get; }
        IGestureSensor GestureSensor { get; set; }
        IPoseSensor PoseSensor { get; set; }
        void Start();
    }
}