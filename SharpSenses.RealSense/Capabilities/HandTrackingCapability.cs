using System;
using System.Collections.Generic;
using SharpSenses.RealSense.Util;
using SharpSenses.Util;
using static SharpSenses.PositionHelper;
using static SharpSenses.RealSense.Errors;

namespace SharpSenses.RealSense.Capabilities {

    public class HandTrackingCapability : ICapability {
        private RealSenseCamera _camera;
        private Dictionary<Item, PXCMSmoother.Smoother3D> _smoothers = new Dictionary<Item, PXCMSmoother.Smoother3D>(); 
        private PXCMSmoother _smootherFactory;
        private PXCMHandData _handData;
        private LoopObjects _loopObjects;

        public static string LeftHandDataKey = "LeftHandDataKey"; 
        public static string RightHandDataKey = "RightHandDataKey"; 

        public IEnumerable<Capability> Dependencies => new List<Capability>();

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            _camera.Manager.EnableHand();
            _camera.Session.CreateImpl(out _smootherFactory);
            _smoothers.Add(_camera.LeftHand, _smootherFactory.Create3DQuadratic(0.8f));
            _smoothers.Add(_camera.RightHand, _smootherFactory.Create3DQuadratic(0.8f));
        }

        public void Loop(LoopObjects loopObjects) {
            _loopObjects = loopObjects;
            var handModule = _camera.Manager.QueryHand();
            if (handModule == null) {
                return;
            }
            _handData = handModule.CreateOutput();
            _handData.Update();

            TrackHand(_camera.LeftHand, PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS);
            TrackHand(_camera.RightHand, PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS);
        }

        private void TrackHand(Hand hand, PXCMHandData.AccessOrderType label) {
            PXCMHandData.IHand handInfo;
            if (_handData.QueryHandData(label, 0, out handInfo) != NoError) {
                hand.IsVisible = false;
                return;
            }
            hand.IsVisible = true;

            SetHandOrientation(hand, handInfo);
            SetHandOpenness(hand, handInfo);
            SetHandPosition(hand, handInfo);

            _loopObjects.Add(handInfo, label.ToString());
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
            var imagePosition = handInfo.QueryMassCenterImage().ToPoint3D();
            var worldPosition = world.ToPoint3D();
            hand.Position = CreatePosition(imagePosition, worldPosition);
        }

        private void SetHandOpenness(Hand hand, PXCMHandData.IHand handInfo) {
            int openness = handInfo.QueryOpenness();
            hand.SetOpenness(openness);
        }

        public void Dispose() {
            _smootherFactory.SilentlyDispose();
            foreach (var item in _smoothers.Values) {
                item.SilentlyDispose();
            }
            _handData.Dispose();
        }
    }
}
