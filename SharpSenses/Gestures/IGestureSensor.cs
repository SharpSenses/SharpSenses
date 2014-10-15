using System;

namespace SharpSenses.Gestures {
    public interface IGestureSensor {
        event Action<Hand> SwipeLeft;
        event Action<Hand> SwipeRight;
        event Action<Hand> SwipeUp;
        event Action<Hand> SwipeDown;
        event Action<Hand> MoveForward;
    }
}