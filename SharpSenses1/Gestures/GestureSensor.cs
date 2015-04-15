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
            var handler = SlideLeft;
            if (handler != null) handler(this, e);
        }

        public virtual void OnSlideRight(GestureEventArgs e) {
            var handler = SlideRight;
            if (handler != null) handler(this, e);
        }

        public virtual void OnSlideUp(GestureEventArgs e) {
            var handler = SlideUp;
            if (handler != null) handler(this, e);
        }

        public virtual void OnSlideDown(GestureEventArgs e) {
            var handler = SlideDown;
            if (handler != null) handler(this, e);
        }

        public virtual void OnMoveForward(GestureEventArgs e) {
            var handler = MoveForward;
            if (handler != null) handler(this, e);
        }

        public virtual void OnWave(GestureEventArgs e) {
            var handler = Wave;
            if (handler != null) handler(this, e);
        }
    }
}