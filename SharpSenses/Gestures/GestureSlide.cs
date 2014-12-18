using System;

namespace SharpSenses.Gestures {
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
            swipe.AddStep(300, Movement.CreateMovement(direction, hand, 12));
            swipe.Activate();
            swipe.GestureDetected += (s, a) => handler.Invoke(a);
        }
    }
}