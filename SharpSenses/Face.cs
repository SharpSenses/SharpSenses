using System;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses {
    public class Face : Item {
        private readonly IFaceRecognizer _faceRecognizer;
        private int _userId;
        private FacialExpression _facialExpression;
        private Direction _eyesDirection;
        public Mouth Mouth { get; private set; }
        public Eye LeftEye { get; set; }
        public Eye RightEye { get; set; }

        public event EventHandler<FaceRecognizedEventArgs> FaceRecognized;
        public event EventHandler<FacialExpressionEventArgs> FacialExpresssionChanged;
        public event EventHandler<DirectionEventArgs> EyesDirectionChanged;
        public event EventHandler<EventArgs> WinkedLeft;
        public event EventHandler<EventArgs> WinkedRight;
        public event EventHandler<EventArgs> Yawned;

        public Face(IFaceRecognizer faceRecognizer) {
            _faceRecognizer = faceRecognizer;
            Mouth = new Mouth();
            LeftEye = new Eye(Side.Left);
            RightEye = new Eye(Side.Right);

            Mouth.Opened += DetectYawn;
            LeftEye.Closed += DetectYawn;
            RightEye.Closed += DetectYawn;

            ConfigLeftWink();
            ConfigRightWink();
        }

        private void ConfigLeftWink() {
            Pose pose = PoseBuilder.Create()
                .ShouldBe(LeftEye, State.Closed)
                .ShouldBe(RightEye, State.Opened)
                .HoldPoseFor(200).Build("LeftWinked");
            pose.Begin += (sender, args) => { FireWinkedLeft(); };
        }
        private void ConfigRightWink() {
            Pose pose = PoseBuilder.Create()
                .ShouldBe(LeftEye, State.Opened)
                .ShouldBe(RightEye, State.Closed)
                .HoldPoseFor(200)
                .Build("RightWinked");
            pose.Begin += (sender, args) => { FireWinkedRight(); };
        }

        public int UserId {
            get { return _userId; }
            set {
                if (_userId == value) {
                    return;
                }
                _userId = value;
                RaisePropertyChanged(() => UserId);
                OnPersonRecognized();
            }
        }

        public FacialExpression FacialExpression {
            get { return _facialExpression; }
            set {
                if (_facialExpression == value) {
                    return;
                }
                var old = _facialExpression;
                _facialExpression = value;
                RaisePropertyChanged(() => FacialExpression);
                OnFacialExpresssionChanged(old, value);
            }
        }

        public Direction EyesDirection {
            get { return _eyesDirection; }
            set {
                if (_eyesDirection == value) {
                    return;
                }
                var old = _eyesDirection;
                _eyesDirection = value;
                RaisePropertyChanged(() => EyesDirection);
                FireEyesDirectionChanged(old, value);
            }
        }

        private DateTime _startYawn;

        private void DetectYawn(object sender, EventArgs e) {
            var diff = (DateTime.Now - _startYawn).TotalMilliseconds;
            if (diff > 2000 && diff < 8000) {
                FireYawned();
                _startYawn = DateTime.MinValue;
                return;
            }
            if (Mouth.IsOpen && !LeftEye.IsOpen && !RightEye.IsOpen) {
                _startYawn = DateTime.Now;
            }   
        }

        public bool RecognizeFace() {
            if (_faceRecognizer == null) {
                return false;
            }
            _faceRecognizer.RecognizeFace();
            return true;
        }

        protected virtual void OnFacialExpresssionChanged(FacialExpression old, FacialExpression @new) {
            var handler = FacialExpresssionChanged;
            if (handler != null) handler(this, new FacialExpressionEventArgs(old, @new));
        }

        protected virtual void OnPersonRecognized() {
            var handler = FaceRecognized;
            if (handler != null) handler(this, new FaceRecognizedEventArgs(UserId));
        }

        protected virtual void FireEyesDirectionChanged(Direction old, Direction newDirection) {
            var handler = EyesDirectionChanged;
            if (handler != null) handler(this, new DirectionEventArgs(old, newDirection));
        }

        protected virtual void FireYawned() {
            var handler = Yawned;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void FireWinkedLeft() {
            var handler = WinkedLeft;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void FireWinkedRight() {
            var handler = WinkedRight;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}