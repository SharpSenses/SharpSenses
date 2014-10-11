using System;

namespace SharpSenses.Gestures {

    public enum MovementStatus {
        Idle,
        Working,
        Completed
    }

    public abstract class Movement {
        private int _count;
        public MovementStatus Status { get; private set; }
        protected Point3D StartPosition { get; set; }
        protected Point3D LastPosition { get; set; }

        public Item Item { get; protected set; }
        public double Distance { get; protected set; }

        public event Action Update;
        public event Action Completed;

        protected Movement(Item item, double distance) {
            Item = item;
            Distance = distance;
        }

        public bool Active {
            set {
                if (value) {
                    Status = MovementStatus.Idle;
                    Item.Moved += ItemOnMoved;
                    Item.NotVisible += Reset;
                }
                else {
                    Status = MovementStatus.Idle;
                    Item.Moved -= ItemOnMoved;
                    Item.NotVisible -= Reset;
                }
            }
        }

        private void ItemOnMoved(Point3D point3D) {
            if (ComputePosition(point3D)) {
                OnCompleted();                
                return;
            }
            OnUpdate();
        }

        private bool ComputePosition(Point3D position) {
            if (Status == MovementStatus.Completed || _count++%2 != 0) return false;
            position = RemoveNoise(position);
            if (Status == MovementStatus.Idle) {
                Status = Status = MovementStatus.Working;
                StartPosition = position;
                LastPosition = position;
                return false;
            }
            if (!IsRightDirection(position)) {
                Status = MovementStatus.Working;
                return false;
            }
            if (StepCompleted(position)) {
                Status = MovementStatus.Completed;
                return true;
            }
            return false;
        }

        public void Reset() {
            Status = MovementStatus.Idle;
        }

        protected abstract bool StepCompleted(Point3D position);

        protected virtual Point3D RemoveNoise(Point3D position) {
            position.Z = Math.Round(position.Z, 2);
            return position;
        }

        protected abstract bool IsRightDirection(Point3D currentLocation);

        protected virtual void OnUpdate() {
            Action handler = Update;
            if (handler != null) handler();
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