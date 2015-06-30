using System;

namespace SharpSenses {
    public class RotationablePart : FlexiblePart {
        private Rotation _rotation;

        public Rotation Rotation {
            get { return _rotation; }
            set {
                if (Equals(_rotation, value)) {
                    return;
                }
                _rotation = value;
                OnRotationChanged();
                RaisePropertyChanged(() => Rotation);
            }
        }

        public event EventHandler RotationChanged;

        protected virtual void OnRotationChanged() {
            var handler = RotationChanged;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}