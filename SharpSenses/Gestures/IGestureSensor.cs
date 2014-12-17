using System;

namespace SharpSenses.Gestures {
    public interface IGestureSensor {
        event EventHandler<GestureEventArgs> SlideLeft;
        event EventHandler<GestureEventArgs> SlideRight;
        event EventHandler<GestureEventArgs> SlideUp;
        event EventHandler<GestureEventArgs> SlideDown;
        event EventHandler<GestureEventArgs> MoveForward;
    }
}