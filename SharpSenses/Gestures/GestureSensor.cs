using System;

namespace SharpSenses.Gestures {
    public class GestureSensor : IGestureSensor {
  
        public event Action GestureSwipeLeft;
        public event Action GestureSwipeRight;
        public event Action GestureSwipeUp;
        public event Action GestureSwipeDown;

        public event Action GestureHandWave;
        public event Action GestureHandCircle;

        public GestureSensor(ICamera camera) {
                
        }

        public void OnGestureHandCircle() {
            Action handler = GestureHandCircle;
            if (handler != null) handler();
        }

        public void OnGestureHandWave() {
            Action handler = GestureHandWave;
            if (handler != null) handler();
        }

        public void OnGestureSwipeRight() {
            Action handler = GestureSwipeRight;
            if (handler != null) handler();
        }

        public void OnGestureSwipeLeft() {
            Action handler = GestureSwipeLeft;
            if (handler != null) handler();
        }

        public virtual void OnGestureSwipeUp() {
            Action handler = GestureSwipeUp;
            if (handler != null) handler();
        }
        public virtual void OnGestureSwipeDown() {
            Action handler = GestureSwipeDown;
            if (handler != null) handler();
        }
    }
}