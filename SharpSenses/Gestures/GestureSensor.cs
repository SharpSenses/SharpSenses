using System;

namespace SharpSenses.Gestures {
    public class GestureSensor : IGestureSensor {
        public event EventHandler<GestureEventArgs> SlideLeft;
        public event EventHandler<GestureEventArgs> SlideRight;
        public event EventHandler<GestureEventArgs> SlideUp;
        public event EventHandler<GestureEventArgs> SlideDown;
        public event EventHandler<GestureEventArgs> MoveForward;
        public event EventHandler<GestureEventArgs> Wave;

        //public GestureSensor(ICamera camera) {
        //    CreateSlideGesture(camera.LeftHand, Direction.Left, SlideLeft);
        //    CreateSlideGesture(camera.RightHand, Direction.Left, SlideLeft);

        //    CreateSlideGesture(camera.LeftHand, Direction.Right, SlideRight);
        //    CreateSlideGesture(camera.RightHand, Direction.Right, SlideRight);

        //    CreateSlideGesture(camera.LeftHand, Direction.Up, SlideUp);
        //    CreateSlideGesture(camera.RightHand, Direction.Up, SlideUp);

        //    CreateSlideGesture(camera.LeftHand, Direction.Down, SlideDown);
        //    CreateSlideGesture(camera.RightHand, Direction.Down, SlideDown);

        //    CreateSlideGesture(camera.LeftHand, Direction.Forward, MoveForward);
        //    CreateSlideGesture(camera.RightHand, Direction.Forward, MoveForward);
        //}

        
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