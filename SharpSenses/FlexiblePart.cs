using System;

namespace SharpSenses {
    public class FlexiblePart : Item {
        
        private bool _isOpen;
        public event Action Opened;
        public event Action Closed;

        public virtual string GetInfo() {
            return String.Format("{0} o:{1} x:{2} y:{3}", IsVisible, IsOpen, Position.X, Position.Y);
        }

        public bool IsOpen {
            get { return _isOpen; }
            set {
                if (_isOpen == value) return;
                _isOpen = value;
                if (_isOpen) {
                    OnOpened();
                }
                else {
                    OnClosed();
                }
            }
        }

        protected virtual void OnClosed() {
            Action handler = Closed;
            if (handler != null) handler();
        }

        protected virtual void OnOpened() {
            Action handler = Opened;
            if (handler != null) handler();
        }
    }
}