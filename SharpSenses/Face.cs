using System;

namespace SharpSenses {
    public class Face : Item {
        private FacialExpression _facialExpression;
        public Mouth Mouth { get; private set; }

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

        public event EventHandler<FacialExpressionEventArgs> FacialExpresssionChanged;

        public Face() {
            Mouth = new Mouth();
        }
        protected virtual void OnFacialExpresssionChanged(FacialExpression old, FacialExpression @new) {
            var handler = FacialExpresssionChanged;
            if (handler != null) handler(this, new FacialExpressionEventArgs(old, @new));
        }
    }

    public class FacialExpressionEventArgs : EventArgs {
        public FacialExpression OldFacialExpression { get; set; }
        public FacialExpression NewFacialExpression { get; set; }

        public FacialExpressionEventArgs(FacialExpression oldFacialExpression, FacialExpression newFacialExpression) {
            OldFacialExpression = oldFacialExpression;
            NewFacialExpression = newFacialExpression;
        }
    }
}