using System;

namespace SharpSenses {
    public class ImageEventArgs : EventArgs {
        public byte[] BitmapImage { get; set; }

        public ImageEventArgs(byte[] bitmapImage) {
            BitmapImage = bitmapImage;
        }
    }
}