using System;

namespace SharpSenses.Gestures {

    public abstract class Movement {

        public static MovementForward Forward(double distanceInCm, int millis) {
            return new MovementForward(distanceInCm, TimeSpan.FromMilliseconds(millis));
        }

        public static MovementBackward Backward(double distanceInCm, int millis) {
            return new MovementBackward(distanceInCm, TimeSpan.FromMilliseconds(millis));
        }

        public double Distance { get; protected set; }
        public TimeSpan Window { get; protected set; }

        private int _count;
        private bool _idle;
        protected Point3D StartPosition { get; set; }
        protected Point3D LastPosition { get; set; }
        protected DateTime StartTime { get; set; }

        protected Movement(double distance, TimeSpan window) {
            Distance = distance;
            Window = window;
        }

        public bool ComputePosition(Point3D position) {
            if (_count++%2 != 0) return false;
            position = RemoveNoise(position);
            if (_idle) {
                _idle = false;
                StartPosition = position;
                LastPosition = position;
                StartTime = DateTime.Now;
                return false;
            }
            if (TimedOut()) {
                _idle = true;
            }
            if (!IsRightDirection(position)) {
                _idle = true;
                return false;
            }
            if (StepCompleted(position)) {
                _idle = true;
                return true;
            }
            return false;
        }

        private bool TimedOut() {
            return (DateTime.Now - StartTime) > Window;
        }

        protected abstract bool StepCompleted(Point3D position);

        protected virtual Point3D RemoveNoise(Point3D position) {
            position.Z = Math.Round(position.Z, 2);
            return position;
        }

        protected abstract bool IsRightDirection(Point3D currentLocation);
    }
}