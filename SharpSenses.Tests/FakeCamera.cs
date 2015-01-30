using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.Tests {
    public class FakeCamera : ICamera {
        
        public Hand LeftHand { get; private set; }
        public Hand RightHand { get; private set; }
        public Face Face { get; private set; }
        public IGestureSensor Gestures { get; set; }
        public IPoseSensor Poses { get; set; }

        public ISpeech Speech {
            get { return null; }
        }

        public int ResolutionWidth {
            get { return 320; }
        }

        public int ResolutionHeight {
            get { return 240; }
        }

        public FakeCamera() {
            LeftHand = new Hand(Side.Left);
            RightHand = new Hand(Side.Right);
            Face = new Face(null);
            Gestures = new GestureSensor();
            Poses = new PoseSensor();
        }

        public void MoveLeftHandZ(double z) {
            var p = LeftHand.Position.World;
            LeftHand.Position = CreatePosition(p.X, p.Y, z);
        }

        public void MoveRightHandZ(double z) {
            var p = RightHand.Position.World;
            RightHand.Position = CreatePosition(p.X, p.Y, z);
        }

        public void MoveLeftHandX(double x) {
            var p = LeftHand.Position.World;
            LeftHand.Position = CreatePosition(x, p.Y, p.Z);
        }

        public void MoveRightHandX(double x) {
            var p = RightHand.Position.World;
            RightHand.Position = CreatePosition(x, p.Y, p.Z);
        }

        private Position CreatePosition(double x, double y, double z) {
            return new Position {
                Image = new Point3D(x, y, z),
                World = new Point3D(x, y, z)
            };
        }

        public void Start() {
        }
        public void Dispose() {
        }

    }
}