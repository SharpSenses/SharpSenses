using System;

namespace SharpSenses.Gestures {
    public class GestureSensor : IGestureSensor {

        public event Action<Hand> SwipeLeft;
        public event Action<Hand> SwipeRight;
        public event Action<Hand> SwipeUp;
        public event Action<Hand> SwipeDown;
        public event Action<Hand> MoveForward;

        public GestureSensor(ICamera camera) {
            CreateSwipeGesture(camera.LeftHand, Direction.Left).GestureDetected += () => OnGestureSwipeLeft(camera.LeftHand);
            CreateSwipeGesture(camera.RightHand, Direction.Left).GestureDetected += () => OnGestureSwipeLeft(camera.RightHand);

            CreateSwipeGesture(camera.LeftHand, Direction.Right).GestureDetected += () => OnGestureSwipeRight(camera.LeftHand);
            CreateSwipeGesture(camera.RightHand, Direction.Right).GestureDetected += () => OnGestureSwipeRight(camera.RightHand);

            CreateSwipeGesture(camera.LeftHand, Direction.Up).GestureDetected += () => OnGestureSwipeUp(camera.LeftHand);
            CreateSwipeGesture(camera.RightHand, Direction.Up).GestureDetected += () => OnGestureSwipeUp(camera.RightHand);

            CreateSwipeGesture(camera.LeftHand, Direction.Down).GestureDetected += () => OnGestureSwipeDown(camera.LeftHand);
            CreateSwipeGesture(camera.RightHand, Direction.Down).GestureDetected += () => OnGestureSwipeDown(camera.RightHand);

            CreateSwipeGesture(camera.LeftHand, Direction.Forward).GestureDetected += () => OnMoveForward(camera.LeftHand);
            CreateSwipeGesture(camera.RightHand, Direction.Forward).GestureDetected += () => OnMoveForward(camera.RightHand);
            
        }

        private Gesture CreateSwipeGesture(Hand hand, Direction direction) {
            var swipe = new Gesture("hand"+hand.Side + "_" + direction);
            swipe.AddStep(800, Movement.CreateMovement(direction, hand, 18));
            swipe.Activate();
            return swipe;
        }

        public void OnGestureSwipeRight(Hand hand) {
            Action<Hand> handler = SwipeRight;
            if (handler != null) handler(hand);
        }

        public void OnGestureSwipeLeft(Hand hand) {
            Action<Hand> handler = SwipeLeft;
            if (handler != null) handler(hand);
        }

        public virtual void OnGestureSwipeUp(Hand hand) {
            Action<Hand> handler = SwipeUp;
            if (handler != null) handler(hand);
        }
        public virtual void OnGestureSwipeDown(Hand hand) {
            Action<Hand> handler = SwipeDown;
            if (handler != null) handler(hand);
        }

        protected virtual void OnMoveForward(Hand hand) {
            Action<Hand> handler = MoveForward;
            if (handler != null) handler(hand);
        }

    }
}