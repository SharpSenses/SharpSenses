using System;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses {
    public interface ICamera : IDisposable {
        Hand LeftHand { get; }
        Hand RightHand { get; }
        IGestureSensor Gestures { get; set; }
        IPoseSensor Poses { get; set; }
        void Start();
    }
}