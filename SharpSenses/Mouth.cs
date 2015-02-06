using System;

namespace SharpSenses {
    public class Mouth : Item {
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
            var handler = Smiled;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
