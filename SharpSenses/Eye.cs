using System;

namespace SharpSenses {
    public class Eye : FlexiblePart {
        public Side Side { get; set; }
        public event EventHandler Blink;
        public event EventHandler DoubleBlink;

        private DateTime _lastClose;
        private DateTime _lastBlink;

        public Eye(Side side) {
            Side = side;
        }

        protected override void OnOpened() {
            base.OnOpened();
            if ((DateTime.Now - _lastBlink).TotalMilliseconds < 500) {
                FireDoubleBlink();
            }
            if ((DateTime.Now - _lastClose).TotalMilliseconds < 500) {
                FireBlink();
                _lastBlink = DateTime.Now;
            }
        }

        protected override void OnClosed() {
            base.OnClosed();
            _lastClose = DateTime.Now;
        }

        protected virtual void FireBlink() {
            var handler = Blink;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void FireDoubleBlink() {
            var handler = DoubleBlink;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}