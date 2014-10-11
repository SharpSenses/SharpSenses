using System;

namespace SharpSenses.Gestures {
    public interface IGestureSensor {
        event Action GestureSwipeLeft;
        event Action GestureSwipeRight;
        event Action GestureSwipeUp;
        event Action GestureSwipeDown;
        event Action GestureHandWave;
        event Action GestureHandCircle;
    }
}