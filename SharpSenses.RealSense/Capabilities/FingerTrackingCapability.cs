using System.Collections.Generic;
using SharpSenses.RealSense.Util;
using SharpSenses.Util;
using static SharpSenses.PositionHelper;
using static SharpSenses.RealSense.Errors;
using System.Diagnostics;
using System;
using System.Runtime.ExceptionServices;

namespace SharpSenses.RealSense.Capabilities {
    public class FingerTrackingCapability : ICapability {
        private RealSenseCamera _camera;
        public IEnumerable<Capability> Dependencies => new[] { Capability.HandTracking };

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
        }

        public void Loop(LoopObjects loopObjects) {
            TrackFingers(_camera.LeftHand, PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS, loopObjects);
            TrackFingers(_camera.LeftHand, PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS, loopObjects);
        }

        private void TrackFingers(Hand hand, PXCMHandData.AccessOrderType label, LoopObjects loopObjects) {
            var handInfo = loopObjects.Get<PXCMHandData.IHand>(label.ToString());
            if (handInfo == null) {
                return;
            }
            TrackIndex(hand.Index, handInfo);
            TrackMiddle(hand.Middle, handInfo);
            TrackRing(hand.Ring, handInfo);
            TrackPinky(hand.Pinky, handInfo);
            TrackThumb(hand.Thumb, handInfo);
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
            finger.SetOpenness(fingerData.foldedness);
        }

        //[HandleProcessCorruptedStateExceptions]
        private void SetJointdata(PXCMHandData.IHand handInfo, PXCMHandData.JointType jointType, Item joint) {
            PXCMHandData.JointData jointData;
            if (handInfo.QueryTrackedJoint(jointType, out jointData) != NoError) {
                joint.IsVisible = false;
                return;
            }
            SetVisibleJointPosition(joint, jointData);
        }

        private void SetVisibleJointPosition(Item item, PXCMHandData.JointData jointData) {
            var imagePosition = jointData.positionImage.ToPoint3D();
            var worldPosition = jointData.positionWorld.ToPoint3D();
            item.IsVisible = true;
            item.Position = CreatePosition(imagePosition, worldPosition);
        }

        public void Dispose() {
        }
    }
}
