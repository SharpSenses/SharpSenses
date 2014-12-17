using System;

namespace SharpSenses.Gestures {
    public class GestureSensor : IGestureSensor {
        public event EventHandler<GestureEventArgs> SlideLeft;
        public event EventHandler<GestureEventArgs> SlideRight;
        public event EventHandler<GestureEventArgs> SlideUp;
        public event EventHandler<GestureEventArgs> SlideDown;
        public event EventHandler<GestureEventArgs> MoveForward;

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
    }

    public static class GestureSlide {

        private static GestureSensor _gestureSensor;

        public static void Configue(ICamera camera, GestureSensor gestureSensor) {
            _gestureSensor = gestureSensor;
            CreateSlideGesture(camera.LeftHand, Direction.Left, g => _gestureSensor.OnSlideLeft(g));
            CreateSlideGesture(camera.RightHand, Direction.Left, g => _gestureSensor.OnSlideLeft(g));

            CreateSlideGesture(camera.LeftHand, Direction.Right, g => _gestureSensor.OnSlideRight(g));
            CreateSlideGesture(camera.RightHand, Direction.Right, g => _gestureSensor.OnSlideRight(g));

            CreateSlideGesture(camera.LeftHand, Direction.Up, g => _gestureSensor.OnSlideUp(g));
            CreateSlideGesture(camera.RightHand, Direction.Up, g => _gestureSensor.OnSlideUp(g));

            CreateSlideGesture(camera.LeftHand, Direction.Down, g => _gestureSensor.OnSlideDown(g));
            CreateSlideGesture(camera.RightHand, Direction.Down, g => _gestureSensor.OnSlideDown(g));

            CreateSlideGesture(camera.LeftHand, Direction.Forward, g => _gestureSensor.OnMoveForward(g));
            CreateSlideGesture(camera.RightHand, Direction.Forward, g => _gestureSensor.OnMoveForward(g));
        }
   
        private static void CreateSlideGesture(Hand hand, Direction direction, Action<GestureEventArgs> handler) {
            var swipe = new Gesture("hand" + hand.Side + "_" + direction);
            swipe.AddStep(1000, Movement.CreateMovement(direction, hand, 11));
            swipe.Activate();
            swipe.GestureDetected += (s, a) => handler.Invoke(a);
        }
    }
}