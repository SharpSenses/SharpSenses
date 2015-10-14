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
            var streamProfile = PXCMCapture.StreamTypeToIndex(PXCMCapture.StreamType.STREAM_TYPE_COLOR);
            var format = PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2;
            var desc = new PXCMSession.ImplDesc();
            desc.group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR;
            desc.subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE;

            for (int i = 0; ; i++) {
                PXCMSession.ImplDesc implDesc;
                if (_camera.Session.QueryImpl(desc, i, out implDesc) < Errors.NoError) {
                    break;
                }
                PXCMCapture capture;
                if (_camera.Session.CreateImpl<PXCMCapture>(implDesc, out capture) < Errors.NoError) {
                    continue;
                }
                for (int j = 0; ; j++) {
                    PXCMCapture.DeviceInfo dinfo;
                    if (capture.QueryDeviceInfo(j, out dinfo) < Errors.NoError) {
                        break;
                    }
                    //ToolStripMenuItem sm1 = new ToolStripMenuItem(dinfo.name, null, new EventHandler(Device_Item_Click));
                    //devices[sm1] = dinfo;
                    //devices_iuid[sm1] = implDesc.iuid;
                    //DeviceMenu.DropDownItems.Add(sm1);
                }
                capture.Dispose();
            }
            //_camera.Manager.captureManager.FilterByDeviceInfo(dinfo);
            //_camera.Manager.captureManager.FilterByStreamProfiles(profiles);
            //_camera.Manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, _camera.ResolutionWidth, _camera.ResolutionHeight, _camera.FramesPerSecond);
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