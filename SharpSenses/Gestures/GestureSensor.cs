using System;
using System.Security.Policy;

namespace SharpSenses.Gestures {
    public class GestureSensor : IGestureSensor {

        public event Action SwipeLeft;
        public event Action SwipeRight;
        public event Action SwipeUp;
        public event Action SwipeDown;


        public GestureSensor(ICamera camera) {
            CreateSwipeGesture(camera.LeftHand, Direction.Left).GestureDetected += OnGestureSwipeLeft;
            CreateSwipeGesture(camera.RightHand, Direction.Left).GestureDetected += OnGestureSwipeLeft;

            CreateSwipeGesture(camera.LeftHand, Direction.Right).GestureDetected += OnGestureSwipeRight;
            CreateSwipeGesture(camera.RightHand, Direction.Right).GestureDetected += OnGestureSwipeRight;

            CreateSwipeGesture(camera.LeftHand, Direction.Up).GestureDetected += OnGestureSwipeUp;
            CreateSwipeGesture(camera.RightHand, Direction.Up).GestureDetected += OnGestureSwipeUp;

            CreateSwipeGesture(camera.LeftHand, Direction.Down).GestureDetected += OnGestureSwipeDown;
            CreateSwipeGesture(camera.RightHand, Direction.Down).GestureDetected += OnGestureSwipeDown;
        }

        private Gesture CreateSwipeGesture(Hand hand, Direction direction) {
            var swipe = new Gesture();
            swipe.AddStep(1000, Movement.CreateMovement(direction, hand, 20));
            swipe.Activate();
            return swipe;
        }

        public void OnGestureSwipeRight() {
            Action handler = SwipeRight;
            if (handler != null) handler();
        }

        public void OnGestureSwipeLeft() {
            Action handler = SwipeLeft;
            if (handler != null) handler();
        }

        public virtual void OnGestureSwipeUp() {
            Action handler = SwipeUp;
            if (handler != null) handler();
        }
        public virtual void OnGestureSwipeDown() {
            Action handler = SwipeDown;
            if (handler != null) handler();
        }
    }
}