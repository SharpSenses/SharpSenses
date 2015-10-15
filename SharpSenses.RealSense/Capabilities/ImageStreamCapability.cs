using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SharpSenses.RealSense.Capabilities {
    public class ImageStreamCapability : ICapability {
        private RealSenseCamera _camera;
        public IEnumerable<Capability> Dependencies => new List<Capability>();
        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            _camera.Manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 
                                        _camera.ResolutionWidth, 
                                        _camera.ResolutionHeight, 
                                        _camera.FramesPerSecond);
        }

        public void Loop(LoopObjects loopObjects) {
            var sample = _camera.Manager.QuerySample();
            if (sample == null) {
                return;
            }
            PXCMImage image = sample.color;
            PXCMImage.ImageData imageData;
            image.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                                PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32,
                                out imageData);
            Bitmap bitmap = imageData.ToBitmap(0, image.info.width, image.info.height);
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            _camera.ImageStream.CurrentBitmapImage = ms.ToArray();
            image.ReleaseAccess(imageData);
        }
        public void Dispose() {}
    }
}