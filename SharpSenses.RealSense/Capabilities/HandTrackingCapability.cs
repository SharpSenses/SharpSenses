using System;
using System.Collections.Generic;
using static SharpSenses.PositionHelper;
using static SharpSenses.RealSense.Errors;

namespace SharpSenses.RealSense.Capabilities {
    public class HandTrackingCapability : ICapability {
        private RealSenseCamera _camera;
        private Dictionary<Item, PXCMSmoother.Smoother3D> _smoothers = new Dictionary<Item, PXCMSmoother.Smoother3D>(); 
        private PXCMSmoother _smootherFactory;

        public IEnumerable<Capability> Dependencies => new List<Capability>();

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            _camera.Manager.EnableHand();
            _camera.Session.CreateImpl(out _smootherFactory);
            _smoothers.Add(_camera.LeftHand, _smootherFactory.Create3DQuadratic(0.8f));
            _smoothers.Add(_camera.RightHand, _smootherFactory.Create3DQuadratic(0.8f));
        }

        public void Loop(LoopObjects loopObjects) {
            var handModule = _camera.Manager.QueryHand();
            if (handModule == null) {
                return;
            }
            var handData = handModule.CreateOutput();
            handData.Update();
            TrackHandAndFingers(_camera.LeftHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS);
            TrackHandAndFingers(_camera.RightHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS);
            handData.Dispose();
        }

        private void TrackHandAndFingers(Hand hand, PXCMHandData data, PXCMHandData.AccessOrderType label) {
            PXCMHandData.IHand handInfo;
            if (data.QueryHandData(label, 0, out handInfo) != NoError) {
                hand.IsVisible = false;
                return;
            }
            hand.IsVisible = true;

            SetHandOrientation(hand, handInfo);
            SetHandOpenness(hand, handInfo);
            SetHandPosition(hand, handInfo);
            TrackIndex(hand.Index, handInfo);
            TrackMiddle(hand.Middle, handInfo);
            TrackRing(hand.Ring, handInfo);
            TrackPinky(hand.Pinky, handInfo);
            TrackThumb(hand.Thumb, handInfo);
        }

        private void SetHandOrientation(Hand hand, PXCMHandData.IHand handInfo) {
            var d4 = handInfo.QueryPalmOrientation();
            PXCMRotation rotationHelper;
            _camera.Session.CreateImpl(out rotationHelper);
            rotationHelper.SetFromQuaternion(d4);
            var rotationEuler = rotationHelper.QueryEulerAngles(PXCMRotation.EulerOrder.PITCH_YAW_ROLL);

            var x = rotationEuler.x * 180 / Math.PI;
            var y = rotationEuler.y * 180 / Math.PI;
            var z = rotationEuler.z * 180 / Math.PI;
            hand.Rotation = new Rotation(x, y, z);
            rotationHelper.Dispose();
        }

        private void SetHandPosition(Hand hand, PXCMHandData.IHand handInfo) {
            var world = handInfo.QueryMassCenterWorld();
            if (_smoothers.ContainsKey(hand)) {
                world = _smoothers[hand].SmoothValue(world);
            }
            var imagePosition = ToPoint3D(handInfo.QueryMassCenterImage());
            var worldPosition = ToPoint3D(world);
            hand.Position = CreatePosition(imagePosition, worldPosition);
        }

        private void SetHandOpenness(Hand hand, PXCMHandData.IHand handInfo) {
            int openness = handInfo.QueryOpenness();
            SetOpenness(hand, openness);
        }

        private void TrackIndex(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_INDEX, handInfo);
        }

        private void TrackMiddle(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_MIDDLE, handInfo);
        }

        private void TrackRing(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_RING, handInfo);
        }

        private void TrackPinky(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_PINKY, handInfo);
        }

        private void TrackThumb(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_THUMB, handInfo);
        }

        private void SetFingerOpenness(Finger finger, PXCMHandData.FingerType fingerType, PXCMHandData.IHand handInfo) {
            PXCMHandData.FingerData fingerData;
            if (handInfo.QueryFingerData(fingerType, out fingerData) != NoError) {
                return;
            }
            SetOpenness(finger, fingerData.foldedness);
        }

        private void SetJointdata(PXCMHandData.IHand handInfo, PXCMHandData.JointType jointType, Item joint) {
            PXCMHandData.JointData jointData;
            if (handInfo.QueryTrackedJoint(jointType, out jointData) != NoError) {
                joint.IsVisible = false;
                return;
            }
            SetVisibleJointPosition(joint, jointData);
        }

        private void SetOpenness(FlexiblePart part, int scaleZeroToHundred) {
            if (scaleZeroToHundred > 75) {
                part.IsOpen = true;
            }
            else if (scaleZeroToHundred < 35) {
                part.IsOpen = false;
            }
        }

        private void SetVisibleJointPosition(Item item, PXCMHandData.JointData jointData) {
            var imagePosition = ToPoint3D(jointData.positionImage);
            var worldPosition = ToPoint3D(jointData.positionWorld);
            item.IsVisible = true;
            item.Position = CreatePosition(imagePosition, worldPosition);
        }

        private Point3D ToPoint3D(PXCMPointF32 p) {
            return new Point3D(p.x, p.y);
        }

        private Point3D ToPoint3D(PXCMPoint3DF32 p) {
            return new Point3D(p.x, p.y, p.z);
        }

        public void Dispose() {
            _smootherFactory.SilentlyDispose();
            foreach (var item in _smoothers.Values) {
                item.SilentlyDispose();
            }
        }
    }
}
