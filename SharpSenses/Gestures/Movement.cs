using System;
using System.Diagnostics;

namespace SharpSenses.Gestures {
    public abstract class Movement {
        private int _count;
        private object _sync = new object();
        public string Name { get; set; }
        public MovementStatus Status { get; private set; }
        protected Point3D StartPosition { get; set; }
        protected Point3D LastPosition { get; set; }
        public Item Item { get; protected set; }
        public double Distance { get; protected set; }
        public bool AutoRestart { get; set; }
        private int _wrongDirectionFaults;
        public int ToleranceForWrongDirection { get; set; }

        public event Action Restarted;
        public event Action<double> Progress;
        public event Action Completed;

        protected Movement(Item item, double distance, string name = "") {
            Item = item;
            Distance = distance;
            AutoRestart = true;
            ToleranceForWrongDirection = 3;
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

        private void ItemOnMoved(Position position) {
            var point = position.World;
            if (Status == MovementStatus.Completed) {
                if (AutoRestart && !IsRightDirection(point)) {
                    Status = MovementStatus.Idle;
                }
                return;
            }
            point = RemoveNoise(point);
            if (Status == MovementStatus.Idle) {
                Status = Status = MovementStatus.Working;
                StartPosition = point;
                LastPosition = point;
                return;
            }
            if (!IsRightDirection(point)) {
                _wrongDirectionFaults++;
                if (_wrongDirectionFaults > ToleranceForWrongDirection) {
                    Restart();                    
                }
                return;
            }
            _wrongDirectionFaults = 0;
            if (IsMovementCompleted(point)) {
                Status = MovementStatus.Completed;
                OnCompleted();
                return;
            }
            LastPosition = point;
            OnProgress(GetProgress(point));
        }

        public void Restart() {
            Status = MovementStatus.Idle;
            OnRestarted();
        }

        protected bool IsMovementCompleted(Point3D position) {
            return GetProgress(position) >= Distance;
        }
        protected abstract double GetProgress(Point3D currentLocation);
        protected abstract bool IsRightDirection(Point3D currentLocation);
        protected virtual Point3D RemoveNoise(Point3D position) {
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

        public static MovementLeft Left(Item item, double distanceInCm) {
            return new MovementLeft(item, distanceInCm);
        }

        public static MovementRight Right(Item item, double distanceInCm) {
            return new MovementRight(item, distanceInCm);
        }

        public static MovementUp Up(Item item, double distanceInCm) {
            return new MovementUp(item, distanceInCm);
        }

        public static MovementDown Down(Item item, double distanceInCm) {
            return new MovementDown(item, distanceInCm);
        }

        public static Movement CreateMovement(Direction direction, Item item, double distanceInCm) {
            switch (direction) {
                case Direction.Forward:
                    return new MovementForward(item, distanceInCm);
                case Direction.Backward:
                    return new MovementBackward(item, distanceInCm);
                case Direction.Up:
                    return new MovementUp(item, distanceInCm);
                case Direction.Down:
                    return new MovementDown(item, distanceInCm);
                case Direction.Left:
                    return new MovementLeft(item, distanceInCm);
                case Direction.Right:
                    return new MovementRight(item, distanceInCm);
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }
    }
}