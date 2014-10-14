using System;

namespace SharpSenses.Gestures {
    public interface IGestureSensor {
        event Action SwipeLeft;
        event Action SwipeRight;
        event Action SwipeUp;
        event Action SwipeDown;
        event Action MoveForward;
    }
}