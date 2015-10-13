using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses {
    public abstract class BaseCamera : ICamera {
        private Face _face;
        
        protected GestureSensor _gestures;
        protected PoseSensor _poses;
        
        public abstract int ResolutionWidth { get; }
        public abstract int ResolutionHeight { get; }
        public abstract int FramesPerSecond { get; }
        public Hand LeftHand { get; private set; }
        public Hand RightHand { get; private set; }

        public ImageStream ImageStream { get; set; }
        
        public Face Face {
            get {
                return _face ?? (_face = new Face(GetFaceRecognizer()));
            }
        }

        public IGestureSensor Gestures {
            get { return _gestures; }
        }

        public IPoseSensor Poses {
            get { return _poses; }
        }

        public abstract ISpeech Speech { get; }
        public abstract void Start();
        public abstract void Dispose();

        protected BaseCamera() {
            LeftHand = new Hand(Side.Left);
            RightHand = new Hand(Side.Right);
            _gestures = new GestureSensor();
            _poses = new PoseSensor();
            ImageStream = new ImageStream();
        }

        protected abstract IFaceRecognizer GetFaceRecognizer();
    }
}