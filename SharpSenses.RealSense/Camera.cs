using System;
using System.Diagnostics;
using System.Linq;
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
                handConfig.ApplyChanges();
                handConfig.Update();
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
            SetHandOpenness(hand, handInfo);
            SetPosition(hand, handInfo.QueryMassCenterWorld());
            TrackFinger(hand.Index, handInfo, PXCMHandData.JointType.JOINT_INDEX_TIP);
            TrackFinger(hand.Middle, handInfo, PXCMHandData.JointType.JOINT_MIDDLE_TIP);
            TrackFinger(hand.Ring, handInfo, PXCMHandData.JointType.JOINT_RING_TIP);
            TrackFinger(hand.Pinky, handInfo, PXCMHandData.JointType.JOINT_PINKY_TIP);
            TrackFinger(hand.Thumb, handInfo, PXCMHandData.JointType.JOINT_THUMB_TIP);
        }

        private static void SetHandOpenness(Hand hand, PXCMHandData.IHand handInfo) {
            int openness = handInfo.QueryOpenness();
            if (openness > 75) {
                hand.IsOpen = true;
            }
            else if (openness < 35) {
                hand.IsOpen = false;
            }
        }

        private void TrackFinger(Finger finger, PXCMHandData.IHand handInfo, PXCMHandData.JointType jointKind) {
            PXCMHandData.JointData jointData;
            if (handInfo.QueryTrackedJoint(jointKind, out jointData) != NoError) {
                finger.IsVisible = false;
                return;
            }
            finger.IsVisible = true;
            SetPosition(finger, jointData.positionWorld);
        }

        private void SetPosition(Item item, PXCMPoint3DF32 position) {
            item.Position = new Point3D(ToRoundedCentimeters(position.x),
                                        ToRoundedCentimeters(position.y),
                                        ToRoundedCentimeters(position.z));
        }

        private double ToRoundedCentimeters(double value) {
            return Math.Round(value * 100, 2);
        }

        public void Dispose() {
            _manager.Dispose();
            _session.Dispose();
        }
    }
}
