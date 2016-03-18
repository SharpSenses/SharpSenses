using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SharpSenses.RealSense.Capabilities {
    public class SegmentationStreamCapability : ICapability {
        private RealSenseCamera _camera;

        public IEnumerable<Capability> Dependencies => new List<Capability>();

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            _camera.Manager.Enable3DSeg();
        }

        public void Loop(LoopObjects loopObjects) {
            var segmentation = _camera.Manager.Query3DSeg();
            if (segmentation == null) {
                return;
            }
            PXCMImage image = segmentation.AcquireSegmentedImage();
            PXCMImage.ImageData imageData;
            image.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                                PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32,
                                out imageData);
            PXCMImage.ImageInfo imageInfo = image.QueryInfo();
            using (var bitmap = new Bitmap(imageData.ToBitmap(0, imageInfo.width, imageInfo.height))) {
                using (var ms = new MemoryStream()) {
                    bitmap.Save(ms, ImageFormat.Bmp);
                    _camera.SegmentationStream.CurrentBitmapImage = ms.ToArray();
                    image.ReleaseAccess(imageData);
                }
            }
        }

        public void Dispose() { }
    }
}
