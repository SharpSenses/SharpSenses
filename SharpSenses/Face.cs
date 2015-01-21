using System;

namespace SharpSenses {
    public class Face : Item {
        private readonly IFaceRecognizer _faceRecognizer;
        private int _userId;
        private FacialExpression _facialExpression;
        public Mouth Mouth { get; private set; }

        public event EventHandler<FaceRecognizedEventArgs> PersonRecognized;
        public event EventHandler<FacialExpressionEventArgs> FacialExpresssionChanged;

        public int UserId {
            get { return _userId; }
            set {
                if (_userId == value) {
                    return;
                }
                _userId = value;
                RaisePropertyChanged(() => UserId);
                if (_userId > 0) {
                    OnPersonRecognized();                    
                }
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
        
        public Face(IFaceRecognizer faceRecognizer) {
            _faceRecognizer = faceRecognizer;
            Mouth = new Mouth();
        }

        public bool RecognizeFace() {
            if (_faceRecognizer == null) {
                return false;
            }
            _faceRecognizer.Recognize();
            return true;
        }

        protected virtual void OnFacialExpresssionChanged(FacialExpression old, FacialExpression @new) {
            var handler = FacialExpresssionChanged;
            if (handler != null) handler(this, new FacialExpressionEventArgs(old, @new));
        }

        protected virtual void OnPersonRecognized() {
            var handler = PersonRecognized;
            if (handler != null) handler(this, new FaceRecognizedEventArgs(UserId));
        }
    }
}