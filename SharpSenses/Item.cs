using System;
using System.Diagnostics;
using SharpSenses.Util;

namespace SharpSenses {
    public class Item {

        public static int NoiseThreshold = 2;

        private bool _isVisible;
        public bool IsVisible {
            get { return _isVisible; }
            set {
                if (_isVisible == value) return;
                _isVisible = value;
                if (_isVisible) {
                    OnVisible();
                }
                else {
                    _position = new Position();
                    OnNotVisible();
                }
            }
        }

        public event Action Visible;
        public event Action NotVisible;

        protected virtual void OnNotVisible() {
            Action handler = NotVisible;
            if (handler != null) handler();
        }
        protected virtual void OnVisible() {
            Action handler = Visible;
            if (handler != null) handler();
        }

        private Position _position;

        public Position Position {
            get { return _position; }
            set {
                if (IsNoise(value)) {
                    return;
                }
                _position = value;
                OnMove(value);
            }
        }

        private bool IsNoise(Position value) {
            return MathEx.CalcDistance(_position.Image, value.Image) <= 2;
        }

        protected virtual void OnMove(Position moveRecord) {
            Action<Position> handler = Moved;
            if (handler != null) handler(moveRecord);
        }

        public event Action<Position> Moved;
        
    }
}