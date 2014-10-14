using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.Tests {
    public class FakeCamera : ICamera {
        
        public Hand LeftHand { get; private set; }
        public Hand RightHand { get; private set; }
        public IGestureSensor GestureSensor { get; set; }
        public IPoseSensor PoseSensor { get; set; }

        public FakeCamera() {
            LeftHand = new Hand(Side.Left);
            RightHand = new Hand(Side.Right);
            GestureSensor = new GestureSensor(this);
            PoseSensor = new PoseSensor(this);
        }

        public void MoveLeftHandZ(double z) {
            var p = LeftHand.Position;
            LeftHand.Position = new Point3d(p.X, p.Y, z);
        }

        public void MoveRightHandZ(double z) {
            var p = RightHand.Position;
            RightHand.Position = new Point3d(p.X, p.Y, z);
        }

        public void MoveLeftHandX(double x) {
            var p = LeftHand.Position;
            LeftHand.Position = new Point3d(x, p.Y, p.Z);
        }

        public void MoveRightHandX(double x) {
            var p = RightHand.Position;
            RightHand.Position = new Point3d(x, p.Y, p.Z);
        }

        public void Start() {
        }
        public void Dispose() {
        }

    }
}