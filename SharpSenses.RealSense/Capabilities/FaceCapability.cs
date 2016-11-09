using System.Collections.Generic;
using System.Linq;
using SharpSenses.RealSense.Util;
using static SharpSenses.PositionHelper;

namespace SharpSenses.RealSense.Capabilities {
    public class FaceCapability : ICapability {
        private RealSenseCamera _camera;
        private PXCMFaceModule _faceModule;
        private PXCMFaceData _faceData;
        public IEnumerable<Capability> Dependencies => new List<Capability>();

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            _camera.Manager.EnableFace();
            _faceModule = camera.Manager.QueryFace();
            _faceData = _faceModule.CreateOutput();
        }
        public void Loop(LoopObjects loopObjects) {
            _faceData.Update();
            var face = FindFace(_faceData);
            if (face == null) {
                return;
            }
            loopObjects.Add(face);
            loopObjects.Add(_faceData);
            TrackFace(face);
        }

        private PXCMFaceData.Face FindFace(PXCMFaceData faceData) {
            if (faceData.QueryNumberOfDetectedFaces() == 0) {
                _camera.Face.IsVisible = false;
                return null;
            }
            _camera.Face.IsVisible = true;
            return faceData.QueryFaces().First();
        }

        private void TrackFace(PXCMFaceData.Face face) {
            PXCMFaceData.LandmarksData landmarksData = face.QueryLandmarks();
            if (landmarksData == null) {
                return;
            }
            PXCMFaceData.LandmarkPoint[] facePoints;
            landmarksData.QueryPoints(out facePoints);
            if (facePoints == null) {
                return;
            }
            SetLandmarkPoint(landmarksData, facePoints, PXCMFaceData.LandmarkType.LANDMARK_UPPER_LIP_CENTER, _camera.Face.Mouth);
            SetLandmarkPoint(landmarksData, facePoints, PXCMFaceData.LandmarkType.LANDMARK_EYE_LEFT_CENTER, _camera.Face.LeftEye);
            SetLandmarkPoint(landmarksData, facePoints, PXCMFaceData.LandmarkType.LANDMARK_EYE_RIGHT_CENTER, _camera.Face.RightEye);
            SetLandmarkPoint(landmarksData, facePoints, PXCMFaceData.LandmarkType.LANDMARK_NOSE_TOP, _camera.Face);
        }

        private void SetLandmarkPoint(PXCMFaceData.LandmarksData data, PXCMFaceData.LandmarkPoint[] points, PXCMFaceData.LandmarkType type, Item item) {
            var index = data.QueryPointIndex(type);
            var pos = points[index];
            item.Position = CreatePosition(pos.image.ToPoint3D(), pos.world.ToPoint3D());
        }

        public void Dispose() {
            _faceModule.SilentlyDispose();
            _faceData.SilentlyDispose();
        }
    }
}