using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpSenses.Gestures;

namespace SharpSenses.RealSense.Capabilities {
    public class GesturesCapability : ICapability {
        private RealSenseCamera _camera;
        private GestureSensor _sensor;

        public IEnumerable<Capability> Dependencies => new[] {Capability.HandTracking};

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            camera.Manager.EnableHand();
            using (var handModule = camera.Manager.QueryHand()) {
                using (var handConfig = handModule.CreateActiveConfiguration()) {
                    //handConfig.EnableAllAlerts();
                    int numGestures = handConfig.QueryGesturesTotalNumber();
                    for (int i = 0; i < numGestures; i++) {
                        string name;
                        handConfig.QueryGestureNameByIndex(i, out name);
                        Debug.WriteLine("Gestures: " + name);
                    }
                    handConfig.EnableAllGestures();
                    handConfig.SubscribeGesture(OnGesture);
                    handConfig.EnableTrackedJoints(true);
                    handConfig.ApplyChanges();
                }
            }
            _sensor = (GestureSensor) camera.Gestures;
            GestureSlide.Configure(camera, (GestureSensor) camera.Gestures);
        }

        private void OnGesture(PXCMHandData.GestureData gesturedata) {
            string g = $"Gesture: {gesturedata.name}-{gesturedata.handId}-{gesturedata.state}";
            Debug.WriteLine(g);
            switch (gesturedata.name) {
                case "wave":
                    //_gestures.OnWave(new GestureEventArgs("wave"));
                    return;
                //case "swipe_left":
                //    _sensor.OnSlideLeft(new GestureEventArgs("Swipe Left"));
                //    return;
            }
        }

        public void Loop() {
        }

        public void Dispose() {
        }
    }
}