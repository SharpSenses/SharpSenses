using System;

namespace SharpSenses {
    public class ImageStream {
        private byte[] _currentBitmapImage;

        public byte[] CurrentBitmapImage {
            get { return _currentBitmapImage; }
            set {
                if (_currentBitmapImage == value) {
                    return;
                }
                _currentBitmapImage = value;
                RaiseNewImageAvailable(value);
            }
        }

        public event EventHandler<ImageEventArgs> NewImageAvailable;

        protected virtual void RaiseNewImageAvailable(byte[] bitmapImage) {
            NewImageAvailable?.Invoke(this, new ImageEventArgs(bitmapImage));
        }
    }
}