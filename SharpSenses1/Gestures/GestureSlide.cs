using System;
using System.Diagnostics;

namespace SharpSenses.Gestures {
    public abstract class GestureSlide {
        protected int Middle { get; private set; }
        private Position _last;
        private double _startedSecundaryValue;

        public static int GestureLength = 30;
        public static int SecundaryDirectionTolerance = 40;
        public static int WrongDirectionTolerance = 4;

        public bool GestureHappening { get; private set; }

        public event EventHandler SlideDetected;

        public static void Configure(ICamera camera, GestureSensor gestureSensor) {
            var middleWidth = camera.ResolutionWidth/2;
            var middleHeight = camera.ResolutionHeight/2;
            var l = camera.LeftHand;
            var r = camera.RightHand;

            new GestureSlideLeft(l, middleWidth).SlideDetected += (s, a) =>
                gestureSensor.OnSlideLeft(new GestureEventArgs("Left Hand Slide Left"));
            new GestureSlideRight(l, middleWidth).SlideDetected += (s, a) =>
                gestureSensor.OnSlideRight(new GestureEventArgs("Left Hand Slide Right"));
            new GestureSlideUp(l, middleHeight).SlideDetected += (s, a) =>
                gestureSensor.OnSlideUp(new GestureEventArgs("Left Hand Slide Up"));
            new GestureSlideDown(l, middleHeight).SlideDetected += (s, a) =>
                gestureSensor.OnSlideDown(new GestureEventArgs("Left Hand Slide Down"));

            new GestureSlideLeft(r, middleWidth).SlideDetected += (s, a) =>
                gestureSensor.OnSlideLeft(new GestureEventArgs("Right Hand Slide Left"));
            new GestureSlideRight(r, middleWidth).SlideDetected += (s, a) =>
                gestureSensor.OnSlideRight(new GestureEventArgs("Right Hand Slide Right"));
            new GestureSlideUp(r, middleHeight).SlideDetected += (s, a) =>
                gestureSensor.OnSlideUp(new GestureEventArgs("Right Hand Slide Up"));
            new GestureSlideDown(r, middleHeight).SlideDetected += (s, a) =>
                gestureSensor.OnSlideDown(new GestureEventArgs("Right Hand Slide Down"));
        }

        protected GestureSlide(Hand hand, int middle) {
            Middle = middle;
            hand.Moved += HandOnMoved;
        }

        private void HandOnMoved(object sender, PositionEventArgs positionEventArgs) {
            var current = positionEventArgs.NewPosition;
            var lastPrimaryValue = GetLastPrimaryValue(_last);
            var currentPrimaryValue = GetCurrentPrimaryValue(current);
            var currentSecundaryValue = GetCurrentSecundaryValue(current);
            if (!GestureHappening) {
                if (IsInStartArea(currentPrimaryValue, GetBeginLimit())) {
                    _startedSecundaryValue = currentSecundaryValue;
                    GestureHappening = true;
                }
                _last = current;
                return;
            }
            var dif = Math.Abs(currentSecundaryValue - _startedSecundaryValue);
            if (dif > SecundaryDirectionTolerance ||
                !IsRightDirection(currentPrimaryValue, lastPrimaryValue)) {
                GestureHappening = false;
                _last = current;
            }
            if (IsInEndArea(currentPrimaryValue, GetEndLimit())) {
                OnSlideDetected();
                GestureHappening = false;
            }
            _last = current;
        }

        protected abstract double GetBeginLimit();
        protected abstract double GetEndLimit();
        protected abstract bool IsRightDirection(double currentPrimaryValue, double lastPrimaryValue);
        protected abstract bool IsInEndArea(double currentPrimaryValue, double endLimit);
        protected abstract bool IsInStartArea(double currentPrimaryValue, double beginLimit);
        protected abstract double GetLastPrimaryValue(Position lastPosition);
        protected abstract double GetCurrentSecundaryValue(Position position);
        protected abstract double GetCurrentPrimaryValue(Position position);

        protected virtual void OnSlideDetected() {
            var handler = SlideDetected;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}