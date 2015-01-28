using System;
using System.Diagnostics;

namespace SharpSenses.Gestures {
    public abstract class GestureSlide {
        private Position _last;
        private double _beginLimit;
        private double _endLimit;
        private double _startedSecundaryValue;

        public static int WrongDirectionTolerance = 5;
        public static int SecundaryDirectionTolerance = 30;
        public static double BeginLimitModifier = 1.3;
        public static double EndLimitModifier = 0.7;


        public bool GestureHappening { get; private set; }

        public event EventHandler SlideDetected;

        public static void Configure(ICamera camera, GestureSensor gestureSensor) {
            var middleWidth = camera.ResolutionWidth/2;
            var middleHeight = camera.ResolutionHeight/2;
            var l = camera.LeftHand;
            var r = camera.RightHand;

            //new GestureSlideLeft(l, middleWidth).SlideDetected += (s, a) =>
            //    gestureSensor.OnSlideLeft(new GestureEventArgs("Left Hand Slide Left"));
            //new GestureSlideLeft(r, middleWidth).SlideDetected += (s, a) =>
            //    gestureSensor.OnSlideLeft(new GestureEventArgs("Right Hand Slide Left"));
            new GestureSlideRight(l, middleWidth).SlideDetected += (s, a) =>
                gestureSensor.OnSlideRight(new GestureEventArgs("Left Hand Slide Right"));
            //new GestureSlideRight(r, middleWidth).SlideDetected += (s, a) =>
            //    gestureSensor.OnSlideRight(new GestureEventArgs("Right Hand Slide Right"));
            //new GestureSlideUp(r, middleHeight).SlideDetected += (s, a) =>
            //    gestureSensor.OnSlideUp(new GestureEventArgs("Left Hand Slide Up"));
            //new GestureSlideUp(r, middleHeight).SlideDetected += (s, a) =>
            //    gestureSensor.OnSlideUp(new GestureEventArgs("Right Hand Slide Up"));
            //new GestureSlideDown(r, middleHeight).SlideDetected += (s, a) =>
            //    gestureSensor.OnSlideDown(new GestureEventArgs("Left Hand Slide Down"));
            //new GestureSlideDown(r, middleHeight).SlideDetected += (s, a) =>
            //    gestureSensor.OnSlideDown(new GestureEventArgs("Right Hand Slide Down"));
        }

        protected GestureSlide(Hand hand, int middle) {
            _beginLimit = middle * BeginLimitModifier;
            _endLimit = middle * EndLimitModifier;
            hand.Moved += HandOnMoved;
        }

        private void HandOnMoved(object sender, PositionEventArgs positionEventArgs) {
            var current = positionEventArgs.NewPosition;
            var lastPrimaryValue = GetLastPrimaryValue(_last);
            var currentPrimaryValue = GetCurrentPrimaryValue(current);
            var currentSecundaryValue = GetCurrentSecundaryValue(current);
            if (!GestureHappening) {
                if (IsInStartArea(currentPrimaryValue, _beginLimit)) {
                    _startedSecundaryValue = currentSecundaryValue;
                    GestureHappening = true;
                }
                _last = current;
                return;
            }
            var dif = Math.Abs(currentSecundaryValue - _startedSecundaryValue);
            Debug.WriteLine("Start: BeginLimit: {0} Current: {1} Dif Secundary: {2}", _beginLimit, current, dif);
            if (dif > SecundaryDirectionTolerance ||
                IsWrongDirection(currentPrimaryValue, lastPrimaryValue)) {
                GestureHappening = false;
                Debug.WriteLine("Error: Current: {0} Last{1}", currentPrimaryValue, lastPrimaryValue);
                _last = current;
                return;
            }
            if (IsInEndArea(currentPrimaryValue, _endLimit)) {
                OnSlideDetected();
                GestureHappening = false;
                Debug.WriteLine("Good job!");
            }
            _last = current;
        }

        protected abstract bool IsWrongDirection(double currentPrimaryValue, double lastPrimaryValue);
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