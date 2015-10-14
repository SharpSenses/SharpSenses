using System;

namespace SharpSenses {
    public class Mouth : FlexiblePart {
        private bool _isSmiling;

        public event EventHandler Smiled;

        public bool IsSmiling {
            get { return _isSmiling; }
            set {
                if (_isSmiling == value) {
                    return;
                }
                _isSmiling = value;
                if (_isSmiling) {
                    FireSmiled();
                }
            }
        }

        protected virtual void FireSmiled() {
            Smiled?.Invoke(this, EventArgs.Empty);
        }
    }
}
