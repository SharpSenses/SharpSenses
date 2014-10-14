using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.RealSense {
    public class Camera : ICamera {
        private pxcmStatus NoError = pxcmStatus.PXCM_STATUS_NO_ERROR;
        private PXCMSession _session;
        private PXCMSenseManager _manager;
        private CancellationTokenSource _cancellationToken;

        public Hand LeftHand { get; private set; }
        public Hand RightHand { get; private set; }
        public IGestureSensor GestureSensor { get; set; }
        public IPoseSensor PoseSensor { get; set; }

        public Camera() {
            LeftHand = new Hand(Side.Left);
            RightHand = new Hand(Side.Right);
            GestureSensor = new GestureSensor(this);
            PoseSensor = new PoseSensor(this);
            _session = PXCMSession.CreateInstance();
            _manager = _session.CreateSenseManager();
            Debug.WriteLine("SDK Version {0}.{1}", _session.QueryVersion().major, _session.QueryVersion().minor);
        }

        public void Start() {
            _cancellationToken = new CancellationTokenSource();
            _manager.EnableHand();
            using (var handModule = _manager.QueryHand()) {
                using (var handConfig = handModule.CreateActiveConfiguration()) {
                    handConfig.EnableAllAlerts();
                    int numGestures = handConfig.QueryGesturesTotalNumber();
                    for (int i = 0; i < numGestures; i++) {
                        string name;
                        handConfig.QueryGestureNameByIndex(i, out name);
                        Debug.WriteLine("Gestures: " + name);
                    }
                    handConfig.EnableAllGestures();
                    handConfig.EnableTrackedJoints(true);
                    handConfig.SubscribeGesture(OnGesture);
                    handConfig.ApplyChanges();
                }
            }
            var status = _manager.Init();
            if (status != NoError) {
                throw new CameraException(status.ToString());
            }
            Task.Factory.StartNew(Loop,
                                  TaskCreationOptions.LongRunning,
                                  _cancellationToken.Token);
        }

        private void Loop(object notUsed) {
            using (var handModule = _manager.QueryHand()) {
                using (var handData = handModule.CreateOutput()) {
                    while (!_cancellationToken.IsCancellationRequested) {
                        _manager.AcquireFrame(true);
                        handData.Update();
                        TrackHandAndFingers(LeftHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS);
                        TrackHandAndFingers(RightHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS);
                        _manager.ReleaseFrame();
                    }
                }
            }
        }

        private string _last = "";

        private void OnGesture(PXCMHandData.GestureData gesturedata) {
            string g = String.Format("Gesture: {0}-{1}-{2}",
                gesturedata.name,
                gesturedata.handId,
                gesturedata.state);
            if (_last == g) {
                return;
            }
            _last = g;
            Debug.WriteLine(g);
        }

        private void TrackHandAndFingers(Hand hand, PXCMHandData data, PXCMHandData.AccessOrderType label) {
            PXCMHandData.IHand handInfo;
            if (data.QueryHandData(label, 0, out handInfo) != NoError) {
                hand.IsVisible = false;
                return;
            }
            hand.IsVisible = true;
            //SetHandOpenness(hand, handInfo);
            SetPosition(hand, handInfo.QueryMassCenterWorld());
            TrackFinger(hand.Index, handInfo, PXCMHandData.JointType.JOINT_INDEX_TIP, PXCMHandData.FingerType.FINGER_INDEX);
            TrackFinger(hand.Middle, handInfo, PXCMHandData.JointType.JOINT_MIDDLE_TIP, PXCMHandData.FingerType.FINGER_MIDDLE);
            TrackFinger(hand.Ring, handInfo, PXCMHandData.JointType.JOINT_RING_TIP, PXCMHandData.FingerType.FINGER_RING);
            TrackFinger(hand.Pinky, handInfo, PXCMHandData.JointType.JOINT_PINKY_TIP, PXCMHandData.FingerType.FINGER_PINKY);
            TrackFinger(hand.Thumb, handInfo, PXCMHandData.JointType.JOINT_THUMB_TIP, PXCMHandData.FingerType.FINGER_THUMB);
            SetHandOpenness(hand);
        }

        private void SetHandOpenness(Hand hand) {
            if (hand.IsOpen) {
                hand.IsOpen = hand.GetAllFingers().Any(f => f.IsOpen);
                return;
            }
            hand.IsOpen = hand.GetAllFingers().All(f => f.IsOpen);
        }

        private static void SetHandOpenness(Hand hand, PXCMHandData.IHand handInfo) {
            //allways returns 0 for me :(
            int openness = handInfo.QueryOpenness();
            if (openness > 75) {
                hand.IsOpen = true;
            }
            else if (openness < 35) {
                hand.IsOpen = false;
            }
        }

        private void TrackFinger(Finger finger, PXCMHandData.IHand handInfo, PXCMHandData.JointType jointKind, PXCMHandData.FingerType fingerType) {
            PXCMHandData.JointData jointData;
            if (handInfo.QueryTrackedJoint(jointKind, out jointData) != NoError) {
                finger.IsVisible = false;
                return;
            }
            finger.IsVisible = true;
            SetPosition(finger, jointData.positionWorld);
            PXCMHandData.FingerData fingerData;
            if (handInfo.QueryFingerData(fingerType, out fingerData) != NoError) {
                return;
            }
            finger.IsOpen = fingerData.foldedness == 100;
        }

        private void SetPosition(Item item, PXCMPoint3DF32 position) {
            item.Position = new Point3d(ToRoundedCentimeters(position.x),
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
