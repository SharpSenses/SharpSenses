using System;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses {
    public class Face : Item {
        private readonly IFaceRecognizer _faceRecognizer;
        private int _userId;
        private Emotion _emotion;
        private Direction _eyesDirection;
        public Mouth Mouth { get; private set; }
        public Eye LeftEye { get; set; }
        public Eye RightEye { get; set; }

        public event EventHandler<FaceRecognizedEventArgs> FaceRecognized;
        public event EventHandler<FacialExpressionEventArgs> EmotionChanged;
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

        public Emotion Emotion {
            get { return _emotion; }
            set {
                if (_emotion == value) {
                    return;
                }
                var old = _emotion;
                _emotion = value;
                RaisePropertyChanged(() => Emotion);
                OnEmotionChanged(old, value);
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

        protected virtual void OnEmotionChanged(Emotion old, Emotion @new) {
            EmotionChanged?.Invoke(this, new FacialExpressionEventArgs(old, @new));
        }

        protected virtual void OnPersonRecognized() {
            FaceRecognized?.Invoke(this, new FaceRecognizedEventArgs(UserId));
        }

        protected virtual void FireEyesDirectionChanged(Direction old, Direction newDirection) {
            EyesDirectionChanged?.Invoke(this, new DirectionEventArgs(old, newDirection));
        }

        protected virtual void FireYawned() {
            Yawned?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void FireWinkedLeft() {
            WinkedLeft?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void FireWinkedRight() {
            WinkedRight?.Invoke(this, EventArgs.Empty);
        }
    }
}