using System;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses {
    public interface ICamera : IDisposable {
        int ResolutionWidth { get; }
        int ResolutionHeight { get; }
        Hand LeftHand { get; }
        Hand RightHand { get; }
        Face Face { get; }
        IGestureSensor Gestures { get; }
        IPoseSensor Poses { get; }
        void Start();
    }
}