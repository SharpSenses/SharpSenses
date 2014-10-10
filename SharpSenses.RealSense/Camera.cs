using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSenses.RealSense {
    public class Camera : ICamera {
        private pxcmStatus NoError = pxcmStatus.PXCM_STATUS_NO_ERROR;
        private PXCMSession _session;
        private PXCMSenseManager _manager;
        private CancellationTokenSource _cancellationToken;

        public Hand LeftHand { get; private set; }
        public Hand RightHand { get; private set; }

        public Camera() {
            LeftHand = new Hand(Side.Left);
            RightHand = new Hand(Side.Right);
            _session = PXCMSession.CreateInstance();
            _manager = _session.CreateSenseManager();
            Debug.WriteLine("SDK Version {0}.{1}", _session.QueryVersion().major, _session.QueryVersion().minor);
        }

        public void Start() {
            _cancellationToken = new CancellationTokenSource();
            Task.Factory.StartNew(Loop, 
                                  TaskCreationOptions.LongRunning,
                                  _cancellationToken.Token);
        }

        private void Loop(object notUsed) {
            if (_manager.EnableHand() == NoError) {
                Debug.WriteLine("HandModule OK");
            }
            var handModule = _manager.QueryHand();
            using (var handConfig = handModule.CreateActiveConfiguration()) {
                handConfig.EnableAllAlerts();
                handConfig.EnableAllGestures();
                handConfig.EnableTrackedJoints(true);
                handConfig.EnableSegmentationImage(true);
                handConfig.Update();
                handConfig.ApplyChanges();
            }
            var handData = handModule.CreateOutput();
            handModule.Dispose();
            _manager.Init();

            while (!_cancellationToken.IsCancellationRequested) {
                _manager.AcquireFrame(true);
                handData.Update();
                TrackHandAndFingers(LeftHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS);
                TrackHandAndFingers(RightHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS);
                _manager.ReleaseFrame();
            }
        }

        private void TrackHandAndFingers(Hand hand, PXCMHandData data, PXCMHandData.AccessOrderType label) {
            PXCMHandData.IHand handInfo;
            if (data.QueryHandData(label, 0, out handInfo) != NoError) {
                hand.IsVisible = false;
                return;
            }
            hand.IsVisible = true;
            hand.IsOpen = handInfo.QueryOpenness() > 75;
            var location = handInfo.QueryMassCenterWorld();
            Func<double, double> meterToCentimeters = p => p * 100;
            hand.Position = new Point3D(meterToCentimeters(location.x), 
                                        meterToCentimeters(location.y),
                                        meterToCentimeters(location.z));
        }

        public void Dispose() {
            _manager.Dispose();
            _session.Dispose();
        }
    }
}
