using System;
using System.Threading.Tasks;

namespace SharpSenses {
    public class Face : Item {
        private int _userId;
        private FacialExpression _facialExpression;
        private bool _savingProcessRunning;
        public Mouth Mouth { get; private set; }

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

        public event EventHandler<PersonRecognizedEventArgs> PersonRecognized;

        public event EventHandler<FacialExpressionEventArgs> FacialExpresssionChanged;

        public Face() {
            Mouth = new Mouth();
        }

        public void SavePerson() {
            _savingProcessRunning = true;
        }

        protected virtual void OnFacialExpresssionChanged(FacialExpression old, FacialExpression @new) {
            var handler = FacialExpresssionChanged;
            if (handler != null) handler(this, new FacialExpressionEventArgs(old, @new));
        }

        protected virtual void OnPersonRecognized() {
            _savingProcessRunning = false;
            var handler = PersonRecognized;
            if (handler != null) handler(this, new PersonRecognizedEventArgs { UserId = UserId});
        }
    }
}