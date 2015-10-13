using System;

namespace SharpSenses.Gestures {
    public class GestureSensor : IGestureSensor {
        public event EventHandler<GestureEventArgs> SlideLeft;
        public event EventHandler<GestureEventArgs> SlideRight;
        public event EventHandler<GestureEventArgs> SlideUp;
        public event EventHandler<GestureEventArgs> SlideDown;
        public event EventHandler<GestureEventArgs> MoveForward;
        public event EventHandler<GestureEventArgs> Wave;

        public virtual void OnSlideLeft(GestureEventArgs e) {
            SlideLeft?.Invoke(this, e);
        }

        public virtual void OnSlideRight(GestureEventArgs e) {
            SlideRight?.Invoke(this, e);
        }

        public virtual void OnSlideUp(GestureEventArgs e) {
            SlideUp?.Invoke(this, e);
        }

        public virtual void OnSlideDown(GestureEventArgs e) {
            SlideDown?.Invoke(this, e);
        }

        public virtual void OnMoveForward(GestureEventArgs e) {
            MoveForward?.Invoke(this, e);
        }

        public virtual void OnWave(GestureEventArgs e) {
            Wave?.Invoke(this, e);
        }
    }
}