using System;

namespace SharpSenses {
    public class Item : ObservableObject {

        public static int DefaultNoiseThreshold = 0;

        public int NoiseThreshold = DefaultNoiseThreshold;

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
                RaisePropertyChanged(() => IsVisible);
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
                value = RemoveNoise(value);
                if (DidNotChange(value)) {
                    return;
                }
                _position = value;
                OnMove(value);
                RaisePropertyChanged(() => Position);
            }
        }

        public Position RemoveNoise(Position nextPosition) {
            if (NoiseThreshold == 0) {
                return nextPosition;
            }
            var i0 = _position.Image;
            var w0 = _position.World;
            var i1 = nextPosition.Image;
            var w1 = nextPosition.World;

            if (Math.Abs(i0.X - i1.X) <= NoiseThreshold) {
                i1.X = i0.X;
            }
            if (Math.Abs(i0.Y - i1.Y) <= NoiseThreshold) {
                i1.Y = i0.Y;
            }
            if (Math.Abs(i0.Z - i1.Z) <= NoiseThreshold) {
                i1.Z = i0.Z;
            }
            if (Math.Abs(w0.X - w1.X) <= NoiseThreshold) {
                w1.X = w0.X;
            }
            if (Math.Abs(w0.Y - w1.Y) <= NoiseThreshold) {
                w1.Y = w0.Y;
            }
            if (Math.Abs(w0.Z - w1.Z) <= NoiseThreshold) {
                w1.Z = w0.Z;
            }
            return new Position {
                Image = i1,
                World = w1
            };
        }

        private bool DidNotChange(Position nextPosition) {
            return nextPosition.Image.Equals(_position.Image);
        }

        protected virtual void OnMove(Position moveRecord) {
            Action<Position> handler = Moved;
            if (handler != null) handler(moveRecord);
        }

        public event Action<Position> Moved;
        
    }
}