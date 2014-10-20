using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSenses.Perceptual {
    public class Pipeline : UtilMPipeline {
        private PerceptualCamera _camera;
        private Task _loopingTask;

        public Pipeline(PerceptualCamera camera) {
            _camera = camera;
        }

        public void Start() {
            EnableGesture();
            EnableFaceLandmark();
            if (!Init()) {
                throw new CameraException("Could not initialize the camera");
            }
            _loopingTask = Task.Run(() => {
                try {
                    Loop();
                }
                catch (Exception ex) {
                    Debug.WriteLine("EXCEPTION! " + ex);
                }
            });
        }

        private void Loop() {
            while (true) {
                AcquireFrame(true);
                _camera.OnNewFrame();
                ReleaseFrame();
            }
        }

        public override void OnGesture(ref PXCMGesture.Gesture gesture) {
            _camera.OnGesture(gesture);
        }
    }
}
