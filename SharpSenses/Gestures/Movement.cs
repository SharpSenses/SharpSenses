using System;
using System.Diagnostics;

namespace SharpSenses.Gestures {
    public abstract class Movement {
        private int _count;
        private object _sync = new object();
        public MovementStatus Status { get; private set; }
        protected Point3d StartPosition { get; set; }
        protected Point3d LastPosition { get; set; }
        public Item Item { get; protected set; }
        public double Distance { get; protected set; }

        public event Action Restarted;
        public event Action<double> Progress;
        public event Action Completed;

        protected Movement(Item item, double distance) {
            Item = item;
            Distance = distance;
        }

        public void Activate() {
            lock (_sync) {
                Deactivate();
                Status = MovementStatus.Idle;
                Item.Moved += ItemOnMoved;
                Item.NotVisible += Restart;                
            }
        }

        public void Deactivate() {
            lock (_sync) {
                Status = MovementStatus.Idle;
                Item.Moved -= ItemOnMoved;
                Item.NotVisible -= Restart;                
            }
        }

        private void ItemOnMoved(Point3d point3D) {
            Debug.WriteLine("Mov -> " + point3D);
            ComputePosition(point3D);
        }

        private void ComputePosition(Point3d position) {
            if (Status == MovementStatus.Completed) {
                return;
            }
            position = RemoveNoise(position);
            if (Status == MovementStatus.Idle) {
                Status = Status = MovementStatus.Working;
                StartPosition = position;
                LastPosition = position;
                return;
            }
            if (!IsRightDirection(position)) {
                Restart();
                return;
            }
            if (IsMovementCompleted(position)) {
                Status = MovementStatus.Completed;
                OnCompleted();
                return;
            }
            LastPosition = position;
            OnProgress(GetProgress(position));
        }

        public void Restart() {
            Status = MovementStatus.Idle;
            OnRestarted();
        }

        protected bool IsMovementCompleted(Point3d position) {
            return GetProgress(position) >= Distance;
        }
        protected abstract double GetProgress(Point3d currentLocation);
        protected abstract bool IsRightDirection(Point3d currentLocation);
        protected virtual Point3d RemoveNoise(Point3d position) {
            position.Z = Math.Round(position.Z, 2);
            return position;
        }
        protected virtual void OnRestarted() {
            Action handler = Restarted;
            if (handler != null) handler();
        }

        protected virtual void OnProgress(double progress) {
            Action<double> handler = Progress;
            if (handler != null) handler(progress);
        }

        protected virtual void OnCompleted() {
            Action handler = Completed;
            if (handler != null) handler();
        }

        public static MovementForward Forward(Item item, double distanceInCm) {
            return new MovementForward(item, distanceInCm);
        }

        public static MovementBackward Backward(Item item, double distanceInCm) {
            return new MovementBackward(item, distanceInCm);
        }
    }
}